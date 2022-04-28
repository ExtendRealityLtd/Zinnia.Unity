namespace Zinnia.Data.Collection.Stack
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;
    using Zinnia.Extension;

    /// <summary>
    /// A collection of events to emit when a new <see cref="TElement"/> is added or removed from the stack.
    /// </summary>
    public abstract class ObservableStackElementEvents<TElement, TEvent> where TEvent : UnityEvent<TElement>, new()
    {
        /// <summary>
        /// Emitted when a new <see cref="TElement"/> is added to the end of the stack.
        /// </summary>
        public TEvent Pushed = new TEvent();
        /// <summary>
        /// Emitted when the <see cref="TElement"/> is removed from the end of the stack.
        /// </summary>
        public TEvent Popped = new TEvent();
        /// <summary>
        /// Emitted when the <see cref="TElement"/> is removed from the stack due to a <see cref="TElement"/> lower down in the stack being removed.
        /// </summary>
        public TEvent ForcePopped = new TEvent();
        /// <summary>
        /// Emitted when the <see cref="TElement"/> of the stack is restored to being at the top after elements above it being popped off.
        /// </summary>
        public TEvent Restored = new TEvent();
    }

    /// <summary>
    /// Allows observing changes to a stack.
    /// </summary>
    /// <typeparam name="TElement">The type of the elements in the stack.</typeparam>
    /// <typeparam name="TElementEvents">The events to emit per element.</typeparam>
    /// <typeparam name="TEvent">The <see cref="UnityEvent"/> type to use.</typeparam>
    public abstract class ObservableStack<TElement, TElementEvents, TEvent> : MonoBehaviour where TElementEvents : ObservableStackElementEvents<TElement, TEvent> where TEvent : UnityEvent<TElement>, new()
    {
        [Tooltip("The events to emit for the TElement that is added to the same index within the stack.")]
        [SerializeField]
        private List<TElementEvents> elementEvents = new List<TElementEvents>();
        /// <summary>
        /// The events to emit for the <see cref="TElement"/> that is added to the same index within the stack.
        /// </summary>
        public List<TElementEvents> ElementEvents
        {
            get
            {
                return elementEvents;
            }
            set
            {
                elementEvents = value;
            }
        }

        /// <summary>
        /// The current index the events to emit is at in relation to the <see cref="TElement"/> stack.
        /// </summary>
        public int EventIndex { get; protected set; }

        /// <summary>
        /// A representation of the <see cref="TElement"/> stack.
        /// </summary>
        public List<TElement> Stack { get; protected set; } = new List<TElement>();

        /// <summary>
        /// Determines whether to abort a running pop process.
        /// </summary>
        protected bool abortPopProcess;

        /// <summary>
        /// Push an element onto the stack and emit the related events.
        /// </summary>
        /// <param name="element">The element to push onto the stack and to become the payload of the related event.</param>
        public virtual void Push(TElement element)
        {
            if (!this.IsValidState() || EventIndex >= ElementEvents.Count || Stack.Contains(element))
            {
                return;
            }

            Stack.Add(element);
            EventIndex++;
            ElementEvents[Stack.Count - 1].Pushed?.Invoke(element);
        }

        /// <summary>
        /// Pops the last element off the stack and emit the related events.
        /// </summary>
        public virtual void Pop()
        {
            if (!this.IsValidState())
            {
                return;
            }

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
            if (!this.IsValidState())
            {
                return;
            }

            if (index < Stack.Count && index <= EventIndex)
            {
                PopAt(Stack[index]);
            }
        }

        /// <summary>
        /// Pops the given element from the stack and subsequently remove any elements that are above the given element in the stack and emit the related events.
        /// </summary>
        /// <param name="element">The element to pop from the stack.</param>
        public virtual void PopAt(TElement element)
        {
            if (!this.IsValidState())
            {
                return;
            }

            int elementIndex = Stack.IndexOf(element);
            if (elementIndex < 0)
            {
                return;
            }

            for (int currentIndex = ElementEvents.Count - 1; currentIndex >= elementIndex; currentIndex--)
            {
                if (abortPopProcess)
                {
                    abortPopProcess = false;
                    return;
                }

                if (elementIndex < Stack.Count && currentIndex < Stack.Count)
                {
                    TElement currentElement = Stack[currentIndex];
                    Stack.Remove(currentElement);
                    EventIndex = elementIndex;
                    if (currentIndex == elementIndex)
                    {
                        ElementEvents[currentIndex].Popped?.Invoke(currentElement);
                    }
                    else
                    {
                        ElementEvents[currentIndex].ForcePopped?.Invoke(currentElement);
                    }
                }
            }

            if (EventIndex < 0)
            {
                EventIndex = 0;
            }
            else if (EventIndex > 0)
            {
                ElementEvents[EventIndex - 1].Restored?.Invoke(Stack[EventIndex - 1]);
            }
        }

        /// <summary>
        /// Aborts the current stack pop process to prevent any further elements from being popped off the stack.
        /// </summary>
        public virtual void AbortPop()
        {
            if (!this.IsValidState())
            {
                return;
            }

            abortPopProcess = true;
        }
    }
}