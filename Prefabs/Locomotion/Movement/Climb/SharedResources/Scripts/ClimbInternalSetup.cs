namespace VRTK.Core.Prefabs.Locomotion.Movement.Climb
{
    using System.Linq;
    using UnityEngine;
    using VRTK.Core.Data.Collection;
    using VRTK.Core.Data.Type.Transformation;
    using VRTK.Core.Data.Operation;
    using VRTK.Core.Prefabs.Locomotion.BodyRepresentation;
    using VRTK.Core.Tracking.Follow;
    using VRTK.Core.Tracking.Velocity;

    /// <summary>
    /// Sets up the Climb prefab based on the provided user settings.
    /// </summary>
    public class ClimbInternalSetup : MonoBehaviour
    {
        #region Facade Settings
        /// <summary>
        /// The public interface facade.
        /// </summary>
        [Header("Facade Settings"), Tooltip("The public interface facade.")]
        public ClimbFacade facade;
        #endregion

        #region Reference Settings
        /// <summary>
        /// The objects defining the sources of movement.
        /// </summary>
        [Header("Reference Settings"), Tooltip("The objects defining the sources of movement.")]
        public GameObjectObservableList interactors;
        /// <summary>
        /// The objects defining the offsets of movement.
        /// </summary>
        [Tooltip("The objects defining the offsets of movement.")]
        public GameObjectObservableList interactables;
        /// <summary>
        /// The <see cref="ObjectDistanceComparator"/> component for the source.
        /// </summary>
        [Tooltip("The ObjectDistanceComparator component for the source.")]
        public ObjectDistanceComparator sourceDistanceComparator;
        /// <summary>
        /// The <see cref="ObjectDistanceComparator"/> component for the offset.
        /// </summary>
        [Tooltip("The ObjectDistanceComparator component for the offset.")]
        public ObjectDistanceComparator offsetDistanceComparator;
        /// <summary>
        /// The <see cref="TransformPositionMutator"/> component for the offset.
        /// </summary>
        [Tooltip("The TransformPositionMutator component for the target.")]
        public TransformPositionMutator targetPositionProperty;
        /// <summary>
        /// The <see cref="ComponentTrackerProxy"/> component for obtaining velocity data.
        /// </summary>
        [Tooltip("The ComponentTrackerProxy component for obtaining velocity data.")]
        public ComponentTrackerProxy velocityProxy;
        /// <summary>
        /// The <see cref="VelocityEmitter"/> component for emitting velocity data.
        /// </summary>
        [Tooltip("The VelocityEmitter component for emitting velocity data.")]
        public VelocityEmitter velocityEmitter;
        /// <summary>
        /// The <see cref="Vector3Multiplier"/> component for multiplying velocity data.
        /// </summary>
        [Tooltip("The Vector3Multiplier component for multiplying velocity data.")]
        public Vector3Multiplier velocityMultiplier;
        #endregion

        /// <summary>
        /// Applies velocity to the <see cref="BodyRepresentation"/>.
        /// </summary>
        public virtual void ApplyVelocity()
        {
            if (!isActiveAndEnabled || interactors.Elements.Any() || velocityProxy.proxySource == null)
            {
                return;
            }

            velocityEmitter.EmitVelocity();
            facade.bodyRepresentationFacade.ListenToRigidbodyMovement();
            facade.bodyRepresentationFacade.PhysicsBody.velocity += velocityMultiplier.Result;
            velocityProxy.ClearProxySource();
        }

        protected virtual void OnEnable()
        {
            interactors.BecamePopulated.AddListener(OnListBecamePopulated);
            interactors.ElementAdded.AddListener(OnInteractorAdded);
            interactors.ElementRemoved.AddListener(OnInteractorRemoved);
            interactors.BecameEmpty.AddListener(OnListBecameEmpty);

            interactables.ElementAdded.AddListener(OnInteractableAdded);
            interactables.ElementRemoved.AddListener(OnInteractableRemoved);

            sourceDistanceComparator.enabled = false;
            offsetDistanceComparator.enabled = false;

            targetPositionProperty.target = facade.bodyRepresentationFacade.Offset == null
                ? facade.bodyRepresentationFacade.Source
                : facade.bodyRepresentationFacade.Offset;
        }

        protected virtual void OnDisable()
        {
            targetPositionProperty.target = null;

            offsetDistanceComparator.enabled = false;
            sourceDistanceComparator.enabled = false;

            interactables.ElementRemoved.RemoveListener(OnInteractableRemoved);
            interactables.ElementAdded.RemoveListener(OnInteractableAdded);

            interactors.BecameEmpty.RemoveListener(OnListBecameEmpty);
            interactors.ElementRemoved.RemoveListener(OnInteractorRemoved);
            interactors.ElementAdded.RemoveListener(OnInteractorAdded);
            interactors.BecamePopulated.RemoveListener(OnListBecamePopulated);
        }

        protected virtual void OnListBecamePopulated(GameObject addedElement)
        {
            if (interactors.Elements.Any() || interactables.Elements.Any())
            {
                facade.ClimbStarted?.Invoke();
            }
        }

        protected virtual void OnListBecameEmpty(GameObject removedElement)
        {
            if (!interactors.Elements.Any() && !interactables.Elements.Any())
            {
                facade.ClimbStopped?.Invoke();
            }
        }

        protected virtual void OnInteractorAdded(GameObject interactor)
        {
            sourceDistanceComparator.source = interactor;
            sourceDistanceComparator.target = interactor;
            sourceDistanceComparator.enabled = interactor != null;
            sourceDistanceComparator.SavePosition();

            if (interactor != null)
            {
                facade.bodyRepresentationFacade.Interest = BodyRepresentationInternalSetup.MovementInterest.CharacterController;
            }
        }

        protected virtual void OnInteractorRemoved(GameObject interactor)
        {
            OnInteractorAdded(interactors.Elements.LastOrDefault());
        }

        protected virtual void OnInteractableAdded(GameObject interactable)
        {
            offsetDistanceComparator.source = interactable;
            offsetDistanceComparator.target = interactable;
            offsetDistanceComparator.enabled = interactable != null;
            offsetDistanceComparator.SavePosition();

            if (interactable != null)
            {
                facade.bodyRepresentationFacade.Interest = BodyRepresentationInternalSetup.MovementInterest.CharacterController;
            }
        }

        protected virtual void OnInteractableRemoved(GameObject interactable)
        {
            OnInteractableAdded(interactables.Elements.LastOrDefault());
        }
    }
}