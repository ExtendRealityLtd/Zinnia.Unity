namespace VRTK.Core.Prefabs.Interactions.Interactables
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using System.Collections.Generic;
    using VRTK.Core.Extension;
    using VRTK.Core.Data.Attribute;
    using VRTK.Core.Prefabs.Interactions.Interactors;

    /// <summary>
    /// The public interface into the Interactable Prefab.
    /// </summary>
    public class InteractableFacade : MonoBehaviour
    {
        /// <summary>
        /// Defines the event with the <see cref="InteractorFacade"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<InteractorFacade>
        {
        }

        /// <summary>
        /// The way in which the grab is kept active.
        /// </summary>
        public enum ActiveType
        {
            /// <summary>
            /// The grab will occur when the button is held down and will ungrab when the button is released.
            /// </summary>
            HoldTillRelease,
            /// <summary>
            /// The grab will occur on the first press of the button and stay grabbed until a second press of the button.
            /// </summary>
            Toggle
        }

        /// <summary>
        /// The way in which the object is moved.
        /// </summary>
        public enum TrackingType
        {
            /// <summary>
            /// Updates the transform data directly, outside of the physics system.
            /// </summary>
            FollowTransform,
            /// <summary>
            /// Updates the rigidbody using velocity to stay within the bounds of the physics system.
            /// </summary>
            FollowRigidbody
        }

        /// <summary>
        /// The offset to apply on grab.
        /// </summary>
        public enum GrabOffset
        {
            /// <summary>
            /// No offset is applied.
            /// </summary>
            None,
            /// <summary>
            /// An offset of where the collision between the Interactor and Interactable is applied for precision grabbing.
            /// </summary>
            PrecisionGrab,
            /// <summary>
            /// An offset of a specified <see cref="GameObject"/> is applied to orientate the interactable on grab.
            /// </summary>
            SnapHandle
        }

        #region Interactable Settings
        /// <summary>
        /// The mechanism of how to keep the grab action active.
        /// </summary>
        [Header("Interactable Settings"), Tooltip("The mechanism of how to keep the grab action active.")]
        public ActiveType grabType = ActiveType.HoldTillRelease;
        /// <summary>
        /// Determines how to track the interactable to the interactor.
        /// </summary>
        [Tooltip("Determines how to track the interactable to the interactor.")]
        public TrackingType trackingType = TrackingType.FollowTransform;
        /// <summary>
        /// The offset to apply when grabbing the Interactable.
        /// </summary>
        [Tooltip("The offset to apply when grabbing the Interactable.")]
        public GrabOffset grabOffset = GrabOffset.None;
        #endregion

        #region Restriction Settings
        /// <summary>
        /// A collection of interactors that are not allowed to touch this interactable.
        /// </summary>
        [Header("Restriction Settings"), Tooltip("A collection of interactors that are not allowed to touch this interactable.")]
        public List<InteractorFacade> disallowedTouchInteractors = new List<InteractorFacade>();
        /// <summary>
        /// A collection of interactors that are not allowed to grab this interactable.
        /// </summary>
        [Tooltip("A collection of interactors that are not allowed to grab this interactable.")]
        public List<InteractorFacade> disallowedGrabInteractors = new List<InteractorFacade>();
        #endregion

        #region Touch Events
        /// <summary>
        /// Emitted when the Interactable is touched for the first time by an Interactor.
        /// </summary>
        [Header("Touch Events")]
        public UnityEvent FirstTouched = new UnityEvent();
        /// <summary>
        /// Emitted when an Interactor touches the Interactable.
        /// </summary>
        public UnityEvent Touched = new UnityEvent();
        /// <summary>
        /// Emitted when an Interactor stops touching the Interactable.
        /// </summary>
        public UnityEvent Untouched = new UnityEvent();
        /// <summary>
        /// Emitted when the Interactable is untouched for the last time by an Interactor.
        /// </summary>
        public UnityEvent LastUntouched = new UnityEvent();
        #endregion

        #region Grab Events
        /// <summary>
        /// Emitted when the Interactable is grabbed for the first time by an Interactor.
        /// </summary>
        [Header("Grab Events")]
        public UnityEvent FirstGrabbed = new UnityEvent();
        /// <summary>
        /// Emitted when an Interactor grabs the Interactable.
        /// </summary>
        public UnityEvent Grabbed = new UnityEvent();
        /// <summary>
        /// Emitted when an Interactor ungrabs the Interactable.
        /// </summary>
        public UnityEvent Ungrabbed = new UnityEvent();
        /// <summary>
        /// Emitted when the Interactable is ungrabbed for the last time by an Interactor.
        /// </summary>
        public UnityEvent LastUngrabbed = new UnityEvent();
        #endregion

        #region Internal Settings
        /// <summary>
        /// The linked Touch Internal Setup.
        /// </summary>
        [Header("Internal Settings"), Tooltip("The linked Touch Internal Setup."), InternalSetting, SerializeField]
        protected TouchInteractableInternalSetup touchInteractableSetup;
        /// <summary>
        /// The linked Grab Internal Setup.
        /// </summary>
        [Tooltip("The linked Grab Internal Setup."), InternalSetting, SerializeField]
        protected GrabInteractableInternalSetup grabInteractableSetup;
        #endregion

        /// <summary>
        /// A collection of Interactors that are currently touching the Interactable.
        /// </summary>
        public List<InteractorFacade> TouchingInteractors => touchInteractableSetup.TouchingInteractors;
        /// <summary>
        /// A collection of Interactors that are currently grabbing the Interactable.
        /// </summary>
        public List<InteractorFacade> GrabbingInteractors => grabInteractableSetup.GrabbingInteractors;

        /// <summary>
        /// Attempt to grab the Interactable to the given <see cref="GameObject"/> that contains an Interactor.
        /// </summary>
        /// <param name="interactor">The GameObject that the Interactor is on.</param>
        public virtual void Grab(GameObject interactor)
        {
            Grab(interactor.TryGetComponent<InteractorFacade>(true, true));
        }

        /// <summary>
        /// Attempt to grab the Interactable to the given Interactor.
        /// </summary>
        /// <param name="interactor">The Interactor to attach the Interactable to.</param>
        public virtual void Grab(InteractorFacade interactor)
        {
            grabInteractableSetup.Grab(interactor);
        }

        /// <summary>
        /// Attempt to ungrab the Interactable to the given <see cref="GameObject"/> that contains an Interactor.
        /// </summary>
        /// <param name="interactor">The GameObject that the Interactor is on.</param>
        public virtual void Ungrab(GameObject interactor)
        {
            Ungrab(interactor.TryGetComponent<InteractorFacade>(true, true));
        }

        /// <summary>
        /// Attempt to ungrab the Interactable.
        /// </summary>
        /// <param name="interactor">The Interactor to ungrab from.</param>
        public virtual void Ungrab(InteractorFacade interactor)
        {
            grabInteractableSetup.Ungrab(interactor);
        }

        /// <summary>
        /// Attempt to ungrab the Interactable at a specific grabbing index.
        /// </summary>
        /// <param name="sequenceIndex">The Interactor sequence index to ungrab from.</param>
        public virtual void Ungrab(int sequenceIndex = 0)
        {
            grabInteractableSetup.Ungrab(sequenceIndex);
        }

        /// <summary>
        /// Refreshes the interactor restrictions.
        /// </summary>
        public virtual void RefreshInteractorRestrictions()
        {
            touchInteractableSetup.ConfigureTouchValidity();
            grabInteractableSetup.ConfigureGrabValidity();
        }
    }
}