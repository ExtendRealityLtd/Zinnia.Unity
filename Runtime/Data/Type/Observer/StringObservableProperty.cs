namespace Zinnia.Data.Type.Observer
{
    using System;
    using UnityEngine.Events;

    /// <summary>
    /// Allows observing changes of a <see cref="string"/>.
    /// </summary>
    public class StringObservableProperty : ObservableProperty<string, StringObservableProperty.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the <see cref="string"/> state.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<string> { }
    }
}