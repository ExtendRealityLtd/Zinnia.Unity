namespace Zinnia.Rule
{
    using UnityEngine;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.BehaviourStateRequirementMethod;
    using Zinnia.Extension;

    /// <summary>
    /// Negates the acceptance of an object based on the acceptance of another <see cref="IRule"/>.
    /// </summary>
    public class NegationRule : MonoBehaviour, IRule
    {
        /// <summary>
        /// The <see cref="IRule"/> to negate.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public RuleContainer Rule { get; set; }

        /// <inheritdoc />
        [RequiresBehaviourState]
        public bool Accepts(object target)
        {
            return !Rule.Accepts(target);
        }
    }
}