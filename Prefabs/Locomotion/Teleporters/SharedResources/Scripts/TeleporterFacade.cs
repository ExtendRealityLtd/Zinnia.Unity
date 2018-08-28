namespace VRTK.Core.Prefabs.Locomotion.Teleporters
{
    using UnityEngine;
    using VRTK.Core.Data.Attribute;
    using VRTK.Core.Data.Type;
    using VRTK.Core.Rule;
    using VRTK.Core.Tracking.Modification;

    /// <summary>
    /// The public interface into the Teleporter Prefab.
    /// </summary>
    public class TeleporterFacade : MonoBehaviour
    {
        #region Teleporter Settings
        /// <summary>
        /// The alias for the CameraRig Play Area.
        /// </summary>
        [Header("Teleporter Settings"), Tooltip("The alias for the CameraRig Play Area.")]
        public GameObject playAreaAlias;
        /// <summary>
        /// The alias for the CameraRig Headset.
        /// </summary>
        [Tooltip("The alias for the CameraRig Headset.")]
        public GameObject headsetAlias;
        /// <summary>
        /// The <see cref="CameraList"/> of scene <see cref="Camera"/>s to apply a fade to.
        /// </summary>
        [Tooltip("The list of scene Cameras to apply a fade to.")]
        public CameraList sceneCameras;
        /// <summary>
        /// Allows to optionally determine targets based on the set rules.
        /// </summary>
        [Tooltip("Allows to optionally determine targets based on the set rules.")]
        public RuleContainer targetValidity;
        #endregion

        #region Teleporter Events
        /// <summary>
        /// Emitted when the teleporting is about to initiate.
        /// </summary>
        [Header("Teleporter Events")]
        public TransformPropertyApplier.UnityEvent Teleporting = new TransformPropertyApplier.UnityEvent();
        /// <summary>
        /// Emitted when the teleporting has completed.
        /// </summary>
        public TransformPropertyApplier.UnityEvent Teleported = new TransformPropertyApplier.UnityEvent();
        #endregion

        #region Internal Settings
        /// <summary>
        /// The linked Internal Setup.
        /// </summary>
        [Header("Internal Settings"), Tooltip("The linked Internal Setup."), InternalSetting]
        public TeleporterInternalSetup internalSetup;
        #endregion

        /// <summary>
        /// Attempts to teleport the <see cref="playAreaAlias"/>.
        /// </summary>
        /// <param name="destination">The location to attempt to teleport to.</param>
        public virtual void Teleport(TransformData destination)
        {
            internalSetup?.Teleport(destination);
        }
    }
}