namespace Zinnia.Data.Type
{
    using Malimbe.XmlDocumentationAttribute;
    using System;
    using UnityObject = UnityEngine.Object;

    /// <summary>
    /// Provides a linkable reference to a <see cref="UnityObject"/>.
    /// </summary>
    [Serializable]

    public struct ObjectReference
    {
        /// <summary>
        /// The <see cref="UnityObject"/> to reference.
        /// </summary>
        [field: DocumentedByXml]
        public UnityObject linkedObject;
        /// <summary>
        /// The text to use for describing the linked reference.
        /// </summary>
        [field: DocumentedByXml]
        public string linkText;
        /// <summary>
        /// Whether the reference should be active.
        /// </summary>
        [field: DocumentedByXml]
        public bool isActive;
    }
}