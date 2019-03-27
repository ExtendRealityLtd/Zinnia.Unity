namespace Zinnia.Extension
{
    using System.Collections.Generic;

    /// <summary>
    /// Extended methods for <see cref="List{T}"/>.
    /// </summary>
    public static class ListExtensions
    {
        private static class EmptyList<T>
        {
            public static readonly List<T> Value = new List<T>(0);
        }

        /// <summary>
        /// A reusable, empty <see cref="List{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <returns>An empty <see cref="List{T}"/>.</returns>
        public static List<T> Empty<T>()
        {
            return EmptyList<T>.Value;
        }
    }
}