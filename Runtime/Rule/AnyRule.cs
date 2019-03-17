namespace Zinnia.Rule
{
    using UnityEngine;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.BehaviourStateRequirementMethod;
    using Malimbe.PropertySerializationAttribute;
    using Zinnia.Extension;
    using Zinnia.Rule.Collection;

    /// <summary>
    /// Determines whether any <see cref="IRule"/> in a list is accepting an object.
    /// </summary>
    public class AnyRule : MonoBehaviour, IRule
    {
        /// <summary>
        /// The <see cref="IRule"/>s to check against.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public RuleContainerObservableList Rules { get; set; }

        /// <inheritdoc />
        [RequiresBehaviourState]
        public bool Accepts(object target)
        {
            if (Rules == null)
            {
                return false;
            }

            foreach (RuleContainer rule in Rules.NonSubscribableElements)
            {
                if (rule.Accepts(target))
                {
                    return true;
                }
            }

            return false;
        }
    }
}