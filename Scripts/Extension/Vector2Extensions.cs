namespace VRTK.Core.Extension
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
            return (Vector2.Distance(a, b) <= tolerance);
        }
    }
}