namespace Zinnia.Rule
{
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;
    using Zinnia.Extension;
    using Zinnia.Rule.Collection;

    /// <summary>
    /// Determines whether all <see cref="IRule"/>s in a list are accepting an object.
    /// </summary>
    public class AllRule : Rule
    {
        /// <summary>
        /// The <see cref="IRule"/>s to check against.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public RuleContainerObservableList Rules { get; set; }

        /// <inheritdoc/>
        public override bool Accepts(object target)
        {
            if (ShouldAutoRejectDueToState() || Rules == null || Rules.NonSubscribableElements.Count == 0)
            {
                return false;
            }

            foreach (RuleContainer rule in Rules.NonSubscribableElements)
            {
                if (!rule.Accepts(target))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
