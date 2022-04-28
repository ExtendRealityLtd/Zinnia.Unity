namespace Zinnia.Rule
{
    using System.Text.RegularExpressions;
    using UnityEngine;

    /// <summary>
    /// Determines whether a given <see cref="string"/> matches the <see cref="TargetPattern"/> regular expression.
    /// </summary>
    public class StringMatchesRule : StringRule
    {
        [Tooltip("The regular expression pattern to match against a string against.")]
        [SerializeField]
        private string targetPattern;
        /// <summary>
        /// The regular expression pattern to match against a string against.
        /// </summary>
        public string TargetPattern
        {
            get
            {
                return targetPattern;
            }
            set
            {
                targetPattern = value;
            }
        }

        /// <inheritdoc />
        protected override bool Accepts(string targetString)
        {
            return Regex.IsMatch(targetString, TargetPattern);
        }
    }
}