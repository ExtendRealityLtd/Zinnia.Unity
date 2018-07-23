namespace VRTK.Core.Prefabs.Interactions.Interactables
{
    using UnityEngine;
    using System.Collections.Generic;
    using VRTK.Core.Tracking.Collision;
    using VRTK.Core.Tracking.Collision.Active;
    using VRTK.Core.Prefabs.Interactions.Interactors;

    public class TouchInteractableInternalSetup : MonoBehaviour
    {
        [Header("Facade Settings")]

        /// <summary>
        /// The public interface facade.
        /// </summary>
        [Tooltip("The public interface facade.")]
        public InteractableFacade facade;

        [Header("Touch Settings")]

        /// <summary>
        /// The <see cref="ActiveCollisionsContainer"/> that holds all current collisions.
        /// </summary>
        [Tooltip("The ActiveCollisionsContainer that holds all current collisions.")]
        public ActiveCollisionsContainer activeCollisionsContainer;

        /// <summary>
        /// A collection of Interactors that are currently touching the Interactable.
        /// </summary>
        public List<InteractorFacade> TouchingInteractors => GetTouchingInteractors();

        /// <summary>
        /// Notifies that the Interactable is being touched.
        /// </summary>
        /// <param name="data">The touching object.</param>
        public virtual void NotifyTouch(CollisionNotifier.EventData data)
        {
            InteractorFacade interactor = InteractorFacade.TryGetFromGameObject(data.forwardSource.gameObject);
            if (interactor != null)
            {
                facade?.Touched?.Invoke(interactor);
            }
        }

        /// <summary>
        /// Notifies that the Interactable is being no longer touched.
        /// </summary>
        /// <param name="data">The previous touching object.</param>
        public virtual void NotifyUntouch(CollisionNotifier.EventData data)
        {
            InteractorFacade interactor = InteractorFacade.TryGetFromGameObject(data.forwardSource.gameObject);
            if (interactor != null)
            {
                facade?.Untouched?.Invoke(interactor);
            }
        }

        /// <summary>
        /// Retreives a collection of Interactor that are touching the Interactable.
        /// </summary>
        /// <returns>The touching Interactors.</returns>
        protected virtual List<InteractorFacade> GetTouchingInteractors()
        {
            List<InteractorFacade> returnList = new List<InteractorFacade>();

            if (activeCollisionsContainer == null)
            {
                return returnList;
            }

            foreach (CollisionNotifier.EventData element in activeCollisionsContainer.Elements)
            {
                InteractorFacade interactor = InteractorFacade.TryGetFromGameObject(element.forwardSource.gameObject);
                if (interactor != null)
                {
                    returnList.Add(interactor);
                }
            }

            return returnList;
        }
    }
}