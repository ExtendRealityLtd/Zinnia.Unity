namespace Zinnia.Action
{
    using Malimbe.BehaviourStateRequirementMethod;
    using Malimbe.MemberChangeMethod;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;
    using Zinnia.Action.Collection;

    /// <summary>
    /// Emits a <see cref="bool"/> value when any given actions are in their active state.
    /// </summary>
    public class AnyAction : BooleanAction
    {
        /// <summary>
        /// <see cref="Action"/>s to check the active state on.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public ActionObservableList Actions { get; set; }

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
                action.ActivationStateChanged.RemoveListener(OnActionActivationStateChanged);
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

            bool areAllActionsActivated = DefaultValue;
            foreach (Action action in Actions.SubscribableElements)
            {
                if (action.IsActivated)
                {
                    areAllActionsActivated = !DefaultValue;
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
        [RequiresBehaviourState]
        protected virtual void OnActionActivationStateChanged(bool isActionActivated)
        {
            if (IsActivated && !isActionActivated)
            {
                CheckAllActions();
            }
            else if (!IsActivated && isActionActivated)
            {
                Receive(!DefaultValue);
            }
        }

        /// <summary>
        /// Called after an element is added to <see cref="Actions"/>.
        /// </summary>
        /// <param name="action">The element added to the collection.</param>
        [RequiresBehaviourState]
        protected virtual void OnActionAdded(Action action)
        {
            if (action == null)
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
        [RequiresBehaviourState]
        protected virtual void OnActionRemoved(Action action)
        {
            if (action != null)
            {
                action.ActivationStateChanged.RemoveListener(OnActionActivationStateChanged);
            }

            CheckAllActions();
        }

        /// <summary>
        /// Called before <see cref="Actions"/> has been changed.
        /// </summary>
        [CalledBeforeChangeOf(nameof(Actions))]
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
        [CalledAfterChangeOf(nameof(Actions))]
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