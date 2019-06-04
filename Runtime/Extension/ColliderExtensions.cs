namespace Zinnia.Extension
{
    using UnityEngine;
    using System.Collections.Generic;

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

            Rigidbody attachedRigidbody = collider.GetAttachedRigidbody();
            return attachedRigidbody == null ? collider.transform : attachedRigidbody.transform;
        }

        /// <summary>
        /// Gets the parent <see cref="Rigidbody"/> for the given <see cref="Collider"/> even if the <see cref="GameObject"/> is disabled.
        /// </summary>
        /// <param name="collider">The <see cref="Collider"/> to check against.</param>
        /// <returns>The parent <see cref="Rigidbody"/>.</returns>
        public static Rigidbody GetAttachedRigidbody(this Collider collider)
        {
            Rigidbody attachedRigidbody = collider.attachedRigidbody;
            if (!collider.gameObject.activeInHierarchy)
            {
                collider.GetComponentsInParent(true, foundRigidbodies);
                foreach (Rigidbody foundRigidbody in foundRigidbodies)
                {
                    attachedRigidbody = foundRigidbody;
                    break;
                }
            }
            return attachedRigidbody;
        }

        /// <summary>
        /// A <see cref="Rigidbody"/> collection to store found <see cref="Rigidbody"/>s in.
        /// </summary>
        private static List<Rigidbody> foundRigidbodies = new List<Rigidbody>();
    }
}