namespace Zinnia.Data.Collection.List
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Allows observing changes to a <see cref="List{T}"/> of <see cref="Vector3"/>s.
    /// </summary>
    public class Vector3ObservableList : DefaultObservableList<Vector3, Vector3ObservableList.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the <see cref="Vector3"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<Vector3>
        {
        }
    }
}
