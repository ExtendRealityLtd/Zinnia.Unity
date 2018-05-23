namespace VRTK.Core.Extension
{
    using UnityEngine;

    public static class TransformExtensions
    {
        /// <summary>
        /// The SetGlobalScale method is used to set a transform scale based on a global scale instead of a local scale.
        /// </summary>
        /// <param name="transform">The reference to the transform to scale.</param>
        /// <param name="globalScale">A Vector3 of a global scale to apply to the given transform.</param>
        public static void SetGlobalScale(this Transform transform, Vector3 globalScale)
        {
            transform.localScale = Vector3.one;
            transform.localScale = new Vector3(globalScale.x / transform.lossyScale.x, globalScale.y / transform.lossyScale.y, globalScale.z / transform.lossyScale.z);
        }
    }
}
