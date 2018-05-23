namespace VRTK.Core.Prefabs.Locomotion.Teleporters
{
    using UnityEngine;
    using System;
    using VRTK.Core.Tracking;
    using VRTK.Core.Visual;
    using VRTK.Core.Data.Type;

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
        /// The <see cref="TransformModify"/> to use for the teleporting event.
        /// </summary>
        [Tooltip("The Transform Modify to use for the teleporting event.")]
        public TransformModify modifyTeleporter;
        /// <summary>
        /// The <see cref="SurfaceLocator"/> to set aliases on.
        /// </summary>
        [Tooltip("The Surface Locators to set aliases on.")]
        public SurfaceLocator[] surfaceLocatorAliases = Array.Empty<SurfaceLocator>();
        /// <summary>
        /// The <see cref="TransformModify"/> to set aliases on.
        /// </summary>
        [Tooltip("The Transform Modifiers to set aliases on.")]
        public TransformModify[] transformModifierAliases = Array.Empty<TransformModify>();
        /// <summary>
        /// The scene <see cref="Camera"/>s to set the <see cref="CameraColorOverlay"/>s to affect.
        /// </summary>
        [Tooltip("The scene Cameras to set the CameraColorOverlays to affect.")]
        public CameraColorOverlay[] cameraColorOverlays = Array.Empty<CameraColorOverlay>();

        /// <summary>
        /// Attempts to teleport the <see cref="userSetup.playAreaAlias"/>.
        /// </summary>
        /// <param name="givenLocation">The location to attempt to teleport to.</param>
        /// <param name="initiator">The <see cref="object"/> which initiated the method.</param>
        public virtual void Teleport(TransformData givenLocation, object initiator = null)
        {
            if (surfaceTeleporter != null)
            {
                surfaceTeleporter.Locate(givenLocation, initiator);
            }

            if (modifyTeleporter != null)
            {
                modifyTeleporter.Modify(givenLocation, initiator);
            }
        }

        protected virtual void OnEnable()
        {
            foreach (SurfaceLocator currentLocator in surfaceLocatorAliases)
            {
                currentLocator.searchOrigin = userSetup.headsetAlias;
            }

            foreach (TransformModify currentModifier in transformModifierAliases)
            {
                currentModifier.source = userSetup.playAreaAlias;
                currentModifier.offset = userSetup.headsetAlias;
            }

            foreach (CameraColorOverlay currentOverlay in cameraColorOverlays)
            {
                currentOverlay.validCameras = userSetup.sceneCameras;
            }
        }
    }
}