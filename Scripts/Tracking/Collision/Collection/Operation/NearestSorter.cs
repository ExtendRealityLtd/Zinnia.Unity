namespace VRTK.Core.Tracking.Collision.Collection.Operation
{
    using UnityEngine;
    using VRTK.Core.Extension;

    /// <summary>
    /// Sorts the given collision collection based on which collision <see cref="Transform"/> components are nearest to the specified <see cref="source"/>.
    /// </summary>
    public class NearestSorter : MonoBehaviour
    {
        /// <summary>
        /// The source <see cref="Transform"/> to determine the closest collision to.
        /// </summary>
        [Tooltip("The source Transform to determine the closest collision to.")]
        public Transform source;

        /// <summary>
        /// Emitted when the collection is sorted.
        /// </summary>
        public ActiveCollisionsContainer.UnityEvent Sorted = new ActiveCollisionsContainer.UnityEvent();

        protected ActiveCollisionsContainer.EventData sortedList = new ActiveCollisionsContainer.EventData();

        /// <summary>
        /// Sorts the given collision collection by the collisions that are nearest to the source <see cref="Transform"/>.
        /// </summary>
        /// <param name="originalList">The original collision collection.</param>
        public virtual void DoSort(ActiveCollisionsContainer.EventData originalList)
        {
            Sort(originalList);
        }

        /// <summary>
        /// Sorts the given collision collection by the collisions that are nearest to the source <see cref="Transform"/>.
        /// </summary>
        /// <param name="originalList">The original collision collection.</param>
        /// <returns>The sorted collision collection.</returns>
        public virtual ActiveCollisionsContainer.EventData Sort(ActiveCollisionsContainer.EventData originalList)
        {
            if (!isActiveAndEnabled || source == null)
            {
                return originalList;
            }

            sortedList.Set(originalList);

            sortedList.activeCollisions.Sort(
            (data1, data2) =>
                {
                    float distance1 = Vector3.Distance(data1.collider.GetContainingTransform().position, source.position);
                    float distance2 = Vector3.Distance(data2.collider.GetContainingTransform().position, source.position);
                    return distance1.CompareTo(distance2);
                }
            );

            Sorted?.Invoke(sortedList);
            return sortedList;
        }
    }
}