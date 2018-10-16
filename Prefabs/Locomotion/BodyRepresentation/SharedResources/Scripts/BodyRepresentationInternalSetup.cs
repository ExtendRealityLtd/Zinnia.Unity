namespace VRTK.Core.Prefabs.Locomotion.BodyRepresentation
{
    using System;
    using System.Linq;
    using UnityEngine;
    using VRTK.Core.Extension;
    using VRTK.Core.Process;

    /// <summary>
    /// Sets up the BodyRepresentation prefab based on the provided user settings and implements the logic to represent a body.
    /// </summary>
    public class BodyRepresentationInternalSetup : MonoBehaviour, IProcessable
    {
        /// <summary>
        /// The object that defines the main source of truth for movement.
        /// </summary>
        public enum MovementInterest
        {
            /// <summary>
            /// The source of truth for movement comes from <see cref="characterController"/>.
            /// </summary>
            CharacterController,
            /// <summary>
            /// The source of truth for movement comes from <see cref="characterController"/> until <see cref="rigidbody"/> is in the air, then <see cref="rigidbody"/> is the new source of truth.
            /// </summary>
            CharacterControllerUntilAirborne,
            /// <summary>
            /// The source of truth for movement comes from <see cref="rigidbody"/>.
            /// </summary>
            Rigidbody,
            /// <summary>
            /// The source of truth for movement comes from <see cref="rigidbody"/> until <see cref="characterController"/> hits the ground, then <see cref="characterController"/> is the new source of truth.
            /// </summary>
            RigidbodyUntilGrounded
        }

        #region Facade Settings
        /// <summary>
        /// The public interface facade.
        /// </summary>
        [Header("Facade Settings"), Tooltip("The public interface facade.")]
        public BodyRepresentationFacade facade;
        #endregion

        #region Reference Settings
        /// <summary>
        /// The <see cref="CharacterController"/> that acts as the main representation of the body.
        /// </summary>
        [Header("Reference Settings"), Tooltip("The Character Controller that acts as the main representation of the body.")]
        public CharacterController characterController;
        /// <summary>
        /// The <see cref="Rigidbody"/> that acts as the physical representation of the body.
        /// </summary>
        [Tooltip("The Rigidbody that acts as the physical representation of the body.")]
        public new Rigidbody rigidbody;
        /// <summary>
        /// The <see cref="CapsuleCollider"/> that acts as the physical collider representation of the body.
        /// </summary>
        [Tooltip("The Rigidbody that acts as the physical collider representation of the body.")]
        public CapsuleCollider rigidbodyCollider;
        #endregion

        /// <summary>
        /// The object that defines the main source of truth for movement.
        /// </summary>
        public MovementInterest Interest
        {
            get
            {
                return interest;
            }
            set
            {
                interest = value;

                switch (value)
                {
                    case MovementInterest.CharacterController:
                    case MovementInterest.CharacterControllerUntilAirborne:
                        rigidbody.isKinematic = true;
                        rigidbodySetFrameCount = 0;
                        break;
                    case MovementInterest.Rigidbody:
                    case MovementInterest.RigidbodyUntilGrounded:
                        rigidbody.isKinematic = false;
                        rigidbodySetFrameCount = Time.frameCount;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value), value, null);
                }
            }
        }

        /// <summary>
        /// Whether <see cref="characterController"/> touches ground.
        /// </summary>
        public bool IsCharacterControllerGrounded => wasCharacterControllerGrounded == true;

        /// <summary>
        /// The previous position of <see cref="rigidbody"/>.
        /// </summary>
        protected Vector3 previousRigidbodyPosition;
        /// <summary>
        /// Whether <see cref="characterController"/> was grounded previously.
        /// </summary>
        protected bool? wasCharacterControllerGrounded;
        /// <summary>
        /// The frame count of the last time <see cref="Interest"/> was set to <see cref="MovementInterest.Rigidbody"/> or <see cref="MovementInterest.RigidbodyUntilGrounded"/>.
        /// </summary>
        protected int rigidbodySetFrameCount;

        private MovementInterest interest = MovementInterest.CharacterControllerUntilAirborne;

        /// <summary>
        /// Positions, sizes and controls all variables necessary to make a body representation follow the given <see cref="BodyRepresentationFacade.source"/>.
        /// </summary>
        public virtual void Process()
        {
            if (Interest != MovementInterest.CharacterController && facade.offset != null)
            {
                Vector3 position = facade.offset.transform.position;
                position.y = rigidbody.position.y - characterController.skinWidth;

                Vector3 previousPosition = facade.offset.transform.position;
                facade.offset.transform.position = position;
                facade.source.transform.position += facade.offset.transform.position - previousPosition;
            }

            Vector3 previousCharacterControllerPosition;

            // Handle walking down stairs/slopes and physics affecting the Rigidbody in general.
            Vector3 rigidbodyPhysicsMovement = rigidbody.position - previousRigidbodyPosition;
            if (Interest == MovementInterest.Rigidbody || Interest == MovementInterest.RigidbodyUntilGrounded)
            {
                previousCharacterControllerPosition = characterController.transform.position;
                characterController.Move(rigidbodyPhysicsMovement);

                if (facade.offset != null)
                {
                    Vector3 movement = characterController.transform.position - previousCharacterControllerPosition;
                    facade.offset.transform.position += movement;
                    facade.source.transform.position += movement;
                }
            }

            // Position the CharacterController and handle moving the source relative to the offset.
            previousCharacterControllerPosition = characterController.transform.position;
            MatchCharacterControllerWithSource(false);
            Vector3 characterControllerSourceMovement = characterController.transform.position - previousCharacterControllerPosition;

            bool isGrounded = CheckIfCharacterControllerIsGrounded();

            // Allow moving the Rigidbody via physics.
            if (Interest == MovementInterest.CharacterControllerUntilAirborne && !isGrounded)
            {
                Interest = MovementInterest.RigidbodyUntilGrounded;
            }
            else if (Interest == MovementInterest.RigidbodyUntilGrounded
                && isGrounded
                && rigidbodyPhysicsMovement.sqrMagnitude <= 1E-06F
                && rigidbodySetFrameCount > 0
                && rigidbodySetFrameCount + 1 < Time.frameCount)
            {
                Interest = MovementInterest.CharacterControllerUntilAirborne;
            }

            // Handle walking up stairs/slopes via the CharacterController.
            if (isGrounded && facade.offset != null && characterControllerSourceMovement.y > 0f)
            {
                facade.offset.transform.position += Vector3.up * characterControllerSourceMovement.y;
            }

            MatchRigidbodyAndColliderWithCharacterController();

            RememberCurrentPositions();
            EmitIsGroundedChangedEvent(isGrounded);
        }

        protected virtual void Awake()
        {
            Physics.IgnoreCollision(characterController, rigidbodyCollider, true);
        }

        protected virtual void OnEnable()
        {
            Interest = MovementInterest.CharacterControllerUntilAirborne;
            MatchCharacterControllerWithSource(true);
            MatchRigidbodyAndColliderWithCharacterController();
            RememberCurrentPositions();
        }

        /// <summary>
        /// Changes the height and position of <see cref="characterController"/> to match <see cref="BodyRepresentationFacade.source"/>.
        /// </summary>
        /// <param name="setPositionDirectly">Whether to set the position directly or tell <see cref="characterController"/> to move to it.</param>
        protected virtual void MatchCharacterControllerWithSource(bool setPositionDirectly)
        {
            float height = facade.offset == null
                ? facade.source.transform.position.y
                : facade.offset.transform.InverseTransformPoint(facade.source.transform.position).y;
            height -= characterController.skinWidth;

            // CharacterController enforces a minimum height of twice its radius, so let's match that here.
            height = Mathf.Max(height, 2f * characterController.radius);

            Vector3 position = facade.source.transform.position;
            position.y -= height;

            if (facade.offset != null)
            {
                // The offset defines the source's "floor".
                position.y = Mathf.Max(position.y, facade.offset.transform.position.y + characterController.skinWidth);
            }

            if (setPositionDirectly)
            {
                characterController.transform.position = position;
            }
            else
            {
                characterController.Move(position - characterController.transform.position);
            }

            characterController.height = height;

            Vector3 center = characterController.center;
            center.y = height / 2f;
            characterController.center = center;
        }

        /// <summary>
        /// Changes <see cref="rigidbodyCollider"/> to match the collider settings of <see cref="characterController"/> and moves <see cref="rigidbody"/> to match <see cref="characterController"/>.
        /// </summary>
        protected virtual void MatchRigidbodyAndColliderWithCharacterController()
        {
            rigidbodyCollider.radius = characterController.radius;
            rigidbodyCollider.height = characterController.height + characterController.skinWidth;

            Vector3 center = characterController.center;
            center.y = (characterController.height - characterController.skinWidth) / 2f;
            rigidbodyCollider.center = center;

            rigidbody.position = characterController.transform.position;
        }

        /// <summary>
        /// Checks whether <see cref="characterController"/> is grounded.
        /// </summary>
        /// <remarks>
        /// <see cref="CharacterController.isGrounded"/> isn't accurate so this method does an additional check using <see cref="Physics"/>.
        /// </remarks>
        /// <returns>Whether <see cref="characterController"/> is grounded.</returns>
        protected virtual bool CheckIfCharacterControllerIsGrounded()
        {
            if (characterController.isGrounded)
            {
                return true;
            }

            return Physics
                .OverlapSphere(
                    characterController.transform.position
                    + (Vector3.up * (characterController.radius - characterController.skinWidth - 0.001f)),
                    characterController.radius)
                .Except(facade.ignoredColliders.EmptyIfNull())
                .Except(
                    new Collider[]
                    {
                        characterController, rigidbodyCollider
                    })
                .Any(
                    collider =>
                        !Physics.GetIgnoreLayerCollision(
                            collider.gameObject.layer,
                            characterController.gameObject.layer)
                        && !Physics.GetIgnoreLayerCollision(collider.gameObject.layer, rigidbody.gameObject.layer));
        }

        /// <summary>
        /// Updates the previous position variables to remember the current state.
        /// </summary>
        protected virtual void RememberCurrentPositions()
        {
            previousRigidbodyPosition = rigidbody.position;
        }

        /// <summary>
        /// Emits <see cref="BodyRepresentationFacade.BecameGrounded"/> or <see cref="BodyRepresentationFacade.BecameAirborne"/>.
        /// </summary>
        /// <param name="isCharacterControllerGrounded">The current state.</param>
        protected virtual void EmitIsGroundedChangedEvent(bool isCharacterControllerGrounded)
        {
            if (wasCharacterControllerGrounded == isCharacterControllerGrounded)
            {
                return;
            }

            wasCharacterControllerGrounded = isCharacterControllerGrounded;

            if (isCharacterControllerGrounded)
            {
                facade.BecameGrounded?.Invoke();
            }
            else
            {
                facade.BecameAirborne?.Invoke();
            }
        }
    }
}