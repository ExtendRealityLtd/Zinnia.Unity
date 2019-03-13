namespace Zinnia.Utility
{
    using UnityEditor;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Extension methods for <see cref="SerializedProperty"/>.
    /// </summary>
    public static class SerializedPropertyExtensions
    {
        /// <summary>
        /// Matches the index found in <see cref="SerializedProperty.propertyPath"/> if it exists.
        /// </summary>
        private static readonly Regex indexRegex = new Regex(@"\.Array\.data\[(?'index'\d*)\]$", RegexOptions.Compiled);

        /// <summary>
        /// The index found in <see cref="SerializedProperty.propertyPath"/> if it exists.
        /// </summary>
        /// <param name="property">The property to search on.</param>
        /// <returns>The index if found, otherwise <see langword="null"/>.</returns>
        public static int? TryGetIndex(this SerializedProperty property)
        {
            Match match = indexRegex.Match(property.propertyPath);
            return match.Success ? int.Parse(match.Groups["index"].Value) : (int?)null;
        }
    }
}