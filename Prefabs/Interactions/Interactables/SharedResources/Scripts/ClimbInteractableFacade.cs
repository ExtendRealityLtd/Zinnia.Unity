namespace VRTK.Core.Prefabs.Interactions.Interactables
{
    using UnityEngine;
    using VRTK.Core.Data.Attribute;
    using VRTK.Core.Prefabs.Locomotion.Movement.Climb;

    /// <summary>
    /// The public interface for the Interactable.Climbable prefab.
    /// </summary>
    public class ClimbInteractableFacade : MonoBehaviour
    {
        #region Climb Settings
        /// <summary>
        /// The <see cref="ClimbFacade"/> to use.
        /// </summary>
        [Header("Climb Settings"), Tooltip("The Climb Facade to use.")]
        public ClimbFacade climbFacade;
        /// <summary>
        /// The multiplier to apply to the velocity of the interactor when the interactable is released and climbing stops.
        /// </summary>
        [Tooltip("The multiplier to apply to the velocity of the interactor when the interactable is released and climbing stops.")]
        public Vector3 releaseMultiplier = Vector3.one;
        #endregion

        #region Internal Settings
        /// <summary>
        /// The linked Internal Setup.
        /// </summary>
        [Header("Internal Settings"), Tooltip("The linked Internal Setup."), InternalSetting]
        public ClimbInteractableInternalSetup internalSetup;
        #endregion
    }
}