namespace VRTK.Core.Prefabs.Locomotion.Movement.MovementAmplifier
{
    using UnityEngine;
    using VRTK.Core.Data.Attribute;

    /// <summary>
    /// The public interface for the MovementAmplifier prefab.
    /// </summary>
    public class MovementAmplifierFacade : MonoBehaviour
    {
        #region Tracking Settings
        /// <summary>
        /// The source to observe movement of.
        /// </summary>
        [Header("Tracking Settings"), Tooltip("The source to observe movement of.")]
        public GameObject source;
        /// <summary>
        /// The target to apply amplified movement to.
        /// </summary>
        [Tooltip("The target to apply amplified movement to.")]
        public GameObject target;
        #endregion

        #region Movement Settings
        /// <summary>
        /// The radius in which <see cref="source"/> movement is ignored. Too small values can result in movement amplification happening during crouching which is often unexpected.
        /// </summary>
        [Header("Movement Settings"), Tooltip("The radius in which source movement is ignored. Too small values can result in movement amplification happening during crouching which is often unexpected.")]
        public float ignoredRadius = 0.25f;
        /// <summary>
        /// How much to amplify movement of <see cref="source"/> to apply to <see cref="target"/>.
        /// </summary>
        [Tooltip("How much to amplify movement of source to apply to target.")]
        public float multiplier = 2f;
        #endregion

        #region Internal Settings
        /// <summary>
        /// The linked Internal Setup.
        /// </summary>
        [Header("Internal Settings"), Tooltip("The linked Internal Setup."), InternalSetting]
        public MovementAmplifierInternalSetup internalSetup;
        #endregion
    }
}