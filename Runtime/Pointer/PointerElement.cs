namespace Zinnia.Pointer
{
    using UnityEngine;
    using UnityEngine.Events;
    using Malimbe.MemberChangeMethod;
    using Malimbe.MemberClearanceMethod;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;

    /// <summary>
    /// Describes an element of the rendered <see cref="ObjectPointer"/>.
    /// </summary>
    public class PointerElement : MonoBehaviour
    {
        /// <summary>
        /// The visibility of a <see cref="PointerElement"/>.
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

        #region Valid Container Settings
        /// <summary>
        /// The containing <see cref="GameObject"/> that represents the element when a valid collision is occuring.
        /// </summary>
        [Serialized, Cleared]
        [field: Header("Valid Container Settings"), DocumentedByXml]
        public GameObject ValidElementContainer { get; set; }
        /// <summary>
        /// The <see cref="GameObject"/> containing the visible mesh for the <see cref="PointerElement"/> when a valid collision is occuring.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public GameObject ValidMeshContainer { get; set; }
        #endregion

        #region Invalid Container Settings
        /// <summary>
        /// The containing <see cref="GameObject"/> that represents the element when an invalid collision or no collision is occuring.
        /// </summary>
        [Serialized, Cleared]
        [field: Header("Invalid Container Settings"), DocumentedByXml]
        public GameObject InvalidElementContainer { get; set; }
        /// <summary>
        /// The <see cref="GameObject"/> containing the visible mesh for the <see cref="PointerElement"/> when an invalid collision or no collision is occuring.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public GameObject InvalidMeshContainer { get; set; }
        #endregion

        #region Visibility Settings
        /// <summary>
        /// Determines when the <see cref="PointerElement"/> is visible.
        /// </summary>
        [Serialized]
        [field: Header("Visibility Settings"), DocumentedByXml]
        public Visibility ElementVisibility { get; set; } = Visibility.OnWhenPointerActivated;
        #endregion

        #region Element Events
        /// <summary>
        /// Emitted when the visibility of the element changes.
        /// </summary>
        [Header("Element Events")]
        public UnityEvent VisibilityChanged = new UnityEvent();
        #endregion

        /// <summary>
        /// Whether the element is currently visible.
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        /// Called after <see cref="ElementVisibility"/> has been changed.
        /// </summary>
        [CalledAfterChangeOf(nameof(ElementVisibility))]
        protected virtual void OnAfterElementVisibilityChange()
        {
            VisibilityChanged?.Invoke();
        }
    }
}