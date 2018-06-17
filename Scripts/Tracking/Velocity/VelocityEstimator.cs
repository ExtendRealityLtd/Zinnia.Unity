namespace VRTK.Core.Tracking.Velocity
{
    using UnityEngine;
    using System;

    /// <summary>
    /// Takes average samples of a <see cref="Transform.position"/> and <see cref="Transform.rotation"/> and uses this cached data to estimate velocity and angular velocity.
    /// </summary>
    public class VelocityEstimator : VelocityTracker
    {
        /// <summary>
        /// The source <see cref="Transform"/> to track and estimate velocities for.
        /// </summary>
        [Tooltip("The source Transform to track and estimate velocities for.")]
        public Transform source;
        /// <summary>
        /// Automatically begin collecting samples for estimation.
        /// </summary>
        [Tooltip("Automatically begin collecting samples for estimation.")]
        public bool autoStartSampling = true;
        /// <summary>
        /// The number of average frames to collect samples for velocity estimation.
        /// </summary>
        [Tooltip("The number of average frames to collect samples for velocity estimation.")]
        public int velocityAverageFrames = 5;
        /// <summary>
        /// The number of average frames to collect samples for angular velocity estimation.
        /// </summary>
        [Tooltip("The number of average frames to collect samples for angular velocity estimation.")]
        public int angularVelocityAverageFrames = 10;

        protected bool collectSamples;
        protected int currentSampleCount = 0;
        protected Vector3[] velocitySamples = Array.Empty<Vector3>();
        protected Vector3[] angularVelocitySamples = Array.Empty<Vector3>();
        protected Vector3 previousPosition = Vector3.zero;
        Quaternion previousRotation = Quaternion.identity;

        /// <inheritdoc />
        public override bool IsActive()
        {
            return (isActiveAndEnabled && source != null && source.gameObject.activeInHierarchy);
        }

        /// <summary>
        /// Manually enables the collecting of samples for the estimation process.
        /// </summary>
        public virtual void StartEstimation()
        {
            collectSamples = true;
        }

        /// <summary>
        /// Manually disables the collecting of samples for the estimation process.
        /// </summary>
        public virtual void EndEstimation()
        {
            collectSamples = false;
        }

        /// <summary>
        /// Gets whether samples are currently being collected.
        /// </summary>
        /// <returns><see langword="true"/> if samples are currently being collected.</returns>
        public virtual bool IsEstimating()
        {
            return collectSamples;
        }

        /// <summary>
        /// The acceleration of the <see cref="source"/>.
        /// </summary>
        /// <returns>Acceleration of the <see cref="source"/>.</returns>
        public virtual Vector3 GetAcceleration()
        {
            if (!IsActive())
            {
                return Vector3.zero;
            }

            Vector3 average = Vector3.zero;
            for (int i = 2 + currentSampleCount - velocitySamples.Length; i < currentSampleCount; i++)
            {
                if (i >= 2)
                {
                    int first = i - 2;
                    int second = i - 1;

                    Vector3 v1 = velocitySamples[first % velocitySamples.Length];
                    Vector3 v2 = velocitySamples[second % velocitySamples.Length];
                    average += v2 - v1;
                }
            }
            average *= GetFactor();
            return average;
        }

        protected virtual void OnEnable()
        {
            velocitySamples = new Vector3[velocityAverageFrames];
            angularVelocitySamples = new Vector3[angularVelocityAverageFrames];
            previousPosition = (source != null ? source.localPosition : Vector3.zero);
            previousRotation = (source != null ? source.localRotation : Quaternion.identity);
            collectSamples = autoStartSampling;
        }

        protected virtual void LateUpdate()
        {
            ProcessEstimation();
        }

        /// <inheritdoc />
        protected override Vector3 DoGetVelocity()
        {
            return GetEstimate(velocitySamples);
        }

        /// <inheritdoc />
        protected override Vector3 DoGetAngularVelocity()
        {
            return GetEstimate(angularVelocitySamples);
        }

        /// <summary>
        /// Calulates the multiplication factor for the velocities.
        /// </summary>
        /// <returns>Multiplication value.</returns>
        protected virtual float GetFactor()
        {
            return 1.0f / Time.deltaTime;
        }

        /// <summary>
        /// Calculates the average estiamte for the given sample set.
        /// </summary>
        /// <param name="samples">An array of samples to estimate with.</param>
        /// <returns>The estimated result.</returns>
        protected virtual Vector3 GetEstimate(Vector3[] samples)
        {
            Vector3 estimate = Vector3.zero;
            int sampleCount = Mathf.Min(currentSampleCount, samples.Length);
            if (sampleCount != 0)
            {
                for (int i = 0; i < sampleCount; i++)
                {
                    estimate += samples[i];
                }
                estimate *= (1.0f / sampleCount);
            }
            return estimate;
        }

        /// <summary>
        /// Collects the appropriate samples for velocities and estimates the results.
        /// </summary>
        protected virtual void ProcessEstimation()
        {
            if (collectSamples)
            {
                float factor = GetFactor();
                EstimateVelocity(factor);
                EstimateAngularVelocity(factor);
                currentSampleCount++;
            }
            else
            {
                currentSampleCount = 0;
            }
        }

        /// <summary>
        /// Collects samples for velocity.
        /// </summary>
        /// <param name="factor">The multiplier to apply to the transform difference.</param>
        protected virtual void EstimateVelocity(float factor)
        {
            Vector3 currentPosition = (source != null ? source.localPosition : Vector3.zero);
            if (velocitySamples.Length > 0)
            {
                int sampleIndex = currentSampleCount % velocitySamples.Length;
                velocitySamples[sampleIndex] = factor * (currentPosition - previousPosition);
                previousPosition = currentPosition;
            }
        }

        /// <summary>
        /// Collects samples for angular velocity.
        /// </summary>
        /// <param name="factor">The multiplier to apply to the transform difference.</param>
        protected virtual void EstimateAngularVelocity(float factor)
        {
            Quaternion currentRotation = (source != null ? source.localRotation : Quaternion.identity);
            Quaternion deltaRotation = currentRotation * Quaternion.Inverse(previousRotation);
            float theta = 2.0f * Mathf.Acos(Mathf.Clamp(deltaRotation.w, -1.0f, 1.0f));
            if (theta > Mathf.PI)
            {
                theta -= 2.0f * Mathf.PI;
            }

            Vector3 angularVelocity = new Vector3(deltaRotation.x, deltaRotation.y, deltaRotation.z);
            if (angularVelocity.sqrMagnitude > 0.0f)
            {
                angularVelocity = theta * factor * angularVelocity.normalized;
            }

            if (angularVelocitySamples.Length > 0)
            {
                int sampleIndex = currentSampleCount % angularVelocitySamples.Length;
                angularVelocitySamples[sampleIndex] = angularVelocity;
                previousRotation = currentRotation;
            }
        }
    }
}