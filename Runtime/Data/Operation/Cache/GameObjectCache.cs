namespace Zinnia.Data.Operation.Cache
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;

    public class GameObjectCache : ValueCache<GameObject, GameObjectCache.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the specified <see cref="GameObject"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<GameObject>
        {
        }
    }
}