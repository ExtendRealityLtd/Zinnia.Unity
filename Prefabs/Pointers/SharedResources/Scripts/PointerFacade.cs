namespace VRTK.Core.Prefabs.Pointers
{
    using UnityEngine;
    using VRTK.Core.Action;
    using VRTK.Core.Rule;
    using VRTK.Core.Data.Attribute;

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

        #region Pointer Settings
        /// <summary>
        /// The target for the pointer to follow around.
        /// </summary>
        [Header("Pointer Settings"), Tooltip("The target for the pointer to follow around.")]
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
        /// <summary>
        /// Allows to optionally determine targets based on the set rules.
        /// </summary>
        [Tooltip("Allows to optionally determine targets based on the set rules.")]
        public RuleContainer targetValidity;
        #endregion

        #region Internal Settings
        /// <summary>
        /// The linked Internal Setup.
        /// </summary>
        [Header("Internal Settings"), Tooltip("The linked Internal Setup."), InternalSetting]
        public PointerInternalSetup internalSetup;
        #endregion
    }
}