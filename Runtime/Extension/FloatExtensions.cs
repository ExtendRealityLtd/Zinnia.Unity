namespace Zinnia.Extension
{
    using UnityEngine;

    /// <summary>
    /// Extended methods for the <see cref="float"/> Type.
    /// </summary>
    public static class FloatExtensions
    {
        /// <summary>
        /// Determines if two <see cref="float"/> values are equal based on a given tolerance.
        /// </summary>
        /// <param name="a">The <see cref="float"/> to compare against.</param>
        /// <param name="b">The <see cref="float"/> to compare with.</param>
        /// <param name="tolerance">The tolerance in which the two <see cref="float"/> values can be within to be considered equal.</param>
        /// <returns><see langword="true"/> if the two <see cref="float"/> values are considered equal.</returns>
        public static bool ApproxEquals(this float a, float b, float tolerance = float.Epsilon)
        {
            float difference = Mathf.Abs(tolerance);
            return (Mathf.Abs(a - b) <= difference);
        }
    }
}