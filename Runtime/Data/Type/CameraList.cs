namespace Zinnia.Data.Type
{
    using UnityEngine;
    using System.Collections.Generic;
    using Malimbe.XmlDocumentationAttribute;

    /// <summary>
    /// Provides a collection of scene cameras.
    /// </summary>
    public class CameraList : MonoBehaviour
    {
        /// <summary>
        /// A scene <see cref="Camera"/> list.
        /// </summary>
        [DocumentedByXml]
        public List<Camera> cameras = new List<Camera>();
    }
}