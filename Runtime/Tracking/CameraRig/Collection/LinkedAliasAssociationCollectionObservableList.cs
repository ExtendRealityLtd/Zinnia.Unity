namespace Zinnia.Tracking.CameraRig.Collection
{
    using UnityEngine.Events;
    using System;
    using System.Collections.Generic;
    using Zinnia.Data.Collection.List;

    /// <summary>
    /// Allows observing changes to a <see cref="List{T}"/> of <see cref="LinkedAliasAssociationCollection"/>s.
    /// </summary>
    public class LinkedAliasAssociationCollectionObservableList : DefaultObservableList<LinkedAliasAssociationCollection, LinkedAliasAssociationCollectionObservableList.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the <see cref="LinkedAliasAssociationCollection"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<LinkedAliasAssociationCollection>
        {
        }
    }
}