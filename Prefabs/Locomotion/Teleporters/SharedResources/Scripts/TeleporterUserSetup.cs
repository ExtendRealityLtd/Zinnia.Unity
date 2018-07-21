namespace VRTK.Core.Prefabs.Locomotion.Teleporters
{
    using UnityEngine;
    using VRTK.Core.Data.Type;

    /// <summary>
    /// Provides user specific setup information for the teleporter prefabs.
    /// </summary>
    public class TeleporterUserSetup : MonoBehaviour
    {
        /// <summary>
        /// The alias for the CameraRig Play Area.
        /// </summary>
        [Tooltip("The alias for the CameraRig Play Area.")]
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
    }
}