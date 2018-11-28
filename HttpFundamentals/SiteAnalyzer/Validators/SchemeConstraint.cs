using SiteAnalizer.Infrastructure.Interfaces;
using System;

namespace SiteAnalyzer.Validators
{
    /// <summary>
    /// Represents a <see cref="SchemeConstraint"/> class.
    /// </summary>
    public class SchemeConstraint : IConstraintRule
    {
        /// <inheritdoc/>
        public bool IsValid(Uri uri)
        {
            return uri.Scheme.Equals("http") || uri.Scheme.Equals("https");
        }

        /// <inheritdoc/>
        public bool IsValid(string url) => url.StartsWith("http") | url.StartsWith("https") &&
                                              CountSubstringInUrl(url, "http") == 1 |
                                               CountSubstringInUrl(url, "https") == 1;

        /// <summary>
        /// Count substring in string
        /// </summary>
        /// <param name="inputString">string</param>
        /// <param name="substring">substring for count</param>
        /// <returns>count substring in string</returns>
        private static int CountSubstringInUrl(string inputString, string substring)
        {
            int count = (inputString.Length - inputString.Replace(substring, "").Length) / substring.Length;
            return count;
        }
    }
}
