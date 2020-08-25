namespace Zinnia.Data.Type.Observer
{
    using System;
    using UnityEngine.Events;

    /// <summary>
    /// Allows observing changes of a <see cref="int"/>.
    /// </summary>
    public class IntObservableProperty : ObservableProperty<int, IntObservableProperty.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the <see cref="int"/> state.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<int> { }
    }
}