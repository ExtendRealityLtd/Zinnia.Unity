namespace VRTK.Core.Action
{
    using UnityEngine.Events;
    using System;

    /// <summary>
    /// Emits a <see cref="bool"/> value.
    /// </summary>
    public class BooleanAction : BaseAction<bool, BooleanAction.BooleanActionUnityEvent>
    {
        /// <summary>
        /// Defines the event with the <see cref="bool"/> state and sender <see cref="object"/>.
        /// </summary>
        [Serializable]
        public class BooleanActionUnityEvent : UnityEvent<bool, object>
        {
        }
    }
}