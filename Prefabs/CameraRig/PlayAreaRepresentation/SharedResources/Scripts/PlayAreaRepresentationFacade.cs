namespace VRTK.Core.Prefabs.PlayAreaRepresentation
{
    using UnityEngine;
    using VRTK.Core.Data.Attribute;

    /// <summary>
    /// The public interface into the PlayAreaRepresentation Prefab.
    /// </summary>
    public class PlayAreaRepresentationFacade : MonoBehaviour
    {
        #region Target Settings
        /// <summary>
        /// The target to represent the PlayArea.
        /// </summary>
        [Header("Target Settings"), Tooltip("The target to represent the PlayArea.")]
        public GameObject target;
        /// <summary>
        /// An optional origin to use in a position offset calculation.
        /// </summary>
        [Tooltip("An optional origin to use in a position offset calculation.")]
        public GameObject offsetOrigin;
        /// <summary>
        /// An optional destination to use in a position offset calculation.
        /// </summary>
        [Tooltip("An optional destination to use in a position offset calculation.")]
        public GameObject offsetDestination;
        #endregion

        #region Internal Settings
        /// <summary>
        /// The linked Internal Setup.
        /// </summary>
        [Header("Internal Settings"), Tooltip("The linked Internal Setup."), InternalSetting]
        public PlayAreaRepresentationInternalSetup internalSetup;
        #endregion

        /// <summary>
        /// Recalculates the PlayArea dimensions.
        /// </summary>
        public virtual void Recalculate()
        {
            internalSetup.dimensionExtractor.DoExtract();
        }
    }
}