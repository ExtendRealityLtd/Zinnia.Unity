namespace Zinnia.Extension
{
    using UnityEngine;

    /// <summary>
    /// Extended methods for the <see cref="Vector3"/> Type.
    /// </summary>
    public static class Vector3Extensions
    {
        /// <summary>
        /// Determines if two <see cref="Vector3"/> values are equal based on a given tolerance.
        /// </summary>
        /// <param name="a">The <see cref="Vector3"/> to compare against.</param>
        /// <param name="b">The <see cref="Vector3"/> to compare with.</param>
        /// <param name="tolerance">The tolerance in which the two <see cref="Vector3"/> values can be within to be considered equal.</param>
        /// <returns><see langword="true"/> if the two <see cref="Vector3"/> values are considered equal.</returns>
        public static bool ApproxEquals(this Vector3 a, Vector3 b, float tolerance = float.Epsilon)
        {
            return (Vector3.Distance(a, b) <= tolerance);
        }

        /// <summary>
        /// Divides each component of the given <see cref="Vector3"/> against the given <see cref="float"/>.
        /// </summary>
        /// <param name="dividend">The value to divide by each component.</param>
        /// <param name="divisor">The components to divide with.</param>
        /// <returns>The quotient.</returns>
        public static Vector3 Divide(float dividend, Vector3 divisor)
        {
            return new Vector3(dividend / divisor.x, dividend / divisor.y, dividend / divisor.z);
        }
        /// <summary>
        /// Divides two <see cref="Vector3"/>s component-wise.
        /// </summary>
        /// <param name="dividend">The value to divide by each component.</param>
        /// <param name="divisor">The components to divide with.</param>
        /// <returns>The quotient.</returns>
        public static Vector3 Divide(this Vector3 dividend, Vector3 divisor)
        {
            return Vector3.Scale(dividend, Divide(1, divisor));
        }

        /// <summary>
        /// Check whether the source and target points are within a given tolerance distance of each other.
        /// </summary>
        /// <param name="a">The source point.</param>
        /// <param name="b">The target point.</param>
        /// <param name="tolerance">The tolerance of distance equality.</param>
        /// <returns>Whether the source and target are within the distance equality tolerance.</returns>
        public static bool WithinDistance(this Vector3 a, Vector3 b, Vector3 tolerance)
        {
            return a.x.ApproxEquals(b.x, tolerance.x) && a.y.ApproxEquals(b.y, tolerance.y) && a.z.ApproxEquals(b.z, tolerance.z);
        }

        /// <summary>
        /// Converts an unsigned Euler angle into a signed Euler angle.
        /// </summary>
        /// <param name="eulerAngles">The unsigned Euler angle to convert.</param>
        /// <returns>The converted signed Euler angle.</returns>
        public static Vector3 UnsignedEulerToSignedEuler(this Vector3 eulerAngles)
        {
            return new Vector3(eulerAngles.x.GetSignedDegree(), eulerAngles.y.GetSignedDegree(), eulerAngles.z.GetSignedDegree());
        }
    }
}