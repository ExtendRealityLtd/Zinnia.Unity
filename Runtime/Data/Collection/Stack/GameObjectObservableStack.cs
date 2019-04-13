namespace Zinnia.Data.Collection.Stack
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;

    /// <summary>
    /// Emits the events at the current stack index every time a new <see cref="GameObject"/> is added to or removed from the stack.
    /// </summary>
    public class GameObjectObservableStack : ObservableStack<GameObject, GameObjectObservableStack.GameObjectElementEvents, GameObjectObservableStack.GameObjectElementEvents.UnityEvent>
    {
        /// <summary>
        /// A collection of events to emit when a new <see cref="GameObject"/> is added or removed from the stack.
        /// </summary>
        [Serializable]
        public class GameObjectElementEvents : ObservableStackElementEvents<GameObject, GameObjectElementEvents.UnityEvent>
        {
            /// <summary>
            /// Defines the event with the <see cref="GameObject"/>.
            /// </summary>
            [Serializable]
            public class UnityEvent : UnityEvent<GameObject>
            {
            }
        }
    }
}