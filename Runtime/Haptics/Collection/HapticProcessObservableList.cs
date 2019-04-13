namespace Zinnia.Haptics.Collection
{
    using UnityEngine.Events;
    using System;
    using System.Collections.Generic;
    using Zinnia.Data.Collection.List;

    /// <summary>
    /// Allows observing changes to a <see cref="List{T}"/> of <see cref="HapticProcess"/>s.
    /// </summary>
    public class HapticProcessObservableList : DefaultObservableList<HapticProcess, HapticProcessObservableList.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the <see cref="HapticProcess"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<HapticProcess>
        {
        }
    }
}