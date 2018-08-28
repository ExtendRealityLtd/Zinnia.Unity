namespace VRTK.Core.Prefabs.Interactions.Interactors
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using System.Collections.Generic;
    using VRTK.Core.Action;
    using VRTK.Core.Tracking.Velocity;
    using VRTK.Core.Prefabs.Interactions.Interactables;
    using VRTK.Core.Data.Attribute;

    /// <summary>
    /// The public interface into the Interactable Prefab.
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
        /// Emitted when a new collision occurs.
        /// </summary>
        [Header("Interactor Events")]
        public UnityEvent Touched = new UnityEvent();
        /// <summary>
        /// Emitted when an existing collision ends.
        /// </summary>
        public UnityEvent Untouched = new UnityEvent();
        /// <summary>
        /// Emitted when a new collision occurs.
        /// </summary>
        public UnityEvent Grabbed = new UnityEvent();
        /// <summary>
        /// Emitted when an existing collision ends.
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
        /// Attempts to retrieve from a given GameObject.
        /// </summary>
        /// <param name="data">The GameObject to retreive from.</param>
        /// <param name="searchChildren">Optionally searches the children of the given GameObject.</param>
        /// <param name="searchParents">Optionally searches the parents of the given GameObject.</param>
        /// <returns>The found Interactor.</returns>
        public static InteractorFacade TryGetFromGameObject(GameObject data, bool searchChildren = true, bool searchParents = true)
        {
            InteractorFacade returnFacade = data.GetComponent<InteractorFacade>();

            if (searchChildren && returnFacade == null)
            {
                returnFacade = data.GetComponentInChildren<InteractorFacade>();
            }

            if (searchParents && returnFacade == null)
            {
                returnFacade = data.GetComponentInParent<InteractorFacade>();
            }

            return returnFacade;
        }
    }
}