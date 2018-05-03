namespace VRTK.Core.Extension
{
    using UnityEngine;

    /// <summary>
    /// The ComponentExtensions provide extended methods for the Component Type.
    /// </summary>
    public static class ComponentExtensions
    {
        /// <summary>
        /// The TryGetTransform method attempts to retrieve the Transform from a given Component.
        /// </summary>
        /// <param name="component">The Component to retrieve the Transform from.</param>
        /// <returns>A Transform if one exists on the given Component.</returns>
        public static Transform TryGetTransform(this Component component)
        {
            return (component == null ? null : component.transform);
        }
    }
}