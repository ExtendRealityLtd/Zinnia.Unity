namespace Zinnia.Data.Collection.List
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Allows observing changes to a <see cref="List{T}"/> of <see cref="Behaviour"/>s.
    /// </summary>
    public class BehaviourObservableList : DefaultObservableList<Behaviour, BehaviourObservableList.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the <see cref="Behaviour"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<Behaviour>
        {
        }
    }
}
