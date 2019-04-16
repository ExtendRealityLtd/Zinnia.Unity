namespace Zinnia.Tracking.Modification
{
    using UnityEngine;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.BehaviourStateRequirementMethod;
    using Zinnia.Data.Collection.List;

    /// <summary>
    /// Sets the state of the current target to the specified active state.
    /// </summary>
    public class GameObjectStateSwitcher : MonoBehaviour
    {
        /// <summary>
        /// A collection of targets to set the state on when it is the active index.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public GameObjectObservableList Targets { get; set; }
        /// <summary>
        /// The state to set the active index target. All other targets will be set to the opposite state.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public bool TargetState { get; set; } = true;

        /// <summary>
        /// Switches to the next target in the collection and sets to the appropriate state.
        /// </summary>
        [RequiresBehaviourState]
        public virtual void SwitchNext()
        {
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
        [RequiresBehaviourState]
        public virtual void SwitchPrevious()
        {
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
        [RequiresBehaviourState]
        public virtual void SwitchTo(int index)
        {
            Targets.CurrentIndex = Mathf.Clamp(index, 0, Targets.NonSubscribableElements.Count - 1);
            Switch();
        }

        /// <summary>
        /// Switches to the target at the <see cref="CurrentIndex"/> in the collection and sets to the appropriate state.
        /// </summary>
        [RequiresBehaviourState]
        public virtual void SwitchToCurrentIndex()
        {
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