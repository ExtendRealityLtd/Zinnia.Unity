namespace VRTK.Core.Prefabs.Pointers
{
    using UnityEngine;
    using VRTK.Core.Action;

    /// <summary>
    /// The public interface into the Pointer Prefab.
    /// </summary>
    public class PointerFacade : MonoBehaviour
    {
        /// <summary>
        /// The pointer selection type.
        /// </summary>
        public enum SelectionType
        {
            /// <summary>
            /// Initiates the select action when the selection action is activated (e.g. button pressed).
            /// </summary>
            SelectOnActivate,
            /// <summary>
            /// Initiates the select action when the selection action is deactivated (e.g. button released).
            /// </summary>
            SelectOnDeactivate
        }

        [Header("Pointer Settings")]

        /// <summary>
        /// The target for the pointer to follow around.
        /// </summary>
        [Tooltip("The target for the pointer to follow around.")]
        public GameObject followTarget;
        /// <summary>
        /// The <see cref="BooleanAction"/> that will activate/deactivate the pointer.
        /// </summary>
        [Tooltip("The BooleanAction that will activate/deactivate the pointer.")]
        public BooleanAction activationAction;
        /// <summary>
        /// The <see cref="BooleanAction"/> that initiates the pointer selection.
        /// </summary>
        [Tooltip("The BooleanAction that initiates the pointer selection.")]
        public BooleanAction selectionAction;
        /// <summary>
        /// The action moment when to initiate the select action.
        /// </summary>
        [Tooltip("The action moment when to initiate the select action.")]
        public SelectionType selectionType = SelectionType.SelectOnActivate;

        [Header("Internal Settings")]

        /// <summary>
        /// **DO NOT CHANGE** - The linked Internal Setup.
        /// </summary>
        [Tooltip("**DO NOT CHANGE** - The linked Internal Setup.")]
        public PointerInternalSetup internalSetup;
    }
}