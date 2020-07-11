namespace Zinnia.Data.Collection.List
{
    using System;
    using System.Collections.Generic;
    using UnityEngine.Events;
    using Object = UnityEngine.Object;

    /// <summary>
    /// Allows observing changes to a <see cref="List{T}"/> of <see cref="Object"/>s.
    /// </summary>
    public class UnityObjectObservableList : DefaultObservableList<Object, UnityObjectObservableList.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the <see cref="Object"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<Object> { }
    }
}