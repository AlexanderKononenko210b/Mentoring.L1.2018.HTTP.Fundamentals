using System;
using System.Collections.Generic;
using SiteAnalizer.Infrastructure.Interfaces;

namespace SiteAnalyzer
{
    /// <summary>
    /// Represents a <see cref="Logger"/> class.
    /// </summary>
    public class Logger : ILogger
    {
        /// <inheritdoc/>
        public void Log(string message)
        {
            Console.WriteLine(message);
        }

        /// <inheritdoc/>
        public void Log(IEnumerable<Uri> uries)
        {
            foreach (var uri in uries)
            {
                Console.WriteLine(uri.OriginalString);
            }
        }
    }
}
