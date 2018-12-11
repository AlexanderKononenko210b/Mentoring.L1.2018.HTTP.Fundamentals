using System.IO;
using System.Linq;
using SiteAnalyzer.Infrastructure.Interfaces;

namespace SiteAnalyzer
{
    /// <summary>
    /// Represents a <see cref="SiteSaver"/> class.
    /// </summary>
    public class SiteSaver : ISiteSaver
    {
        private readonly string _baseDirectory;
        private readonly ILogger _logger;

        /// <summary>
        /// Initialize a <see cref="SiteSaver"/> class.
        /// </summary>
        /// <param name="baseDirectory">The base directory.</param>
        /// <param name="logger">The logger.</param>
        public SiteSaver(string baseDirectory, Logger logger)
        {
            _baseDirectory = baseDirectory;
            this.CreateDirectory(_baseDirectory);
            _logger = logger;
        }

        /// <inheritdoc/>
        public void Save(Stream contentStream, string fileWithExtensionName, int currentLevel)
        {
            var directoryPath = Path.Combine(_baseDirectory, $"Level{currentLevel}");
            CreateDirectory(directoryPath);

            var filePath = Path.Combine(directoryPath, GetValidPathName(fileWithExtensionName));
            if (!File.Exists(filePath))
            {
                SaveToFile(filePath, contentStream);
                _logger.Log($"Save content from: {filePath}");
            }
        }

        /// <summary>
        /// Create directory for save content.
        /// </summary>
        private void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        /// <summary>
        /// Get valid file name.
        /// </summary>
        /// <param name="inputFileName">The input file name.</param>
        /// <returns>The valid file name.</returns>
        private string GetValidPathName(string inputFileName) => 
            string.Concat(inputFileName.Where(simbol => Path.GetInvalidFileNameChars().All(invalidChar => invalidChar != simbol)));

        /// <summary>
        /// Save content to file.
        /// </summary>
        /// <param name="path">The path for save.</param>
        /// <param name="content">The content.</param>
        private void SaveToFile(string path, Stream content)
        {
            using (var fileStream = new FileStream(path, FileMode.OpenOrCreate))
            {
                content.Seek(0, SeekOrigin.Begin);
                content.CopyTo(fileStream);
            }
        }
    }
}
