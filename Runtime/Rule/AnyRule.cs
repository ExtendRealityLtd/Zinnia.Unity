namespace Zinnia.Rule
{
    using UnityEngine;
    using System.Collections.Generic;
    using System.Linq;
    using Malimbe.XmlDocumentationAttribute;
    using Zinnia.Extension;

    /// <summary>
    /// Determines whether any <see cref="IRule"/> in a list is accepting an object.
    /// </summary>
    public class AnyRule : MonoBehaviour, IRule
    {
        /// <summary>
        /// The <see cref="IRule"/>s to check against.
        /// </summary>
        [DocumentedByXml]
        public List<RuleContainer> rules = new List<RuleContainer>();

        /// <inheritdoc />
        public bool Accepts(object target)
        {
            return rules.EmptyIfNull().Any(rule => rule.Accepts(target));
        }
    }
}