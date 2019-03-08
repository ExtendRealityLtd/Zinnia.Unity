namespace Zinnia.Extension
{
    using UnityEngine;
    using System.Collections.Generic;

    /// <summary>
    /// Extended methods for <see cref="IReadOnlyCollection{T}"/>.
    /// </summary>
    public static class IReadOnlyCollectionExtensions
    {
        /// <summary>
        /// Returns a wrapped and clamped collection index to prevent an index being out of bounds.
        /// </summary>
        /// <remarks>
        /// An index less than zero will return the element based on that index value starting from the end of the collection.
        /// </remarks>
        /// <example>
        /// ReadOnlyCollection[] { A, B, C, D, E }
        /// > ReadOnlyCollection[1] -> 1 (B)
        /// > ReadOnlyCollection[-1] -> 4 (E)
        /// > ReadOnlyCollection[-2] -> 3 (D)
        /// > ReadOnlyCollection[7] -> 4 (E)
        /// > ReadOnlyCollection[-7] -> 0 (A)
        /// </example>
        /// <typeparam name="T">The type of the source.</typeparam>
        /// <param name="source">The source <see cref="IReadOnlyCollection{T}"/></param>
        /// <param name="index">The index of the collection to wrap and clamp</param>
        /// <returns>The wrapped and clamped index to be within the bounds of the collection.</returns>
        public static int GetWrappedAndClampedIndex<T>(this IReadOnlyCollection<T> source, int index)
        {
            index = index >= 0 ? index : source.Count + index;
            return Mathf.Clamp(index, 0, source.Count - 1);
        }
    }
}