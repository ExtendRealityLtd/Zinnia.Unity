namespace VRTK.Core.Extension
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
        /// <returns>The <see cref="Transform"/> if one exists on the given <see cref="Transform"/>.</returns>
        public static Transform TryGetTransform(this Component component)
        {
            return (component == null ? null : component.transform);
        }
    }
}