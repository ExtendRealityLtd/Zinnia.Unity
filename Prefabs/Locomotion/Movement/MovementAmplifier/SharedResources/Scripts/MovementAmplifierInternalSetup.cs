namespace VRTK.Core.Prefabs.Locomotion.Movement.MovementAmplifier
{
    using UnityEngine;
    using VRTK.Core.Data.Operation;
    using VRTK.Core.Data.Type.Transformation;
    using VRTK.Core.Tracking.Follow;

    /// <summary>
    /// Sets up the MovementAmplifier prefab based on the provided user settings.
    /// </summary>
    public class MovementAmplifierInternalSetup : MonoBehaviour
    {
        #region Facade Settings
        /// <summary>
        /// The public interface facade.
        /// </summary>
        [Header("Facade Settings"), Tooltip("The public interface facade.")]
        public MovementAmplifierFacade facade;
        #endregion

        #region Reference Settings
        /// <summary>
        /// Moves the radius origin.
        /// </summary>
        [Header("Reference Settings"), Tooltip("Moves the radius origin.")]
        public ObjectDistanceComparator radiusOriginMover;
        /// <summary>
        /// Determines whether <see cref="MovementAmplifierFacade.source"/> is inside the radius.
        /// </summary>
        [Tooltip("Determines whether MovementAmplifierFacade.source is inside the radius.")]
        public ObjectDistanceComparator distanceChecker;
        /// <summary>
        /// Moves the objects.
        /// </summary>
        [Tooltip("Moves the objects.")]
        public ObjectDistanceComparator objectMover;
        /// <summary>
        /// Subtracts the radius.
        /// </summary>
        [Tooltip("Subtracts the radius.")]
        public FloatAdder radiusSubtractor;
        /// <summary>
        /// Stabilizes the radius by ensuring <see cref="MovementAmplifierFacade.target"/> moves back into the radius.
        /// </summary>
        [Tooltip("Stabilizes the radius by ensuring MovementAmplifierFacade.target moves back into the radius.")]
        public float radiusStabilizer = 0.001f;
        /// <summary>
        /// Amplifies the movement.
        /// </summary>
        [Tooltip("Amplifies the movement.")]
        public Vector3Multiplier movementMultiplier;
        /// <summary>
        /// Moves the target.
        /// </summary>
        [Tooltip("Moves the target.")]
        public TransformPositionMutator targetPositionMutator;
        #endregion

        public virtual void OnEnable()
        {
            radiusOriginMover.transform.parent.position = facade.source.transform.position;
            radiusOriginMover.SetTarget(facade.source);

            distanceChecker.SetSource(facade.source);
            distanceChecker.distanceThreshold = facade.ignoredRadius;

            objectMover.SetSource(facade.source);

            radiusSubtractor.SetElement(1, -facade.ignoredRadius + radiusStabilizer);

            movementMultiplier.SetElement(1, Vector3.one * (facade.multiplier - 1f));

            targetPositionMutator.SetTarget(facade.target);
        }

        public virtual void OnDisable()
        {
            objectMover.enabled = false;
        }
    }
}