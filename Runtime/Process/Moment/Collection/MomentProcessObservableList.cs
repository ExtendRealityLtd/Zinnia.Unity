namespace Zinnia.Process.Moment.Collection
{
    using UnityEngine.Events;
    using System;
    using System.Collections.Generic;
    using Zinnia.Data.Collection.List;

    /// <summary>
    /// Allows observing changes to a <see cref="List{T}"/> of <see cref="MomentProcess"/>s.
    /// </summary>
    public class MomentProcessObservableList : DefaultObservableList<MomentProcess, MomentProcessObservableList.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the <see cref="MomentProcess"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<MomentProcess>
        {
        }
    }
}