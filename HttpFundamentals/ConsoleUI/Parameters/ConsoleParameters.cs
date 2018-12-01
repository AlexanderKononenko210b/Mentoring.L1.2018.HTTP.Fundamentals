using CommandLine;
using SiteAnalizer.Infrastructure.Enums;

namespace ConsoleUI.Parameters
{
    /// <summary>
    /// Represents a <see cref="ConsoleParameters"/> class.
    /// </summary>
    public class ConsoleParameters
    {
        /// <summary>
        /// Base Url.
        /// </summary>
        [Option('u', "BaseUrl", DefaultValue = null, HelpText = "Base Url starts as http or https", Required = true)]
        public string BaseUrl { get; set; }

        /// <summary>
        /// Max deep level analyze.
        /// </summary>
        [Option('d', "MaxDeepLevel", DefaultValue = 1, HelpText = "Maximum depth of analysis - type int, minimum value - 1")]
        public int MaxDeepLevel { get; set; }

        /// <summary>
        /// Domain restriction.
        /// </summary>
        [Option('r', "DomainRestriction", DefaultValue = 1, HelpText = "DomainRestriction (AllDomain = 1, InsideCurrentDomain = 2, NotHigherSourceUrl = 3)")]
        public DomainRestriction DomainRestriction { get; set; }

        /// <summary>
        /// Content extensions.
        /// </summary>
        [Option ('e', "ContentExtensions", DefaultValue = "gif,jpeg,jpg,pdf", HelpText = "ContentExtensions - type string array (example: gif,jpeg,jpg,pdf)")]
        public string ContentExtensions { get; set; }
    }
}
