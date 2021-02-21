namespace Zinnia.Rule
{
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;
    using Zinnia.Data.Type;

    /// <summary>
    /// Determines whether a given <see cref="int"/> is within the range of the specified <see cref="Range"/>.
    /// </summary>
    public class IntInRangeRule : IntRule
    {
        /// <summary>
        /// The range in which the given <see cref="int"/> must be equal to the bounds or within the range.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public IntRange Range { get; set; }

        /// <inheritdoc />
        protected override bool Accepts(int targetInt)
        {
            return Range.Contains(targetInt);
        }
    }
}