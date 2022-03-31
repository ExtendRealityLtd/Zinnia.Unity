namespace Zinnia.Association
{
    using UnityEngine;
    using Zinnia.Extension;
    using Zinnia.Rule;

    /// <summary>
    /// Holds a <see cref="GameObject"/> collection to activate and/or deactivate based on the given <see cref="RuleContainer"/>.
    /// </summary>
    public class RuleAssociation : GameObjectsAssociation
    {
        /// <summary>
        /// The <see cref="RuleContainer"/> to match the association with.
        /// </summary>
        [Tooltip("The RuleContainer to match the association with.")]
        [SerializeField]
        private RuleContainer _rule;
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

        /// <inheritdoc/>
        public override bool ShouldBeActive()
        {
            return Rule.Accepts(null);
        }
    }
}