namespace Zinnia.Extension
{
    using UnityEngine;

    /// <summary>
    /// Extended methods for the <see cref="LayerMask"/> Type.
    /// </summary>
    public static class LayerMaskExtensions
    {
        /// <summary>
        /// Sets the <see cref="LayerMask"/> value to a single given int.
        /// </summary>
        /// <param name="layerMask">The layer mask to update.</param>
        /// <param name="value">The layer value to set.</param>
        /// <returns>The updated<see cref="LayerMask"/>.</returns>
        public static LayerMask Set(this LayerMask layerMask, int value)
        {
            return layerMask.Set(LayerMask.LayerToName(value));
        }

        /// <summary>
        /// Sets the <see cref="LayerMask"/> value to the given name.
        /// </summary>
        /// <param name="layerMask">The layer mask to update.</param>
        /// <param name="name">The layer name to set.</param>
        /// <returns>The updated<see cref="LayerMask"/>.</returns>
        public static LayerMask Set(this LayerMask layerMask, string name)
        {
            return LayerMask.GetMask(name);
        }
    }
}