namespace VRTK.Core.Extension
{
    using UnityEngine;

    /// <summary>
    /// Extended methods for the <see cref="Transform"/> Type.
    /// </summary>
    public static class TransformExtensions
    {
        /// <summary>
        /// The SetGlobalScale method is used to set a <see cref="Transform"/> scale based on a global scale instead of a local scale.
        /// </summary>
        /// <param name="transform">The reference to the <see cref="Transform"/> to scale.</param>
        /// <param name="globalScale">The global scale to apply to the given <see cref="Transform"/>.</param>
        public static void SetGlobalScale(this Transform transform, Vector3 globalScale)
        {
            transform.localScale = Vector3.one;
            transform.localScale = new Vector3(globalScale.x / transform.lossyScale.x, globalScale.y / transform.lossyScale.y, globalScale.z / transform.lossyScale.z);
        }

        /// <summary>
        /// Attempts to retreive the <see cref="Rigidbody"/> for a the <see cref="Transform"/> or if one is not found then optionally search children then search parents.
        /// </summary>
        /// <param name="transform">The reference <see cref="Transform"/> to search on.</param>
        /// <param name="searchAncestors">Optionally searches all ancestors in the hierarchy for a rigidbody.</param>
        /// <param name="searchDescendants">Optionally searches all descendants in the hierarchy for a rigidbody.</param>
        /// <returns>The found <see cref="Rigidbody"/></returns>
        public static Rigidbody FindRigidbody(this Transform transform, bool searchDescendants = true, bool searchAncestors = false)
        {
            Rigidbody foundRigidbody = transform.GetComponent<Rigidbody>();

            if (foundRigidbody == null && searchDescendants)
            {
                foundRigidbody = transform.GetComponentInChildren<Rigidbody>();
            }

            if (foundRigidbody == null && searchAncestors)
            {
                foundRigidbody = transform.GetComponentInParent<Rigidbody>();
            }

            return foundRigidbody;
        }
    }
}
