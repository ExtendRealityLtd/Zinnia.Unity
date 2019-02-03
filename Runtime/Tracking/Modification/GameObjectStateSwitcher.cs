namespace Zinnia.Tracking.Modification
{
    using UnityEngine;
    using System.Collections.Generic;
    using Malimbe.XmlDocumentationAttribute;

    /// <summary>
    /// Sets the state of the current target to the specified active state.
    /// </summary>
    public class GameObjectStateSwitcher : MonoBehaviour
    {
        /// <summary>
        /// A collection of targets to set the state on when it is the active index.
        /// </summary>
        [DocumentedByXml]
        public List<GameObject> targets = new List<GameObject>();
        /// <summary>
        /// The state to set the active index target. All other targets will be set to the opposite state.
        /// </summary>
        [DocumentedByXml]
        public bool targetState = true;
        /// <summary>
        /// Determines if to execute a switch when the component is enabled.
        /// </summary>
        [DocumentedByXml]
        public bool switchOnEnable = true;
        /// <summary>
        /// The index in the collection to start at.
        /// </summary>
        [DocumentedByXml]
        public int startIndex;

        /// <summary>
        /// The current active index in the targets collection.
        /// </summary>
        protected int activeIndex;

        /// <summary>
        /// Switches to the next target in the collection and sets to the appropriate state.
        /// </summary>
        public virtual void SwitchNext()
        {
            if (!isActiveAndEnabled)
            {
                return;
            }

            activeIndex++;
            if (activeIndex >= targets.Count)
            {
                activeIndex = 0;
            }

            Switch();
        }

        /// <summary>
        /// Switches to the previous target in the collection and sets to the appropriate state.
        /// </summary>
        public virtual void SwitchPrevious()
        {
            if (!isActiveAndEnabled)
            {
                return;
            }

            activeIndex--;
            if (activeIndex < 0)
            {
                activeIndex = targets.Count - 1;
            }

            Switch();
        }

        /// <summary>
        /// Switches to the a specific target in the collection and sets to the appropriate state.
        /// </summary>
        /// <param name="index">The index of the collection to switch to.</param>
        public virtual void SwitchTo(int index)
        {
            if (!isActiveAndEnabled)
            {
                return;
            }

            activeIndex = Mathf.Clamp(index, 0, targets.Count - 1);
            Switch();
        }

        protected virtual void OnEnable()
        {
            activeIndex = startIndex;
            if (switchOnEnable)
            {
                SwitchTo(startIndex);
            }
        }

        /// <summary>
        /// Switches the current active target state.
        /// </summary>
        protected virtual void Switch()
        {
            if (!isActiveAndEnabled)
            {
                return;
            }

            for (int index = 0; index < targets.Count; index++)
            {
                targets[index].SetActive(index == activeIndex ? targetState : !targetState);
            }
        }
    }
}