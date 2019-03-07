namespace Zinnia.Rule
{
    using UnityEngine.Events;
    using System;
    using Zinnia.Data.Collection;

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