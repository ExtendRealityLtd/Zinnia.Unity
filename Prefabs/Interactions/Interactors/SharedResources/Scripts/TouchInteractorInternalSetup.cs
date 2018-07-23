namespace VRTK.Core.Prefabs.Interactions.Interactors
{
    using UnityEngine;
    using System.Collections.Generic;
    using VRTK.Core.Extension;
    using VRTK.Core.Tracking.Collision;
    using VRTK.Core.Tracking.Collision.Active;
    using VRTK.Core.Tracking.Collision.Active.Operation;

    /// <summary>
    /// Sets up the Interactor Prefab touch settings based on the provided user settings.
    /// </summary>
    public class TouchInteractorInternalSetup : MonoBehaviour
    {
        [Header("Facade Settings")]

        /// <summary>
        /// The public interface facade.
        /// </summary>
        [Tooltip("The public interface facade.")]
        public InteractorFacade facade;

        [Header("Touch Settings")]

        /// <summary>
        /// The <see cref="ActiveCollisionsContainer"/> that holds all current collisions.
        /// </summary>
        [Tooltip("The ActiveCollisionsContainer that holds all current collisions.")]
        public ActiveCollisionsContainer activeCollisionsContainer;
        /// <summary>
        /// The <see cref="Slicer"/> that holds the current active collision.
        /// </summary>
        [Tooltip("The Slicer that holds the current active collision.")]
        public Slicer currentActiveCollision;
        /// <summary>
        /// The <see cref="ActiveCollisionPublisher"/> for checking valid start touching collisions.
        /// </summary>
        [Tooltip("The ActiveCollisionPublisher for checking valid start touching collisions.")]
        public ActiveCollisionPublisher startTouchingPublisher;
        /// <summary>
        /// The <see cref="ActiveCollisionPublisher"/> for checking valid stop touching collisions.
        /// </summary>
        [Tooltip("The ActiveCollisionPublisher for checking valid stop touching collisions.")]
        public ActiveCollisionPublisher stopTouchingPublisher;

        /// <summary>
        /// A collection of currently touched GameObjects.
        /// </summary>
        public List<GameObject> TouchedObjects => GetTouchedObjects();
        /// <summary>
        /// The currently active touched GameObject.
        /// </summary>
        public GameObject ActiveTouchedObject => GetActiveTouchedObject();

        /// <summary>
        /// Sets up the Interactor prefab with the specified settings.
        /// </summary>
        public virtual void Setup()
        {
            if (InvalidParameters())
            {
                return;
            }

            startTouchingPublisher.sourceContainer = facade.interactorContainer;
            stopTouchingPublisher.sourceContainer = facade.interactorContainer;
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

            startTouchingPublisher.sourceContainer = null;
            stopTouchingPublisher.sourceContainer = null;
        }

        /// <summary>
        /// Notifies that the Interactor is touching an object.
        /// </summary>
        /// <param name="data">The collision data.</param>
        public virtual void NotifyTouch(CollisionNotifier.EventData data)
        {
            facade?.Touched?.Invoke(data);
        }

        /// <summary>
        /// Notifies that the Interactor is no longer touching the object.
        /// </summary>
        /// <param name="data">The collision data.</param>
        public virtual void NotifyUntouch(CollisionNotifier.EventData data)
        {
            facade?.Untouched?.Invoke(data);
        }

        /// <summary>
        /// Retreives a collection of currently touched GameObjects.
        /// </summary>
        /// <returns>The currently touched GameObjects.</returns>
        protected virtual List<GameObject> GetTouchedObjects()
        {
            List<GameObject> returnList = new List<GameObject>();

            if (activeCollisionsContainer == null)
            {
                return returnList;
            }

            foreach (CollisionNotifier.EventData element in activeCollisionsContainer.Elements)
            {
                returnList.Add(element.collider.GetContainingTransform().gameObject);
            }

            return returnList;
        }

        /// <summary>
        /// Retreives the currently active touched GameObject.
        /// </summary>
        /// <returns>The currently active touched GameObject.</returns>
        protected virtual GameObject GetActiveTouchedObject()
        {
            if (currentActiveCollision == null || currentActiveCollision.SlicedList.activeCollisions.Count == 0)
            {
                return null;
            }

            return currentActiveCollision.SlicedList.activeCollisions[0].collider.GetContainingTransform().gameObject;
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
            return (activeCollisionsContainer == null || currentActiveCollision == null || startTouchingPublisher == null || stopTouchingPublisher == null || facade == null || facade.interactorContainer == null || facade.grabAction == null || facade.velocityTracker == null);
        }
    }
}