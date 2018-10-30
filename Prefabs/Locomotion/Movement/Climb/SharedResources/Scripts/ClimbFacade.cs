﻿namespace VRTK.Core.Prefabs.Locomotion.Movement.Climb
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.Events;
    using VRTK.Core.Data.Attribute;
    using VRTK.Core.Prefabs.Locomotion.BodyRepresentation;

    /// <summary>
    /// The public interface for the Climb prefab.
    /// </summary>
    public class ClimbFacade : MonoBehaviour
    {
        #region Facade Settings
        /// <summary>
        /// The body representation to control.
        /// </summary>
        [Header("Facade Settings"), Tooltip("The body representation to control.")]
        public BodyRepresentationFacade bodyRepresentationFacade;
        #endregion

        #region Events
        /// <summary>
        /// Emitted when a climb starts.
        /// </summary>
        [Header("Events"), Tooltip("Emitted when a climb starts.")]
        public UnityEvent ClimbStarted = new UnityEvent();
        /// <summary>
        /// Emitted when the climb stops.
        /// </summary>
        [Tooltip("Emitted when the climb stops.")]
        public UnityEvent ClimbStopped = new UnityEvent();
        #endregion

        #region Internal Settings
        /// <summary>
        /// The linked Internal Setup.
        /// </summary>
        [Header("Internal Settings"), Tooltip("The linked Internal Setup."), InternalSetting]
        public ClimbInternalSetup internalSetup;
        #endregion

        /// <summary>
        /// The current source of the movement. The body will be moved in reverse direction in case this object moves.
        /// </summary>
        public GameObject CurrentInteractor => Interactors.LastOrDefault();
        /// <summary>
        /// The current optional offset of the movement. The body will be moved in case this object moves.
        /// </summary>
        public GameObject CurrentInteractable => Interactables.LastOrDefault();
        /// <summary>
        /// Whether a climb is happening right now.
        /// </summary>
        public bool IsClimbing => Interactors.Any() || Interactables.Any();

        /// <summary>
        /// The objects that define the source of movement in order they should be used. The last object defines <see cref="CurrentInteractor"/>.
        /// </summary>
        public IReadOnlyList<GameObject> Interactors => internalSetup.interactors.Elements;
        /// <summary>
        /// The objects that define the optional offsets of movement in order they should be used. The last object defines <see cref="CurrentInteractable"/>.
        /// </summary>
        public IReadOnlyList<GameObject> Interactables => internalSetup.interactables.Elements;

        /// <summary>
        /// Adds a source of movement for the body.
        /// </summary>
        /// <param name="interactor">The object to use as a source of the movement.</param>
        public virtual void AddInteractor(GameObject interactor)
        {
            internalSetup.interactors.AddToEnd(interactor);
        }

        /// <summary>
        /// Removes a source of movement for the body.
        /// </summary>
        /// <param name="interactor">The object used as a source of the movement.</param>
        public virtual void RemoveInteractor(GameObject interactor)
        {
            if (!internalSetup.interactors.Elements.Contains(interactor))
            {
                return;
            }

            internalSetup.interactors.RemoveLast(interactor);
            internalSetup.ApplyVelocity();
        }

        /// <summary>
        /// Clears the sources of the movement.
        /// </summary>
        public virtual void ClearInteractors()
        {
            internalSetup.interactors.Clear(false);
        }

        /// <summary>
        /// Adds an optional offset of movement for the body.
        /// </summary>
        /// <param name="interactable">The object to use as an optional offset of the movement.</param>
        public virtual void AddInteractable(GameObject interactable)
        {
            internalSetup.interactables.AddToEnd(interactable);
        }

        /// <summary>
        /// Removes an optional offset of movement for the body.
        /// </summary>
        /// <param name="interactable">The object used as an optional offset of the movement.</param>
        public virtual void RemoveInteractable(GameObject interactable)
        {
            internalSetup.interactables.RemoveLast(interactable);
        }

        /// <summary>
        /// Clears the optional offsets of the movement.
        /// </summary>
        public virtual void ClearInteractables()
        {
            internalSetup.interactables.Clear(false);
        }

        /// <summary>
        /// Sets a source to track the velocity from.
        /// </summary>
        /// <param name="source">The tracked velocity source.</param>
        public virtual void SetVelocitySource(GameObject source)
        {
            internalSetup.velocityProxy.SetProxySource(source);
        }

        /// <summary>
        /// Sets the multiplier to apply to any tracked velocity.
        /// </summary>
        /// <param name="multiplier">The multiplier to apply to tracked velocity.</param>
        public virtual void SetVelocityMultiplier(Vector3 multiplier)
        {
            internalSetup.velocityMultiplier.SetMultiplier(multiplier);
        }
    }
}