namespace Zinnia.Tracking.Velocity
{
    using UnityEngine;
    using System;
    using Zinnia.Extension;

    /// <summary>
    /// Takes average samples of a <see cref="Transform.position"/> and <see cref="Transform.rotation"/> and uses this cached data to estimate velocity and angular velocity.
    /// </summary>
    public class AverageVelocityEstimator : VelocityTracker
    {
        /// <summary>
        /// The source to track and estimate velocities for.
        /// </summary>
        public GameObject source;
        /// <summary>
        /// An optional object to consider the source relative to when estimating the velocities.
        /// </summary>
        public GameObject relativeTo;
        /// <summary>
        /// Automatically begin collecting samples for estimation.
        /// </summary>
        public bool autoStartSampling = true;
        /// <summary>
        /// The number of average frames to collect samples for velocity estimation.
        /// </summary>
        public int velocityAverageFrames = 5;
        /// <summary>
        /// The number of average frames to collect samples for angular velocity estimation.
        /// </summary>
        public int angularVelocityAverageFrames = 10;

        protected bool collectSamples;
        protected int currentSampleCount;
        protected Vector3[] velocitySamples = Array.Empty<Vector3>();
        protected Vector3[] angularVelocitySamples = Array.Empty<Vector3>();
        protected Vector3 previousPosition = Vector3.zero;
        protected Quaternion previousRotation = Quaternion.identity;
        protected Vector3 previousRelativePosition = Vector3.zero;
        protected Quaternion previousRelativeRotation = Quaternion.identity;

        /// <inheritdoc />
        public override bool IsActive()
        {
            return (base.IsActive() && source != null && source.gameObject.activeInHierarchy);
        }

        /// <summary>
        /// Sets the <see cref="source"/> parameter.
        /// </summary>
        /// <param name="source">The new source value.</param>
        public virtual void SetSource(GameObject source)
        {
            this.source = source;
        }

        /// <summary>
        /// Clears the <see cref="source"/> parameter.
        /// </summary>
        public virtual void ClearSource()
        {
            source = null;
        }

        /// <summary>
        /// Sets the <see cref="relativeTo"/> parameter.
        /// </summary>
        /// <param name="relativeTo">The new relativeTo value.</param>
        public virtual void SetRelativeTo(GameObject relativeTo)
        {
            this.relativeTo = relativeTo;
        }

        /// <summary>
        /// Clears the <see cref="relativeTo"/> parameter.
        /// </summary>
        public virtual void ClearRelativeTo()
        {
            relativeTo = null;
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
            for (int sampleIndex = 2 + currentSampleCount - velocitySamples.Length; sampleIndex < currentSampleCount; sampleIndex++)
            {
                if (sampleIndex >= 2)
                {
                    int first = sampleIndex - 2;
                    int second = sampleIndex - 1;

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
            previousPosition = source.TryGetPosition();
            previousRotation = source.TryGetRotation();
            previousRelativePosition = relativeTo.TryGetPosition();
            previousRelativeRotation = relativeTo.TryGetRotation();
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
                for (int index = 0; index < sampleCount; index++)
                {
                    estimate += samples[index];
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
            if (velocitySamples.Length == 0)
            {
                return;
            }

            Vector3 currentRelativePosition = relativeTo.TryGetPosition();
            Vector3 relativeDeltaPosition = currentRelativePosition - previousRelativePosition;
            Vector3 currentPosition = source.TryGetPosition();
            int sampleIndex = currentSampleCount % velocitySamples.Length;
            velocitySamples[sampleIndex] = factor * ((currentPosition - previousPosition) - relativeDeltaPosition);
            previousPosition = currentPosition;
            previousRelativePosition = currentRelativePosition;
        }

        /// <summary>
        /// Collects samples for angular velocity.
        /// </summary>
        /// <param name="factor">The multiplier to apply to the transform difference.</param>
        protected virtual void EstimateAngularVelocity(float factor)
        {
            if (angularVelocitySamples.Length == 0)
            {
                return;
            }

            Quaternion currentRelativeRotation = relativeTo.TryGetRotation();
            Quaternion relativeDelataRotation = currentRelativeRotation * Quaternion.Inverse(previousRelativeRotation);
            Quaternion currentRotation = source.TryGetRotation();
            Quaternion deltaRotation = Quaternion.Inverse(relativeDelataRotation) * (currentRotation * Quaternion.Inverse(previousRotation));
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

            int sampleIndex = currentSampleCount % angularVelocitySamples.Length;
            angularVelocitySamples[sampleIndex] = angularVelocity;
            previousRotation = currentRotation;
            previousRelativeRotation = currentRelativeRotation;
        }
    }
}