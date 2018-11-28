using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using SiteAnalizer.Infrastructure.Interfaces;

namespace SiteAnalyzer
{
    /// <summary>
    /// Represents a <see cref="SiteManager"/> class.
    /// </summary>
    public class SiteManager
    {
        private readonly ISiteDownloader _siteDownloader;
        private readonly IValidator _validator;
        private readonly ISiteSaver _siteSaver;
        private readonly ILogger _logger;
        private readonly int _maxDeepLevel;
        private HashSet<Uri>[] _linksArray;
        
        public SiteManager(
            ISiteDownloader siteDownloader,
            IValidator validator,
            ISiteSaver siteSaver,
            ILogger logger,
            int maxDeepLevel)
        {
            _siteDownloader = siteDownloader;
            _siteSaver = siteSaver;
            _logger = logger;
            _validator = validator;
            _maxDeepLevel = maxDeepLevel;
            _linksArray = new HashSet<Uri>[_maxDeepLevel + 1];
        }

        public HashSet<Uri>[] LinksArray => _linksArray;

        /// <summary>
        /// Start analyzing Uri.
        /// </summary>
        /// <param name="uries">The uries.</param>
        /// <param name="currentLevel">The current level.</param>
        /// <returns>The <see cref="IEnumerable{Uri}"/> instance.</returns>
        public IEnumerable<Uri> StartAnalyze(IEnumerable<Uri> uries, int currentLevel)
        {
            var workLinks = new HashSet<Uri>();

            try
            {
                var content = Task.WhenAll(uries.Select(url => _siteDownloader.DownloadAsync(url)
                    .ContinueWith(task =>
                    {
                        _siteSaver.Save(task.Result);
                        return GetLinks(task.Result);
                    }
                ))).ContinueWith(task =>
                {
                    foreach (var link in task.Result)
                    {
                        workLinks.UnionWith(link);
                    }

                    return workLinks;
                });

                if (_linksArray[currentLevel] == null)
                {
                    _linksArray[currentLevel] = new HashSet<Uri>();
                }

                workLinks = content.GetAwaiter().GetResult();
                _linksArray[currentLevel].UnionWith(workLinks);

                return workLinks;
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
            HtmlDocument document = new HtmlDocument();

            try
            {
                document.Load(content, Encoding.UTF8);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            var test = document.DocumentNode
                .Descendants()
                .SelectMany(d => d.Attributes.Where(IsValidLink))
                .Where(uri => _validator.IsValid(uri.Value))
                .Select(link => new Uri(link.Value));

            return test;
        }

        /// <summary>
        /// Check attribute as src or href.
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns>True if link has name src or href otherwise false.</returns>
        private bool IsValidLink(HtmlAttribute attribute) => attribute.Name == "src" || attribute.Name == "href";
    }
}
