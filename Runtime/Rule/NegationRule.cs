namespace Zinnia.Rule
{
    using UnityEngine;
    using Zinnia.Extension;

    /// <summary>
    /// Negates the acceptance of an object based on the acceptance of another <see cref="IRule"/>.
    /// </summary>
    public class NegationRule : Rule
    {
        [Tooltip("The IRule to negate.")]
        [SerializeField]
        private RuleContainer _rule;
        /// <summary>
        /// The <see cref="IRule"/> to negate.
        /// </summary>
        public RuleContainer Rule
        {
            get
            {
                return _rule;
            }
            set
            {
                _rule = value;
            }
        }

        /// <inheritdoc />
        public override bool Accepts(object target)
        {
            if (ShouldAutoRejectDueToState())
            {
                return false;
            }

            return !Rule.Accepts(target);
        }
    }
}