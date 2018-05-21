namespace VRTK.Core.Cast
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using System.Collections.Generic;
    using VRTK.Core.Process;
    using VRTK.Core.Utility;

    /// <summary>
    /// Contains information about the current <see cref="PointsCast"/> state.
    /// </summary>
    public class PointsCastData
    {
        /// <summary>
        /// The result of the most recent cast. <see langword="null"/> when the cast didn't hit anything.
        /// </summary>
        public RaycastHit? targetHit;
        /// <summary>
        /// The points along the the most recent cast.
        /// </summary>
        public IReadOnlyList<Vector3> points;
    }

    /// <summary>
    /// The base of casting components that result in points along the cast.
    /// </summary>
    public abstract class PointsCast : MonoBehaviour, IProcessable
    {
        /// <summary>
        /// Allows to optionally affect the cast.
        /// </summary>
        [Tooltip("Allows to optionally affect the cast.")]
        public PhysicsCast physicsCast;
        /// <summary>
        /// Allows to optionally determine targets based on the set rules.
        /// </summary>
        [Tooltip("Allows to optionally determine targets based on the set rules.")]
        public ExclusionRule targetValidity;

        /// <summary>
        /// Defines the event with the <see cref="PointsCastData"/> state and sender <see cref="object"/>.
        /// </summary>
        [Serializable]
        public class PointsCastUnityEvent : UnityEvent<PointsCastData, object>
        {
        }

        /// <summary>
        /// Emitted whenever the cast result changes.
        /// </summary>
        public PointsCastUnityEvent CastResultsChanged = new PointsCastUnityEvent();

        /// <summary>
        /// The result of the most recent cast. <see langword="null"/> when the cast didn't hit anything or an invalid target according to <see cref="targetValidity"/>.
        /// </summary>
        public RaycastHit? TargetHit
        {
            get
            {
                return targetHit;
            }
            protected set
            {
                targetHit = value != null && !ExclusionRule.ShouldExclude(value.Value.transform.gameObject, targetValidity) ? value : null;
            }
        }

        /// <summary>
        /// The points along the the most recent cast.
        /// </summary>
        public IReadOnlyList<Vector3> Points => points;

        protected List<Vector3> points = new List<Vector3>();

        private RaycastHit? targetHit;

        /// <summary>
        /// Casts and creates points along the cast.
        /// </summary>
        public abstract void CastPoints();

        /// <inheritdoc />
        public void Process()
        {
            CastPoints();
        }

        /// <summary>
        /// Builds the event payload for the current state of the cast.
        /// </summary>
        /// <returns>The current state of the cast.</returns>
        protected virtual PointsCastData GetPayload()
        {
            return new PointsCastData
            {
                targetHit = TargetHit,
                points = Points
            };
        }

        protected virtual void OnCastResultsChanged()
        {
            CastResultsChanged?.Invoke(GetPayload(), this);
        }
    }
}