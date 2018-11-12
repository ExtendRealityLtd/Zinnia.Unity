namespace VRTK.Core.Prefabs.Locomotion.Teleporters
{
    using UnityEngine;
    using System.Collections.Generic;
    using VRTK.Core.Tracking;
    using VRTK.Core.Tracking.Modification;
    using VRTK.Core.Visual;
    using VRTK.Core.Data.Type;
    using VRTK.Core.Extension;

    /// <summary>
    /// Sets up the Teleport Prefab based on the provided user settings.
    /// </summary>
    public class TeleporterInternalSetup : MonoBehaviour
    {
        #region Facade Settings
        /// <summary>
        /// The public interface facade.
        /// </summary>
        [Header("Facade Settings"), Tooltip("The public interface facade.")]
        public TeleporterFacade facade;
        #endregion

        #region Teleporter Settings
        /// <summary>
        /// The <see cref="SurfaceLocator"/> to use for the teleporting event.
        /// </summary>
        [Header("Teleporter Settings"), Tooltip("The Surface Locator to use for the teleporting event.")]
        public SurfaceLocator surfaceTeleporter;
        /// <summary>
        /// The <see cref="TransformPropertyApplier"/> to use for the teleporting event.
        /// </summary>
        [Tooltip("The Transform Property Applier to use for the teleporting event.")]
        public TransformPropertyApplier modifyTeleporter;
        #endregion

        #region Alias Settings
        /// <summary>
        /// The <see cref="SurfaceLocator"/> to set aliases on.
        /// </summary>
        [Header("Alias Settings"), Tooltip("The Surface Locators to set aliases on.")]
        public List<SurfaceLocator> surfaceLocatorAliases = new List<SurfaceLocator>();
        /// <summary>
        /// The <see cref="SurfaceLocator"/> to set rules on.
        /// </summary>
        [Tooltip("The Surface Locators to set rules on.")]
        public List<SurfaceLocator> surfaceLocatorRules = new List<SurfaceLocator>();
        /// <summary>
        /// The <see cref="TransformPropertyApplier"/> collection to set aliases on.
        /// </summary>
        [Tooltip("The Transform Property Applier collection to set aliases on.")]
        public List<TransformPropertyApplier> transformPropertyApplierAliases = new List<TransformPropertyApplier>();
        /// <summary>
        /// The <see cref="TransformPropertyApplier"/> collection to ignore offsets on.
        /// </summary>
        [Tooltip("The Transform Property Applier collection to ignore offsets on.")]
        public List<TransformPropertyApplier> transformPropertyApplierIgnoreOffsetAliases = new List<TransformPropertyApplier>();
        /// <summary>
        /// The scene <see cref="Camera"/>s to set the <see cref="CameraColorOverlay"/>s to affect.
        /// </summary>
        [Tooltip("The scene Cameras to set the CameraColorOverlays to affect.")]
        public List<CameraColorOverlay> cameraColorOverlays = new List<CameraColorOverlay>();
        #endregion

        /// <summary>
        /// Attempts to teleport the <see cref="playAreaAlias"/>.
        /// </summary>
        /// <param name="destination">The location to attempt to teleport to.</param>
        public virtual void Teleport(TransformData destination)
        {
            if (surfaceTeleporter != null)
            {
                surfaceTeleporter.Locate(destination);
            }

            if (modifyTeleporter != null)
            {
                modifyTeleporter.SetSource(destination);
                modifyTeleporter.Apply();
            }
        }

        /// <summary>
        /// Notifies that the teleporter is about to initiate.
        /// </summary>
        /// <param name="data">The location data.</param>
        public virtual void NotifyTeleporting(TransformPropertyApplier.EventData data)
        {
            facade.Teleporting?.Invoke(data);
        }

        /// <summary>
        /// Notifies that the teleporter has completed.
        /// </summary>
        /// <param name="data">The location data.</param>
        public virtual void NotifyTeleported(TransformPropertyApplier.EventData data)
        {
            facade.Teleported?.Invoke(data);
        }

        /// <summary>
        /// Applies the provided settings to the Teleporter prefab.
        /// </summary>
        protected virtual void Setup()
        {
            foreach (SurfaceLocator currentLocator in surfaceLocatorAliases.EmptyIfNull())
            {
                currentLocator.searchOrigin = facade.offset;
            }

            foreach (SurfaceLocator currentLocator in surfaceLocatorRules.EmptyIfNull())
            {
                currentLocator.targetValidity = facade.targetValidity;
            }

            foreach (TransformPropertyApplier currentApplier in transformPropertyApplierAliases.EmptyIfNull())
            {
                currentApplier.target = facade.target;
                currentApplier.offset = null;
                if (!facade.onlyOffsetFloorSnap || !transformPropertyApplierIgnoreOffsetAliases.Contains(currentApplier))
                {
                    currentApplier.offset = facade.offset;
                }
            }

            foreach (CameraColorOverlay currentOverlay in cameraColorOverlays.EmptyIfNull())
            {
                currentOverlay.validCameras = facade.sceneCameras;
            }
        }

        protected virtual void OnEnable()
        {
            Setup();
        }
    }
}