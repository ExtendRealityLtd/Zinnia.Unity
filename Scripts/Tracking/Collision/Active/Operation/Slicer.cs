namespace VRTK.Core.Tracking.Collision.Active.Operation
{
    using UnityEngine;
    using System.Linq;

    /// <summary>
    /// Slices a selection of the collection from the given <see cref="startIndex"/> for the given <see cref="length"/> and provides the sliced collection and the remaining collection separately.
    /// </summary>
    public class Slicer : MonoBehaviour
    {
        /// <summary>
        /// The zero-based index to start the slice at. A negative value counts backwards from the last index in the collection.
        /// </summary>
        [Tooltip("The zero-based index to start the slice at. A negative value counts backwards from the last index in the collection.")]
        public int startIndex = 0;
        /// <summary>
        /// The number of elements in the slice.
        /// </summary>
        [Tooltip("The number of elements in the slice.")]
        public uint length = 1;

        /// <summary>
        /// The elements that have been sliced out of the list.
        /// </summary>
        public ActiveCollisionsContainer.EventData SlicedList
        {
            get;
            protected set;
        } = new ActiveCollisionsContainer.EventData();
        /// <summary>
        /// The elements that are still remaining in the list after a slice.
        /// </summary>
        public ActiveCollisionsContainer.EventData RemainingList
        {
            get;
            protected set;
        } = new ActiveCollisionsContainer.EventData();

        /// <summary>
        /// Emitted when the sliced elements are taken from the collection.
        /// </summary>
        public ActiveCollisionsContainer.UnityEvent Sliced = new ActiveCollisionsContainer.UnityEvent();
        /// <summary>
        /// Emitted when the remaining elements are left after slicing.
        /// </summary>
        public ActiveCollisionsContainer.UnityEvent Remained = new ActiveCollisionsContainer.UnityEvent();

        /// <summary>
        /// Slices the collision collection.
        /// </summary>
        /// <param name="originalList">The original collision collection.</param>
        public virtual void DoSlice(ActiveCollisionsContainer.EventData originalList)
        {
            Slice(originalList);
        }

        /// <summary>
        /// Slices the collision collection.
        /// </summary>
        /// <param name="originalList">The original collision collection.</param>
        /// <returns>The sliced collection.</returns>
        public virtual ActiveCollisionsContainer.EventData Slice(ActiveCollisionsContainer.EventData originalList)
        {
            SlicedList.Clear();
            RemainingList.Clear();

            if (!isActiveAndEnabled)
            {
                return SlicedList;
            }

            uint collectionCount = (uint)originalList.activeCollisions.Count;
            uint actualStartIndex = GetStartIndex(startIndex, collectionCount);
            uint actualLength = GetRangeLength(actualStartIndex, length, collectionCount);

            SlicedList.activeCollisions.AddRange(originalList.activeCollisions.GetRange((int)actualStartIndex, (int)actualLength));
            Sliced?.Invoke(SlicedList);

            RemainingList.activeCollisions.AddRange(originalList.activeCollisions.Except(SlicedList.activeCollisions));
            Remained?.Invoke(RemainingList);

            return SlicedList;
        }

        /// <summary>
        /// Slices the collision collection.
        /// </summary>
        /// <param name="originalList">The original collision collection.</param>
        /// <param name="remaining">The collection of remaining elements that were not included in the sliced collection.</param>
        /// <returns>The sliced collection.</returns>
        public virtual ActiveCollisionsContainer.EventData Slice(ActiveCollisionsContainer.EventData originalList, out ActiveCollisionsContainer.EventData remaining)
        {
            ActiveCollisionsContainer.EventData returnList = Slice(originalList);
            remaining = (isActiveAndEnabled ? RemainingList : originalList);
            return returnList;
        }

        /// <summary>
        /// Gets the actual start index even if the index is a negative value.
        /// </summary>
        /// <param name="checkIndex">The index to start from.</param>
        /// <param name="count">The total length of the entire collection</param>
        /// <returns>The actual start index to start from.</returns>
        protected virtual uint GetStartIndex(int checkIndex, uint count)
        {
            return (uint)Mathf.Clamp((checkIndex < 0 ? count + checkIndex : checkIndex), 0, count);
        }

        /// <summary>
        /// Gets the actual valid length for the proposed range.
        /// </summary>
        /// <param name="checkIndex">The index to start from.</param>
        /// <param name="checkLength">The length of elements to return.</param>
        /// <param name="count">The total length of the entire collection</param>
        /// <returns>The actual valid length for the given range.</returns>
        protected virtual uint GetRangeLength(uint checkIndex, uint checkLength, uint count)
        {
            uint returnLength = checkLength;
            uint actualLength = checkIndex + checkLength;
            if (actualLength >= count)
            {
                uint offset = actualLength - count;
                returnLength = checkLength - offset;
            }

            return returnLength;
        }
    }
}