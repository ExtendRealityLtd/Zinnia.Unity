namespace VRTK.Core.Prefabs.Locomotion.Movement.AxesToVector3
{
    using UnityEngine;
    using VRTK.Core.Action;
    using VRTK.Core.Process;
    using VRTK.Core.Data.Type.Transformation;

    /// <summary>
    /// Sets up the AxisSlide prefab based on the provided settings and implements the logic to allow moving an object via input from two axes.
    /// </summary>
    public class AxesToVector3InternalSetup : MonoBehaviour, IProcessable
    {
        #region Facade Settings
        /// <summary>
        /// The public interface facade.
        /// </summary>
        [Header("Facade Settings"), Tooltip("The public interface facade.")]
        public AxesToVector3Facade facade;
        #endregion

        #region Axis Settings
        /// <summary>
        /// The lateral <see cref="FloatAction"/> to map to.
        /// </summary>
        [Header("Axis Settings"), Tooltip("The lateral FloatAction to map to.")]
        public FloatAction lateralAxis;
        /// <summary>
        /// The longitudinal <see cref="FloatAction"/> to map to.
        /// </summary>
        [Tooltip("The longitudinal FloatAction to map to.")]
        public FloatAction longitudinalAxis;
        /// <summary>
        /// The multiplier that operates on the final <see cref="Vector3"/> to modify the speed.
        /// </summary>
        [Tooltip("The multiplier that operates on the final Vector3 to modify the speed.")]
        public Vector3Multiplier speedMultiplier;
        /// <summary>
        /// The multiplier to use as the input mask to limit the forward direction.
        /// </summary>
        [Tooltip("The multiplier to use as the input mask to limit the forward direction.")]
        public Vector3Multiplier inputMask;
        #endregion

        /// <summary>
        /// The current calculated movement.
        /// </summary>
        public Vector3 CurrentMovement
        {
            get;
            protected set;
        }

        /// <summary>
        /// The current axis data to use when processing movement.
        /// </summary>
        protected Vector3 currentAxisData = Vector3.zero;

        /// <summary>
        /// Emits the Converted event for the last calculated <see cref="Vector3"/>.
        /// </summary>
        public virtual void Process()
        {
            if (!isActiveAndEnabled)
            {
                return;
            }

            Vector3 axisDirection = facade.sourceOfForwardDirection != null ? ApplyForwardSourceToAxis(currentAxisData) : currentAxisData;
            CurrentMovement = inputMask.Transform(axisDirection) * (Time.inFixedTimeStep ? Time.fixedDeltaTime : Time.deltaTime);
            facade.Processed?.Invoke(CurrentMovement);
        }

        /// <summary>
        /// Sets the speed multipliers.
        /// </summary>
        public virtual void SetMultipliers()
        {
            speedMultiplier.SetXMultiplier(facade.LateralSpeedMultiplier);
            speedMultiplier.SetZMultiplier(facade.LongitudinalSpeedMultiplier);
        }

        /// <summary>
        /// Sets the axis sources.
        /// </summary>
        /// <param name="clearOnly">Whether to only clear the existing sources and not add new ones.</param>
        public virtual void SetAxisSources(bool clearOnly = false)
        {
            if (lateralAxis != null)
            {
                lateralAxis.ClearSources();
                if (!clearOnly && facade.LateralAxis != null)
                {
                    lateralAxis.AddSource(facade.LateralAxis);
                }
            }

            if (longitudinalAxis != null)
            {
                longitudinalAxis.ClearSources();
                if (!clearOnly && facade.LongitudinalAxis != null)
                {
                    longitudinalAxis.AddSource(facade.LongitudinalAxis);
                }
            }
        }

        protected virtual void OnEnable()
        {
            SetAxisSources();
            speedMultiplier.Transformed.AddListener(SetAxisData);
            SetMultipliers();
        }

        protected virtual void OnDisable()
        {
            SetAxisSources(true);
            speedMultiplier.Transformed.RemoveListener(SetAxisData);
        }

        /// <summary>
        /// Processes the axis data into a movement.
        /// </summary>
        /// <param name="axisData">The axis data to process.</param>
        protected virtual void SetAxisData(Vector3 axisData)
        {
            if (facade == null)
            {
                return;
            }

            currentAxisData = axisData;
        }

        /// <summary>
        /// Applies the forward following source data to the axis data.
        /// </summary>
        /// <param name="axisData">The axis data to apply.</param>
        /// <returns>The applied result of the forward following data against the axis data.</returns>
        protected virtual Vector3 ApplyForwardSourceToAxis(Vector3 axisData)
        {
            return (facade.sourceOfForwardDirection.transform.right * axisData.x)
                + (facade.sourceOfForwardDirection.transform.up * axisData.y)
                + (facade.sourceOfForwardDirection.transform.forward * axisData.z);
        }
    }
}