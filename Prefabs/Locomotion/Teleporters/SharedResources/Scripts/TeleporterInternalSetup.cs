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
    /// Provides internal settings for the teleporter prefabs.
    /// </summary>
    public class TeleporterInternalSetup : MonoBehaviour
    {
        /// <summary>
        /// The <see cref="TeleporterUserSetup"/> for providing required alias information.
        /// </summary>
        [Tooltip("The Teleporter User Setup for providing required alias information.")]
        public TeleporterUserSetup userSetup;
        /// <summary>
        /// The <see cref="SurfaceLocator"/> to use for the teleporting event.
        /// </summary>
        [Tooltip("The Surface Locator to use for the teleporting event.")]
        public SurfaceLocator surfaceTeleporter;
        /// <summary>
        /// The <see cref="TransformModifier"/> to use for the teleporting event.
        /// </summary>
        [Tooltip("The Transform Modifier to use for the teleporting event.")]
        public TransformModifier modifyTeleporter;
        /// <summary>
        /// The <see cref="SurfaceLocator"/> to set aliases on.
        /// </summary>
        [Tooltip("The Surface Locators to set aliases on.")]
        public List<SurfaceLocator> surfaceLocatorAliases = new List<SurfaceLocator>();
        /// <summary>
        /// The <see cref="TransformModifier"/> to set aliases on.
        /// </summary>
        [Tooltip("The Transform Modifiers to set aliases on.")]
        public List<TransformModifier> transformModifierAliases = new List<TransformModifier>();
        /// <summary>
        /// The scene <see cref="Camera"/>s to set the <see cref="CameraColorOverlay"/>s to affect.
        /// </summary>
        [Tooltip("The scene Cameras to set the CameraColorOverlays to affect.")]
        public List<CameraColorOverlay> cameraColorOverlays = new List<CameraColorOverlay>();

        /// <summary>
        /// Attempts to teleport the <see cref="userSetup.playAreaAlias"/>.
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
                modifyTeleporter.Modify();
            }
        }

        protected virtual void OnEnable()
        {
            foreach (SurfaceLocator currentLocator in surfaceLocatorAliases.EmptyIfNull())
            {
                currentLocator.searchOrigin = userSetup.headsetAlias;
            }

            foreach (TransformModifier currentModifier in transformModifierAliases.EmptyIfNull())
            {
                currentModifier.target = userSetup.playAreaAlias;
                currentModifier.offset = userSetup.headsetAlias;
            }

            foreach (CameraColorOverlay currentOverlay in cameraColorOverlays.EmptyIfNull())
            {
                currentOverlay.validCameras = userSetup.sceneCameras;
            }
        }
    }
}