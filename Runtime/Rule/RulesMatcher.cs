namespace Zinnia.Rule
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.BehaviourStateRequirementMethod;
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
            /// <summary>
            /// The rule to match against.
            /// </summary>
            [Serialized]
            [field: DocumentedByXml]
            public RuleContainer Rule { get; set; }

            /// <summary>
            /// Emitted when the <see cref="Rule"/> is valid.
            /// </summary>
            [DocumentedByXml]
            public UnityEvent Matched = new UnityEvent();
        }

        /// <summary>
        /// A collection of rules to potentially match against.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public RulesMatcherElementObservableList Elements { get; set; }

        /// <summary>
        /// Attempts to match the given object to the rules within the <see cref="Elements"/> collection. If a match occurs then the appropriate event is emitted.
        /// </summary>
        /// <param name="source">The source to provide to the rule for validity checking.</param>
        [RequiresBehaviourState]
        public virtual void Match(object source)
        {
            if (Elements == null)
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