namespace Zinnia.Data.Type
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a read-only collection of elements that can be accessed by index. Accessing it will not create any heap allocations.
    /// </summary>
    /// <typeparam name="T">The type of the elements.</typeparam>
    public struct HeapAllocationFreeReadOnlyList<T> : IReadOnlyList<T>
    {
        /// <summary>
        /// Enumerates a <see cref="IList{T}"/>.
        /// </summary>
        public struct Enumerator : IEnumerator<T>
        {
            private readonly IList<T> list;
            private readonly int start;
            private readonly int count;
            private int index;

            /// <inheritdoc />
            public T Current { get; private set; }

            /// <inheritdoc />
            object IEnumerator.Current
            {
                get
                {
                    if (index == start || index == count + 1)
                    {
                        throw new InvalidOperationException();
                    }

                    return Current;
                }
            }

            /// <summary>
            /// Creates a new <see cref="Enumerator"/> that can enumerate the given <see cref="IList{T}"/>.
            /// </summary>
            /// <param name="list">The list to enumerate.</param>
            /// <param name="start">The index to start enumerating at.</param>
            /// <param name="count">How many items to enumerate over.</param>
            public Enumerator(IList<T> list, int start, int count)
            {
                this.list = list ?? Array.Empty<T>();
                this.start = start;
                this.count = count;
                index = start;
                Current = default;
            }

            /// <inheritdoc />
            public void Dispose()
            {
            }

            /// <inheritdoc />
            public bool MoveNext()
            {
                if (index < count)
                {
                    Current = list[index];
                    index++;
                    return true;
                }

                index = count + 1;
                Current = default;
                return false;
            }

            /// <inheritdoc />
            public void Reset()
            {
                index = start;
                Current = default;
            }
        }

        private readonly IList<T> list;
        private readonly int start;
        private readonly int count;

        /// <inheritdoc/>
        public int Count => list?.Count ?? 0;
        /// <inheritdoc/>
        public T this[int index] => (list ?? Array.Empty<T>())[index];

        /// <summary>
        /// Creates a new instance of <see cref="HeapAllocationFreeReadOnlyList{T}"/>.
        /// </summary>
        /// <param name="list">The list to enumerate.</param>
        /// <param name="start">The index to start enumerating at.</param>
        /// <param name="count">How many items to enumerate over.</param>
        public HeapAllocationFreeReadOnlyList(IList<T> list, int start, int count)
        {
            this.list = list;
            this.start = start;
            this.count = count;
        }

        /// <summary>
        /// Implicitly converts an instance of <see cref="List{T}"/> to a <see cref="HeapAllocationFreeReadOnlyList{T}"/>.
        /// </summary>
        /// <param name="list">The <see cref="List{T}"/> to convert.</param>
        public static implicit operator HeapAllocationFreeReadOnlyList<T>(List<T> list)
        {
            return new HeapAllocationFreeReadOnlyList<T>(list, 0, list?.Count ?? 0);
        }

        /// <summary>
        /// Implicitly converts an instance of <see cref="T:T[]"/> to a <see cref="HeapAllocationFreeReadOnlyList{T}"/>.
        /// </summary>
        /// <param name="array">The <see cref="T:T[]"/> to convert.</param>
        public static implicit operator HeapAllocationFreeReadOnlyList<T>(T[] array)
        {
            return new HeapAllocationFreeReadOnlyList<T>(array, 0, array?.Length ?? 0);
        }

        /// <summary>
        /// Implicitly converts an instance of <see cref="ArraySegment{T}"/> to a <see cref="HeapAllocationFreeReadOnlyList{T}"/>.
        /// </summary>
        /// <param name="arraySegment">The <see cref="ArraySegment{T}"/> to convert.</param>
        public static implicit operator HeapAllocationFreeReadOnlyList<T>(ArraySegment<T> arraySegment)
        {
            return new HeapAllocationFreeReadOnlyList<T>(arraySegment.Array, arraySegment.Offset, arraySegment.Count);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the elements.
        /// </summary>
        /// <returns>An enumerator to iterate through the elements.</returns>
        public Enumerator GetEnumerator()
        {
            return new Enumerator(list, start, count);
        }

        /// <inheritdoc/>
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}