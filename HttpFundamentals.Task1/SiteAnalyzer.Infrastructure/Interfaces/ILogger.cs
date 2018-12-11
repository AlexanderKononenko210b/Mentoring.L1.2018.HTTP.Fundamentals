using System;
using System.Collections.Generic;

namespace SiteAnalyzer.Infrastructure.Interfaces
{
    /// <summary>
    /// Represents an <see cref="ILogger"/> interface.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Log work.
        /// </summary>
        /// <param name="message">The message.</param>
        void Log(string message);

        /// <summary>
        /// Log <see cref="IEnumerable{Uri}"/> info.
        /// </summary>
        /// <param name="uries"></param>
        void Log(IEnumerable<Uri> uries);
    }
}
