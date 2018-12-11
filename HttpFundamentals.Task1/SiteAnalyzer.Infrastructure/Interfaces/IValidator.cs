using System;

namespace SiteAnalyzer.Infrastructure.Interfaces
{
    /// <summary>
    /// Represents an <see cref="IValidator"/> interface.
    /// </summary>
    public interface IValidator
    {
        /// <summary>
        /// Check uri.
        /// </summary>
        /// <param name="uri">The uri.</param>
        /// <returns>True if uri is corresponds to all rules otherwise false.</returns>
        bool IsValid(Uri uri);

        /// <summary>
        /// Check url as string.
        /// </summary>
        /// <param name="url">The url.</param>
        /// <returns>True if url is corresponds to all rules otherwise false.</returns>
        bool IsValid(string url);
    }
}
