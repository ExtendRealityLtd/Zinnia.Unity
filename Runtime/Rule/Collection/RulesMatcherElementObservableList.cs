namespace Zinnia.Rule.Collection
{
    using UnityEngine.Events;
    using System;
    using System.Collections.Generic;
    using Zinnia.Data.Collection.List;

    /// <summary>
    /// Allows observing changes to a <see cref="List{T}"/> of <see cref="RulesMatcher.Element"/>s.
    /// </summary>
    public class RulesMatcherElementObservableList : DefaultObservableList<RulesMatcher.Element, RulesMatcherElementObservableList.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the <see cref="RulesMatcher.Element"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<RulesMatcher.Element>
        {
        }
    }
}