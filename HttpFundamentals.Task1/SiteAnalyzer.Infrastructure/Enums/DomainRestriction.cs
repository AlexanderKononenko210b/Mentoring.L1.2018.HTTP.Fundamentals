namespace SiteAnalyzer.Infrastructure.Enums
{
    /// <summary>
    /// Domain restriction.
    /// </summary>
    public enum DomainRestriction
    {
        /// <summary>
        /// All domains.
        /// </summary>
        AllDomain = 1,

        /// <summary>
        /// Inside current domain.
        /// </summary>
        InsideCurrentDomain = 2,

        /// <summary>
        /// Not higher source url.
        /// </summary>
        NotHigherSourceUrl = 3
    }
}
