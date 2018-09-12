namespace VRTK.Core.Prefabs.Interactions.Interactors
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using System.Collections.Generic;
    using VRTK.Core.Action;
    using VRTK.Core.Extension;
    using VRTK.Core.Data.Attribute;
    using VRTK.Core.Tracking.Velocity;
    using VRTK.Core.Prefabs.Interactions.Interactables;

    /// <summary>
    /// The public interface into the Interactor Prefab.
    /// </summary>
    public class InteractorFacade : MonoBehaviour
    {
        /// <summary>
        /// Defines the event with the <see cref="InteractableFacade"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<InteractableFacade>
        {
        }

        #region Interactor Settings
        /// <summary>
        /// The <see cref="BooleanAction"/> that will initiate the Interactor grab mechanism.
        /// </summary>
        [Header("Interactor Settings"), Tooltip("The BooleanAction that will initiate the Interactor grab mechanism.")]
        public BooleanAction grabAction;
        /// <summary>
        /// The <see cref="VelocityTrackerProcessor"/> to measure the interactors current velocity.
        /// </summary>
        [Tooltip("The VelocityTrackerProcessor to measure the interactors current velocity.")]
        public VelocityTrackerProcessor velocityTracker;
        #endregion

        #region Interactor Events
        /// <summary>
        /// Emitted when the Interactor starts touching a valid Interactable.
        /// </summary>
        [Header("Interactor Events")]
        public UnityEvent Touched = new UnityEvent();
        /// <summary>
        /// Emitted when the Interactor stops touching a valid Interactable.
        /// </summary>
        public UnityEvent Untouched = new UnityEvent();
        /// <summary>
        /// Emitted when the Interactor starts grabbing a valid Interactable.
        /// </summary>
        public UnityEvent Grabbed = new UnityEvent();
        /// <summary>
        /// Emitted when the Interactor stops grabbing a valid Interactable.
        /// </summary>
        public UnityEvent Ungrabbed = new UnityEvent();
        #endregion

        #region Internal Settings
        /// <summary>
        /// The linked Touch Internal Setup.
        /// </summary>
        [Header("Internal Settings"), Tooltip("The linked Touch Internal Setup."), InternalSetting]
        public TouchInteractorInternalSetup touchInteractorSetup;
        /// <summary>
        /// The linked Grab Internal Setup.
        /// </summary>
        [Tooltip("The linked Grab Internal Setup."), InternalSetting]
        public GrabInteractorInternalSetup grabInteractorSetup;
        #endregion

        /// <summary>
        /// A collection of currently touched GameObjects.
        /// </summary>
        public List<GameObject> TouchedObjects => touchInteractorSetup?.TouchedObjects;
        /// <summary>
        /// The currently active touched GameObject.
        /// </summary>
        public GameObject ActiveTouchedObject => touchInteractorSetup?.ActiveTouchedObject;
        /// <summary>
        /// A collection of currently grabbed GameObjects.
        /// </summary>
        public List<GameObject> GrabbedObjects => grabInteractorSetup?.GrabbedObjects;

        /// <summary>
        /// Attempt to grab a <see cref="GameObject"/> that contains an Interactable to the current Interactor.
        /// </summary>
        /// <param name="interactor">The GameObject that the Interactor is on.</param>
        public virtual void Grab(GameObject interactable)
        {
            Grab(interactable.TryGetComponent<InteractableFacade>(true, true));
        }

        /// <summary>
        /// Attempt to grab an Interactable to the current Interactor.
        /// </summary>
        /// <param name="interactable">The Interactable to attempt to grab.</param>
        public virtual void Grab(InteractableFacade interactable)
        {
            Grab(interactable, null, null);
        }

        /// <summary>
        /// Attempt to grab an Interactable to the current Interactor utilising custom collision data.
        /// </summary>
        /// <param name="interactable">The Interactable to attempt to grab.</param>
        /// <param name="collision">Custom collision data.</param>
        /// <param name="collider">Custom collider data.</param>
        public virtual void Grab(InteractableFacade interactable, Collision collision, Collider collider)
        {
            grabInteractorSetup?.Grab(interactable, collision, collider);
        }

        /// <summary>
        /// Attempt to ungrab currently grabbed Interactables to the current Interactor.
        /// </summary>
        public virtual void Ungrab()
        {
            grabInteractorSetup?.Ungrab();
        }
    }
}