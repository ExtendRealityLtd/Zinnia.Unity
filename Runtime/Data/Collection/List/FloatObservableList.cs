namespace Zinnia.Data.Collection.List
{
    using UnityEngine.Events;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Allows observing changes to a <see cref="List{T}"/> of <see cref="float"/>s.
    /// </summary>
    public class FloatObservableList : DefaultObservableList<float, FloatObservableList.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the <see cref="float"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<float>
        {
        }
    }
}
