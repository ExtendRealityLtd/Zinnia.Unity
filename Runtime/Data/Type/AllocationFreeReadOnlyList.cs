namespace Zinnia.Data.Type
{
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a read-only collection of elements that can be accessed by index. Accessing it will not create any heap allocations.
    /// </summary>
    /// <typeparam name="T">The type of the elements.</typeparam>
    public struct AllocationFreeReadOnlyList<T> : IReadOnlyList<T>
    {
        private readonly List<T> actualList;

        /// <inheritdoc/>
        public int Count => actualList.Count;
        /// <inheritdoc/>
        public T this[int index] => actualList[index];

        private AllocationFreeReadOnlyList(List<T> actualList)
        {
            this.actualList = actualList;
        }

        /// <summary>
        /// Implicitly converts an instance of <see cref="List{T}"/> to a <see cref="AllocationFreeReadOnlyList{T}"/>.
        /// </summary>
        /// <param name="list">The <see cref="List{T}"/> to convert.</param>
        public static implicit operator AllocationFreeReadOnlyList<T>(List<T> list)
        {
            return new AllocationFreeReadOnlyList<T>(list);
        }

        /// <inheritdoc/>
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return actualList.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the elements.
        /// </summary>
        /// <returns>An enumerator to iterate through the elements.</returns>
        public List<T>.Enumerator GetEnumerator()
        {
            return actualList.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)actualList).GetEnumerator();
        }
    }
}