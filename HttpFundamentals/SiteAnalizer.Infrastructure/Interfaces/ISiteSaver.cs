using System.IO;

namespace SiteAnalizer.Infrastructure.Interfaces
{
    /// <summary>
    /// Represents an <see cref="ISiteSaver"/> interface.
    /// </summary>
    public interface ISiteSaver
    {
        /// <summary>
        /// Save sites content.
        /// </summary>
        /// <param name="contentStream">The content stream.</param>
        void Save(Stream contentStream);
    }
}
