namespace Zinnia.Association.Collection
{
    using UnityEngine.Events;
    using System;
    using System.Collections.Generic;
    using Zinnia.Data.Collection.List;

    /// <summary>
    /// Allows observing changes to a <see cref="List{T}"/> of <see cref="GameObjectsAssociation"/>s.
    /// </summary>
    public class GameObjectsAssociationObservableList : DefaultObservableList<GameObjectsAssociation, GameObjectsAssociationObservableList.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the <see cref="GameObjectsAssociation"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<GameObjectsAssociation>
        {
        }
    }
}