namespace Zinnia.Data.Type.Observer
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// Allows observing changes of a <see cref="Camera"/>.
    /// </summary>
    public class CameraObservableProperty : ObservableProperty<Camera, CameraObservableProperty.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the <see cref="Camera"/> state.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<Camera> { }
    }
}