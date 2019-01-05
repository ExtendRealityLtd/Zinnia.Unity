namespace Zinnia.Rule
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Zinnia.Extension;

    /// <summary>
    /// Matches a given object against a collections of rules and emits the associated event.
    /// </summary>
    public class RulesMatcher : MonoBehaviour
    {
        /// <summary>
        /// The rule and event association that can be matched against.
        /// </summary>
        [Serializable]
        public class Element
        {
            /// <summary>
            /// The rule to match against.
            /// </summary>
            public RuleContainer rule;
            /// <summary>
            /// Emitted when the <see cref="rule"/> is valid.
            /// </summary>
            public UnityEvent Matched = new UnityEvent();
        }

        /// <summary>
        /// A collection of rules to potentially match against.
        /// </summary>
        public List<Element> elements = new List<Element>();

        /// <summary>
        /// Attempts to match the given object to the rules within the <see cref="elements"/> collection. If a match occurs then the appropriate event is emitted.
        /// </summary>
        /// <param name="source">The source to provide to the rule for validity checking.</param>
        public virtual void Match(object source)
        {
            if (!isActiveAndEnabled)
            {
                return;
            }

            foreach (Element element in elements.EmptyIfNull().Where(target => target.rule.Accepts(source)))
            {
                element.Matched?.Invoke();
            }
        }
    }
}