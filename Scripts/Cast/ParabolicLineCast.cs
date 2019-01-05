namespace Zinnia.Cast
{
    using UnityEngine;
    using Zinnia.Utility;

    /// <summary>
    /// Casts a parabolic line and creates points at the origin, the target and in between.
    /// </summary>
    public class ParabolicLineCast : PointsCast
    {
        /// <summary>
        /// The maximum length of the projected cast. The x value is the length of the forward cast, the y value is the length of the downward cast.
        /// </summary>
        public Vector2 maximumLength = new Vector2(10f, float.PositiveInfinity);
        /// <summary>
        /// The maximum angle in degrees of the origin before the cast line height is restricted. A lower angle setting will prevent the cast being projected high into the sky and curving back down.
        /// </summary>
        [Range(1, 100)]
        public float heightLimitAngle = 100f;
        /// <summary>
        /// The number of points to generate on the parabolic line.
        /// </summary>
        /// <remarks>The higher the number, the more CPU intensive the point generation becomes.</remarks>
        public int segmentCount = 10;
        /// <summary>
        /// The number of points along the parabolic line to check for an early cast collision. Useful if the parabolic line is appearing to clip through locations. 0 won't make any checks and it will be capped at <see cref="segmentCount" />.
        /// </summary>
        /// <remarks>The higher the number, the more CPU intensive the checks become.</remarks>
        public int collisionCheckFrequency;
        /// <summary>
        /// The amount of height offset to apply to the projected cast to generate a smoother line even when the cast is pointing straight.
        /// </summary>
        public float curveOffset = 1f;

        /// <summary>
        /// Used to move the points back and up a bit to prevent the cast clipping at the collision points.
        /// </summary>
        protected const float ADJUSTMENT_OFFSET = 0.0001f;

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
            float rotation = Vector3.Dot(Vector3.up, origin.transform.forward.normalized);
            float length = maximumLength.x;

            if ((rotation * 100f) > heightLimitAngle)
            {
                float controllerRotationOffset = 1f - (rotation - heightLimitAngle / 100f);
                length = maximumLength.x * controllerRotationOffset * controllerRotationOffset;
            }

            Ray ray = new Ray(origin.transform.position, origin.transform.forward);
            RaycastHit hitData;
            bool hasCollided = PhysicsCast.Raycast(physicsCast, ray, out hitData, length, Physics.IgnoreRaycastLayer);

            // Adjust the cast length if something is blocking it.
            if (hasCollided && hitData.distance < length)
            {
                length = hitData.distance;
            }

            // Use an offset to move the point back and up a bit to prevent the cast clipping at the collision point.
            return ray.GetPoint(length - ADJUSTMENT_OFFSET) + (Vector3.up * ADJUSTMENT_OFFSET);
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
            RaycastHit hitData;

            bool downRayHit = PhysicsCast.Raycast(physicsCast, ray, out hitData, maximumLength.y, Physics.IgnoreRaycastLayer);

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

            collisionCheckFrequency = Mathf.Clamp(collisionCheckFrequency, 0, segmentCount);
            int step = segmentCount / (collisionCheckFrequency > 0 ? collisionCheckFrequency : 1);

            for (int index = 0; index < segmentCount - step; index += step)
            {
                Vector3 currentPoint = points[index];
                Vector3 nextPoint = index + step < points.Count ? points[index + step] : points[points.Count - 1];
                Vector3 nextPointDirection = (nextPoint - currentPoint).normalized;
                float nextPointDistance = Vector3.Distance(currentPoint, nextPoint);

                Ray pointsRay = new Ray(currentPoint, nextPointDirection);
                RaycastHit pointsHitData;

                if (!PhysicsCast.Raycast(physicsCast, pointsRay, out pointsHitData, nextPointDistance, Physics.IgnoreRaycastLayer))
                {
                    continue;
                }

                Vector3 collisionPoint = pointsRay.GetPoint(pointsHitData.distance);
                Ray downwardRay = new Ray(collisionPoint + Vector3.up * 0.01f, Vector3.down);
                RaycastHit downwardHitData;

                if (!PhysicsCast.Raycast(physicsCast, downwardRay, out downwardHitData, float.PositiveInfinity, Physics.IgnoreRaycastLayer))
                {
                    continue;
                }

                TargetHit = downwardHitData;

                Vector3 newDownPosition = downwardRay.GetPoint(downwardHitData.distance);
                Vector3 newJointPosition = newDownPosition.y < forward.y ? new Vector3(newDownPosition.x, forward.y, newDownPosition.z) : forward;
                GeneratePoints(newJointPosition, newDownPosition);

                break;
            }

            ResultsChanged?.Invoke(eventData.Set(TargetHit, Points));
        }

        /// <summary>
        /// Generates points on a parabolic line.
        /// </summary>
        /// <param name="forward">The end point of the forward cast.</param>
        /// <param name="down">The end point of the down cast.</param>
        /// <returns>The generated points on the parabolic line.</returns>
        protected virtual void GeneratePoints(Vector3 forward, Vector3 down)
        {
            Vector3[] curvePoints =
            {
                origin.transform.position,
                forward + new Vector3(0f, curveOffset, 0f),
                down,
                down
            };

            points.Clear();
            points.AddRange(BezierCurveGenerator.GeneratePoints(segmentCount, curvePoints));
        }
    }
}