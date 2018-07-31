namespace VRTK.Core.Prefabs.Interactions.Interactors
{
    using UnityEngine;
    using VRTK.Core.Action;
    using VRTK.Core.Tracking.Collision;
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
        /// Sets up the Interactor prefab with the specified settings.
        /// </summary>
        public virtual void Setup()
        {
            if (InvalidParameters())
            {
                return;
            }

            grabAction.AddSource(facade.grabAction);
            velocityTracker.velocityTrackers.Clear();
            velocityTracker.velocityTrackers.Add(facade.velocityTracker);
        }

        /// <summary>
        /// Clears all of the settings from the Interactor prefab.
        /// </summary>
        public virtual void Clear()
        {
            if (InvalidParameters())
            {
                return;
            }

            grabAction.ClearSources();
            velocityTracker.velocityTrackers.Clear();
        }

        /// <summary>
        /// Determines an object is being grabbed.
        /// </summary>
        /// <param name="data">The collision data.</param>
        public virtual void Grab(CollisionNotifier.EventData data)
        {
            facade?.Grabbed?.Invoke(data);
        }

        /// <summary>
        /// Determines an object is being ungrabbed.
        /// </summary>
        /// <param name="data">The collision data.</param>
        public virtual void Ungrab(CollisionNotifier.EventData data)
        {
            facade?.Ungrabbed?.Invoke(data);
        }

        protected virtual void OnEnable()
        {
            Setup();
        }

        protected virtual void OnDisable()
        {
            Clear();
        }

        /// <summary>
        /// Determines if the setup parameters are invalid.
        /// </summary>
        /// <returns><see langword="true"/> if the parameters are invalid.</returns>
        protected virtual bool InvalidParameters()
        {
            return (grabAction == null || attachPoint == null || velocityTracker == null || facade == null || facade.interactorContainer == null || facade.grabAction == null || facade.velocityTracker == null);
        }
    }
}