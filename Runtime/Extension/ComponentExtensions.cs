namespace Zinnia.Extension
{
    using UnityEngine;

    /// <summary>
    /// Extended methods for the <see cref="Component"/> Type.
    /// </summary>
    public static class ComponentExtensions
    {
        /// <summary>
        /// Attempts to retrieve the <see cref="Transform"/> from a given <see cref="Component"/>.
        /// </summary>
        /// <param name="component">The <see cref="Component"/> to retrieve the <see cref="Transform"/> from.</param>
        /// <returns>The <see cref="Transform"/> if one exists on the given <see cref="Component"/>.</returns>
        public static Transform TryGetTransform(this Component component)
        {
            return (component == null ? null : component.transform);
        }

        /// <summary>
        /// Attempts to retrieve the <see cref="GameObject"/> from a given <see cref="Component"/>.
        /// </summary>
        /// <param name="component">The <see cref="Component"/> to retrieve the <see cref="GameObject"/> from.</param>
        /// <returns>The <see cref="GameObject"/> if one exists on the given <see cref="Component"/>.</returns>
        public static GameObject TryGetGameObject(this Component component)
        {
            return (component == null ? null : component.gameObject);
        }
    }
}