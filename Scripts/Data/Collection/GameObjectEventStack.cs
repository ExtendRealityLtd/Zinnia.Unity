namespace VRTK.Core.Data.Collection
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Emits the events at the current stack index every time a new GameObject is added to or removed from the stack.
    /// </summary>
    public class GameObjectEventStack : MonoBehaviour
    {
        /// <summary>
        /// Defines the event for the output <see cref="GameObject"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<GameObject>
        {
        }

        /// <summary>
        /// A collection of events to emit when a new <see cref="GameObject"/> is added or removed from the stack.
        /// </summary>
        [Serializable]
        public class ElementEvents
        {
            /// <summary>
            /// Emitted when a new <see cref="GameObject"/> is added to the end of the stack.
            /// </summary>
            public UnityEvent Pushed = new UnityEvent();
            /// <summary>
            /// Emitted when the <see cref="GameObject"/> is removed from the end of the stack.
            /// </summary>
            public UnityEvent Popped = new UnityEvent();
            /// <summary>
            /// Emitted when the <see cref="GameObject"/> is removed from the stack due to a <see cref="GameObject"/> lower down in the stack being removed.
            /// </summary>
            public UnityEvent ForcePopped = new UnityEvent();
        }

        /// <summary>
        /// The events to emit for the <see cref="GameObject"/> that is added to the same index within the stack.
        /// </summary>
        [Tooltip("The events to emit for the GameObject that is added to the same index within the stack.")]
        public List<ElementEvents> elementEvents = new List<ElementEvents>();

        /// <summary>
        /// The current index the events to emit is at in relation to the <see cref="GameObject"/> stack.
        /// </summary>
        public int EventIndex
        {
            get;
            protected set;
        } = 0;

        /// <summary>
        /// A representation of the <see cref="GameObject"/> stack.
        /// </summary>
        protected List<GameObject> stack = new List<GameObject>();

        /// <summary>
        /// Push an element onto the stack and emit the related events.
        /// </summary>
        /// <param name="element">The element to push onto the stack and to become the payload of the related event.</param>
        public virtual void Push(GameObject element)
        {
            if (!isActiveAndEnabled || EventIndex >= elementEvents.Count || stack.Contains(element))
            {
                return;
            }

            elementEvents[EventIndex].Pushed?.Invoke(element);
            stack.Add(element);
            EventIndex++;
        }

        /// <summary>
        /// Pops the last element off the stack and emit the related events.
        /// </summary>
        public virtual void Pop()
        {
            if (stack.Count > 0)
            {
                PopAt(stack[stack.Count - 1]);
            }
        }

        /// <summary>
        /// Pops from the stack at the given stack index.
        /// </summary>
        /// <param name="index">The index at which to pop the stack at.</param>
        public virtual void PopAt(int index)
        {
            if (index < stack.Count && index <= EventIndex)
            {
                PopAt(stack[index]);
            }
        }

        /// <summary>
        /// Pops the given element from the stack and subsequently remove any elements that are above the given element in the stack and emit the related events.
        /// </summary>
        /// <param name="element">The element to pop from the stack.</param>
        public virtual void PopAt(GameObject element)
        {
            int elementIndex = stack.IndexOf(element);
            if (!isActiveAndEnabled || elementIndex < 0)
            {
                return;
            }

            for (int i = elementEvents.Count - 1; i >= elementIndex; i--)
            {
                if (elementIndex < stack.Count && i < stack.Count)
                {
                    GameObject currentElement = stack[i];
                    if (i == elementIndex)
                    {
                        elementEvents[i].Popped?.Invoke(currentElement);
                    }
                    else
                    {
                        elementEvents[i].ForcePopped?.Invoke(currentElement);
                    }
                    stack.Remove(currentElement);
                }
            }

            EventIndex = elementIndex;
            if (EventIndex < 0)
            {
                EventIndex = 0;
            }
        }
    }
}