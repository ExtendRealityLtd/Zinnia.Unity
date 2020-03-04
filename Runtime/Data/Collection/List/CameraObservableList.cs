namespace Zinnia.Data.Collection.List
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Allows observing changes to a <see cref="List{T}"/> of <see cref="Camera"/>s.
    /// </summary>
    public class CameraObservableList : DefaultObservableList<Camera, CameraObservableList.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the <see cref="Camera"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<Camera>
        {
        }
    }
}