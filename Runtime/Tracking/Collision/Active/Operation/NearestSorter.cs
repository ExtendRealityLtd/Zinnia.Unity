namespace Zinnia.Tracking.Collision.Active.Operation
{
    using UnityEngine;
    using System;
    using System.Collections.Generic;
    using Malimbe.MemberClearanceMethod;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;
    using Zinnia.Extension;

    /// <summary>
    /// Sorts the given collision collection based on which collision <see cref="Transform"/> components are nearest to the specified <see cref="Source"/>.
    /// </summary>
    public class NearestSorter : MonoBehaviour
    {
        /// <summary>
        /// Compares two <see cref="CollisionNotifier.EventData"/> based on their containing <see cref="Transform"/>'s distance to a given <see cref="Vector3"/>.
        /// </summary>
        protected class EventDataComparer : IComparer<CollisionNotifier.EventData>
        {
            /// <summary>
            /// The position to check against.
            /// </summary>
            [Serialized]
            [field: DocumentedByXml]
            public Vector3 SourcePosition { get; set; }

            /// <inheritdoc />
            public virtual int Compare(CollisionNotifier.EventData x, CollisionNotifier.EventData y)
            {
                float distance1 = Vector3.Distance(x.ColliderData.GetContainingTransform().position, SourcePosition);
                float distance2 = Vector3.Distance(y.ColliderData.GetContainingTransform().position, SourcePosition);
                return distance1.CompareTo(distance2);
            }
        }

        /// <summary>
        /// The source to determine the closest collision to.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public GameObject Source { get; set; }

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
        public ActiveCollisionsContainer.ActiveCollisionUnityEvent Sorted = new ActiveCollisionsContainer.ActiveCollisionUnityEvent();

        /// <summary>
        /// Compares two <see cref="CollisionNotifier.EventData"/>.
        /// </summary>
        protected static readonly EventDataComparer Comparer = new EventDataComparer();
        /// <summary>
        /// The comparison <see cref="Comparer"/> does.
        /// </summary>
        protected static readonly Comparison<CollisionNotifier.EventData> Comparison = Comparer.Compare;

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
            if (!isActiveAndEnabled || Source == null)
            {
                return originalList;
            }

            SortedList.Set(originalList);
            Comparer.SourcePosition = Source.transform.position;
            SortedList.ActiveCollisions.Sort(Comparison);

            Sorted?.Invoke(SortedList);
            return SortedList;
        }
    }
}