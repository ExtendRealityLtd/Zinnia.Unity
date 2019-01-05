namespace Zinnia.Data.Type
{
    using UnityEngine;
    using System.Collections.Generic;

    /// <summary>
    /// Provides a collection of scene cameras.
    /// </summary>
    public class CameraList : MonoBehaviour
    {
        /// <summary>
        /// A scene <see cref="Camera"/> list.
        /// </summary>
        public List<Camera> cameras = new List<Camera>();
    }
}