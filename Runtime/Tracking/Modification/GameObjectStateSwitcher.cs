namespace Zinnia.Tracking.Modification
{
    using UnityEngine;
    using Zinnia.Data.Collection.List;
    using Zinnia.Extension;

    /// <summary>
    /// Sets the state of the current target to the specified active state.
    /// </summary>
    public class GameObjectStateSwitcher : MonoBehaviour
    {
        [Tooltip("A collection of targets to set the state on when it is the active index.")]
        [SerializeField]
        private GameObjectObservableList targets;
        /// <summary>
        /// A collection of targets to set the state on when it is the active index.
        /// </summary>
        public GameObjectObservableList Targets
        {
            get
            {
                return targets;
            }
            set
            {
                targets = value;
            }
        }
        [Tooltip("The state to set the active index target. All other targets will be set to the opposite state.")]
        [SerializeField]
        private bool targetState = true;
        /// <summary>
        /// The state to set the active index target. All other targets will be set to the opposite state.
        /// </summary>
        public bool TargetState
        {
            get
            {
                return targetState;
            }
            set
            {
                targetState = value;
            }
        }

        /// <summary>
        /// Switches to the next target in the collection and sets to the appropriate state.
        /// </summary>
        public virtual void SwitchNext()
        {
            if (!this.IsValidState())
            {
                return;
            }

            Targets.CurrentIndex++;
            if (Targets.CurrentIndex >= Targets.NonSubscribableElements.Count)
            {
                Targets.CurrentIndex = 0;
            }

            Switch();
        }

        /// <summary>
        /// Switches to the previous target in the collection and sets to the appropriate state.
        /// </summary>
        public virtual void SwitchPrevious()
        {
            if (!this.IsValidState())
            {
                return;
            }

            Targets.CurrentIndex--;
            if (Targets.CurrentIndex < 0)
            {
                Targets.CurrentIndex = Targets.NonSubscribableElements.Count - 1;
            }

            Switch();
        }

        /// <summary>
        /// Switches to the a specific target in the collection and sets to the appropriate state.
        /// </summary>
        /// <param name="index">The index of the collection to switch to.</param>
        public virtual void SwitchTo(int index)
        {
            if (!this.IsValidState())
            {
                return;
            }

            Targets.CurrentIndex = Mathf.Clamp(index, 0, Targets.NonSubscribableElements.Count - 1);
            Switch();
        }

        /// <summary>
        /// Switches to the target at the <see cref="CurrentIndex"/> in the collection and sets to the appropriate state.
        /// </summary>
        public virtual void SwitchToCurrentIndex()
        {
            if (!this.IsValidState())
            {
                return;
            }

            SwitchTo(Targets.CurrentIndex);
        }

        /// <summary>
        /// Switches the current active target state.
        /// </summary>
        protected virtual void Switch()
        {
            for (int index = 0; index < Targets.NonSubscribableElements.Count; index++)
            {
                Targets.NonSubscribableElements[index].SetActive(index == Targets.CurrentIndex ? TargetState : !TargetState);
            }
        }
    }
}