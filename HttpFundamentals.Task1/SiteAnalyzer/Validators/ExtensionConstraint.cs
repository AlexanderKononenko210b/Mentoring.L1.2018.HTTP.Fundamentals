using SiteAnalyzer.Infrastructure.Interfaces;
using System;
using System.Linq;

namespace SiteAnalyzer.Validators
{
    /// <summary>
    /// Represents a <see cref="ExtensionConstraint"/> class.
    /// </summary>
    public class ExtensionConstraint : IConstraintRule
    {
        private readonly string[] _extensions;

        public ExtensionConstraint(string extensions)
        {
            _extensions = this.GetExtensions(extensions);
        }

        ///<inheritdoc/>
        public bool IsValid(Uri uri)
        {
            var currentExtension = GetCurrentExtension(uri);
            return currentExtension != null && _extensions.Any(ext => ext.Equals(currentExtension, StringComparison.InvariantCulture));
        }

        ///<inheritdoc/>
        public bool IsValid(string url)
        {
            return IsValid(new Uri(url));
        }
        
        /// <summary>
        /// Get array file extensions from string.
        /// </summary>
        /// <param name="inputExtensions">The file extensions as string.</param>
        /// <returns></returns>
        private string[] GetExtensions(string inputExtensions)
        {
            return inputExtensions.Split(',');
        }

        /// <summary>
        /// Get current files extension.
        /// </summary>
        /// <param name="uri">The uri.</param>
        /// <returns></returns>
        private string GetCurrentExtension(Uri uri)
        {
            var lastSegment = uri.Segments.Last();
            var index = lastSegment.LastIndexOf('.');

            return index != -1 && lastSegment.Length != index + 1 ? lastSegment.Substring(index + 1) : null;
        }
    }
}
