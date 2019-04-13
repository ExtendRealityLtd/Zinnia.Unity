namespace Zinnia.Data.Collection.List
{
    using Object = UnityEngine.Object;
    using UnityEngine.Events;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Allows observing changes to a <see cref="List{T}"/> of <see cref="Object"/>s.
    /// </summary>
    public class UnityObjectObservableList : DefaultObservableList<Object, UnityObjectObservableList.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the <see cref="Object"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<Object>
        {
        }
    }
}