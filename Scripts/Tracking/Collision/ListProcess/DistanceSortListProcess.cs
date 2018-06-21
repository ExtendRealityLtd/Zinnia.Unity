namespace VRTK.Core.Tracking.Collision.ListProcess
{
    using UnityEngine;
    using System.Collections.Generic;
    using VRTK.Core.Process;
    using VRTK.Core.Extension;

    /// <summary>
    /// Sorts a collision collection based on the distance between the <see cref="Transform.position"/> distances.
    /// </summary>
    public class DistanceSortListProcess : BaseListProcess
    {
        /// <summary>
        /// The method of distance sorting.
        /// </summary>
        public enum SortMode
        {
            /// <summary>
            /// The nearest collision will be first in the collection.
            /// </summary>
            NearestFirst,
            /// <summary>
            /// The furthest collision will be first in the collection.
            /// </summary>
            FurthestFirst
        }

        public Transform source;

        /// <summary>
        /// The mechanism of sorting the collection based on distance.
        /// </summary>
        [Tooltip("The mechanism of sorting the collection based on distance.")]
        public SortMode mode = SortMode.NearestFirst;

        protected List<CollisionNotifier.EventData> sortedList = new List<CollisionNotifier.EventData>();

        /// <summary>
        /// Processes the given collision collection and sorts them based on their distance from the source collider.
        /// </summary>
        /// <param name="collisionList">The collision collection to process.</param>
        /// <returns>A sorted by distance collision collection.</returns>
        protected override List<CollisionNotifier.EventData> DoProcess(List<CollisionNotifier.EventData> collisionList)
        {
            if (source == null)
            {
                return collisionList;
            }

            sortedList.Clear();
            sortedList.AddRange(collisionList);

            sortedList.Sort(
                (data1, data2) =>
                    {
                        float distance1 = Vector3.Distance(data1.collider.GetContainingTransform().position, source.position);
                        float distance2 = Vector3.Distance(data2.collider.GetContainingTransform().position, source.position);
                        int result = distance1.CompareTo(distance2);
                        return (mode == SortMode.NearestFirst ? result : -result);
                    }
                );

            if (executeColliderActions)
            {
                sortedList.ForEach(collisionItem => ExecuteCollisionActions(sortedList, collisionItem, false));
            }

            return sortedList;
        }
    }
}