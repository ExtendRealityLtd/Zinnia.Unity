namespace Zinnia.Cast
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using System.Collections.Generic;
    using Malimbe.MemberChangeMethod;
    using Malimbe.MemberClearanceMethod;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.BehaviourStateRequirementMethod;
    using Zinnia.Rule;
    using Zinnia.Process;
    using Zinnia.Data.Type;
    using Zinnia.Extension;

    /// <summary>
    /// The base of casting components that result in points along the cast.
    /// </summary>
    public abstract class PointsCast : MonoBehaviour, IProcessable
    {
        /// <summary>
        /// Holds data about a <see cref="PointsCast"/> event.
        /// </summary>
        [Serializable]
        public class EventData
        {
            /// <summary>
            /// The result of the most recent cast. <see langword="null"/> when the cast didn't hit anything.
            /// </summary>
            [Serialized]
            [field: DocumentedByXml]
            public RaycastHit? HitData { get; set; }

            /// <summary>
            /// The validity of the most recent <see cref="HitData"/> based on the <see cref="TargetValidity"/> rule.
            /// </summary>
            [Serialized]
            [field: DocumentedByXml]
            public bool IsValid { get; set; }

            /// <summary>
            /// The points along the the most recent cast.
            /// </summary>
            public HeapAllocationFreeReadOnlyList<Vector3> Points { get; set; }

            public EventData Set(EventData source)
            {
                return Set(source.HitData, source.IsValid, source.Points);
            }

            public EventData Set(RaycastHit? targetHit, bool isValid, HeapAllocationFreeReadOnlyList<Vector3> points)
            {
                HitData = targetHit;
                IsValid = isValid;
                Points = points;
                return this;
            }

            public void Clear()
            {
                Set(default, default, default);
            }
        }

        /// <summary>
        /// Defines the event with the <see cref="EventData"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<EventData> { }

        /// <summary>
        /// The origin point for the cast.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public GameObject Origin { get; set; }
        /// <summary>
        /// Allows to optionally affect the cast.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public PhysicsCast PhysicsCast { get; set; }
        /// <summary>
        /// Allows to optionally determine targets based on the set rules.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public RuleContainer TargetValidity { get; set; }

        /// <summary>
        /// An override for the destination location point in world space.
        /// </summary>
        public Vector3? DestinationPointOverride { get; set; }

        /// <summary>
        /// Emitted whenever the cast result changes.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent ResultsChanged = new UnityEvent();

        /// <summary>
        /// The result of the most recent cast. <see langword="null"/> when the cast didn't hit anything or an invalid target according to <see cref="TargetValidity"/>.
        /// </summary>
        public RaycastHit? TargetHit { get; protected set; }
        /// <summary>
        /// Whether the current <see cref="TargetHit"/> is valid based on the <see cref="TargetValidity"/> rule.
        /// </summary>
        public bool IsTargetHitValid { get; protected set; }
        /// <summary>
        /// The points along the the most recent cast.
        /// </summary>
        public HeapAllocationFreeReadOnlyList<Vector3> Points => points;

        /// <summary>
        /// The points along the the most recent cast.
        /// </summary>
        protected readonly List<Vector3> points = new List<Vector3>();
        /// <summary>
        /// The data to emit with an event.
        /// </summary>
        protected readonly EventData eventData = new EventData();

        /// <summary>
        /// Casts and creates points along the cast.
        /// </summary>
        [RequiresBehaviourState]
        public virtual void CastPoints()
        {
            if (Origin == null)
            {
                return;
            }
            DoCastPoints();
        }

        /// <inheritdoc />
        public virtual void Process()
        {
            CastPoints();
        }

        /// <summary>
        /// Clears the <see cref="DestinationPointOverride"/>.
        /// </summary>
        public virtual void ClearDestinationPointOverride()
        {
            DestinationPointOverride = null;
        }

        /// <summary>
        /// Performs the implemented way of casting points.
        /// </summary>
        protected abstract void DoCastPoints();

        protected virtual void OnEnable()
        {
            OnAfterTargetHitChange();
        }

        protected virtual void OnDisable()
        {
            ClearDestinationPointOverride();
        }

        /// <summary>
        /// Called after <see cref="TargetHit"/> has been changed.
        /// </summary>
        [CalledAfterChangeOf(nameof(TargetHit))]
        protected virtual void OnAfterTargetHitChange()
        {
            IsTargetHitValid = TargetHit != null && TargetValidity.Accepts(TargetHit.Value.transform.gameObject);
        }
    }
}