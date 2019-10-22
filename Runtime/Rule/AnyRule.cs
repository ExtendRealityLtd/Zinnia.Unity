namespace Zinnia.Rule
{
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;
    using Zinnia.Extension;
    using Zinnia.Rule.Collection;

    /// <summary>
    /// Determines whether any <see cref="IRule"/> in a list is accepting an object.
    /// </summary>
    public class AnyRule : Rule
    {
        /// <summary>
        /// The <see cref="IRule"/>s to check against.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public RuleContainerObservableList Rules { get; set; }

        /// <inheritdoc />
        public override bool Accepts(object target)
        {
            if (ShouldAutoRejectDueToState() || Rules == null)
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