namespace Zinnia.Rule
{
    using UnityEngine;
    using Zinnia.Extension;
    using Zinnia.Rule.Collection;

    /// <summary>
    /// Determines whether all <see cref="IRule"/>s in a list are accepting an object.
    /// </summary>
    public class AllRule : Rule
    {
        [Tooltip("The IRules to check against.")]
        [SerializeField]
        private RuleContainerObservableList _rules;
        /// <summary>
        /// The <see cref="IRule"/>s to check against.
        /// </summary>
        public RuleContainerObservableList Rules
        {
            get
            {
                return _rules;
            }
            set
            {
                _rules = value;
            }
        }

        /// <inheritdoc/>
        public override bool Accepts(object target)
        {
            if (ShouldAutoRejectDueToState() || Rules == null || Rules.NonSubscribableElements.Count == 0)
            {
                return false;
            }

            foreach (RuleContainer rule in Rules.NonSubscribableElements)
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
