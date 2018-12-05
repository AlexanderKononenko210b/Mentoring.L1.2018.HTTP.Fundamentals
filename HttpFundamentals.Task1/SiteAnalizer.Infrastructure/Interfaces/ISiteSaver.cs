using System.IO;

namespace SiteAnalizer.Infrastructure.Interfaces
{
    /// <summary>
    /// Represents an <see cref="ISiteSaver"/> interface.
    /// </summary>
    public interface ISiteSaver
    {
        /// <summary>
        /// Save sites content as file.
        /// </summary>
        /// <param name="contentStream">The content stream.</param>
        /// <param name="nameFileWithExtension">The name file with extension.</param>
        /// <param name="currentLevel">The current level.</param>
        void Save(Stream contentStream, string nameFileWithExtension, int currentLevel);
    }
}
