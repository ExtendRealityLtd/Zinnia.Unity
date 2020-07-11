namespace Zinnia.Data.Collection.Counter
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// Allows counting the amount of attempts a <see cref="GameObject"/> is added or removed from a <see cref="Dictionary{TKey,TValue}"/>.
    /// </summary>
    public class GameObjectObservableCounter : ObservableCounter<GameObject, GameObjectObservableCounter.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the <see cref="GameObject"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<GameObject> { }
    }
}