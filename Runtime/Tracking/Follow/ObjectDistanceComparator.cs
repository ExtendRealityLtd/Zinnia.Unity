namespace Zinnia.Tracking.Follow
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using Malimbe.MemberChangeMethod;
    using Malimbe.MemberClearanceMethod;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.BehaviourStateRequirementMethod;
    using Zinnia.Process;

    /// <summary>
    /// Compares the distance between two GameObjects and emits an event when a given threshold is exceeded or falls within it.
    /// </summary>
    /// <remarks>
    /// If the <see cref="Source"/> and the <see cref="Target"/> are the same <see cref="GameObject"/> then the initial position of the <see cref="Target"/> is used as the <see cref="Source"/> position.
    /// </remarks>
    public class ObjectDistanceComparator : MonoBehaviour, IProcessable
    {
        /// <summary>
        /// Holds data about a <see cref="ObjectDistanceComparator"/> event.
        /// </summary>
        [Serializable]
        public class EventData
        {
            /// <summary>
            /// The difference of the positions of the target and source.
            /// </summary>
            [Serialized]
            [field: DocumentedByXml]
            public Vector3 CurrentDifference { get; set; }
            /// <summary>
            /// The distance between the source and target.
            /// </summary>
            [Serialized]
            [field: DocumentedByXml]
            public float CurrentDistance { get; set; }

            public EventData Set(EventData source)
            {
                return Set(source.CurrentDifference, source.CurrentDistance);
            }

            public EventData Set(Vector3 difference, float distance)
            {
                CurrentDifference = difference;
                CurrentDistance = distance;
                return this;
            }

            public void Clear()
            {
                Set(default, default);
            }
        }

        /// <summary>
        /// Defines the event with the <see cref="EventData"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<EventData>
        {
        }

        /// <summary>
        /// The source of the distance measurement.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public GameObject Source { get; set; }
        /// <summary>
        /// The target of the distance measurement.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public GameObject Target { get; set; }
        /// <summary>
        /// The distance between the source and target that is considered to be exceeding the given threshold.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public float DistanceThreshold { get; set; } = 1f;

        /// <summary>
        /// Emitted when the distance between the source and the target exceeds the threshold.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent ThresholdExceeded = new UnityEvent();
        /// <summary>
        /// Emitted when the distance between the source and the target falls back within the threshold.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent ThresholdResumed = new UnityEvent();

        /// <summary>
        /// The difference of the positions of the target and source.
        /// </summary>
        public Vector3 Difference { get; protected set; }

        /// <summary>
        /// The distance between the source and target.
        /// </summary>
        public float Distance { get; protected set; }

        /// <summary>
        /// Determines if the distance between the source and target is exceeding the threshold.
        /// </summary>
        public bool Exceeding { get; protected set; }

        /// <summary>
        /// The previous state of the distance threshold being exceeded.
        /// </summary>
        protected bool previousState;
        /// <summary>
        /// The current position of <see cref="Source"/>.
        /// </summary>
        protected Vector3 sourcePosition;
        /// <summary>
        /// The event data to emit.
        /// </summary>
        protected readonly EventData eventData = new EventData();

        /// <summary>
        /// Checks to see if the distance between the source and target exceed the threshold.
        /// </summary>
        [RequiresBehaviourState]
        public virtual void Process()
        {
            if (Source == null || Target == null)
            {
                return;
            }

            Difference = Target.transform.position - GetSourcePosition();
            Distance = Difference.magnitude;
            Exceeding = Distance >= DistanceThreshold;

            bool didStateChange = previousState != Exceeding;
            previousState = Exceeding;

            if (!didStateChange && DistanceThreshold > 0f)
            {
                return;
            }

            eventData.Set(Difference, Distance);

            if (Exceeding)
            {
                ThresholdExceeded?.Invoke(eventData);
            }
            else
            {
                ThresholdResumed?.Invoke(eventData);
            }
        }

        /// <summary>
        /// Attempts to save the current <see cref="Target"/> position as the initial position if the <see cref="Source"/> and the <see cref="Target"/> are the same <see cref="GameObject"/>.
        /// </summary>
        public virtual void SavePosition()
        {
            if (Source == null || Source != Target)
            {
                return;
            }

            sourcePosition = Target.transform.position;
            previousState = false;
        }

        protected virtual void OnEnable()
        {
            SavePosition();
        }

        /// <summary>
        /// Gets the actual position for the <see cref="Source"/> based on whether it's a different <see cref="GameObject"/> or whether it is set up to use the initial position of the <see cref="Target"/>.
        /// </summary>
        /// <returns>The appropriate position.</returns>
        protected virtual Vector3 GetSourcePosition()
        {
            if (Source == null)
            {
                return Vector3.zero;
            }

            return Source == Target ? sourcePosition : Source.transform.position;
        }

        /// <summary>
        /// Called after <see cref="Source"/> has been changed.
        /// </summary>
        [CalledAfterChangeOf(nameof(Source))]
        protected virtual void OnAfterSourceChange()
        {
            SavePosition();
        }

        /// <summary>
        /// Called after <see cref="Target"/> has been changed.
        /// </summary>
        [CalledAfterChangeOf(nameof(Target))]
        protected virtual void OnAfterTargetChange()
        {
            SavePosition();
        }
    }
}