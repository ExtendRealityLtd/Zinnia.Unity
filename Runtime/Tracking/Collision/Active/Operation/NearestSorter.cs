namespace Zinnia.Tracking.Collision.Active.Operation
{
    using System;
    using System.Collections.Generic;
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
            [Tooltip("The position to check against.")]
            [SerializeField]
            private Vector3 sourcePosition;
            /// <summary>
            /// The position to check against.
            /// </summary>
            public Vector3 SourcePosition
            {
                get
                {
                    return sourcePosition;
                }
                set
                {
                    sourcePosition = value;
                }
            }

            /// <inheritdoc />
            public virtual int Compare(CollisionNotifier.EventData x, CollisionNotifier.EventData y)
            {
                Transform xTransform = x.ColliderData.GetContainingTransform();
                Transform yTransform = y.ColliderData.GetContainingTransform();

                if (xTransform == null && yTransform == null)
                {
                    return 0;
                }

                float distance1 = xTransform != null ? Vector3.Distance(xTransform.position, SourcePosition) : float.MaxValue;
                float distance2 = yTransform != null ? Vector3.Distance(yTransform.position, SourcePosition) : float.MaxValue;
                return distance1.CompareTo(distance2);
            }
        }

        [Tooltip("The source to determine the closest collision to.")]
        [SerializeField]
        private GameObject source;
        /// <summary>
        /// The source to determine the closest collision to.
        /// </summary>
        public GameObject Source
        {
            get
            {
                return source;
            }
            set
            {
                source = value;
            }
        }

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
        /// Clears <see cref="Source"/>.
        /// </summary>
        public virtual void ClearSource()
        {
            if (!this.IsValidState())
            {
                return;
            }

            Source = default;
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