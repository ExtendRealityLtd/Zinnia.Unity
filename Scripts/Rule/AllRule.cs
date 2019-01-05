namespace Zinnia.Rule
{
    using UnityEngine;
    using System.Collections.Generic;
    using System.Linq;
    using Zinnia.Extension;

    /// <summary>
    /// Determines whether all <see cref="IRule"/>s in a list are accepting an object.
    /// </summary>
    public class AllRule : MonoBehaviour, IRule
    {
        /// <summary>
        /// The <see cref="IRule"/>s to check against.
        /// </summary>
        public List<RuleContainer> rules = new List<RuleContainer>();

        /// <inheritdoc/>
        public bool Accepts(object target)
        {
            if (rules == null || rules.Count == 0)
            {
                return false;
            }

            return rules.All(rule => rule.Accepts(target));
        }
    }
}
