namespace Zinnia.Rule
{
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;
    using Zinnia.Extension;

    /// <summary>
    /// Negates the acceptance of an object based on the acceptance of another <see cref="IRule"/>.
    /// </summary>
    public class NegationRule : Rule
    {
        /// <summary>
        /// The <see cref="IRule"/> to negate.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public RuleContainer Rule { get; set; }

        /// <inheritdoc />
        public override bool Accepts(object target)
        {
            if (ShouldAutoRejectDueToState())
            {
                return false;
            }

            return !Rule.Accepts(target);
        }
    }
}