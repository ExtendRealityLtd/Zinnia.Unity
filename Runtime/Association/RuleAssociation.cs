namespace Zinnia.Association
{
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;
    using UnityEngine;
    using Zinnia.Extension;
    using Zinnia.Rule;

    /// <summary>
    /// Holds a <see cref="GameObject"/> collection to activate and/or deactivate based on the given <see cref="RuleContainer"/>.
    /// </summary>
    public class RuleAssociation : GameObjectsAssociation
    {
        /// <summary>
        /// The <see cref="RuleContainer"/> to match the association with.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public RuleContainer Rule { get; set; }

        /// <inheritdoc/>
        public override bool ShouldBeActive()
        {
            return Rule.Accepts(null);
        }
    }
}