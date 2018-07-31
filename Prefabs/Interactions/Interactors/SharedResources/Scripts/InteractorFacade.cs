namespace VRTK.Core.Prefabs.Interactions.Interactors
{
    using UnityEngine;
    using System.Collections.Generic;
    using VRTK.Core.Action;
    using VRTK.Core.Tracking.Velocity;
    using VRTK.Core.Tracking.Collision;

    /// <summary>
    /// The public interface into the Interactable Prefab.
    /// </summary>
    public class InteractorFacade : MonoBehaviour
    {
        [Header("Interactor Settings")]

        /// <summary>
        /// The parent container of the Interactor prefab.
        /// </summary>
        [Tooltip("The parent container of the Interactor prefab.")]
        public GameObject interactorContainer;
        /// <summary>
        /// The <see cref="BooleanAction"/> that will initiate the Interactor grab mechanism.
        /// </summary>
        [Tooltip("The BooleanAction that will initiate the Interactor grab mechanism.")]
        public BooleanAction grabAction;
        /// <summary>
        /// The <see cref="VelocityTrackerProcessor"/> to measure the interactors current velocity.
        /// </summary>
        [Tooltip("The VelocityTrackerProcessor to measure the interactors current velocity.")]
        public VelocityTrackerProcessor velocityTracker;

        [Header("Interactor Events")]

        /// <summary>
        /// Emitted when a new collision occurs.
        /// </summary>
        public CollisionNotifier.UnityEvent Touched = new CollisionNotifier.UnityEvent();
        /// <summary>
        /// Emitted when an existing collision ends.
        /// </summary>
        public CollisionNotifier.UnityEvent Untouched = new CollisionNotifier.UnityEvent();
        /// <summary>
        /// Emitted when a new collision occurs.
        /// </summary>
        public CollisionNotifier.UnityEvent Grabbed = new CollisionNotifier.UnityEvent();
        /// <summary>
        /// Emitted when an existing collision ends.
        /// </summary>
        public CollisionNotifier.UnityEvent Ungrabbed = new CollisionNotifier.UnityEvent();

        [Header("Internal Settings")]

        /// <summary>
        /// **DO NOT CHANGE** - The linked Touch Internal Setup.
        /// </summary>
        [Tooltip("**DO NOT CHANGE** - The linked Touch Internal Setup.")]
        public TouchInteractorInternalSetup touchInteractorSetup;
        /// <summary>
        /// **DO NOT CHANGE** - The linked Grab Internal Setup.
        /// </summary>
        [Tooltip("**DO NOT CHANGE** - The linked Grab Internal Setup.")]
        public GrabInteractorInternalSetup grabInteractorSetup;

        /// <summary>
        /// A collection of currently touched GameObjects.
        /// </summary>
        public List<GameObject> TouchedObjects => touchInteractorSetup?.TouchedObjects;
        /// <summary>
        /// The currently active touched GameObject.
        /// </summary>
        public GameObject ActiveTouchedObject => touchInteractorSetup?.ActiveTouchedObject;
        /// <summary>
        /// The currently active grabbed GameObject.
        /// </summary>
        public GameObject ActiveGrabbedObject => GetActiveGrabbedObject();

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

        /// <summary>
        /// Retreives the currently active grabbed GameObject.
        /// </summary>
        /// <returns>The currently active grabbed GameObject.</returns>
        protected virtual GameObject GetActiveGrabbedObject()
        {
            if (grabAction == null || !grabAction.Value)
            {
                return null;
            }

            return ActiveTouchedObject;
        }
    }
}