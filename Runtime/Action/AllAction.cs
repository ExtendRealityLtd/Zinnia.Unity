namespace Zinnia.Action
{
    using UnityEngine;
    using Zinnia.Action.Collection;
    using Zinnia.Extension;

    /// <summary>
    /// Emits a <see cref="bool"/> value when all given <see cref="Action"/>s are <see cref="Action.IsActivated"/>.
    /// </summary>
    public class AllAction : BooleanAction
    {
        [Tooltip("Actions to check the active state on.")]
        [SerializeField]
        private ActionObservableList actions;
        /// <summary>
        /// <see cref="Action"/>s to check the active state on.
        /// </summary>
        public ActionObservableList Actions
        {
            get
            {
                return actions;
            }
            set
            {
                if (this.IsMemberChangeAllowed())
                {
                    OnBeforeActionsChange();
                }
                actions = value;
                if (this.IsMemberChangeAllowed())
                {
                    OnAfterActionsChange();
                }
            }
        }

        protected override void OnEnable()
        {
            if (Actions == null)
            {
                return;
            }

            AddActionsListeners();
            CheckAllActions();
        }

        protected override void OnDisable()
        {
            if (Actions == null)
            {
                return;
            }

            RemoveActionsListeners();
        }

        /// <summary>
        /// Subscribes to events of <see cref="Actions"/>.
        /// </summary>
        protected virtual void AddActionsListeners()
        {
            Actions.Added.AddListener(OnActionAdded);
            Actions.Removed.AddListener(OnActionRemoved);

            foreach (Action action in Actions.SubscribableElements)
            {
                action.ActivationStateChanged.AddListener(OnActionActivationStateChanged);
            }
        }

        /// <summary>
        /// Unsubscribes from events of <see cref="Actions"/>.
        /// </summary>
        protected virtual void RemoveActionsListeners()
        {
            Actions.Added.RemoveListener(OnActionAdded);
            Actions.Removed.RemoveListener(OnActionRemoved);

            foreach (Action action in Actions.SubscribableElements)
            {
                if (action != null)
                {
                    action.ActivationStateChanged.RemoveListener(OnActionActivationStateChanged);
                }
            }
        }

        /// <summary>
        /// Checks whether all <see cref="Actions"/> are <see cref="Action.IsActivated"/> and calls <see cref="Action{TSelf,TValue,TEvent}.Receive"/> on this instance to update its own activation state if necessary.
        /// </summary>
        protected virtual void CheckAllActions()
        {
            if (Actions == null)
            {
                if (IsActivated)
                {
                    Receive(DefaultValue);
                }

                return;
            }

            bool areAllActionsActivated = Actions.SubscribableElements.Count > 0 != DefaultValue;
            foreach (Action action in Actions.SubscribableElements)
            {
                if (!action.IsActivated)
                {
                    areAllActionsActivated = DefaultValue;
                    break;
                }
            }

            if (areAllActionsActivated != IsActivated)
            {
                Receive(areAllActionsActivated);
            }
        }

        /// <summary>
        /// Called after the <see cref="Action.IsActivated"/> state of any element in <see cref="Actions"/> changes.
        /// </summary>
        /// <param name="isActionActivated">Whether the action is activated.</param>
        protected virtual void OnActionActivationStateChanged(bool isActionActivated)
        {
            if (!this.IsValidState())
            {
                return;
            }

            if (IsActivated && !isActionActivated)
            {
                Receive(DefaultValue);
            }
            else if (!IsActivated && isActionActivated)
            {
                CheckAllActions();
            }
        }

        /// <summary>
        /// Called after an element is added to <see cref="Actions"/>.
        /// </summary>
        /// <param name="action">The element added to the collection.</param>
        protected virtual void OnActionAdded(Action action)
        {
            if (!this.IsValidState() || action == null)
            {
                return;
            }

            OnActionActivationStateChanged(action.IsActivated);
            action.ActivationStateChanged.AddListener(OnActionActivationStateChanged);
        }

        /// <summary>
        /// Called after an element is removed from <see cref="Actions"/>.
        /// </summary>
        /// <param name="action">The element removed from the collection.</param>
        protected virtual void OnActionRemoved(Action action)
        {
            if (!this.IsValidState() || action != null)
            {
                action.ActivationStateChanged.RemoveListener(OnActionActivationStateChanged);
            }

            CheckAllActions();
        }

        /// <summary>
        /// Called before <see cref="Actions"/> has been changed.
        /// </summary>
        protected virtual void OnBeforeActionsChange()
        {
            if (Actions != null)
            {
                RemoveActionsListeners();
            }
        }

        /// <summary>
        /// Called after <see cref="Actions"/> has been changed.
        /// </summary>
        protected virtual void OnAfterActionsChange()
        {
            if (Actions != null)
            {
                AddActionsListeners();
            }

            CheckAllActions();
        }
    }
}