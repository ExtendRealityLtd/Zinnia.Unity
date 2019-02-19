namespace Zinnia.Rule
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Malimbe.BehaviourStateRequirementMethod;
    using Malimbe.XmlDocumentationAttribute;
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
            [DocumentedByXml]
            public RuleContainer rule;
            /// <summary>
            /// Emitted when the <see cref="rule"/> is valid.
            /// </summary>
            [DocumentedByXml]
            public UnityEvent Matched = new UnityEvent();
        }

        /// <summary>
        /// A collection of rules to potentially match against.
        /// </summary>
        [DocumentedByXml]
        public List<Element> elements = new List<Element>();

        /// <summary>
        /// Attempts to match the given object to the rules within the <see cref="elements"/> collection. If a match occurs then the appropriate event is emitted.
        /// </summary>
        /// <param name="source">The source to provide to the rule for validity checking.</param>
        [RequiresBehaviourState]
        public virtual void Match(object source)
        {
            foreach (Element element in elements.Where(target => target.rule.Accepts(source)))
            {
                element.Matched?.Invoke();
            }
        }
    }
}