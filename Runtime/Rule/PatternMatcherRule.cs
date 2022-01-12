namespace Zinnia.Rule
{
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;
    using Zinnia.Pattern;
    using Zinnia.Pattern.Collection;

    /// <summary>
    /// Determines whether all of the given <see cref="PatternMatcher"/> components successfully match.
    /// </summary>
    public class PatternMatcherRule : Rule
    {
        /// <summary>
        /// The patterns to attempt to match.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public PatternMatcherObservableList Patterns { get; set; }

        /// <inheritdoc />
        public override bool Accepts(object _)
        {
            if (ShouldAutoRejectDueToState() || Patterns == null)
            {
                return false;
            }

            foreach (PatternMatcher pattern in Patterns.NonSubscribableElements)
            {
                if (!pattern.DoMatch())
                {
                    return false;
                }
            }

            return true;
        }
    }
}