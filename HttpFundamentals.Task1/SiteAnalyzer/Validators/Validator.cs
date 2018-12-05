using System;
using System.Collections.Generic;
using System.Linq;
using SiteAnalizer.Infrastructure.Interfaces;

namespace SiteAnalyzer.Validators
{
    /// <summary>
    /// Represents a <see cref="Validator"/> class.
    /// </summary>
    public class Validator : IValidator
    {
        private readonly IEnumerable<IConstraintRule> _constraintRules;

        /// <summary>
        /// Initialize a <see cref="Validator"/> class.
        /// </summary>
        /// <param name="constraintRules"></param>
        public Validator(IEnumerable<IConstraintRule> constraintRules)
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
