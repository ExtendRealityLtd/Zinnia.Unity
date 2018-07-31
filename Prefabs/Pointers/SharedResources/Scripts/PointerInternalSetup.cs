namespace VRTK.Core.Prefabs.Pointers
{
    using UnityEngine;
    using VRTK.Core.Action;
    using VRTK.Core.Extension;
    using VRTK.Core.Tracking.Follow;

    /// <summary>
    /// Sets up the Pointer Prefab based on the provided user settings.
    /// </summary>
    public class PointerInternalSetup : MonoBehaviour
    {
        [Header("Facade Settings")]

        /// <summary>
        /// The public interface facade.
        /// </summary>
        [Tooltip("The public interface facade.")]
        public PointerFacade facade;

        [Header("Object Follow Settings")]

        /// <summary>
        /// The <see cref="ObjectFollower"/> component for the Pointer.
        /// </summary>
        [Tooltip("The ObjectFollower component for the Pointer.")]
        public ObjectFollower objectFollow;

        [Header("Action Settions")]

        /// <summary>
        /// The <see cref="BooleanAction"/> that will activate/deactivate the pointer.
        /// </summary>
        [Tooltip("The BooleanAction that will activate/deactivate the pointer.")]
        public BooleanAction activationAction;
        /// <summary>
        /// The <see cref="BooleanAction"/> that initiates the pointer selection when the action is activated.
        /// </summary>
        [Tooltip("The BooleanAction that initiates the pointer selection when the action is activated.")]
        public BooleanAction selectOnActivatedAction;
        /// <summary>
        /// The <see cref="BooleanAction"/> that initiates the pointer selection when the action is deactivated.
        /// </summary>
        [Tooltip("The BooleanAction that initiates the pointer selection when the action is deactivated.")]
        public BooleanAction selectOnDeactivatedAction;

        /// <summary>
        /// Sets up the Pointer prefab with the specified settings.
        /// </summary>
        public virtual void Setup()
        {
            if (InvalidParameters())
            {
                return;
            }

            objectFollow.targetComponents.Clear();
            objectFollow.targetComponents.Add(facade.followTarget.TryGetComponent());

            activationAction.ClearSources();
            activationAction.AddSource(facade.activationAction);
            selectOnActivatedAction.ClearSources();
            selectOnActivatedAction.AddSource(facade.selectionAction);
            selectOnDeactivatedAction.ClearSources();
            selectOnDeactivatedAction.AddSource(facade.selectionAction);

            switch (facade.selectionType)
            {
                case PointerFacade.SelectionType.SelectOnActivate:
                    selectOnActivatedAction.gameObject.SetActive(true);
                    selectOnDeactivatedAction.gameObject.SetActive(false);
                    break;
                case PointerFacade.SelectionType.SelectOnDeactivate:
                    selectOnActivatedAction.gameObject.SetActive(false);
                    selectOnDeactivatedAction.gameObject.SetActive(true);
                    break;
            }
        }

        /// <summary>
        /// Clears all of the settings from the Pointer prefab.
        /// </summary>
        public virtual void Clear()
        {
            if (InvalidParameters())
            {
                return;
            }

            objectFollow.targetComponents.Clear();
            activationAction.ClearSources();
            selectOnActivatedAction.ClearSources();
            selectOnDeactivatedAction.ClearSources();
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
            return (objectFollow == null || facade == null || facade.followTarget == null || facade.activationAction == null || facade.selectionAction == null);
        }
    }
}