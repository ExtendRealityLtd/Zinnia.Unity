namespace Zinnia.Data.Type.Observer
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// Allows observing changes of a <see cref="GameObject"/>.
    /// </summary>
    public class GameObjectObservableProperty : ObservableProperty<GameObject, GameObjectObservableProperty.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the <see cref="GameObject"/> state.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<GameObject> { }
    }
}