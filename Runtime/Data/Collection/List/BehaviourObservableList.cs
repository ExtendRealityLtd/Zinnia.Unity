namespace Zinnia.Data.Collection.List
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// Allows observing changes to a <see cref="List{T}"/> of <see cref="Behaviour"/>s.
    /// </summary>
    public class BehaviourObservableList : DefaultObservableList<Behaviour, BehaviourObservableList.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the <see cref="Behaviour"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<Behaviour> { }
    }
}
