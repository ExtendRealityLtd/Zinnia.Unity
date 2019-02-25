namespace Zinnia.Extension
{
    using UnityEngine;

    /// <summary>
    /// Extended methods for the <see cref="Collider"/> Type.
    /// </summary>
    public static class ColliderExtensions
    {
        /// <summary>
        /// Gets the <see cref="Transform"/> of the container of the collider.
        /// </summary>
        /// <param name="collider">The <see cref="Collider"/> to check against.</param>
        /// <returns>The container.</returns>
        public static Transform GetContainingTransform(this Collider collider)
        {
            if (collider == null)
            {
                return null;
            }

            Rigidbody attachedRigidbody = collider.attachedRigidbody;
            return attachedRigidbody == null ? collider.transform : attachedRigidbody.transform;
        }
    }
}