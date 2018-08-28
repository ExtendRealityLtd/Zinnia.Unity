namespace VRTK.Core.Prefabs.Interactions.Interactors
{
    using UnityEngine;
    using VRTK.Core.Action;
    using VRTK.Core.Tracking.Collision;
    using VRTK.Core.Tracking.Collision.Active;
    using VRTK.Core.Tracking.Velocity;

    /// <summary>
    /// Sets up the Interactor Prefab grab settings based on the provided user settings.
    /// </summary>
    public class GrabInteractorInternalSetup : MonoBehaviour
    {
        [Header("Facade Settings")]

        /// <summary>
        /// The public interface facade.
        /// </summary>
        [Tooltip("The public interface facade.")]
        public InteractorFacade facade;

        [Header("Grab Settings")]

        /// <summary>
        /// The <see cref="BooleanAction"/> that will initiate the Interactor grab mechanism.
        /// </summary>
        [Tooltip("The BooleanAction that will initiate the Interactor grab mechanism.")]
        public BooleanAction grabAction;
        /// <summary>
        /// The point in which to attach a grabbed Interactable to the Interactor.
        /// </summary>
        [Tooltip("The point in which to attach a grabbed Interactable to the Interactor.")]
        public GameObject attachPoint;
        /// <summary>
        /// The <see cref="VelocityTrackerProcessor"/> to measure the interactors current velocity for throwing on release.
        /// </summary>
        [Tooltip("The VelocityTrackerProcessor to measure the interactors current velocity for throwing on release.")]
        public VelocityTrackerProcessor velocityTracker;
        /// <summary>
        /// The <see cref="ActiveCollisionPublisher"/> for checking valid start grabbing action.
        /// </summary>
        [Tooltip("The ActiveCollisionPublisher for checking valid start touching collisions.")]
        public ActiveCollisionPublisher startGrabbingPublisher;
        /// <summary>
        /// The <see cref="ActiveCollisionPublisher"/> for checking valid stop grabbing action.
        /// </summary>
        [Tooltip("The ActiveCollisionPublisher for checking valid stop touching collisions.")]
        public ActiveCollisionPublisher stopGrabbingPublisher;

        /// <summary>
        /// Configures the action used to control grabbing.
        /// </summary>
        public virtual void ConfigureGrabAction()
        {
            if (grabAction != null && facade != null && facade.grabAction != null)
            {
                grabAction.AddSource(facade?.grabAction);
            }
        }

        /// <summary>
        /// Configures the velocity tracker used for grabbing.
        /// </summary>
        public virtual void ConfigureVelocityTrackers()
        {
            if (velocityTracker != null && facade != null && facade.velocityTracker != null)
            {
                velocityTracker.velocityTrackers.Clear();
                velocityTracker.velocityTrackers.Add(facade.velocityTracker);
            }
        }

        /// <summary>
        /// Configures the <see cref="ActiveCollisionPublisher"/> components for touching and untouching.
        /// </summary>
        public virtual void ConfigurePublishers()
        {
            if (startGrabbingPublisher != null)
            {
                startGrabbingPublisher.payload.sourceContainer = attachPoint;
            }

            if (stopGrabbingPublisher != null)
            {
                stopGrabbingPublisher.payload.sourceContainer = attachPoint;
            }
        }

        protected virtual void OnEnable()
        {
            ConfigureGrabAction();
            ConfigureVelocityTrackers();
            ConfigurePublishers();
        }
    }
}