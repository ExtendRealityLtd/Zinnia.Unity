namespace VRTK.Core.Prefabs.Interactions.Interactables
{
    using UnityEngine;
    using System.Collections.Generic;
    using VRTK.Core.Rule;
    using VRTK.Core.Extension;
    using VRTK.Core.Data.Collection;
    using VRTK.Core.Prefabs.Interactions.Interactors;

    public class TouchInteractableInternalSetup : MonoBehaviour
    {
        #region Facade Settings
        /// <summary>
        /// The public interface facade.
        /// </summary>
        [Header("Facade Settings"), Tooltip("The public interface facade.")]
        public InteractableFacade facade;
        #endregion

        #region Touch Settings
        /// <summary>
        /// The <see cref="GameObjectSet"/> that holds the current touching objects data.
        /// </summary>
        [Header("Touch Settings"), Tooltip("The GameObjectSet that holds the current touching objects data.")]
        public GameObjectSet currentTouchingObjects;
        /// <summary>
        /// The <see cref="ListContainsRule"/> used to determine the touch validity.
        /// </summary>
        [Tooltip("The ListContainsRule used to determine the touch validity.")]
        public ListContainsRule touchValidity;
        #endregion

        /// <summary>
        /// A collection of Interactors that are currently touching the Interactable.
        /// </summary>
        public List<InteractorFacade> TouchingInteractors => GetTouchingInteractors();

        /// <summary>
        /// Configures the interactor touch validity.
        /// </summary>
        public virtual void ConfigureTouchValidity()
        {
            if (facade?.disallowedTouchInteractors == null || touchValidity == null)
            {
                return;
            }

            touchValidity.objects.Clear();

            foreach (InteractorFacade interactor in facade.disallowedTouchInteractors)
            {
                touchValidity.objects.Add(interactor.gameObject);
            }
        }

        /// <summary>
        /// Notifies that the Interactable is being touched.
        /// </summary>
        /// <param name="data">The touching object.</param>
        public virtual void NotifyTouch(GameObject data)
        {
            InteractorFacade interactor = data.TryGetComponent<InteractorFacade>(true, true);
            if (interactor != null)
            {
                if (facade?.TouchingInteractors.Count == 1)
                {
                    facade?.FirstTouched?.Invoke(interactor);
                }
                facade?.Touched?.Invoke(interactor);
                interactor.Touched?.Invoke(facade);
            }
        }

        /// <summary>
        /// Notifies that the Interactable is being no longer touched.
        /// </summary>
        /// <param name="data">The previous touching object.</param>
        public virtual void NotifyUntouch(GameObject data)
        {
            InteractorFacade interactor = data.TryGetComponent<InteractorFacade>(true, true);
            if (interactor != null)
            {
                facade?.Untouched?.Invoke(interactor);
                interactor.Untouched?.Invoke(facade);
                if (facade?.TouchingInteractors.Count == 0)
                {
                    facade?.LastUntouched?.Invoke(interactor);
                }
            }
        }

        protected virtual void OnEnable()
        {
            ConfigureTouchValidity();
        }

        /// <summary>
        /// Retreives a collection of Interactors that are touching the Interactable.
        /// </summary>
        /// <returns>The touching Interactors.</returns>
        protected virtual List<InteractorFacade> GetTouchingInteractors()
        {
            List<InteractorFacade> returnList = new List<InteractorFacade>();

            if (currentTouchingObjects == null)
            {
                return returnList;
            }

            foreach (GameObject element in currentTouchingObjects.Elements)
            {
                InteractorFacade interactor = element.TryGetComponent<InteractorFacade>(true, true);
                if (interactor != null)
                {
                    returnList.Add(interactor);
                }
            }

            return returnList;
        }
    }
}