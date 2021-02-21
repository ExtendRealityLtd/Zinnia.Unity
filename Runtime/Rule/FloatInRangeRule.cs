namespace Zinnia.Rule
{
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;
    using Zinnia.Data.Type;

    /// <summary>
    /// Determines whether a given <see cref="float"/> is within the range of the specified <see cref="Range"/>.
    /// </summary>
    public class FloatInRangeRule : FloatRule
    {
        /// <summary>
        /// The range in which the given <see cref="float"/> must be equal to the bounds or within the range.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public FloatRange Range { get; set; }

        /// <inheritdoc />
        protected override bool Accepts(float targetFloat)
        {
            return Range.Contains(targetFloat);
        }
    }
}