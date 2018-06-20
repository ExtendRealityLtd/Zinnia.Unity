namespace VRTK.Core.Action
{
    using UnityEngine.Events;
    using System;

    /// <summary>
    /// Emits a <see cref="bool"/> value.
    /// </summary>
    public class BooleanAction : BaseAction<BooleanAction, bool, BooleanAction.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the <see cref="bool"/> state.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<bool>
        {
        }
    }
}