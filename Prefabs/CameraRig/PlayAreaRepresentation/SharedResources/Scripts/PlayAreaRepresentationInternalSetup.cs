namespace VRTK.Core.Prefabs.PlayAreaRepresentation
{
    using UnityEngine;
    using VRTK.Core.Data.Operation;
    using VRTK.Core.Mutation.TransformProperty;
    using VRTK.Core.Tracking.CameraRig;

    /// <summary>
    /// Sets up the PlayAreaRepresentation Prefab based on the provided user settings.
    /// </summary>
    public class PlayAreaRepresentationInternalSetup : MonoBehaviour
    {
        #region Facade Settings
        /// <summary>
        /// The public interface facade.
        /// </summary>
        [Header("Facade Settings"), Tooltip("The public interface facade.")]
        public PlayAreaRepresentationFacade facade;
        #endregion

        #region Operator Settings
        /// <summary>
        /// The <see cref="PlayAreaDimensionsExtractor"/> component for extracting the PlayArea dimension data.
        /// </summary>
        [Header("Operator Settings"), Tooltip("The PlayAreaDimensionsExtractor component for extracting the PlayArea dimension data.")]
        public PlayAreaDimensionsExtractor dimensionExtractor;
        /// <summary>
        /// The <see cref="ScaleProperty"/> component for scaling the given target.
        /// </summary>
        [Tooltip("The ScaleProperty component for scaling the given target.")]
        public ScaleProperty objectScaler;
        /// <summary>
        /// The <see cref="PositionProperty"/> component for positioning the given target.
        /// </summary>
        [Tooltip("The PositionProperty component for positioning the given target.")]
        public PositionProperty objectPositioner;
        /// <summary>
        /// The <see cref="TransformPositionExtractor"/> component extracting the offset origin position.
        /// </summary>
        [Tooltip("The TransformPositionExtractor component extracting the offset origin position.")]
        public TransformPositionExtractor offsetOriginExtractor;
        /// <summary>
        /// The <see cref="TransformPositionExtractor"/> component extracting the offset destination position.
        /// </summary>
        [Tooltip("The TransformPositionExtractor component extracting the offset destination position.")]
        public TransformPositionExtractor offsetDestinationExtractor;
        #endregion

        /// <summary>
        /// Sets up the PlayAreaRepresentation prefab with the specified settings.
        /// </summary>
        public virtual void SetUp()
        {
            objectScaler.target = facade.target;
            objectPositioner.target = facade.target;
            offsetOriginExtractor.source = facade.offsetOrigin;
            offsetDestinationExtractor.source = facade.offsetDestination;
        }

        protected virtual void OnEnable()
        {
            SetUp();
            objectScaler.gameObject.SetActive(true);
        }

        protected virtual void OnDisable()
        {
            objectScaler.gameObject.SetActive(false);
        }
    }
}