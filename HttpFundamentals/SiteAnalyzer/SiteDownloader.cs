using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using SiteAnalizer.Infrastructure.Interfaces;
using SiteAnalyzer.Validators;

namespace SiteAnalyzer
{
    /// <summary>
    /// Represents a <see cref="SiteDownloader"/> class.
    /// </summary>
    public class SiteDownloader : ISiteDownloader
    {
        private readonly ILogger _logger;
        private readonly ISiteSaver _siteSaver;
        private readonly Validator _fileValidator;
        private const string HtmlTypeContent = "text/html";

        /// <summary>
        /// Initialize a new <see cref="SiteDownloader"/> instance.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="siteSaver">The site saver.</param>
        /// <param name="fileValidator">The file validator.</param>
        public SiteDownloader(ILogger logger, ISiteSaver siteSaver, Validator fileValidator)
        {
            _logger = logger;
            _siteSaver = siteSaver;
            _fileValidator = fileValidator;
        }

        /// <summary>
        /// Download web page as stream.
        /// </summary>
        /// <param name="uri">The uri.</param>
        /// <param name="currentLevel">The current level.</param>
        /// <returns>The web page content.</returns>
        public async Task<Stream> DownloadAsync(Uri uri, int currentLevel)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var responseMessage = await client.GetAsync(uri);

                    if (responseMessage.StatusCode != HttpStatusCode.OK)
                    {
                        return null;
                    }

                    var content = await responseMessage.Content.ReadAsStreamAsync();
                    var fileName = responseMessage.Content.Headers.ContentType.MediaType.Equals(HtmlTypeContent, StringComparison.InvariantCulture) ? 
                        GetHtmlFileNameWithExtansion(content) : 
                        GetFileNameWithExtansion(uri);

                    if (fileName != null)
                    {
                        _siteSaver.Save(content, fileName, currentLevel);
                    }

                    content.Seek(0, SeekOrigin.Begin);
                    return content;
                }
                catch (Exception e)
                {
                    _logger.Log(e.Message);
                }
            }

            return null;
        }

        /// <summary>
        /// Get html file name with extension.
        /// </summary>
        /// <param name="content">The content as stream.</param>
        /// <returns>The html file name with extension.</returns>
        private string GetHtmlFileNameWithExtansion(Stream content)
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

            var titles = document.DocumentNode.Descendants("title").ToArray();
            return titles.Any() ? $"{titles.First().InnerText}.html" : null;
        }

        /// <summary>
        /// Get file name with extension.
        /// </summary>
        /// <param name="uri">The file uri.</param>
        /// <returns>The file name with extension.</returns>
        private string GetFileNameWithExtansion(Uri uri)
        {
            return _fileValidator.IsValid(uri) ? uri.Segments.Last() : null;
        }
    }
}
