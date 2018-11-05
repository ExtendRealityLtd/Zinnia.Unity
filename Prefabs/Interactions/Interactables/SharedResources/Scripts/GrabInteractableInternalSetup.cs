namespace VRTK.Core.Prefabs.Interactions.Interactables
{
    using UnityEngine;
    using System.Collections.Generic;
    using VRTK.Core.Extension;
    using VRTK.Core.Event;
    using VRTK.Core.Rule;
    using VRTK.Core.Tracking.Follow;
    using VRTK.Core.Data.Collection;
    using VRTK.Core.Prefabs.Interactions.Interactors;
    using VRTK.Core.Tracking.Follow.Modifier;
    using VRTK.Core.Tracking.Collision.Active;

    /// <summary>
    /// Sets up the Interactable Prefab grab settings based on the provided user settings.
    /// </summary>
    public class GrabInteractableInternalSetup : MonoBehaviour
    {
        #region Facade Settings
        /// <summary>
        /// The public interface facade.
        /// </summary>
        [Header("Facade Settings"), Tooltip("The public interface facade.")]
        public InteractableFacade facade;
        #endregion

        #region Grab Settings
        /// <summary>
        /// The <see cref="GameObjectEventStack"/> that deals with the grabbing actions.
        /// </summary>
        [Header("Grab Settings"), Tooltip("The GameObjectEventStack that deals with the grabbing actions.")]
        public GameObjectEventStack gameObjectEventStack;
        /// <summary>
        /// The <see cref="GameObjectSet"/> that deals with the grabbing actions.
        /// </summary>
        [Tooltip("The GameObjectSet that deals with the grabbing actions.")]
        public GameObjectSet gameObjectSet;
        #endregion

        #region Attachment Logic
        /// <summary>
        /// The <see cref="GameObject"/> that contains the precision grab mode logic.
        /// </summary>
        [Header("Attachment Logic"), Tooltip("The GameObject that contains the precision grab mode logic.")]
        public GameObject precisionGrabLogic;
        /// <summary>
        /// The <see cref="GameObject"/> that contains the snap handles logic.
        /// </summary>
        [Tooltip("The GameObject that contains the snap handles logic.")]
        public GameObject snapHandleLogic;
        /// <summary>
        /// The <see cref="ObjectFollower"/> script used for the attachment logic.
        /// </summary>
        [Tooltip("The ObjectFollower script used for the attachment logic.")]
        public ObjectFollower attachmentLogic;
        #endregion

        #region Tracking Logic
        /// <summary>
        /// The <see cref="FollowModifier"/> that deals with the Transform Follow tracking.
        /// </summary>
        [Header("Tracking Logic"), Tooltip("The FollowModifier that deals with the Transform Follow tracking.")]
        public FollowModifier TransformTracking;
        /// <summary>
        /// The <see cref="FollowModifier"/> that deals with the Rigidbody Velocity tracking.
        /// </summary>
        [Tooltip("The FollowModifier that deals with the Rigidbody Velocity tracking.")]
        public FollowModifier RigidbodyTracking;
        #endregion

        #region Grab Action Settings
        /// <summary>
        /// The <see cref="ActiveCollisionConsumerEventProxyEmitter"/> that deals with grabbing flow.
        /// </summary>
        [Header("Grab Action Settings"), Tooltip("The ActiveCollisionConsumerEventProxyEmitter that deals with grabbing flow.")]
        public ActiveCollisionConsumerEventProxyEmitter doGrab;
        /// <summary>
        /// The <see cref="ActiveCollisionConsumerEventProxyEmitter"/> that deals with ungrabbing flow.
        /// </summary>
        [Tooltip("The ActiveCollisionConsumerEventProxyEmitter that deals with ungrabbing flow.")]
        public ActiveCollisionConsumerEventProxyEmitter doUngrab;
        /// <summary>
        /// The <see cref="ListContainsRule"/> used to determine the grab validity.
        /// </summary>
        [Tooltip("The ListContainsRule used to determine the grab validity.")]
        public ListContainsRule grabValidity;
        #endregion

        #region Active Type Settings
        /// <summary>
        /// The <see cref="GameObject"/> containing the logic for starting HoldTillRelease grabbing.
        /// </summary>
        [Header("Active Type Settings"), Tooltip("The GameObject containing the logic for starting HoldTillRelease grabbing.")]
        public GameObject StartStateGrab;
        /// <summary>
        /// The <see cref="GameObject"/> containing the logic for ending HoldTillRelease grabbing.
        /// </summary>
        [Tooltip("The GameObject containing the logic for ending HoldTillRelease grabbing.")]
        public GameObject StopStateGrab;
        /// <summary>
        /// The <see cref="GameObject"/> containing the logic for starting and ending Toggle grabbing.
        /// </summary>
        [Tooltip("The GameObject containing the logic for starting and ending Toggle grabbing")]
        public GameObject ToggleGrab;
        #endregion

        /// <summary>
        /// A collection of Interactors that are currently grabbing the Interactable.
        /// </summary>
        public List<InteractorFacade> GrabbingInteractors => GetGrabbingInteractors();

        /// <summary>
        /// Configures the interactor grab validity.
        /// </summary>
        public virtual void ConfigureGrabValidity()
        {
            if (facade?.disallowedGrabInteractors == null || grabValidity == null)
            {
                return;
            }

            grabValidity.objects.Clear();

            foreach (InteractorFacade interactor in facade.disallowedGrabInteractors)
            {
                if (interactor.grabInteractorSetup?.attachPoint != null)
                {
                    grabValidity.objects.Add(interactor.grabInteractorSetup.attachPoint);
                }
            }
        }

        /// <summary>
        /// Configures the Grab Offset to be used.
        /// </summary>
        public virtual void ConfigureGrabOffset()
        {
            switch (facade?.grabOffset)
            {
                case InteractableFacade.GrabOffset.None:
                    precisionGrabLogic.TrySetActive(false);
                    snapHandleLogic.TrySetActive(false);
                    break;
                case InteractableFacade.GrabOffset.PrecisionGrab:
                    precisionGrabLogic.TrySetActive(true);
                    snapHandleLogic.TrySetActive(false);
                    break;
                case InteractableFacade.GrabOffset.SnapHandle:
                    precisionGrabLogic.TrySetActive(false);
                    snapHandleLogic.TrySetActive(true);
                    break;
            }
        }

        /// <summary>
        /// Configures the Tracking Type to be used.
        /// </summary>
        public virtual void ConfigureTrackingType()
        {
            if (attachmentLogic == null)
            {
                return;
            }

            switch (facade?.trackingType)
            {
                case InteractableFacade.TrackingType.FollowTransform:
                    RigidbodyTracking?.gameObject.SetActive(false);
                    TransformTracking?.gameObject.SetActive(true);
                    attachmentLogic.followModifier = TransformTracking;
                    break;
                case InteractableFacade.TrackingType.FollowRigidbody:
                    TransformTracking?.gameObject.SetActive(false);
                    RigidbodyTracking?.gameObject.SetActive(true);
                    attachmentLogic.followModifier = RigidbodyTracking;
                    break;
            }
        }

        /// <summary>
        /// Configures the Grab Type to be used.
        /// </summary>
        public virtual void ConfigureGrabType()
        {
            switch (facade?.grabType)
            {
                case InteractableFacade.ActiveType.HoldTillRelease:
                    StartStateGrab.TrySetActive(true);
                    StopStateGrab.TrySetActive(true);
                    ToggleGrab.TrySetActive(false);
                    break;
                case InteractableFacade.ActiveType.Toggle:
                    StartStateGrab.TrySetActive(false);
                    StopStateGrab.TrySetActive(false);
                    ToggleGrab.TrySetActive(true);
                    break;
            }
        }

        /// <summary>
        /// Attempt to grab the Interactable to the given Interactor.
        /// </summary>
        /// <param name="interactor">The Interactor to attach the Interactable to.</param>
        public virtual void Grab(InteractorFacade interactor)
        {
            interactor?.Grab(facade);
        }

        /// <summary>
        /// Attempt to ungrab the Interactable.
        /// </summary>
        /// <param name="sequenceIndex">The Interactor sequence index to ungrab from.</param>
        public virtual void Ungrab(int sequenceIndex = 0)
        {
            List<InteractorFacade> currentInteractors = GetGrabbingInteractors();
            if (currentInteractors == null || currentInteractors.Count == 0 || sequenceIndex >= currentInteractors.Count)
            {
                return;
            }

            Ungrab(currentInteractors[sequenceIndex]);
        }

        /// <summary>
        /// Attempts to ungrab the Interactable.
        /// </summary>
        /// <param name="interactor">The Interactor to ungrab from.</param>
        public virtual void Ungrab(InteractorFacade interactor)
        {
            interactor?.Ungrab();
        }

        /// <summary>
        /// Notifies that the Interactable is being grabbed.
        /// </summary>
        /// <param name="data">The grabbing object.</param>
        public virtual void NotifyGrab(GameObject data)
        {
            InteractorFacade interactor = data.TryGetComponent<InteractorFacade>(true, true);
            if (interactor != null)
            {
                if (facade?.GrabbingInteractors.Count == 1)
                {
                    facade?.FirstGrabbed?.Invoke(interactor);
                }
                facade?.Grabbed?.Invoke(interactor);
                interactor.Grabbed?.Invoke(facade);
                interactor.grabInteractorSetup?.grabbedObjects?.AddElement(facade?.gameObject);
            }
        }

        /// <summary>
        /// Notifies that the Interactable is no longer being grabbed.
        /// </summary>
        /// <param name="data">The previous grabbing object.</param>
        public virtual void NotifyUngrab(GameObject data)
        {
            InteractorFacade interactor = data.TryGetComponent<InteractorFacade>(true, true);
            if (interactor != null)
            {
                facade?.Ungrabbed?.Invoke(interactor);
                interactor.Ungrabbed?.Invoke(facade);
                interactor.grabInteractorSetup?.grabbedObjects?.RemoveElement(facade?.gameObject);
                if (facade?.GrabbingInteractors.Count == 0)
                {
                    facade?.LastUngrabbed?.Invoke(interactor);
                }
            }
        }

        protected virtual void OnEnable()
        {
            ConfigureGrabOffset();
            ConfigureGrabType();
            ConfigureTrackingType();
            ConfigureGrabValidity();
        }

        /// <summary>
        /// Creates consumer data from a given Interactor.
        /// </summary>
        /// <param name="interactor">The Interactor to create from.</param>
        /// <returns>The created consumer data.</returns>
        protected virtual ActiveCollisionConsumer.EventData CreateConsumerDataFromInteractor(InteractorFacade interactor)
        {
            ActiveCollisionPublisher.PayloadData interactorPublisher = new ActiveCollisionPublisher.PayloadData();
            interactorPublisher.sourceContainer = interactor?.grabInteractorSetup?.attachPoint;
            return new ActiveCollisionConsumer.EventData().Set(interactorPublisher, null);
        }

        /// <summary>
        /// Retreives a collection of Interactors that are grabbing the Interactable.
        /// </summary>
        /// <returns>The grabbing Interactors.</returns>
        protected virtual List<InteractorFacade> GetGrabbingInteractors()
        {
            List<InteractorFacade> returnList = new List<InteractorFacade>();

            if (gameObjectEventStack == null && gameObjectSet == null)
            {
                return returnList;
            }

            foreach (GameObject element in (gameObjectEventStack != null ? gameObjectEventStack.Stack as IEnumerable<GameObject> : gameObjectSet.Elements as IEnumerable<GameObject>))
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