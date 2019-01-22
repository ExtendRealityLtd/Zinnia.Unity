namespace Zinnia.Data.Collection
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;

    /// <summary>
    /// Allows observing changes to a <see cref="HashSet{T}"/> of <see cref="GameObject"/>s.
    /// </summary>
    public class GameObjectObservableSet : ObservableSet<GameObject, GameObjectObservableSet.UnityEvent>
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