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
    }
}
