namespace Zinnia.Rule.Collection
{
    using UnityEngine.Events;
    using System;
    using System.Collections.Generic;
    using Zinnia.Data.Collection.List;

    /// <summary>
    /// Allows observing changes to a <see cref="List{T}"/> of <see cref="RuleContainer"/>s.
    /// </summary>
    public class RuleContainerObservableList : DefaultObservableList<RuleContainer, RuleContainerObservableList.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the <see cref="RuleContainer"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<RuleContainer>
        {
        }
    }
}