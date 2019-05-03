namespace Zinnia.Cast
{
    using UnityEngine;
    using System.Collections.Generic;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;
    using Zinnia.Utility;

    /// <summary>
    /// Casts a parabolic line and creates points at the origin, the target and in between.
    /// </summary>
    public class ParabolicLineCast : PointsCast
    {
        /// <summary>
        /// The maximum length of the projected cast. The x value is the length of the forward cast, the y value is the length of the downward cast.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public Vector2 MaximumLength { get; set; } = new Vector2(10f, float.PositiveInfinity);
        /// <summary>
        /// The maximum angle in degrees of the origin before the cast line height is restricted. A lower angle setting will prevent the cast being projected high into the sky and curving back down.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml, Range(1f, 100f)]
        public float HeightLimitAngle { get; set; } = 100f;
        /// <summary>
        /// The number of points to generate on the parabolic line.
        /// </summary>
        /// <remarks>The higher the number, the more CPU intensive the point generation becomes.</remarks>
        [Serialized]
        [field: DocumentedByXml]
        public int SegmentCount { get; set; } = 10;
        /// <summary>
        /// The number of points along the parabolic line to check for an early cast collision. Useful if the parabolic line is appearing to clip through locations. 0 won't make any checks and it will be capped at <see cref="SegmentCount" />.
        /// </summary>
        /// <remarks>The higher the number, the more CPU intensive the checks become.</remarks>
        [Serialized]
        [field: DocumentedByXml]
        public int CollisionCheckFrequency { get; set; }
        /// <summary>
        /// The amount of height offset to apply to the projected cast to generate a smoother line even when the cast is pointing straight.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public float CurveOffset { get; set; } = 1f;

        /// <summary>
        /// Used to move the points back and up a bit to prevent the cast clipping at the collision points.
        /// </summary>
        protected const float AdjustmentOffset = 0.0001f;
        /// <summary>
        /// A reusable collection of <see cref="Vector3"/>s.
        /// </summary>
        protected readonly List<Vector3> curvePoints = new List<Vector3>();

        protected override void OnEnable()
        {
            base.OnEnable();
            curvePoints.Add(default);
            curvePoints.Add(default);
            curvePoints.Add(default);
            curvePoints.Add(default);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            curvePoints.Clear();
        }

        /// <inheritdoc />
        protected override void DoCastPoints()
        {
            Vector3 forward = ProjectForward();
            Vector3 down = ProjectDown(forward);
            GeneratePointsIncludingSegments(forward, down);
        }

        /// <summary>
        /// Projects a straight line forward from the current origin.
        /// </summary>
        /// <returns>The collision point or the point being the furthest away on the cast line if nothing is hit.</returns>
        protected virtual Vector3 ProjectForward()
        {
            float rotation = Vector3.Dot(Vector3.up, Origin.transform.forward.normalized);
            float length = MaximumLength.x;

            if ((rotation * 100f) > HeightLimitAngle)
            {
                float controllerRotationOffset = 1f - (rotation - HeightLimitAngle / 100f);
                length = MaximumLength.x * controllerRotationOffset * controllerRotationOffset;
            }

            Ray ray = new Ray(Origin.transform.position, Origin.transform.forward);
            bool hasCollided = PhysicsCast.Raycast(PhysicsCast, ray, out RaycastHit hitData, length, Physics.IgnoreRaycastLayer);

            // Adjust the cast length if something is blocking it.
            if (hasCollided && hitData.distance < length)
            {
                length = hitData.distance;
            }

            // Use an offset to move the point back and up a bit to prevent the cast clipping at the collision point.
            return ray.GetPoint(length - AdjustmentOffset) + (Vector3.up * AdjustmentOffset);
        }

        /// <summary>
        /// Projects a straight line downwards from the provided point.
        /// </summary>
        /// <param name="downwardOrigin">The origin of the projected line.</param>
        /// <returns>The collision point or the point being the furthest away on the cast line if nothing is hit.</returns>
        protected virtual Vector3 ProjectDown(Vector3 downwardOrigin)
        {
            Vector3 point = Vector3.zero;
            Ray ray = new Ray(downwardOrigin, Vector3.down);

            bool downRayHit = PhysicsCast.Raycast(PhysicsCast, ray, out RaycastHit hitData, MaximumLength.y, Physics.IgnoreRaycastLayer);

            if (!downRayHit || (TargetHit?.collider != null && TargetHit.Value.collider != hitData.collider))
            {
                TargetHit = null;
                point = ray.GetPoint(0f);
            }

            if (downRayHit)
            {
                point = ray.GetPoint(hitData.distance);
                TargetHit = hitData;
            }

            return point;
        }

        /// <summary>
        /// Checks for collisions along the parabolic line segments and generates the final points.
        /// </summary>
        /// <param name="forward">The forward direction to use for the checks.</param>
        /// <param name="down">The downwards direction to use for the checks.</param>
        protected virtual void GeneratePointsIncludingSegments(Vector3 forward, Vector3 down)
        {
            GeneratePoints(forward, down);

            CollisionCheckFrequency = Mathf.Clamp(CollisionCheckFrequency, 0, SegmentCount);
            int step = SegmentCount / (CollisionCheckFrequency > 0 ? CollisionCheckFrequency : 1);

            for (int index = 0; index < SegmentCount - step; index += step)
            {
                Vector3 currentPoint = points[index];
                Vector3 nextPoint = index + step < points.Count ? points[index + step] : points[points.Count - 1];
                Vector3 nextPointDirection = (nextPoint - currentPoint).normalized;
                float nextPointDistance = Vector3.Distance(currentPoint, nextPoint);

                Ray pointsRay = new Ray(currentPoint, nextPointDirection);

                if (!PhysicsCast.Raycast(PhysicsCast, pointsRay, out RaycastHit pointsHitData, nextPointDistance, Physics.IgnoreRaycastLayer))
                {
                    continue;
                }

                Vector3 collisionPoint = pointsRay.GetPoint(pointsHitData.distance);
                Ray downwardRay = new Ray(collisionPoint + Vector3.up * 0.01f, Vector3.down);

                if (!PhysicsCast.Raycast(PhysicsCast, downwardRay, out RaycastHit downwardHitData, float.PositiveInfinity, Physics.IgnoreRaycastLayer))
                {
                    TargetHit = null;
                    continue;
                }

                TargetHit = downwardHitData;

                Vector3 newDownPosition = downwardRay.GetPoint(downwardHitData.distance);
                Vector3 newJointPosition = newDownPosition.y < forward.y ? new Vector3(newDownPosition.x, forward.y, newDownPosition.z) : forward;
                GeneratePoints(newJointPosition, newDownPosition);

                break;
            }

            ResultsChanged?.Invoke(eventData.Set(TargetHit, IsTargetHitValid, Points));
        }

        /// <summary>
        /// Generates points on a parabolic line.
        /// </summary>
        /// <param name="forward">The end point of the forward cast.</param>
        /// <param name="down">The end point of the down cast.</param>
        /// <returns>The generated points on the parabolic line.</returns>
        protected virtual void GeneratePoints(Vector3 forward, Vector3 down)
        {
            forward = DestinationPointOverride != null ? (Vector3)DestinationPointOverride : forward;
            down = DestinationPointOverride != null ? (Vector3)DestinationPointOverride : down;

            curvePoints[0] = Origin.transform.position;
            curvePoints[1] = forward + (Vector3.up * CurveOffset);
            curvePoints[2] = down;
            curvePoints[3] = down;

            points.Clear();
            foreach (Vector3 generatedPoint in BezierCurveGenerator.GeneratePoints(SegmentCount, curvePoints))
            {
                points.Add(generatedPoint);
            }
        }
    }
}