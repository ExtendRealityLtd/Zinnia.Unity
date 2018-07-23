namespace VRTK.Core.Prefabs.Interactions.Interactables
{
    using UnityEngine;
    using System.Collections.Generic;
    using VRTK.Core.Action;
    using VRTK.Core.Event;
    using VRTK.Core.Tracking.Follow;
    using VRTK.Core.Prefabs.Interactions.Interactors;
    using VRTK.Core.Data.Collection;

    /// <summary>
    /// Sets up the Interactable Prefab grab settings based on the provided user settings.
    /// </summary>
    public class GrabInteractableInternalSetup : MonoBehaviour
    {
        [Header("Facade Settings")]

        /// <summary>
        /// The public interface facade.
        /// </summary>
        [Tooltip("The public interface facade.")]
        public InteractableFacade facade;

        [Header("Grab Settings")]

        /// <summary>
        /// The <see cref="BooleanAction"/> that will initiate the grab mechanism.
        /// </summary>
        [Tooltip("The BooleanAction that will initiate the grab mechanism.")]
        public List<BooleanAction> grabAction = new List<BooleanAction>();
        /// <summary>
        /// The <see cref="ActionRegistrar"/> that will initiate the Interactor grab mechanism.
        /// </summary>
        [Tooltip("The ActionRegistrar that will initiate the Interactor grab mechanism.")]
        public List<ActionRegistrar> grabActionRegistrar = new List<ActionRegistrar>();
        /// <summary>
        /// The <see cref="EmptyEventProxyEmitter"/> that deals with the active consumed collision.
        /// </summary>
        [Tooltip("The EmptyEventProxyEmitter that deals with the active consumed collision.")]
        public GameObjectEventProxyEmitter gameObjectEventProxyEmitter;
        /// <summary>
        /// The <see cref="GameObjectEventStack"/> that deals secondary grabbing.
        /// </summary>
        [Tooltip("The GameObjectEventStack that deals secondary grabbing.")]
        public GameObjectEventStack gameObjectEventStack;
        /// <summary>
        /// The <see cref="GameObject"/> that contains the precision grab mode logic.
        /// </summary>
        [Tooltip("The GameObject that contains the precision grab mode logic.")]
        public GameObject precisionGrabLogic;
        /// <summary>
        /// The <see cref="GameObject"/> that contains the snap handles logic.
        /// </summary>
        [Tooltip("The GameObject that contains the snap handles logic.")]
        public GameObject snapHandleLogic;
        /// <summary>
        /// The <see cref="ObjectFollow"/> script used for the attachment logic.
        /// </summary>
        [Tooltip("The ObjectFollow script used for the attachment logic.")]
        public ObjectFollow attachmentLogic;

        /// <summary>
        /// The Interactor that is grabbing the Interactable.
        /// </summary>
        public InteractorFacade GrabbingInteractor => GetGrabbingInteractor();

        /// <summary>
        /// Sets up the Interactable prefab with the specified settings.
        /// </summary>
        public virtual void Setup()
        {
            if (InvalidParameters())
            {
                return;
            }

            foreach (ActionRegistrar currentRegistrar in grabActionRegistrar)
            {
                currentRegistrar.sources.Clear();
            }

            foreach (InteractorFacade validInteractor in facade.validInteractors)
            {
                ActionRegistrar.ActionSource actionSource = new ActionRegistrar.ActionSource()
                {
                    container = validInteractor.grabInteractorSetup.attachPoint,
                    action = validInteractor.grabInteractorSetup.grabAction
                };

                foreach (ActionRegistrar currentRegistrar in grabActionRegistrar)
                {
                    currentRegistrar.sources.Add(actionSource);
                }
            }

            switch (facade.grabOffset)
            {
                case InteractableFacade.GrabOffset.None:
                    precisionGrabLogic.SetActive(false);
                    snapHandleLogic.SetActive(false);
                    break;
                case InteractableFacade.GrabOffset.PrecisionGrab:
                    precisionGrabLogic.SetActive(true);
                    snapHandleLogic.SetActive(false);
                    break;
                case InteractableFacade.GrabOffset.SnapHandle:
                    precisionGrabLogic.SetActive(false);
                    snapHandleLogic.SetActive(true);
                    break;
            }
        }

        /// <summary>
        /// Clears all of the settings from the Interactable prefab.
        /// </summary>
        public virtual void Clear()
        {
            if (InvalidParameters())
            {
                return;
            }

            foreach (ActionRegistrar currentRegistrar in grabActionRegistrar)
            {
                currentRegistrar.sources.Clear();
            }
            precisionGrabLogic.SetActive(false);
            snapHandleLogic.SetActive(false);
        }

        /// <summary>
        /// Attempt to grab the Interactable to the given Interactor GameObject.
        /// </summary>
        /// <param name="interactor">The Interactor to attach the Interactable to.</param>
        public virtual void Grab(GameObject interactor)
        {
            //Process single handed grab
            if (gameObjectEventStack == null && grabAction.Count > 0)
            {
                gameObjectEventProxyEmitter?.Receive(interactor);
                grabAction[0]?.Receive(true);
            }
            //Process multi handle grab
            else if (grabAction.Count > gameObjectEventStack.EventIndex)
            {
                int grabIndex = gameObjectEventStack.EventIndex;
                gameObjectEventStack.Push(interactor);
                grabAction[grabIndex]?.Receive(true);
            }
        }

        /// <summary>
        /// Attempt to ungrab the Interactable.
        /// </summary>
        /// <param name="sequenceIndex">The Interactor sequence index to ungrab from.</param>
        public virtual void Ungrab(int sequenceIndex = 0)
        {
            if (grabAction.Count <= sequenceIndex)
            {
                return;
            }

            if (grabAction[sequenceIndex] == null || !grabAction[sequenceIndex].Value)
            {
                return;
            }

            if (gameObjectEventStack != null)
            {
                gameObjectEventStack.PopAt(sequenceIndex);
            }
            grabAction[sequenceIndex]?.Receive(false);
        }

        /// <summary>
        /// Notifies that the Interactable is being grabbed.
        /// </summary>
        /// <param name="data">The grabbing object.</param>
        public virtual void NotifyGrab(GameObject data)
        {
            InteractorFacade interactor = InteractorFacade.TryGetFromGameObject(data);
            if (interactor != null)
            {
                facade?.Grabbed?.Invoke(interactor);
            }
        }

        /// <summary>
        /// Notifies that the Interactable is no longer being grabbed.
        /// </summary>
        /// <param name="data">The previous grabbing object.</param>
        public virtual void NotifyUngrab(GameObject data)
        {
            InteractorFacade interactor = InteractorFacade.TryGetFromGameObject(data);
            if (interactor != null)
            {
                facade?.Ungrabbed?.Invoke(interactor);
            }
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
            return (grabActionRegistrar == null || precisionGrabLogic == null || snapHandleLogic == null || attachmentLogic == null || facade == null);
        }

        /// <summary>
        /// Retreives the currently active grabbing Interactor.
        /// </summary>
        /// <returns>The currently active grabbing Interactor.</returns>
        protected virtual InteractorFacade GetGrabbingInteractor()
        {
            if (attachmentLogic == null || attachmentLogic.sourceComponent == null)
            {
                return null;
            }

            return InteractorFacade.TryGetFromGameObject(attachmentLogic.sourceComponent.gameObject);
        }
    }
}