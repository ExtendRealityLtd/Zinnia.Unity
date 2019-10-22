namespace Zinnia.Rule
{
    using UnityEngine;
    using System;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;
    using Zinnia.Data.Attribute;

    /// <summary>
    /// The basis for all rule types.
    /// </summary>
    public abstract class Rule : MonoBehaviour, IRule
    {
        /// <summary>
        /// The states that are considered when determining if a <see cref="Rule"/> should be automatically rejected.
        /// </summary>
        [Flags]
        public enum RejectRuleStates
        {
            /// <summary>
            /// The <see cref="Rule"/> will always be rejected if the component is disabled.
            /// </summary>
            RuleComponentIsDisabled = 1 << 0,
            /// <summary>
            /// The <see cref="Rule"/> will always be rejected if the containing <see cref="GameObject"/> is inactive in the scene hierarchy.
            /// </summary>
            RuleGameObjectIsNotActiveInHierarchy = 1 << 1
        }

        /// <summary>
        /// The states on whether to automatically reject a <see cref="Rule"/>.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml, UnityFlags]
        public RejectRuleStates AutoRejectStates { get; set; } = (RejectRuleStates)(-1);

        /// <inheritdoc/>
        public abstract bool Accepts(object target);

        /// <summary>
        /// Whether to automatically reject the <see cref="Rule"/> based on the <see cref="AutoRejectStates"/>.
        /// </summary>
        /// <returns>Whether the rule should be rejected.</returns>
        public virtual bool ShouldAutoRejectDueToState()
        {
            return ((AutoRejectStates & RejectRuleStates.RuleComponentIsDisabled) != 0 && !enabled)
                   || ((AutoRejectStates & RejectRuleStates.RuleGameObjectIsNotActiveInHierarchy) != 0 && !gameObject.activeInHierarchy);
        }
    }
}