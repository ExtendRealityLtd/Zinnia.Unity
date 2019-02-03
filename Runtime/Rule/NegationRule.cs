namespace Zinnia.Rule
{
    using Malimbe.XmlDocumentationAttribute;
    using UnityEngine;
    using Zinnia.Extension;

    /// <summary>
    /// Negates the acceptance of an object based on the acceptance of another <see cref="IRule"/>.
    /// </summary>
    public class NegationRule : MonoBehaviour, IRule
    {
        /// <summary>
        /// The <see cref="IRule"/> to negate.
        /// </summary>
        [DocumentedByXml]
        public RuleContainer rule;

        /// <inheritdoc />
        public bool Accepts(object target)
        {
            return !rule.Accepts(target);
        }
    }
}