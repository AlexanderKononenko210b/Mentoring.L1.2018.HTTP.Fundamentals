using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using SiteAnalyzer.Infrastructure.Interfaces;

namespace SiteAnalyzer
{
    /// <summary>
    /// Represents a <see cref="SiteManager"/> class.
    /// </summary>
    public class SiteManager
    {
        private readonly ISiteDownloader _siteDownloader;
        private readonly IValidator _validator;
        private readonly ILogger _logger;

        public SiteManager(
            ISiteDownloader siteDownloader,
            IValidator validator,
            ILogger logger)
        {
            _siteDownloader = siteDownloader;
            _logger = logger;
            _validator = validator;
        }

        /// <summary>
        /// Start download information.
        /// </summary>
        /// <param name="uries">The uries for download.</param>
        /// <param name="countLevel">The count level.</param>
        /// <param name="maxDeepLevel">The max deep level.</param>
        public void Start(IEnumerable<Uri> uries, int maxDeepLevel, int countLevel)
        {
            _logger.Log($"Level {countLevel}");
            var links = Analyze(uries, countLevel).ToArray();

            if (!links.Any()) return;

            ++countLevel;

            if (countLevel > maxDeepLevel) return;

            Start(links, countLevel, maxDeepLevel);
        }

        /// <summary>
        /// Start analyzing Uri.
        /// </summary>
        /// <param name="uries">The uries.</param>
        /// <param name="currentLevel">The current level.</param>
        /// <returns>The <see cref="IEnumerable{Uri}"/> instance.</returns>
        private IEnumerable<Uri> Analyze(IEnumerable<Uri> uries, int currentLevel)
        {
            var workLinks = new HashSet<Uri>();

            try
            {
                var content = Task.WhenAll(uries
                        .Select(url => _siteDownloader.DownloadAsync(url, currentLevel)
                            .ContinueWith(task => GetLinks(task.Result))
                    )).ContinueWith(task =>
                    {
                        foreach (var link in task.Result)
                        {
                            workLinks.UnionWith(link);
                        }
                        
                        return workLinks;
                    });

                workLinks = content.GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error source: {ex.Source}, Error message: {ex.Message}");
            }

            return workLinks;
        }

        /// <summary>
        /// Get all links from page.
        /// </summary>
        /// <param name="content">Web page as stream.</param>
        /// <returns>IEnumerable Uri links</returns>
        private IEnumerable<Uri> GetLinks(Stream content)
        {
            if (content == null)
            {
                return new Uri[0];
            }

            HtmlDocument document = new HtmlDocument();
            
            try
            {
                document.Load(content, Encoding.UTF8);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return document.DocumentNode
                .Descendants()
                .SelectMany(d => d.Attributes.Where(IsValidLink))
                .Where(uri => _validator.IsValid(uri.Value))
                .Select(link => new Uri(link.Value));
        }

        /// <summary>
        /// Check attribute as src or href.
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns>True if link has name src or href otherwise false.</returns>
        private bool IsValidLink(HtmlAttribute attribute) => attribute.Name == "src" || attribute.Name == "href";
    }
}
