namespace Zinnia.Utility
{
    using UnityEngine;
    using System.Collections.Generic;
    using Zinnia.Data.Type;

    /// <summary>
    /// A collection of helper methods generating points on a bezier curve.
    /// </summary>
    public static class BezierCurveGenerator
    {
        private static readonly List<Vector3> calculatedPoints = new List<Vector3>();

        /// <summary>
        /// Generates points on a bezier curve.
        /// </summary>
        /// <param name="pointsCount">The number of points to generate.</param>
        /// <param name="controlPoints">Points defining the bezier curve.</param>
        /// <returns>The generated points.</returns>
        public static HeapAllocationFreeReadOnlyList<Vector3> GeneratePoints(int pointsCount, IReadOnlyList<Vector3> controlPoints)
        {
            calculatedPoints.Clear();
            float stepSize = pointsCount != 1 ? 1f / (pointsCount - 1) : pointsCount;

            for (int index = 0; index < pointsCount; index++)
            {
                calculatedPoints.Add(GeneratePoint(controlPoints, index * stepSize));
            }

            return calculatedPoints;
        }

        /// <summary>
        /// Generates a point at a specific location along the control points.
        /// </summary>
        /// <param name="controlPoints">The collection of points where the point can be generated.</param>
        /// <param name="pointLocation">The specific location along the collection where to generate the point.</param>
        /// <returns></returns>
        private static Vector3 GeneratePoint(IReadOnlyList<Vector3> controlPoints, float pointLocation)
        {
            int index;
            if (pointLocation >= 1f)
            {
                pointLocation = 1f;
                index = controlPoints.Count - 4;
            }
            else
            {
                pointLocation = Mathf.Clamp01(pointLocation) * ((controlPoints.Count - 1) / 3f);
                index = (int)pointLocation;
                pointLocation -= index;
                index *= 3;
            }

            float normalizedPointLocation = Mathf.Clamp01(pointLocation);
            float oneMinusT = 1f - normalizedPointLocation;
            return oneMinusT * oneMinusT * oneMinusT * controlPoints[index] + 3f * oneMinusT * oneMinusT * normalizedPointLocation * controlPoints[index + 1] + 3f * oneMinusT * normalizedPointLocation * normalizedPointLocation * controlPoints[index + 2] + normalizedPointLocation * normalizedPointLocation * normalizedPointLocation * controlPoints[index + 3];
        }
    }
}
