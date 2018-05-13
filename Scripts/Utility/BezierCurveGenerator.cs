namespace VRTK.Core.Utility
{
    using UnityEngine;
    using System.Collections.Generic;

    /// <summary>
    /// A collection of helper methods generating points on a bezier curve.
    /// </summary>
    public static class BezierCurveGenerator
    {
        /// <summary>
        /// Generates points on a bezier curve.
        /// </summary>
        /// <param name="count">The number of points to generate.</param>
        /// <param name="controlPoints">Points defining the bezier curve.</param>
        /// <returns>The generated points.</returns>
        public static Vector3[] GeneratePoints(int count, Vector3[] controlPoints)
        {
            Vector3[] calculatedPoints = new Vector3[count];
            float stepSize = count != 1 ? 1f / (count - 1) : count;

            for (int f = 0; f < count; f++)
            {
                calculatedPoints[f] = GeneratePoint(controlPoints, f * stepSize);
            }

            return calculatedPoints;
        }

        private static Vector3 GeneratePoint(IReadOnlyList<Vector3> controlPoints, float t)
        {
            int index;
            if (t >= 1f)
            {
                t = 1f;
                index = controlPoints.Count - 4;
            }
            else
            {
                t = Mathf.Clamp01(t) * ((controlPoints.Count - 1) / 3f);
                index = (int)t;
                t -= index;
                index *= 3;
            }

            float t1 = t;
            t1 = Mathf.Clamp01(t1);
            float oneMinusT = 1f - t1;
            return oneMinusT * oneMinusT * oneMinusT * controlPoints[index] + 3f * oneMinusT * oneMinusT * t1 * controlPoints[index + 1] + 3f * oneMinusT * t1 * t1 * controlPoints[index + 2] + t1 * t1 * t1 * controlPoints[index + 3];
        }
    }
}
