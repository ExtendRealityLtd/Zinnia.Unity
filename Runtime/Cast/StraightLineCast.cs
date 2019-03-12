﻿namespace Zinnia.Cast
{
    using UnityEngine;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.MemberChangeMethod;

    /// <summary>
    /// Casts a straight line and creates points at the origin and target.
    /// </summary>
    public class StraightLineCast : PointsCast
    {
        /// <summary>
        /// The maximum length to cast.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public float MaximumLength { get; set; } = 100f;

        protected virtual void OnEnable()
        {
            points.Add(Vector3.zero);
            points.Add(Vector3.zero);
        }

        protected virtual void OnDisable()
        {
            points.Clear();
        }

        /// <inheritdoc />
        protected override void DoCastPoints()
        {
            GeneratePoints();
            ResultsChanged?.Invoke(eventData.Set(TargetHit, Points));
        }

        /// <summary>
        /// Generates the points for the cast.
        /// </summary>
        protected virtual void GeneratePoints()
        {
            Vector3 originPosition = Origin.transform.position;
            Vector3 originForward = Origin.transform.forward;

            Ray ray = new Ray(originPosition, originForward);
            bool hasCollided = PhysicsCast.Raycast(PhysicsCast, ray, out RaycastHit hitData, MaximumLength, Physics.IgnoreRaycastLayer);
            TargetHit = hasCollided ? hitData : (RaycastHit?)null;

            points[0] = originPosition;
            points[1] = hasCollided ? hitData.point : originPosition + originForward * MaximumLength;
        }

        /// <summary>
        /// Called after <see cref="MaximumLength"/> has been changed.
        /// </summary>
        [CalledAfterChangeOf(nameof(MaximumLength))]
        protected virtual void OnAfterMaximumLengthChange() { }
    }
}