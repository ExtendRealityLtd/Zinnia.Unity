namespace Zinnia.Tracking.Velocity.Collection
{
    using UnityEngine.Events;
    using System;
    using System.Collections.Generic;
    using Zinnia.Data.Collection.List;

    /// <summary>
    /// Allows observing changes to a <see cref="List{T}"/> of <see cref="VelocityTracker"/>s.
    /// </summary>
    public class VelocityTrackerObservableList : DefaultObservableList<VelocityTracker, VelocityTrackerObservableList.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the <see cref="VelocityTracker"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<VelocityTracker>
        {
        }
    }
}