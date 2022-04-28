namespace Zinnia.Rule
{
    using UnityEngine;
    using Zinnia.Extension;
    using Zinnia.Rule.Collection;

    /// <summary>
    /// Determines whether any <see cref="IRule"/> in a list is accepting an object.
    /// </summary>
    public class AnyRule : Rule
    {
        [Tooltip("The IRules to check against.")]
        [SerializeField]
        private RuleContainerObservableList rules;
        /// <summary>
        /// The <see cref="IRule"/>s to check against.
        /// </summary>
        public RuleContainerObservableList Rules
        {
            get
            {
                return rules;
            }
            set
            {
                rules = value;
            }
        }

        /// <inheritdoc />
        public override bool Accepts(object target)
        {
            if (ShouldAutoRejectDueToState() || Rules == null)
            {
                return false;
            }

            foreach (RuleContainer rule in Rules.NonSubscribableElements)
            {
                if (rule.Accepts(target))
                {
                    return true;
                }
            }

            return false;
        }
    }
}