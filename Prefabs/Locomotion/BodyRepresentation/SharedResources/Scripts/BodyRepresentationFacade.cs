namespace VRTK.Core.Prefabs.Locomotion.BodyRepresentation
{
    using UnityEngine;
    using UnityEngine.Events;
    using System.Collections.Generic;
    using VRTK.Core.Data.Attribute;
    using VRTK.Core.Tracking.Follow;
    using VRTK.Core.Prefabs.Interactions.Interactors;

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
        /// The thickness of <see cref="source"/> to be used when resolving body collisions.
        /// </summary>
        [Tooltip("The thickness of source to be used when resolving body collisions.")]
        public float sourceThickness = 0.05f;
        /// <summary>
        /// An optional offset for the <see cref="source"/> to use.
        /// </summary>
        [Tooltip("An optional offset for the source to use.")]
        public GameObject offset;
        /// <summary>
        /// A collection of interactors to exclude from physics collision checks.
        /// </summary>
        [Tooltip("A collection of interactors to exclude from physics collision checks."), SerializeField]
        protected List<InteractorFacade> ignoredInteractors = new List<InteractorFacade>();
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
        /// A collection of interactors to exclude from physics checks.
        /// </summary>
        public List<InteractorFacade> IgnoredInteractors => ignoredInteractors;

        /// <summary>
        /// An optional follower of <see cref="offset"/>.
        /// </summary>
        protected ObjectFollower offsetObjectFollower;
        /// <summary>
        /// An optional follower of <see cref="source"/>.
        /// </summary>
        protected ObjectFollower sourceObjectFollower;

        /// <summary>
        /// Sets the source of truth for movement to come from <see cref="BodyRepresentationInternalSetup.rigidbody"/> until <see cref="BodyRepresentationInternalSetup.characterController"/> hits the ground, then <see cref="BodyRepresentationInternalSetup.characterController"/> is the new source of truth.
        /// </summary>
        /// <remarks>
        /// This method needs to be called right before or right after applying any form of movement to the rigidbody while the body is grounded, i.e. in the same frame and before <see cref="BodyRepresentationInternalSetup.Process"/> is called.
        /// </remarks>
        public virtual void ListenToRigidbodyMovement()
        {
            Interest = BodyRepresentationInternalSetup.MovementInterest.RigidbodyUntilGrounded;
        }

        /// <summary>
        /// Solves body collisions by not moving the body in case it can't go to its current position.
        /// </summary>
        /// <remarks>
        /// If body collisions should be prevented this method needs to be called right before or right after applying any form of movement to the body.
        /// </remarks>
        public virtual void SolveBodyCollisions()
        {
            if (!isActiveAndEnabled || source == null)
            {
                return;
            }

            if (offsetObjectFollower != null)
            {
                offsetObjectFollower.Process();
            }

            if (sourceObjectFollower != null)
            {
                sourceObjectFollower.Process();
            }

            internalSetup.Process();

            Vector3 characterControllerPosition = internalSetup.characterController.transform.position + internalSetup.characterController.center;
            Vector3 difference = source.transform.position - characterControllerPosition;
            difference.y = 0f;

            float minimumDistanceToColliders = internalSetup.characterController.radius - sourceThickness;
            if (difference.magnitude < minimumDistanceToColliders)
            {
                return;
            }

            float newDistance = difference.magnitude - minimumDistanceToColliders;
            (offset == null ? source : offset).transform.position -= difference.normalized * newDistance;

            internalSetup.Process();
        }

        protected virtual void OnEnable()
        {
            if (source != null)
            {
                sourceObjectFollower = source.GetComponent<ObjectFollower>();
            }

            if (offset != null)
            {
                offsetObjectFollower = offset.GetComponent<ObjectFollower>();
            }
        }

        protected virtual void OnDisable()
        {
            sourceObjectFollower = null;
            offsetObjectFollower = null;
        }
    }
}