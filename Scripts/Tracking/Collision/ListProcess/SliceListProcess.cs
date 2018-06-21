namespace VRTK.Core.Tracking.Collision.ListProcess
{
    using UnityEngine;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Slices either the first or the last element from the collision collection.
    /// </summary>
    public class SliceListProcess : BaseListProcess
    {
        /// <summary>
        /// The element from the collection to slice.
        /// </summary>
        public enum ElementSlice
        {
            /// <summary>
            /// The first element in the collection.
            /// </summary>
            First,
            /// <summary>
            /// The last element in the collection.
            /// </summary>
            Last
        }

        /// <summary>
        /// Determines which element from the collection will be processed.
        /// </summary>
        [Tooltip("Determines which element from the collection will be processed.")]
        public ElementSlice sliceType = ElementSlice.Last;

        protected List<CollisionNotifier.EventData> previousList = new List<CollisionNotifier.EventData>();

        /// <summary>
        /// Processes the given collision collection and processes the elements based on their position in the collection.
        /// </summary>
        /// <param name="collisionList">The collision collection to process.</param>
        /// <returns>The sliced collision from either the first or last element of the collection.</returns>
        protected override List<CollisionNotifier.EventData> DoProcess(List<CollisionNotifier.EventData> collisionList)
        {
            List<CollisionNotifier.EventData> returnList = new List<CollisionNotifier.EventData>();

            previousList.Except(collisionList).ToList().ForEach(element => ExecuteCollisionActions(collisionList, element, true));

            for (int i = GetInitialIndex(); i < GetMaxIndex(collisionList.Count); i++)
            {
                CollisionNotifier.EventData currentCollision = collisionList[i];
                ExecuteCollisionActions(collisionList, currentCollision, false);
            }

            if (collisionList.Count > 0)
            {
                CollisionNotifier.EventData selectedCollision = collisionList[GetSelectedIndex(collisionList.Count)];
                ExecuteCollisionActions(collisionList, selectedCollision, true);
                returnList.Add(selectedCollision);
            }

            previousList.Clear();
            previousList.AddRange(collisionList);

            return returnList;
        }

        /// <summary>
        /// Gets the initial index based on <see cref="sliceType"/>.
        /// </summary>
        /// <returns>The initial index for iterating through the collection.</returns>
        protected virtual int GetInitialIndex()
        {
            switch (sliceType)
            {
                case ElementSlice.First:
                    return 1;
                case ElementSlice.Last:
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Gets the current collection index based on <see cref="sliceType"/>.
        /// </summary>
        /// <param name="count">The total number of items in the collection.</param>
        /// <returns>The index of the element to select in the collection.</returns>
        protected virtual int GetSelectedIndex(int count)
        {
            switch (sliceType)
            {
                case ElementSlice.Last:
                    return count - 1;
                case ElementSlice.First:
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Gets the maximum index the collection can be iterated to based on the <see cref="sliceType"/>.
        /// </summary>
        /// <param name="count">The total number of items in the collection.</param>
        /// <returns>The highest index the collection can be iterated to.</returns>
        protected virtual int GetMaxIndex(int count)
        {
            switch (sliceType)
            {
                case ElementSlice.Last:
                    return count - 1;
                case ElementSlice.First:
                default:
                    return count;
            }
        }
    }
}