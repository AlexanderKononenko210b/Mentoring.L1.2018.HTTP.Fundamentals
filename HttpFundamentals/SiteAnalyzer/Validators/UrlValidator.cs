using System;
using System.Collections.Generic;
using System.Linq;
using SiteAnalizer.Infrastructure.Interfaces;

namespace SiteAnalyzer.Validators
{
    /// <summary>
    /// Represents a <see cref="UrlValidator"/> class.
    /// </summary>
    public class UrlValidator : IValidator
    {
        private readonly IEnumerable<IConstraintRule> _constraintRules;

        public UrlValidator(IEnumerable<IConstraintRule> constraintRules)
        {
            _constraintRules = constraintRules;
        }

        /// <inheritdoc/>
        public bool IsValid(Uri uri)
        {
            return _constraintRules.All(rule => rule.IsValid(uri));
        }

        /// <inheritdoc/>
        public bool IsValid(string uri)
        {
            return _constraintRules.All(rule => rule.IsValid(uri));
        }
    }
}
