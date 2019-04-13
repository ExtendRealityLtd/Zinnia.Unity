namespace Zinnia.Action.Collection
{
    using UnityEngine.Events;
    using System;
    using System.Collections.Generic;
    using Zinnia.Data.Collection.List;

    /// <summary>
    /// Allows observing changes to a <see cref="List{T}"/> of <see cref="Zinnia.Action.Action"/>s.
    /// </summary>
    public class ActionObservableList : DefaultObservableList<Zinnia.Action.Action, ActionObservableList.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the <see cref="Zinnia.Action.Action"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<Zinnia.Action.Action> { }
    }
}