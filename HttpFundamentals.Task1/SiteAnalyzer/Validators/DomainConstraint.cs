using System;
using SiteAnalyzer.Infrastructure.Enums;
using SiteAnalyzer.Infrastructure.Interfaces;

namespace SiteAnalyzer.Validators
{
    /// <summary>
    /// Represents a <see cref="DomainConstraint"/> class.
    /// </summary>
    public class DomainConstraint : IConstraintRule
    {
        private readonly Uri _baseUri;
        private readonly DomainRestriction _typeConstraint;

        /// <summary>
        /// Initialize a new <see cref="DomainConstraint"/> instance.
        /// </summary>
        /// <param name="baseUri"></param>
        /// <param name="typeConstraint"></param>
        public DomainConstraint(Uri baseUri, DomainRestriction typeConstraint)
        {
            _baseUri = baseUri;
            _typeConstraint = typeConstraint;
        }

        ///<inheritdoc/>
        public bool IsValid(Uri uri)
        {
            switch (_typeConstraint)
            {
                case DomainRestriction.AllDomain:
                {
                    return true;
                }
                case DomainRestriction.InsideCurrentDomain:
                {
                    return _baseUri.DnsSafeHost == uri.DnsSafeHost;
                }
                case DomainRestriction.NotHigherSourceUrl:
                {
                    return _baseUri.IsBaseOf(uri);
                }
                default:
                {
                    throw new ArgumentException(nameof(_typeConstraint));
                }
            }
        }

        ///<inheritdoc/>
        public bool IsValid(string url)
        {
            return IsValid(new Uri(url));
        }
    }
}
