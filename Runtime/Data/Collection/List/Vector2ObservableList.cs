namespace Zinnia.Data.Collection.List
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// Allows observing changes to a <see cref="List{T}"/> of <see cref="Vector2"/>s.
    /// </summary>
    public class Vector2ObservableList : DefaultObservableList<Vector2, Vector2ObservableList.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the <see cref="Vector2"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<Vector2> { }
    }
}
