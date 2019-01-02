namespace VRTK.Core.Prefabs.Interactions.Controllables
{
    using UnityEngine;
    using VRTK.Core.Data.Type;

    /// <summary>
    /// The basis to drive a control in a linear direction.
    /// </summary>
    public abstract class DirectionalDrive : Drive<DirectionalDriveFacade, DirectionalDrive>
    {
        /// <summary>
        /// Calculates the limits of the drive.
        /// </summary>
        /// <param name="newLimit">The maximum local space limit the drive can reach.</param>
        /// <returns>The minimum and maximum local space limit the drive can reach.</returns>
        public abstract FloatRange CalculateDriveLimits(float newLimit);

        /// <inheritdoc />
        protected override float CalculateStepValue(DirectionalDriveFacade facade)
        {
            float currentValue = Mathf.Lerp(0, facade.DriveLimit, NormalizedValue);
            return Mathf.Round(((facade.stepRange.minimum + Mathf.Clamp01(currentValue / facade.DriveLimit)) * (facade.stepRange.maximum - facade.stepRange.minimum)) / facade.stepIncrement) * facade.stepIncrement;
        }

        /// <inheritdoc />
        protected override FloatRange CalculateDriveLimits(DirectionalDriveFacade facade)
        {
            return CalculateDriveLimits(facade.DriveLimit);
        }
    }
}