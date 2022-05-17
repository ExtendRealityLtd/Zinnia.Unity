namespace Zinnia.Extension
{
    using UnityEngine;

    /// <summary>
    /// Extended methods for the <see cref="Vector2"/> Type.
    /// </summary>
    public static class Vector2Extensions
    {
        /// <summary>
        /// Determines if two <see cref="Vector2"/> values are equal based on a given tolerance.
        /// </summary>
        /// <param name="a">The <see cref="Vector2"/> to compare against.</param>
        /// <param name="b">The <see cref="Vector2"/> to compare with.</param>
        /// <param name="tolerance">The tolerance in which the two <see cref="Vector2"/> values can be within to be considered equal.</param>
        /// <returns><see langword="true"/> if the two <see cref="Vector2"/> values are considered equal.</returns>
        public static bool ApproxEquals(this Vector2 a, Vector2 b, float tolerance = float.Epsilon)
        {
            return Vector2.Distance(a, b) <= tolerance;
        }

        /// <summary>
        /// Divides each component of the given <see cref="Vector2"/> against the given <see cref="float"/>.
        /// </summary>
        /// <param name="dividend">The value to divide by each component.</param>
        /// <param name="divisor">The components to divide with.</param>
        /// <returns>The quotient.</returns>
        public static Vector2 Divide(float dividend, Vector2 divisor)
        {
            return new Vector2(dividend / divisor.x, dividend / divisor.y);
        }
        /// <summary>
        /// Divides two <see cref="Vector2"/>s component-wise.
        /// </summary>
        /// <param name="dividend">The value to divide by each component.</param>
        /// <param name="divisor">The components to divide with.</param>
        /// <returns>The quotient.</returns>
        public static Vector2 Divide(this Vector2 dividend, Vector2 divisor)
        {
            return Vector2.Scale(dividend, Divide(1, divisor));
        }

        /// <summary>
        /// Check whether the source and target points are within a given tolerance distance of each other.
        /// </summary>
        /// <param name="a">The source point.</param>
        /// <param name="b">The target point.</param>
        /// <param name="tolerance">The tolerance of distance equality.</param>
        /// <returns>Whether the source and target are within the distance equality tolerance.</returns>
        public static bool WithinDistance(this Vector2 a, Vector2 b, Vector2 tolerance)
        {
            return a.x.ApproxEquals(b.x, tolerance.x) && a.y.ApproxEquals(b.y, tolerance.y);
        }

        /// <summary>
        /// Converts an unsigned Euler angle into a signed Euler angle.
        /// </summary>
        /// <param name="eulerAngles">The unsigned Euler angle to convert.</param>
        /// <returns>The converted signed Euler angle.</returns>
        public static Vector2 UnsignedEulerToSignedEuler(this Vector2 eulerAngles)
        {
            return new Vector2(eulerAngles.x.GetSignedDegree(), eulerAngles.y.GetSignedDegree());
        }
    }
}