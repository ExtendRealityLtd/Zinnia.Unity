namespace Zinnia.Tracking.Velocity
{
    using UnityEngine;

    /// <summary>
    /// Derived from <see cref="AverageVelocityEstimatorProcess"/> to take average samples of a <see cref="Transform.position"/> and <see cref="Transform.rotation"/> and use this cached data to estimate velocity and angular velocity.
    /// </summary>
    /// <remarks>
    /// This estimator uses LateUpdate().
    /// </remarks>
    public class AverageVelocityEstimator : AverageVelocityEstimatorProcess
    {
        protected virtual void LateUpdate()
        {
            Process();
        }
    }
}