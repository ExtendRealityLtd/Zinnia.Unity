namespace VRTK.Core.Extension
{
    using System.Linq;
    using System.Collections.Generic;

    /// <summary>
    /// Extended methods for the <see cref="IEnumerable<T>"/> Type.
    /// </summary>
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Returns an empty <see cref="IEnumerable<T>"/> if the given source is null.
        /// </summary>
        /// <typeparam name="T">The Type of the source.</typeparam>
        /// <param name="source">The source <see cref="IEnumerable<T>"/></param>
        /// <returns>The source if it is not null or an empty <see cref="IEnumerable<T>"/> if the source is null.</returns>
        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> source)
        {
            return source ?? Enumerable.Empty<T>();
        }
    }
}