namespace VRTK.Core.Tracking.Follow
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using VRTK.Core.Process;

    /// <summary>
    /// Compares the distance between two GameObjects and emits an event when a given threshold is exceeded or falls within it.
    /// </summary>
    public class ObjectDistanceComparator : MonoBehaviour, IProcessable
    {
        /// <summary>
        /// Defines the event with the <see cref="float"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<float>
        {
        }

        /// <summary>
        /// The source of the distance measurement.
        /// </summary>
        [Tooltip("The source of the distance measurement.")]
        public GameObject source;
        /// <summary>
        /// The target of the distance measurement.
        /// </summary>
        [Tooltip("The target of the distance measurement.")]
        public GameObject target;
        /// <summary>
        /// The distance between the source and target that is considered to be exceeding the given threshold.
        /// </summary>
        [Tooltip("The distance between the source and target that is considered to be exceeding the given threshold.")]
        public float distanceThreshold = 1f;

        /// <summary>
        /// Emitted when the distance between the source and the target exceeds the threshold.
        /// </summary>
        public UnityEvent ThresholdExceeded = new UnityEvent();
        /// <summary>
        /// Emitted when the distance between the source and the target falls back within the threshold.
        /// </summary>
        public UnityEvent ThresholdResumed = new UnityEvent();

        /// <summary>
        /// The distance between the source and target.
        /// </summary>
        public float Distance
        {
            get;
            protected set;
        }

        /// <summary>
        /// Determines if the distance between the source and target is exceeding the threshold.
        /// </summary>
        public bool Exceeding
        {
            get;
            protected set;
        }

        protected bool previousState = false;

        /// <summary>
        /// Checks to see if the distance between the source and target exceed the threshold.
        /// </summary>
        public virtual void Process()
        {
            if (!isActiveAndEnabled || source == null || target == null)
            {
                return;
            }

            Distance = Vector3.Distance(source.transform.position, target.transform.position);
            Exceeding = (Distance >= distanceThreshold);
            if (previousState != Exceeding)
            {
                if (Exceeding)
                {
                    ThresholdExceeded?.Invoke(Distance);
                }
                else
                {
                    ThresholdResumed?.Invoke(Distance);
                }
            }

            previousState = Exceeding;
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
        /// Sets the <see cref="target"/> parameter.
        /// </summary>
        /// <param name="target">The new target value.</param>
        public virtual void SetTarget(GameObject target)
        {
            this.target = target;
        }

        /// <summary>
        /// Clears the <see cref="target"/> parameter.
        /// </summary>
        public virtual void ClearTarget()
        {
            target = null;
        }
    }
}