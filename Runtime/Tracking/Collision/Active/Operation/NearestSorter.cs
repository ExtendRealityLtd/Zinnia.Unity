﻿namespace Zinnia.Tracking.Collision.Active.Operation
{
    using System.Collections.Generic;
    using Malimbe.MemberClearanceMethod;
    using Malimbe.PropertySerializationAttribute;
    /*using Malimbe.PropertyValidationMethod;*/
    using Malimbe.XmlDocumentationAttribute;
    using UnityEngine;
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
            public Vector3 sourcePosition;

            /// <inheritdoc />
            public virtual int Compare(CollisionNotifier.EventData x, CollisionNotifier.EventData y)
            {
                float distance1 = Vector3.Distance(x.collider.GetContainingTransform().position, sourcePosition);
                float distance2 = Vector3.Distance(y.collider.GetContainingTransform().position, sourcePosition);
                return distance1.CompareTo(distance2);
            }
        }

        /// <summary>
        /// The source to determine the closest collision to.
        /// </summary>
        [Serialized, /*Validated,*/ Cleared]
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
        public ActiveCollisionsContainer.UnityEvent Sorted = new ActiveCollisionsContainer.UnityEvent();

        /// <summary>
        /// Compares two <see cref="CollisionNotifier.EventData"/>.
        /// </summary>
        protected static readonly EventDataComparer Comparer = new EventDataComparer();

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
            Comparer.sourcePosition = Source.transform.position;
            SortedList.activeCollisions.Sort(Comparer);

            Sorted?.Invoke(SortedList);
            return SortedList;
        }
    }
}