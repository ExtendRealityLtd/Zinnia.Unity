namespace Zinnia.Rule
{
    using UnityEngine;
    using Zinnia.Action;

    /// <summary>
    /// Determines whether an action is activated or deactivated.
    /// </summary>
    public class ActionRule : Rule
    {
        [Tooltip("The Action to check.")]
        [SerializeField]
        private Action action;
        /// <summary>
        /// The <see cref="Action"/> to check.
        /// </summary>
        public Action Action
        {
            get
            {
                return action;
            }
            set
            {
                action = value;
            }
        }

        /// <inheritdoc />
        public override bool Accepts(object _ = null)
        {
            return Accepts(Action);
        }

        /// <summary>
        /// Determines whether the given <see cref="Action"/> is activated.
        /// </summary>
        /// <param name="action">The action to check.</param>
        /// <returns>Whther the action is activated.</returns>
        public virtual bool Accepts(Action action)
        {
            if (ShouldAutoRejectDueToState())
            {
                return false;
            }

            return action.IsActivated;
        }
    }
}