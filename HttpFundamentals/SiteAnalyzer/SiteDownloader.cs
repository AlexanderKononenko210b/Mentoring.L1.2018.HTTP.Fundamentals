using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using SiteAnalizer.Infrastructure.Interfaces;

namespace SiteAnalyzer
{
    /// <summary>
    /// Represents a <see cref="SiteDownloader"/> class.
    /// </summary>
    public class SiteDownloader : ISiteDownloader
    {
        private readonly ILogger _logger;

        /// <summary>
        /// Initialize a new <see cref="SiteDownloader"/> instance.
        /// </summary>
        /// <param name="logger"></param>
        public SiteDownloader(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Download web page as stream.
        /// </summary>
        /// <param name="uri">The uri.</param>
        /// <returns>The web page content.</returns>
        public async Task<Stream> DownloadAsync(Uri uri)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var responseMessage = await client.GetAsync(uri);
                    return await responseMessage.Content.ReadAsStreamAsync();
                }
                catch (Exception e)
                {
                    _logger.Log(e.Message);
                }
            }

            return null;
        }
    }
}
