namespace Zinnia.Pointer.Operation.Mutation
{
    using UnityEngine;
    using Zinnia.Extension;

    /// <summary>
    /// Mutates the properties of a <see cref="PointerElement"/> with the benefit of being able to specify a containing <see cref="GameObject"/> as the target.
    /// </summary>
    public class PointerElementPropertyMutator : MonoBehaviour
    {
        [Tooltip("The PointerElement to mutate.")]
        [SerializeField]
        private PointerElement target;
        /// <summary>
        /// The <see cref="PointerElement"/> to mutate.
        /// </summary>
        public PointerElement Target
        {
            get
            {
                return target;
            }
            set
            {
                target = value;
            }
        }

        /// <summary>
        /// The containing <see cref="GameObject"/> that represents the element when a valid collision is occuring.
        /// </summary>
        private GameObject validElementContainer;
        public GameObject ValidElementContainer
        {
            get
            {
                return validElementContainer;
            }
            set
            {
                validElementContainer = value;
                if (this.IsMemberChangeAllowed())
                {
                    OnAfterValidElementContainerChange();
                }
            }
        }
        /// <summary>
        /// The <see cref="GameObject"/> containing the visible mesh for the <see cref="PointerElement"/> when a valid collision is occuring.
        /// </summary>
        private GameObject validMeshContainer;
        public GameObject ValidMeshContainer
        {
            get
            {
                return validMeshContainer;
            }
            set
            {
                validMeshContainer = value;
                if (this.IsMemberChangeAllowed())
                {
                    OnAfterValidMeshContainerChange();
                }
            }
        }
        /// <summary>
        /// The containing <see cref="GameObject"/> that represents the element when an invalid collision or no collision is occuring.
        /// </summary>
        private GameObject invalidElementContainer;
        public GameObject InvalidElementContainer
        {
            get
            {
                return invalidElementContainer;
            }
            set
            {
                invalidElementContainer = value;
                if (this.IsMemberChangeAllowed())
                {
                    OnAfterInvalidElementContainerChange();
                }
            }
        }
        /// <summary>
        /// The <see cref="GameObject"/> containing the visible mesh for the <see cref="PointerElement"/> when an invalid collision or no collision is occuring.
        /// </summary>
        private GameObject invalidMeshContainer;
        public GameObject InvalidMeshContainer
        {
            get
            {
                return invalidMeshContainer;
            }
            set
            {
                invalidMeshContainer = value;
                if (this.IsMemberChangeAllowed())
                {
                    OnAfterInvalidMeshContainerChange();
                }
            }
        }
        /// <summary>
        /// Determines when the <see cref="PointerElement"/> is visible.
        /// </summary>
        private PointerElement.Visibility elementVisibility;
        public PointerElement.Visibility ElementVisibility
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

        /// <summary>
        /// Clears <see cref="Target"/>.
        /// </summary>
        public virtual void ClearTarget()
        {
            if (!this.IsValidState())
            {
                return;
            }

            Target = default;
        }

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
        /// Sets the <see cref="Target"/> based on the first found <see cref="PointerElement"/> as either a direct, descendant or ancestor of the given <see cref="GameObject"/>.
        /// </summary>
        /// <param name="target">The <see cref="GameObject"/> to search for a <see cref="PointerElement"/> on.</param>
        public virtual void SetTarget(GameObject target)
        {
            if (!this.IsValidState() || target == null)
            {
                return;
            }

            Target = target.TryGetComponent<PointerElement>(true, true);
        }

        /// <summary>
        /// Sets the <see cref="ElementVisibility"/>.
        /// </summary>
        /// <param name="elementVisibilityIndex">The index of the <see cref="PointerElement.Visibility"/>.</param>
        public virtual void SetElementVisibility(int elementVisibilityIndex)
        {
            ElementVisibility = EnumExtensions.GetByIndex<PointerElement.Visibility>(elementVisibilityIndex);
        }

        /// <summary>
        /// Called after <see cref="ValidElementContainer"/> has been changed.
        /// </summary>
        protected virtual void OnAfterValidElementContainerChange()
        {
            if (Target == null)
            {
                return;
            }

            Target.ValidElementContainer = ValidElementContainer;
        }

        /// <summary>
        /// Called after <see cref="ValidMeshContainer"/> has been changed.
        /// </summary>
        protected virtual void OnAfterValidMeshContainerChange()
        {
            if (Target == null)
            {
                return;
            }

            Target.ValidMeshContainer = ValidMeshContainer;
        }

        /// <summary>
        /// Called after <see cref="InvalidElementContainer"/> has been changed.
        /// </summary>
        protected virtual void OnAfterInvalidElementContainerChange()
        {
            if (Target == null)
            {
                return;
            }

            Target.InvalidElementContainer = InvalidElementContainer;
        }

        /// <summary>
        /// Called after <see cref="InvalidMeshContainer"/> has been changed.
        /// </summary>
        protected virtual void OnAfterInvalidMeshContainerChange()
        {
            if (Target == null)
            {
                return;
            }

            Target.InvalidMeshContainer = InvalidMeshContainer;
        }

        /// <summary>
        /// Called after <see cref="ElementVisibility"/> has been changed.
        /// </summary>
        protected virtual void OnAfterElementVisibilityChange()
        {
            if (Target == null)
            {
                return;
            }

            Target.ElementVisibility = ElementVisibility;
        }
    }
}