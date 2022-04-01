namespace Zinnia.Pointer
{
    using UnityEngine;
    using UnityEngine.Events;
    using Zinnia.Extension;

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
        [Header("Valid Container Settings")]
        [Tooltip("The containing GameObject that represents the element when a valid collision is occuring.")]
        [SerializeField]
        private GameObject validElementContainer;
        /// <summary>
        /// The containing <see cref="GameObject"/> that represents the element when a valid collision is occuring.
        /// </summary>
        public GameObject ValidElementContainer
        {
            get
            {
                return validElementContainer;
            }
            set
            {
                validElementContainer = value;
            }
        }
        [Tooltip("The GameObject containing the visible mesh for the PointerElement when a valid collision is occuring.")]
        [SerializeField]
        private GameObject validMeshContainer;
        /// <summary>
        /// The <see cref="GameObject"/> containing the visible mesh for the <see cref="PointerElement"/> when a valid collision is occuring.
        /// </summary>
        public GameObject ValidMeshContainer
        {
            get
            {
                return validMeshContainer;
            }
            set
            {
                validMeshContainer = value;
            }
        }
        #endregion

        #region Invalid Container Settings
        [Header("Invalid Container Settings")]
        [Tooltip("The containing GameObject that represents the element when an invalid collision or no collision is occuring.")]
        [SerializeField]
        private GameObject invalidElementContainer;
        /// <summary>
        /// The containing <see cref="GameObject"/> that represents the element when an invalid collision or no collision is occuring.
        /// </summary>
        public GameObject InvalidElementContainer
        {
            get
            {
                return invalidElementContainer;
            }
            set
            {
                invalidElementContainer = value;
            }
        }
        [Tooltip("The GameObject containing the visible mesh for the PointerElement when an invalid collision or no collision is occuring.")]
        [SerializeField]
        private GameObject invalidMeshContainer;
        /// <summary>
        /// The <see cref="GameObject"/> containing the visible mesh for the <see cref="PointerElement"/> when an invalid collision or no collision is occuring.
        /// </summary>
        public GameObject InvalidMeshContainer
        {
            get
            {
                return invalidMeshContainer;
            }
            set
            {
                invalidMeshContainer = value;
            }
        }
        #endregion

        #region Visibility Settings
        [Header("Visibility Settings")]
        [Tooltip("Determines when the PointerElement is visible.")]
        [SerializeField]
        private Visibility elementVisibility = Visibility.OnWhenPointerActivated;
        /// <summary>
        /// Determines when the <see cref="PointerElement"/> is visible.
        /// </summary>
        public Visibility ElementVisibility
        {
            get
            {
                return elementVisibility;
            }
            set
            {
                elementVisibility = value;
                if (this.IsMemberChangeAllowed())
                {
                    OnAfterElementVisibilityChange();
                }
            }
        }
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
        /// Clears <see cref="ValidElementContainer"/>.
        /// </summary>
        public virtual void ClearValidElementContainer()
        {
            if (!this.IsValidState())
            {
                return;
            }

            ValidElementContainer = default;
        }

        /// <summary>
        /// Clears <see cref="ValidMeshContainer"/>.
        /// </summary>
        public virtual void ClearValidMeshContainer()
        {
            if (!this.IsValidState())
            {
                return;
            }

            ValidMeshContainer = default;
        }

        /// <summary>
        /// Clears <see cref="InvalidElementContainer"/>.
        /// </summary>
        public virtual void ClearInvalidElementContainer()
        {
            if (!this.IsValidState())
            {
                return;
            }

            InvalidElementContainer = default;
        }

        /// <summary>
        /// Clears <see cref="InvalidMeshContainer"/>.
        /// </summary>
        public virtual void ClearInvalidMeshContainer()
        {
            if (!this.IsValidState())
            {
                return;
            }

            InvalidMeshContainer = default;
        }

        /// <summary>
        /// Sets the <see cref="ElementVisibility"/>.
        /// </summary>
        /// <param name="index">The index of the <see cref="Visibility"/>.</param>
        public virtual void SetElementVisibilityt(int index)
        {
            ElementVisibility = EnumExtensions.GetByIndex<Visibility>(index);
        }

        /// <summary>
        /// Called after <see cref="ElementVisibility"/> has been changed.
        /// </summary>
        protected virtual void OnAfterElementVisibilityChange()
        {
            VisibilityChanged?.Invoke();
        }
    }
}