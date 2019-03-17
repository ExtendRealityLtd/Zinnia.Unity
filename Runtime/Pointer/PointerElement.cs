namespace Zinnia.Pointer
{
    using UnityEngine;
    using Malimbe.MemberClearanceMethod;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;

    /// <summary>
    /// Describes an element of the rendered <see cref="ObjectPointer"/>.
    /// </summary>
    public class PointerElement : MonoBehaviour
    {
        /// <summary>
        /// The visibility of an <see cref="PointerElement"/>.
        /// </summary>
        public enum Visibility
        {
            /// <summary>
            /// The <see cref="PointerElement"/> will only be visible when the <see cref="ObjectPointer"/> is activated.
            /// </summary>
            OnWhenPointerActivated,
            /// <summary>
            /// The <see cref="PointerElement"/> will always be visible regardless of the <see cref="ObjectPointer"/> state.
            /// </summary>
            AlwaysOn,
            /// <summary>
            /// The <see cref="PointerElement"/> will never be visible regardless of the <see cref="ObjectPointer"/> state.
            /// </summary>
            AlwaysOff
        }

        /// <summary>
        /// Represents the <see cref="PointerElement"/> when it's colliding with a valid object.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public GameObject ValidObject { get; set; }
        /// <summary>
        /// Represents the <see cref="PointerElement"/> when it's colliding with an invalid object or not colliding at all.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public GameObject InvalidObject { get; set; }
        /// <summary>
        /// Determines when the <see cref="PointerElement"/> is visible.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public Visibility ElementVisibility { get; set; } = Visibility.OnWhenPointerActivated;
    }
}