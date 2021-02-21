namespace Zinnia.Rule
{
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Determines whether a given <see cref="string"/> matches the <see cref="TargetPattern"/> regular expression.
    /// </summary>
    public class StringMatchesRule : StringRule
    {
        /// <summary>
        /// The regular expression pattern to match against a string against.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public string TargetPattern { get; set; }

        /// <inheritdoc />
        protected override bool Accepts(string targetString)
        {
            return Regex.IsMatch(targetString, TargetPattern);
        }
    }
}