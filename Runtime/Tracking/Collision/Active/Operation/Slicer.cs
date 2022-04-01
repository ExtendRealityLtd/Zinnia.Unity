namespace Zinnia.Tracking.Collision.Active.Operation
{
    using System.Linq;
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// Slices a selection of the collection from the given <see cref="StartIndex"/> for the given <see cref="Length"/> and provides the sliced collection and the remaining collection separately.
    /// </summary>
    public class Slicer : MonoBehaviour
    {
        #region Index Settings
        [Header("Index Settings")]
        [Tooltip("The zero-based index to start the slice at. A negative value counts backwards from the last index in the collection.")]
        [SerializeField]
        private int startIndex;
        /// <summary>
        /// The zero-based index to start the slice at. A negative value counts backwards from the last index in the collection.
        /// </summary>
        public int StartIndex
        {
            get
            {
                return startIndex;
            }
            set
            {
                startIndex = value;
            }
        }
        [Tooltip("The number of elements in the slice.")]
        [SerializeField]
        private uint length = 1;
        /// <summary>
        /// The number of elements in the slice.
        /// </summary>
        public uint Length
        {
            get
            {
                return length;
            }
            set
            {
                length = value;
            }
        }
        #endregion

        #region State Events
        /// <summary>
        /// Emitted when the Sliced list has changed since last slice.
        /// </summary>
        [Header("State Events")]
        public UnityEvent SlicedChanged = new UnityEvent();
        /// <summary>
        /// Emitted when the Sliced list has remained unchanged since last slice.
        /// </summary>
        public UnityEvent SlicedUnchanged = new UnityEvent();
        /// <summary>
        /// Emitted when the Remained list has changed since last slice.
        /// </summary>
        public UnityEvent RemainedChanged = new UnityEvent();
        /// <summary>
        /// Emitted when the Remained list has remained unchanged since last slice.
        /// </summary>
        public UnityEvent RemainedUnchanged = new UnityEvent();
        #endregion

        #region Data Events
        /// <summary>
        /// Emitted when the sliced elements are taken from the collection.
        /// </summary>
        [Header("Data Events")]
        public ActiveCollisionsContainer.ActiveCollisionUnityEvent Sliced = new ActiveCollisionsContainer.ActiveCollisionUnityEvent();
        /// <summary>
        /// Emitted when the remaining elements are left after slicing.
        /// </summary>
        public ActiveCollisionsContainer.ActiveCollisionUnityEvent Remained = new ActiveCollisionsContainer.ActiveCollisionUnityEvent();
        #endregion

        /// <summary>
        /// The elements that have been sliced out of the list.
        /// </summary>
        public ActiveCollisionsContainer.EventData SlicedList { get; protected set; } = new ActiveCollisionsContainer.EventData();
        /// <summary>
        /// The elements that are still remaining in the list after a slice.
        /// </summary>
        public ActiveCollisionsContainer.EventData RemainingList { get; protected set; } = new ActiveCollisionsContainer.EventData();

        /// <summary>
        /// The cached <see cref="Sliced"/> list.
        /// </summary>
        protected ActiveCollisionsContainer.EventData cachedSlicedList = new ActiveCollisionsContainer.EventData();
        /// <summary>
        /// The cached <see cref="Remained"/> list.
        /// </summary>
        protected ActiveCollisionsContainer.EventData cachedRemainingList = new ActiveCollisionsContainer.EventData();

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

            CreateSlicedList(originalList);
            CreateRemainedList(originalList);

            return SlicedList;
        }

        /// <summary>
        /// Creates the contents of the sliced list.
        /// </summary>
        /// <param name="originalList">The full list to slice.</param>
        protected virtual void CreateSlicedList(ActiveCollisionsContainer.EventData originalList)
        {
            int collectionCount = originalList.ActiveCollisions.Count;
            int actualStartIndex = GetStartIndex(StartIndex, collectionCount);
            int actualLength = GetRangeLength(actualStartIndex, (int)Length, collectionCount);

            for (int index = actualStartIndex; index < actualStartIndex + actualLength; index++)
            {
                SlicedList.ActiveCollisions.Add(originalList.ActiveCollisions[index]);
            }

            if (!SlicedList.ActiveCollisions.SequenceEqual(cachedSlicedList.ActiveCollisions))
            {
                SlicedChanged?.Invoke();
            }
            else
            {
                SlicedUnchanged?.Invoke();
            }

            Sliced?.Invoke(SlicedList);

            cachedSlicedList.ActiveCollisions = SlicedList.ActiveCollisions.GetRange(0, SlicedList.ActiveCollisions.Count);
        }

        /// <summary>
        /// Creates the contents of the remaining list.
        /// </summary>
        /// <param name="originalList">The full list to slice.</param>
        protected virtual void CreateRemainedList(ActiveCollisionsContainer.EventData originalList)
        {
            foreach (CollisionNotifier.EventData originalCollision in originalList.ActiveCollisions)
            {
                if (!SlicedList.ActiveCollisions.Contains(originalCollision))
                {
                    RemainingList.ActiveCollisions.Add(originalCollision);
                }
            }

            if (!RemainingList.ActiveCollisions.SequenceEqual(cachedRemainingList.ActiveCollisions))
            {
                RemainedChanged?.Invoke();
            }
            else
            {
                RemainedUnchanged?.Invoke();
            }

            Remained?.Invoke(RemainingList);

            cachedRemainingList.ActiveCollisions = RemainingList.ActiveCollisions.GetRange(0, RemainingList.ActiveCollisions.Count);
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
        protected virtual int GetStartIndex(int checkIndex, int count)
        {
            return Mathf.Clamp(checkIndex < 0 ? count + checkIndex : checkIndex, 0, count);
        }

        /// <summary>
        /// Gets the actual valid length for the proposed range.
        /// </summary>
        /// <param name="checkIndex">The index to start from.</param>
        /// <param name="checkLength">The length of elements to return.</param>
        /// <param name="count">The total length of the entire collection</param>
        /// <returns>The actual valid length for the given range.</returns>
        protected virtual int GetRangeLength(int checkIndex, int checkLength, int count)
        {
            int returnLength = checkLength;
            int actualLength = checkIndex + checkLength;
            if (actualLength >= count)
            {
                int offset = actualLength - count;
                returnLength = checkLength - offset;
            }

            return returnLength;
        }
    }
}