namespace VRTK.Core.Extension
{
    using UnityEngine;

    /// <summary>
    /// The Vector3Extensions provide extended methods for the Vector3 Type.
    /// </summary>
    public static class Vector3Extensions
    {
        /// <summary>
        /// The Compare method determines if two vectors are equal based on a given distance.
        /// </summary>
        /// <param name="a">The Vector3 to compare against.</param>
        /// <param name="b">The Vector3 to compare with.</param>
        /// <param name="distance">The distance in which the two Vector3 objects can be within to be considered equal</param>
        /// <returns>Returns `true` if the two Vector3 values are considered equal.</returns>
        public static bool Compare(this Vector3 a, Vector3 b, float distance)
        {
            return (Vector3.Distance(a, b) <= distance);
        }
    }
}