namespace Zinnia.Tracking.Modification
{
    using UnityEngine;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.BehaviourStateRequirementMethod;
    using Zinnia.Data.Collection;

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
        /// The index in the collection to start at.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public int StartIndex { get; set; }

        /// <summary>
        /// The current active index in the targets collection.
        /// </summary>
        public int CurrentIndex { get; protected set; }

        /// <summary>
        /// Switches to the next target in the collection and sets to the appropriate state.
        /// </summary>
        [RequiresBehaviourState]
        public virtual void SwitchNext()
        {
            CurrentIndex++;
            if (CurrentIndex >= Targets.ReadOnlyElements.Count)
            {
                CurrentIndex = 0;
            }

            Switch();
        }

        /// <summary>
        /// Switches to the previous target in the collection and sets to the appropriate state.
        /// </summary>
        [RequiresBehaviourState]
        public virtual void SwitchPrevious()
        {
            CurrentIndex--;
            if (CurrentIndex < 0)
            {
                CurrentIndex = Targets.ReadOnlyElements.Count - 1;
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
            CurrentIndex = Mathf.Clamp(index, 0, Targets.ReadOnlyElements.Count - 1);
            Switch();
        }

        /// <summary>
        /// Switches to the target at the <see cref="StartIndex"/> in the collection and sets to the appropriate state.
        /// </summary>
        [RequiresBehaviourState]
        public virtual void SwitchToStartIndex()
        {
            SwitchTo(StartIndex);
        }

        protected virtual void OnEnable()
        {
            CurrentIndex = StartIndex;
        }

        /// <summary>
        /// Switches the current active target state.
        /// </summary>
        protected virtual void Switch()
        {
            for (int index = 0; index < Targets.ReadOnlyElements.Count; index++)
            {
                Targets.ReadOnlyElements[index].SetActive(index == CurrentIndex ? TargetState : !TargetState);
            }
        }
    }
}