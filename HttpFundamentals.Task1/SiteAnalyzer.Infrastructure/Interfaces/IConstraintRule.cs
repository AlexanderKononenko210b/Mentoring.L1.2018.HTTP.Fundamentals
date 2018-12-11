using System;

namespace SiteAnalyzer.Infrastructure.Interfaces
{
    /// <summary>
    /// Represents an <see cref="IConstraintRule"/> interface.
    /// </summary>
    public interface IConstraintRule
    {
        /// <summary>
        /// Check uri.
        /// </summary>
        /// <param name="uri">The uri</param>
        /// <returns>True if uri corresponds to rule otherwise false.</returns>
        bool IsValid(Uri uri);

        /// <summary>
        /// Check url as string.
        /// </summary>
        /// <param name="url">The url.</param>
        /// <returns>True if url corresponds to rule otherwise false.</returns>
        bool IsValid(string url);
    }
}
