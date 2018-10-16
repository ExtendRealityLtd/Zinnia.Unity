namespace VRTK.Core.Prefabs.Locomotion.BodyRepresentation
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;
    using VRTK.Core.Data.Attribute;

    /// <summary>
    /// The public interface for the BodyRepresentation prefab.
    /// </summary>
    public class BodyRepresentationFacade : MonoBehaviour
    {
        #region Facade Settings
        /// <summary>
        /// The object to follow.
        /// </summary>
        [Header("Facade Settings"), Tooltip("The object to follow.")]
        public GameObject source;
        /// <summary>
        /// An optional offset for the <see cref="source"/> to use.
        /// </summary>
        [Tooltip("An optional offset for the source to use.")]
        public GameObject offset;
        /// <summary>
        /// The colliders to ignore body collisions with.
        /// </summary>
        [Tooltip("The colliders to ignore body collisions with.")]
        public List<Collider> ignoredColliders = new List<Collider>();
        #endregion

        #region Events
        /// <summary>
        /// Emitted when the body starts touching ground.
        /// </summary>
        [Header("Events"), Tooltip("Emitted when the body starts touching ground.")]
        public UnityEvent BecameGrounded = new UnityEvent();
        /// <summary>
        /// Emitted when the body stops touching ground.
        /// </summary>
        [Tooltip("Emitted when the body stops touching ground.")]
        public UnityEvent BecameAirborne = new UnityEvent();
        #endregion

        #region Internal Settings
        /// <summary>
        /// The linked Internal Setup.
        /// </summary>
        [Header("Internal Settings"), Tooltip("The linked Internal Setup."), InternalSetting]
        public BodyRepresentationInternalSetup internalSetup;
        #endregion

        /// <summary>
        /// The object that defines the main source of truth for movement.
        /// </summary>
        public BodyRepresentationInternalSetup.MovementInterest Interest
        {
            get
            {
                return internalSetup.Interest;
            }
            set
            {
                internalSetup.Interest = value;
            }
        }

        /// <summary>
        /// Whether the body touches ground.
        /// </summary>
        public bool IsCharacterControllerGrounded => internalSetup.IsCharacterControllerGrounded;

        /// <summary>
        /// Sets the source of truth for movement to come from <see cref="BodyRepresentationInternalSetup.rigidbody"/> until <see cref="BodyRepresentationInternalSetup.characterController"/> hits the ground, then <see cref="BodyRepresentationInternalSetup.characterController"/> is the new source of truth.
        /// </summary>
        /// <remarks>
        /// This method needs to be called right before or right after applying any form of movement to the rigidbody while the body is grounded, i.e. in the same frame and before <see cref="BodyRepresentationInternalSetup.Process"/> is called.
        /// </remarks>
        public void ListenToRigidbodyMovement()
        {
            Interest = BodyRepresentationInternalSetup.MovementInterest.RigidbodyUntilGrounded;
        }
    }
}