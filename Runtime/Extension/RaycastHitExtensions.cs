namespace Zinnia.Extension
{
    using UnityEngine;

    /// <summary>
    /// Extended methods for the <see cref="RaycastHit"/> Type.
    /// </summary>
    public static class RaycastHitExtensions
    {
        /// <summary>
        /// Returns the <see cref="RaycastHit"/> data in string format.
        /// </summary>
        /// <param name="raycastHit">The data to return in string format.</param>
        /// <returns>The string representation of the data.</returns>
        public static string ToFormattedString(this RaycastHit raycastHit)
        {
            string[] titles = new string[]
            {
                "barycentricCoordinate",
                "Collider",
                "Distance",
                "Lightmap Coord",
                "Normal",
                "Point",
                "Rigidbody",
                "Texture Coord",
                "Texture Coord2",
                "Transform",
                "Triangle Index"
            };

            object[] values = new object[]
            {
                raycastHit.barycentricCoordinate,
                raycastHit.collider,
                raycastHit.distance,
                raycastHit.lightmapCoord,
                raycastHit.normal,
                raycastHit.point,
                raycastHit.rigidbody,
                raycastHit.textureCoord,
                raycastHit.textureCoord2,
                raycastHit.transform,
                raycastHit.triangleIndex
            };

            return StringExtensions.FormatForToString(titles, values);
        }
    }
}