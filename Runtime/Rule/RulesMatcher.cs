namespace Zinnia.Rule
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using Zinnia.Extension;
    using Zinnia.Rule.Collection;

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
            [Tooltip("The rule to match against.")]
            [SerializeField]
            private RuleContainer rule;
            /// <summary>
            /// The rule to match against.
            /// </summary>
            public RuleContainer Rule
            {
                get
                {
                    return rule;
                }
                set
                {
                    rule = value;
                }
            }

            /// <summary>
            /// Emitted when the <see cref="Rule"/> is valid.
            /// </summary>
            public UnityEvent Matched = new UnityEvent();
        }

        [Tooltip("A collection of rules to potentially match against.")]
        [SerializeField]
        private RulesMatcherElementObservableList elements;
        /// <summary>
        /// A collection of rules to potentially match against.
        /// </summary>
        public RulesMatcherElementObservableList Elements
        {
            get
            {
                return elements;
            }
            set
            {
                elements = value;
            }
        }

        /// <summary>
        /// Attempts to match the given object to the rules within the <see cref="Elements"/> collection. If a match occurs then the appropriate event is emitted.
        /// </summary>
        /// <param name="source">The source to provide to the rule for validity checking.</param>
        public virtual void Match(object source)
        {
            if (!this.IsValidState() || Elements == null)
            {
                return;
            }

            foreach (Element element in Elements.NonSubscribableElements)
            {
                if (element.Rule.Accepts(source))
                {
                    element.Matched?.Invoke();
                }
            }
        }
    }
}