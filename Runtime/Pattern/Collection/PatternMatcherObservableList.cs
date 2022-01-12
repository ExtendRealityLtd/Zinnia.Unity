namespace Zinnia.Pattern.Collection
{
    using System;
    using System.Collections.Generic;
    using UnityEngine.Events;
    using Zinnia.Data.Collection.List;

    /// <summary>
    /// Allows observing changes to a <see cref="List{T}"/> of <see cref="PatternMatcher"/>s.
    /// </summary>
    public class PatternMatcherObservableList : DefaultObservableList<PatternMatcher, PatternMatcherObservableList.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the <see cref="PatternMatcher"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<PatternMatcher> { }
    }
}