namespace Zinnia.Cast
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using System.Collections.Generic;
    using Malimbe.MemberClearanceMethod;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.BehaviourStateRequirementMethod;
    using Zinnia.Extension;
    using Zinnia.Process;
    using Zinnia.Rule;

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
            public RaycastHit? TargetHit { get; set; }

            /// <summary>
            /// The points along the the most recent cast.
            /// </summary>
            public IReadOnlyList<Vector3> Points { get; set; }

            public EventData Set(EventData source)
            {
                return Set(source.TargetHit, source.Points);
            }

            public EventData Set(RaycastHit? targetHit, IReadOnlyList<Vector3> points)
            {
                TargetHit = targetHit;
                Points = points;
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
        /// Emitted whenever the cast result changes.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent ResultsChanged = new UnityEvent();

        /// <summary>
        /// The result of the most recent cast. <see langword="null"/> when the cast didn't hit anything or an invalid target according to <see cref="TargetValidity"/>.
        /// </summary>
        public RaycastHit? TargetHit
        {
            get
            {
                return targetHit;
            }
            protected set
            {
                targetHit = value != null && TargetValidity.Accepts(value.Value.transform.gameObject) ? value : null;
            }
        }
        /// <summary>
        /// The data held for a valid raycast hit.
        /// </summary>
        private RaycastHit? targetHit;

        /// <summary>
        /// The points along the the most recent cast.
        /// </summary>
        public IReadOnlyList<Vector3> Points => points;

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
        /// Performs the implemented way of casting points.
        /// </summary>
        protected abstract void DoCastPoints();
    }
}