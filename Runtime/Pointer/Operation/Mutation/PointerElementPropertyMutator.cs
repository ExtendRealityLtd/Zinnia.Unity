namespace Zinnia.Pointer.Operation.Mutation
{
    using UnityEngine;
    using Malimbe.MemberChangeMethod;
    using Malimbe.MemberClearanceMethod;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.BehaviourStateRequirementMethod;
    using Zinnia.Extension;

    /// <summary>
    /// Mutates the properties of a <see cref="PointerElement"/> with the benefit of being able to specify a containing <see cref="GameObject"/> as the target.
    /// </summary>
    public class PointerElementPropertyMutator : MonoBehaviour
    {
        /// <summary>
        /// The <see cref="PointerElement"/> to mutate.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public PointerElement Target { get; set; }

        /// <summary>
        /// The containing <see cref="GameObject"/> that represents the element when a valid collision is occuring.
        /// </summary>
        [Cleared]
        public GameObject ValidElementContainer { get; set; }
        /// <summary>
        /// The <see cref="GameObject"/> containing the visible mesh for the <see cref="PointerElement"/> when a valid collision is occuring.
        /// </summary>
        [Cleared]
        public GameObject ValidMeshContainer { get; set; }
        /// <summary>
        /// The containing <see cref="GameObject"/> that represents the element when an invalid collision or no collision is occuring.
        /// </summary>
        [Cleared]
        public GameObject InvalidElementContainer { get; set; }
        /// <summary>
        /// The <see cref="GameObject"/> containing the visible mesh for the <see cref="PointerElement"/> when an invalid collision or no collision is occuring.
        /// </summary>
        [Cleared]
        public GameObject InvalidMeshContainer { get; set; }
        /// <summary>
        /// Determines when the <see cref="PointerElement"/> is visible.
        /// </summary>
        public PointerElement.Visibility ElementVisibility { get; set; }

        /// <summary>
        /// Sets the <see cref="Target"/> based on the first found <see cref="PointerElement"/> as either a direct, descendant or ancestor of the given <see cref="GameObject"/>.
        /// </summary>
        /// <param name="target">The <see cref="GameObject"/> to search for a <see cref="PointerElement"/> on.</param>
        [RequiresBehaviourState]
        public virtual void SetTarget(GameObject target)
        {
            if (target == null)
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
            ElementVisibility = (PointerElement.Visibility)Mathf.Clamp(elementVisibilityIndex, 0, System.Enum.GetValues(typeof(PointerElement.Visibility)).Length);
        }

        /// <summary>
        /// Called after <see cref="ValidElementContainer"/> has been changed.
        /// </summary>
        [CalledAfterChangeOf(nameof(ValidElementContainer))]
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
        [CalledAfterChangeOf(nameof(ValidMeshContainer))]
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
        [CalledAfterChangeOf(nameof(InvalidElementContainer))]
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
        [CalledAfterChangeOf(nameof(InvalidMeshContainer))]
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
        [CalledAfterChangeOf(nameof(ElementVisibility))]
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