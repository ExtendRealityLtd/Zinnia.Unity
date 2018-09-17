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
            /// <summary>
            /// Emitted when the element of the stack is restored to being at the top after elements above it being popped off.
            /// </summary>
            public UnityEvent Restored = new UnityEvent();
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
        public List<GameObject> Stack
        {
            get;
            protected set;
        } = new List<GameObject>();

        protected bool abortPopProcess = false;

        /// <summary>
        /// Push an element onto the stack and emit the related events.
        /// </summary>
        /// <param name="element">The element to push onto the stack and to become the payload of the related event.</param>
        public virtual void Push(GameObject element)
        {
            if (!isActiveAndEnabled || EventIndex >= elementEvents.Count || Stack.Contains(element))
            {
                return;
            }

            Stack.Add(element);
            EventIndex++;
            elementEvents[Stack.Count - 1].Pushed?.Invoke(element);
        }

        /// <summary>
        /// Pops the last element off the stack and emit the related events.
        /// </summary>
        public virtual void Pop()
        {
            if (Stack.Count > 0)
            {
                PopAt(Stack[Stack.Count - 1]);
            }
        }

        /// <summary>
        /// Pops from the stack at the given stack index.
        /// </summary>
        /// <param name="index">The index at which to pop the stack at.</param>
        public virtual void PopAt(int index)
        {
            if (index < Stack.Count && index <= EventIndex)
            {
                PopAt(Stack[index]);
            }
        }

        /// <summary>
        /// Pops the given element from the stack and subsequently remove any elements that are above the given element in the stack and emit the related events.
        /// </summary>
        /// <param name="element">The element to pop from the stack.</param>
        public virtual void PopAt(GameObject element)
        {
            int elementIndex = Stack.IndexOf(element);
            if (!isActiveAndEnabled || elementIndex < 0)
            {
                return;
            }

            for (int i = elementEvents.Count - 1; i >= elementIndex; i--)
            {
                if (abortPopProcess)
                {
                    abortPopProcess = false;
                    return;
                }

                if (elementIndex < Stack.Count && i < Stack.Count)
                {
                    GameObject currentElement = Stack[i];
                    Stack.Remove(currentElement);
                    EventIndex = elementIndex;
                    if (i == elementIndex)
                    {
                        elementEvents[i].Popped?.Invoke(currentElement);
                    }
                    else
                    {
                        elementEvents[i].ForcePopped?.Invoke(currentElement);
                    }
                }
            }

            if (EventIndex < 0)
            {
                EventIndex = 0;
            }
            else if (EventIndex > 0)
            {
                elementEvents[EventIndex - 1].Restored?.Invoke(Stack[EventIndex - 1]);
            }
        }

        /// <summary>
        /// Aborts the current stack pop process to prevent any further elements from being popped off the stack.
        /// </summary>
        public virtual void AbortPop()
        {
            abortPopProcess = true;
        }
    }
}