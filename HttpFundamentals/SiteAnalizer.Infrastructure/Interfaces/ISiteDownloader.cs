using System;
using System.IO;
using System.Threading.Tasks;

namespace SiteAnalizer.Infrastructure.Interfaces
{
    /// <summary>
    /// Represents an <see cref="ISiteDownloader"/> interface.
    /// </summary>
    public interface ISiteDownloader
    {
        /// <summary>
        /// Download site from uri.
        /// </summary>
        /// <param name="uri">The uri.</param>
        /// <param name="currentLevel">The current level.</param>
        /// <returns>Site content stream.</returns>
        Task<Stream> DownloadAsync(Uri uri, int currentLevel);
    }
}
