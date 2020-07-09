namespace Zinnia.Action
{
    using System;
    using UnityEngine.Events;

    /// <summary>
    /// Emits a <see cref="bool"/> value.
    /// </summary>
    public class BooleanAction : Action<BooleanAction, bool, BooleanAction.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the <see cref="bool"/> state.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<bool> { }
    }
}