namespace VRTK.Core.Prefabs.Interactions.Interactables
{
    using UnityEngine;
    using VRTK.Core.Data.Type.Transformation;
    using VRTK.Core.Event;

    /// <summary>
    /// Sets up the Interactable.Climbable prefab based on the provided user settings.
    /// </summary>
    public class ClimbInteractableInternalSetup : MonoBehaviour
    {
        #region Facade Settings
        /// <summary>
        /// The public interface facade.
        /// </summary>
        [Header("Facade Settings"), Tooltip("The public interface facade.")]
        public ClimbInteractableFacade facade;
        #endregion

        #region Reference Settings
        /// <summary>
        /// The <see cref="InteractableFacade"/> component acting as the interactable for climbing.
        /// </summary>
        [Header("Reference Settings"), Tooltip("The Interactable Facade component acting as the interactable for climbing.")]
        public InteractableFacade interactableFacade;
        /// <summary>
        /// The <see cref="GameObjectEventProxyEmitter"/> component handling a started climb.
        /// </summary>
        [Tooltip("The Game Object Event Proxy Emitter component handling a started climb.")]
        public GameObjectEventProxyEmitter startEventProxyEmitter;
        /// <summary>
        /// The <see cref="GameObjectEventProxyEmitter"/> component handling a stopped climb.
        /// </summary>
        [Tooltip("The Game Object Event Proxy Emitter component handling a stopped climb.")]
        public GameObjectEventProxyEmitter stopEventProxyEmitter;
        #endregion

        protected virtual void OnEnable()
        {
            startEventProxyEmitter.Emitted.AddListener(OnStartEventProxyEmitted);
            stopEventProxyEmitter.Emitted.AddListener(OnStopEventProxyEmitted);
        }

        protected virtual void OnDisable()
        {
            stopEventProxyEmitter.Emitted.RemoveListener(OnStopEventProxyEmitted);
            startEventProxyEmitter.Emitted.RemoveListener(OnStartEventProxyEmitted);
        }

        protected virtual void OnStartEventProxyEmitted(GameObject interactor)
        {
            facade.climbFacade.AddInteractor(interactor);
            facade.climbFacade.AddInteractable(interactableFacade.gameObject);
        }

        protected virtual void OnStopEventProxyEmitted(GameObject interactor)
        {
            facade.climbFacade.SetVelocitySource(interactor);
            facade.climbFacade.SetVelocityMultiplier(facade.releaseMultiplier);
            facade.climbFacade.RemoveInteractable(interactableFacade.gameObject);
            facade.climbFacade.RemoveInteractor(interactor);
        }
    }
}