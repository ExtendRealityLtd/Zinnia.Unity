namespace VRTK.Core.Extension
{
    using UnityEngine;

    /// <summary>
    /// Extended methods for the <see cref="Vector3"/> Type.
    /// </summary>
    public static class Vector3Extensions
    {
        /// <summary>
        /// Determines if two <see cref="Vector3"/> objects are equal based on a given distance.
        /// </summary>
        /// <param name="a">The <see cref="Vector3"/> to compare against.</param>
        /// <param name="b">The <see cref="Vector3"/> to compare with.</param>
        /// <param name="distance">The distance in which the two <see cref="Vector3"/> objects can be within to be considered equal</param>
        /// <returns><see langword="true"/> if the two <see cref="Vector3"/> values are considered equal.</returns>
        public static bool Compare(this Vector3 a, Vector3 b, float distance)
        {
            return (Vector3.Distance(a, b) <= distance);
        }
    }
}