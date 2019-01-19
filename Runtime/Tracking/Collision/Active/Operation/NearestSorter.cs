namespace Zinnia.Tracking.Collision.Active.Operation
{
    using Malimbe.MemberClearanceMethod;
    using Malimbe.XmlDocumentationAttribute;
    using UnityEngine;
    using Zinnia.Extension;

    /// <summary>
    /// Sorts the given collision collection based on which collision <see cref="Transform"/> components are nearest to the specified <see cref="source"/>.
    /// </summary>
    public class NearestSorter : MonoBehaviour
    {
        /// <summary>
        /// The source to determine the closest collision to.
        /// </summary>
        [DocumentedByXml, Cleared]
        public GameObject source;

        /// <summary>
        /// The sorted list.
        /// </summary>
        public ActiveCollisionsContainer.EventData SortedList
        {
            get;
            protected set;
        } = new ActiveCollisionsContainer.EventData();

        /// <summary>
        /// Emitted when the collection is sorted.
        /// </summary>
        [DocumentedByXml]
        public ActiveCollisionsContainer.UnityEvent Sorted = new ActiveCollisionsContainer.UnityEvent();

        /// <summary>
        /// Sets the <see cref="source"/> parameter.
        /// </summary>
        /// <param name="source">The new source value.</param>
        public virtual void SetSource(GameObject source)
        {
            this.source = source;
        }

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

            SortedList.Set(originalList);

            SortedList.activeCollisions.Sort(
            (data1, data2) =>
                {
                    float distance1 = Vector3.Distance(data1.collider.GetContainingTransform().position, source.transform.position);
                    float distance2 = Vector3.Distance(data2.collider.GetContainingTransform().position, source.transform.position);
                    return distance1.CompareTo(distance2);
                }
            );

            Sorted?.Invoke(SortedList);
            return SortedList;
        }
    }
}