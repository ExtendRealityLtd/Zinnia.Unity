namespace VRTK.Core.Extension
{
    using UnityEngine;

    /// <summary>
    /// Extended methods for the <see cref="GameObject"/> Type.
    /// </summary>
    public static class GameObjectExtensions
    {
        /// <summary>
        /// Attempts to retrieve the <see cref="Component"/> from a given <see cref="GameObject"/>.
        /// </summary>
        /// <param name="gameObject">The <see cref="GameObject"/> to retrieve the <see cref="Component"/> from.</param>
        /// <returns>The <see cref="Component"/> if one exists on the given <see cref="GameObject"/>.</returns>
        public static Component TryGetComponent(this GameObject gameObject)
        {
            return (gameObject == null ? null : gameObject.GetComponent<Component>());
        }

        /// <summary>
        /// Attempts to set the active state.
        /// </summary>
        /// <param name="gameObject">The <see cref="GameObject"/> to set the active state on.</param>
        /// <param name="state">The new state.</param>
        public static void TrySetActive(this GameObject gameObject, bool state)
        {
            if (gameObject != null)
            {
                gameObject.SetActive(state);
            }
        }

        /// <summary>
        /// Attempts to retreive the <see cref="Rigidbody"/> or if one is not found then optionally search children then search parents.
        /// </summary>
        /// <param name="gameObject">The reference <see cref="GameObject"/> to search on.</param>
        /// <param name="searchAncestors">Optionally searches all ancestors in the hierarchy for a rigidbody.</param>
        /// <param name="searchDescendants">Optionally searches all descendants in the hierarchy for a rigidbody.</param>
        /// <returns>The found <see cref="Rigidbody"/></returns>
        public static Rigidbody FindRigidbody(this GameObject gameObject, bool searchDescendants = true, bool searchAncestors = false)
        {
            if (gameObject == null)
            {
                return null;
            }

            Rigidbody foundRigidbody = gameObject.GetComponent<Rigidbody>();

            if (foundRigidbody == null && searchDescendants)
            {
                foundRigidbody = gameObject.GetComponentInChildren<Rigidbody>();
            }

            if (foundRigidbody == null && searchAncestors)
            {
                foundRigidbody = gameObject.GetComponentInParent<Rigidbody>();
            }

            return foundRigidbody;
        }
    }
}
