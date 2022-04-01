namespace Zinnia.Tracking.Velocity
{
    using System;
    using UnityEngine;
    using Zinnia.Extension;
    using Zinnia.Process;

    /// <summary>
    /// Takes average samples of a <see cref="Transform.position"/> and <see cref="Transform.rotation"/> and uses this cached data to estimate velocity and angular velocity.
    /// </summary>
    public class AverageVelocityEstimatorProcess : VelocityTracker, IProcessable
    {
        [Tooltip("The source to track and estimate velocities for.")]
        [SerializeField]
        private GameObject source;
        /// <summary>
        /// The source to track and estimate velocities for.
        /// </summary>
        public GameObject Source
        {
            get
            {
                return source;
            }
            set
            {
                source = value;
                if (this.IsMemberChangeAllowed())
                {
                    OnAfterSourceChange();
                }
            }
        }
        [Tooltip("An optional object to consider the source relative to when estimating the velocities.")]
        [SerializeField]
        private GameObject relativeTo;
        /// <summary>
        /// An optional object to consider the source relative to when estimating the velocities.
        /// </summary>
        public GameObject RelativeTo
        {
            get
            {
                return relativeTo;
            }
            set
            {
                relativeTo = value;
                if (this.IsMemberChangeAllowed())
                {
                    OnAfterRelativeToChange();
                }
            }
        }
        [Tooltip("Whether samples are currently being collected.")]
        [SerializeField]
        private bool isEstimating = true;
        /// <summary>
        /// Whether samples are currently being collected.
        /// </summary>
        public bool IsEstimating
        {
            get
            {
                return isEstimating;
            }
            set
            {
                isEstimating = value;
            }
        }
        [Tooltip("The number of average frames to collect samples for velocity estimation.")]
        [SerializeField]
        private int velocityAverageFrames = 5;
        /// <summary>
        /// The number of average frames to collect samples for velocity estimation.
        /// </summary>
        public int VelocityAverageFrames
        {
            get
            {
                return velocityAverageFrames;
            }
            set
            {
                velocityAverageFrames = value;
                if (this.IsMemberChangeAllowed())
                {
                    OnAfterVelocityAverageFramesChange();
                }
            }
        }
        [Tooltip("The number of average frames to collect samples for angular velocity estimation.")]
        [SerializeField]
        private int angularVelocityAverageFrames = 10;
        /// <summary>
        /// The number of average frames to collect samples for angular velocity estimation.
        /// </summary>
        public int AngularVelocityAverageFrames
        {
            get
            {
                return angularVelocityAverageFrames;
            }
            set
            {
                angularVelocityAverageFrames = value;
                if (this.IsMemberChangeAllowed())
                {
                    OnAfterAngularVelocityAverageFramesChange();
                }
            }
        }

        /// <summary>
        /// The current count of samples to calculate the velocity from.
        /// </summary>
        protected int currentSampleCount;
        /// <summary>
        /// The frame samples of velocity to used to calculate final velocity.
        /// </summary>
        protected Vector3[] velocitySamples = Array.Empty<Vector3>();
        /// <summary>
        /// The frame samples of angular velocity to used to calculate final angular velocity.
        /// </summary>
        protected Vector3[] angularVelocitySamples = Array.Empty<Vector3>();
        /// <summary>
        /// The previous position of the <see cref="Source"/>.
        /// </summary>
        protected Vector3 previousPosition = Vector3.zero;
        /// <summary>
        /// The previous rotation of the <see cref="Source"/>.
        /// </summary>
        protected Quaternion previousRotation = Quaternion.identity;
        /// <summary>
        /// The previous position of the <see cref="RelativeTo"/>.
        /// </summary>
        protected Vector3 previousRelativePosition = Vector3.zero;
        /// <summary>
        /// The previous rotation of the <see cref="RelativeTo"/>.
        /// </summary>
        protected Quaternion previousRelativeRotation = Quaternion.identity;

        /// <summary>
        /// Clears <see cref="Source"/>.
        /// </summary>
        public virtual void ClearSource()
        {
            if (!this.IsValidState())
            {
                return;
            }

            Source = default;
        }

        /// <summary>
        /// Clears <see cref="RelativeTo"/>.
        /// </summary>
        public virtual void ClearRelativeTo()
        {
            if (!this.IsValidState())
            {
                return;
            }

            RelativeTo = default;
        }

        /// <inheritdoc />
        public override bool IsActive()
        {
            return base.IsActive() && Source != null && Source.activeInHierarchy;
        }

        /// <summary>
        /// The acceleration of the <see cref="Source"/>.
        /// </summary>
        /// <returns>Acceleration of the <see cref="Source"/>.</returns>
        public virtual Vector3 GetAcceleration()
        {
            if (!this.IsValidState() || !IsActive())
            {
                return default;
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

        /// <inheritdoc />
        public virtual void Process()
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
            return 1.0f / (Time.inFixedTimeStep ? Time.fixedDeltaTime : Time.deltaTime);
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
        protected virtual void OnAfterSourceChange()
        {
            previousPosition = Source.TryGetPosition();
            previousRotation = Source.TryGetRotation();
        }

        /// <summary>
        /// Called after <see cref="RelativeTo"/> has been changed.
        /// </summary>
        protected virtual void OnAfterRelativeToChange()
        {
            previousRelativePosition = RelativeTo.TryGetPosition();
            previousRelativeRotation = RelativeTo.TryGetRotation();
        }

        /// <summary>
        /// Called after <see cref="VelocityAverageFrames"/> has been changed.
        /// </summary>
        protected virtual void OnAfterVelocityAverageFramesChange()
        {
            velocitySamples = new Vector3[VelocityAverageFrames];
        }

        /// <summary>
        /// Called after <see cref="AngularVelocityAverageFrames"/> has been changed.
        /// </summary>
        protected virtual void OnAfterAngularVelocityAverageFramesChange()
        {
            angularVelocitySamples = new Vector3[AngularVelocityAverageFrames];
        }
    }
}