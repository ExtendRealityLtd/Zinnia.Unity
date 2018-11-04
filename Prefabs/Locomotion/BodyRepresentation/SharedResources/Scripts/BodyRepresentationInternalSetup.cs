namespace VRTK.Core.Prefabs.Locomotion.BodyRepresentation
{
    using UnityEngine;
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using VRTK.Core.Process;
    using VRTK.Core.Extension;
    using VRTK.Core.Data.Collection;
    using VRTK.Core.Prefabs.Interactions.Interactors;
    using VRTK.Core.Prefabs.Interactions.Interactables;

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
        /// <summary>
        /// An observable list of GameObjects to ignore collisions on.
        /// </summary>
        [Tooltip("An observable list of GameObjects to ignore collisions on.")]
        public ObservableGameObjectList ignoredGameObjectCollisions;
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
        /// Movement to apply to <see cref="characterController"/> to resolve collisions.
        /// </summary>
        protected static readonly Vector3 collisionResolutionMovement = new Vector3(0.001f, 0f, 0f);
        /// <summary>
        /// The colliders to ignore body collisions with.
        /// </summary>
        protected HashSet<Collider> ignoredColliders = new HashSet<Collider>();
        /// <summary>
        /// The colliders to restore after an ungrab.
        /// </summary>
        protected HashSet<Collider> RestoreColliders = new HashSet<Collider>();
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
        /// <summary>
        /// Stores the routine for ignoring interactor collisions.
        /// </summary>
        protected Coroutine ignoreInteractorCollisions;

        private MovementInterest interest = MovementInterest.CharacterControllerUntilAirborne;

        /// <summary>
        /// Ignores collisions from the given <see cref="GameObject"/> with the <see cref="rigidbodyCollider"/> and <see cref="characterController"/>.
        /// </summary>
        /// <param name="toIgnore">The object to ignore collisions from.</param>
        public virtual void IgnoreCollisionsWith(GameObject toIgnore)
        {
            if (toIgnore == null)
            {
                return;
            }

            foreach (Collider foundCollider in toIgnore.GetComponentsInChildren<Collider>(true))
            {
                IgnoreCollisionsWith(foundCollider);
            }
        }

        /// <summary>
        /// Ignores collisions from the given <see cref="Collider"/> with the <see cref="rigidbodyCollider"/> and <see cref="characterController"/>.
        /// </summary>
        /// <param name="toIgnore">The collider to ignore collisions from.</param>
        public virtual void IgnoreCollisionsWith(Collider toIgnore)
        {
            if (toIgnore == null)
            {
                return;
            }

            if (!ignoredColliders.Contains(toIgnore))
            {
                Physics.IgnoreCollision(toIgnore, rigidbodyCollider, true);
                Physics.IgnoreCollision(toIgnore, characterController, true);
                ignoredColliders.Add(toIgnore);
            }
        }

        /// <summary>
        /// Resumes collisions with the given <see cref="GameObject"/> with the <see cref="rigidbodyCollider"/> and <see cref="characterController"/>.
        /// </summary>
        /// <param name="toResume">The object to resume collisions with.</param>
        public virtual void ResumeCollisionsWith(GameObject toResume)
        {
            if (toResume == null)
            {
                return;
            }

            foreach (Collider foundCollider in toResume.GetComponentsInChildren<Collider>(true))
            {
                ResumeCollisionsWith(foundCollider);
            }
        }

        /// <summary>
        /// Resumes collisions with the given <see cref="Collider"/> with the <see cref="rigidbodyCollider"/> and <see cref="characterController"/>.
        /// </summary>
        /// <param name="toResume">The collider to resume collisions with.</param>
        public virtual void ResumeCollisionsWith(Collider toResume)
        {
            if (toResume == null)
            {
                return;
            }

            if (ignoredColliders.Remove(toResume))
            {
                Physics.IgnoreCollision(toResume, rigidbodyCollider, false);
                Physics.IgnoreCollision(toResume, characterController, false);
            }
        }

        /// <summary>
        /// Positions, sizes and controls all variables necessary to make a body representation follow the given <see cref="BodyRepresentationFacade.source"/>.
        /// </summary>
        public virtual void Process()
        {
            if (!isActiveAndEnabled)
            {
                return;
            }

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

        protected virtual void Start()
        {
            IgnoreInteractorsCollisions();
        }

        /// <summary>
        /// Ignores all of the colliders on the interactor collection.
        /// </summary>
        protected virtual void IgnoreInteractorsCollisions()
        {
            foreach (InteractorFacade interactor in facade.IgnoredInteractors)
            {
                IgnoreInteractorCollision(interactor);
            }
        }

        /// <summary>
        /// Ignores all of the colliders on the given <see cref="InteractorFacade"/>.
        /// </summary>
        /// <param name="interactor">The interactor to ignore.</param>
        protected virtual void IgnoreInteractorCollision(InteractorFacade interactor)
        {
            ignoredGameObjectCollisions.AddToEnd(interactor.gameObject);
            interactor.Grabbed.AddListener(IgnoreInteractorGrabbedCollision);
            interactor.Ungrabbed.AddListener(ResumeInteractorUngrabbedCollision);
        }

        /// <summary>
        /// Ignores the interactable grabbed by the interactor.
        /// </summary>
        /// <param name="interactable">The interactable to ignore.</param>
        protected virtual void IgnoreInteractorGrabbedCollision(InteractableFacade interactable)
        {
            Collider[] interactableColliders = interactable.GetComponentsInChildren<Collider>(true);
            foreach (Collider toRestore in interactableColliders.Except(ignoredColliders))
            {
                RestoreColliders.Add(toRestore);
            }
            IgnoreCollisionsWith(interactable.gameObject);
        }

        /// <summary>
        /// Resumes the interactable ungrabbed by the interactor.
        /// </summary>
        /// <param name="interactable">The interactable to resume.</param>
        protected virtual void ResumeInteractorUngrabbedCollision(InteractableFacade interactable)
        {
            Collider[] interactableColliders = interactable.GetComponentsInChildren<Collider>(true);
            foreach (Collider resumeCollider in interactableColliders.Intersect(RestoreColliders))
            {
                ResumeCollisionsWith(resumeCollider);
                RestoreColliders.Remove(resumeCollider);
            }
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
                Vector3 movement = position - characterController.transform.position;
                // The CharacterController doesn't resolve any potential collisions in case we don't move it.
                characterController.Move(movement == Vector3.zero ? movement + collisionResolutionMovement : movement);
                if (movement == Vector3.zero)
                {
                    characterController.Move(movement - collisionResolutionMovement);
                }
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
                    characterController.transform.position + (Vector3.up * (characterController.radius - characterController.skinWidth - 0.001f)),
                    characterController.radius,
                    1 << characterController.gameObject.layer
                    )
                .Except(ignoredColliders.EmptyIfNull())
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