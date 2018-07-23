namespace VRTK.Core.Prefabs.Interactions.Interactables
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using System.Collections.Generic;
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

        [Header("Interactable Settings")]

        /// <summary>
        /// A collection of Interactors that can interact with this Interactable.
        /// </summary>
        [Tooltip("A collection of Interactors that can interact with this Interactable.")]
        public List<InteractorFacade> validInteractors = new List<InteractorFacade>();
        /// <summary>
        /// The offset to apply when grabbing the Interactable.
        /// </summary>
        [Tooltip("The offset to apply when grabbing the Interactable.")]
        public GrabOffset grabOffset = GrabOffset.None;

        [Header("Interactable Events")]
        /// <summary>
        /// Emitted when an Interactor touches the Interactable.
        /// </summary>
        public UnityEvent Touched = new UnityEvent();
        /// <summary>
        /// Emitted when an Interactor stops touching the Interactable.
        /// </summary>
        public UnityEvent Untouched = new UnityEvent();
        /// <summary>
        /// Emitted when an Interactor grabs the Interactable.
        /// </summary>
        public UnityEvent Grabbed = new UnityEvent();
        /// <summary>
        /// Emitted when an Interactor ungrabs the Interactable.
        /// </summary>
        public UnityEvent Ungrabbed = new UnityEvent();

        [Header("Internal Settings")]

        /// <summary>
        /// **DO NOT CHANGE** - The linked Touch Internal Setup.
        /// </summary>
        [Tooltip("**DO NOT CHANGE** - The linked Touch Internal Setup.")]
        public TouchInteractableInternalSetup touchInteractableSetup;
        /// <summary>
        /// **DO NOT CHANGE** - The linked Grab Internal Setup.
        /// </summary>
        [Tooltip("**DO NOT CHANGE** - The linked Grab Internal Setup.")]
        public GrabInteractableInternalSetup grabInteractableSetup;

        /// <summary>
        /// A collection of Interactors that are currently touching the Interactable.
        /// </summary>
        public List<InteractorFacade> TouchingInteractors => touchInteractableSetup?.TouchingInteractors;
        /// <summary>
        /// The Interactor that is grabbing the Interactable.
        /// </summary>
        public InteractorFacade GrabbingInteractor => grabInteractableSetup?.GrabbingInteractor;

        /// <summary>
        /// Attempt to grab the Interactable to the given Interactor GameObject.
        /// </summary>
        /// <param name="interactor">The Interactor to attach the Interactable to.</param>
        public virtual void Grab(GameObject interactor)
        {
            grabInteractableSetup?.Grab(interactor);
        }

        /// <summary>
        /// Attempt to ungrab the Interactable.
        /// </summary>
        /// <param name="sequenceIndex">The Interactor sequence index to ungrab from.</param>
        public virtual void Ungrab(int sequenceIndex = 0)
        {
            grabInteractableSetup.Ungrab(sequenceIndex);
        }
    }
}