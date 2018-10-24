namespace VRTK.Core.Data.Collection
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// Allows observing changes to a <see cref="List{T}"/> of <see cref="GameObject"/>s.
    /// </summary>
    public class ObservableGameObjectList : ObservableListBase<GameObject, ObservableGameObjectList.UnityEvent>
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