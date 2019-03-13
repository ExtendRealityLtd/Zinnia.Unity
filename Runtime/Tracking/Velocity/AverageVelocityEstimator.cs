namespace Zinnia.Tracking.Velocity
{
    using UnityEngine;
    using System;
    using Malimbe.MemberChangeMethod;
    using Malimbe.MemberClearanceMethod;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.BehaviourStateRequirementMethod;
    using Zinnia.Extension;

    /// <summary>
    /// Takes average samples of a <see cref="Transform.position"/> and <see cref="Transform.rotation"/> and uses this cached data to estimate velocity and angular velocity.
    /// </summary>
    public class AverageVelocityEstimator : VelocityTracker
    {
        /// <summary>
        /// The source to track and estimate velocities for.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public GameObject Source { get; set; }
        /// <summary>
        /// An optional object to consider the source relative to when estimating the velocities.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public GameObject RelativeTo { get; set; }
        /// <summary>
        /// Whether samples are currently being collected.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public bool IsEstimating { get; set; } = true;
        /// <summary>
        /// The number of average frames to collect samples for velocity estimation.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public int VelocityAverageFrames { get; set; } = 5;
        /// <summary>
        /// The number of average frames to collect samples for angular velocity estimation.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public int AngularVelocityAverageFrames { get; set; } = 10;

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
            return base.IsActive() && Source != null && Source.gameObject.activeInHierarchy;
        }

        /// <summary>
        /// The acceleration of the <see cref="Source"/>.
        /// </summary>
        /// <returns>Acceleration of the <see cref="Source"/>.</returns>
        [RequiresBehaviourState]
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
            velocitySamples = new Vector3[VelocityAverageFrames];
            angularVelocitySamples = new Vector3[AngularVelocityAverageFrames];
            previousPosition = Source.TryGetPosition();
            previousRotation = Source.TryGetRotation();
            previousRelativePosition = RelativeTo.TryGetPosition();
            previousRelativeRotation = RelativeTo.TryGetRotation();
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
        /// Calculates the multiplication factor for the velocities.
        /// </summary>
        /// <returns>Multiplication value.</returns>
        protected virtual float GetFactor()
        {
            return 1.0f / Time.deltaTime;
        }

        /// <summary>
        /// Calculates the average estimate for the given sample set.
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
                estimate *= 1.0f / sampleCount;
            }
            return estimate;
        }

        /// <summary>
        /// Collects the appropriate samples for velocities and estimates the results.
        /// </summary>
        protected virtual void ProcessEstimation()
        {
            if (IsEstimating)
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

            Vector3 currentRelativePosition = RelativeTo.TryGetPosition();
            Vector3 relativeDeltaPosition = currentRelativePosition - previousRelativePosition;
            Vector3 currentPosition = Source.TryGetPosition();
            int sampleIndex = currentSampleCount % velocitySamples.Length;
            velocitySamples[sampleIndex] = factor * (currentPosition - previousPosition - relativeDeltaPosition);
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

            Quaternion currentRelativeRotation = RelativeTo.TryGetRotation();
            Quaternion relativeDeltaRotation = currentRelativeRotation * Quaternion.Inverse(previousRelativeRotation);
            Quaternion currentRotation = Source.TryGetRotation();
            Quaternion deltaRotation = Quaternion.Inverse(relativeDeltaRotation) * (currentRotation * Quaternion.Inverse(previousRotation));
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

        /// <summary>
        /// Called after <see cref="Source"/> has been changed.
        /// </summary>
        [CalledAfterChangeOf(nameof(Source))]
        protected virtual void OnAfterSourceChange()
        {
            previousPosition = Source.TryGetPosition();
            previousRotation = Source.TryGetRotation();
        }

        /// <summary>
        /// Called after <see cref="RelativeTo"/> has been changed.
        /// </summary>
        [CalledAfterChangeOf(nameof(RelativeTo))]
        protected virtual void OnAfterRelativeToChange()
        {
            previousRelativePosition = RelativeTo.TryGetPosition();
            previousRelativeRotation = RelativeTo.TryGetRotation();
        }

        /// <summary>
        /// Called after <see cref="VelocityAverageFrames"/> has been changed.
        /// </summary>
        [CalledAfterChangeOf(nameof(VelocityAverageFrames))]
        protected virtual void OnAfterVelocityAverageFramesChange()
        {
            velocitySamples = new Vector3[VelocityAverageFrames];
        }

        /// <summary>
        /// Called after <see cref="AngularVelocityAverageFrames"/> has been changed.
        /// </summary>
        [CalledAfterChangeOf(nameof(AngularVelocityAverageFrames))]
        protected virtual void OnAfterAngularVelocityAverageFramesChange()
        {
            angularVelocitySamples = new Vector3[AngularVelocityAverageFrames];
        }
    }
}