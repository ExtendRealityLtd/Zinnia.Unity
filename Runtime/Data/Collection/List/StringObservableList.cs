namespace Zinnia.Data.Collection.List
{
    using UnityEngine.Events;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Allows observing changes to a <see cref="List{T}"/> of <see cref="string"/>s.
    /// </summary>
    public class StringObservableList : DefaultObservableList<string, StringObservableList.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the <see cref="string"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<string>
        {
        }
    }
}
