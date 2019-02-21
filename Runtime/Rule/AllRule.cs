namespace Zinnia.Rule
{
    using UnityEngine;
    using System.Collections.Generic;
    using Malimbe.XmlDocumentationAttribute;
    using Zinnia.Extension;

    /// <summary>
    /// Determines whether all <see cref="IRule"/>s in a list are accepting an object.
    /// </summary>
    public class AllRule : MonoBehaviour, IRule
    {
        /// <summary>
        /// The <see cref="IRule"/>s to check against.
        /// </summary>
        [DocumentedByXml]
        public List<RuleContainer> rules = new List<RuleContainer>();

        /// <inheritdoc/>
        public bool Accepts(object target)
        {
            if (rules == null || rules.Count == 0)
            {
                return false;
            }

            foreach (RuleContainer rule in rules)
            {
                if (!rule.Accepts(target))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
