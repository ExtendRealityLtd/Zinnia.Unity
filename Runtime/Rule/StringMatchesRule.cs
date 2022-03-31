namespace Zinnia.Rule
{
    using System.Text.RegularExpressions;
    using UnityEngine;

    /// <summary>
    /// Determines whether a given <see cref="string"/> matches the <see cref="TargetPattern"/> regular expression.
    /// </summary>
    public class StringMatchesRule : StringRule
    {
        /// <summary>
        /// The regular expression pattern to match against a string against.
        /// </summary>
        [Tooltip("The regular expression pattern to match against a string against.")]
        [SerializeField]
        private string _targetPattern;
        public string TargetPattern
        {
            get
            {
                return _targetPattern;
            }
            set
            {
                _targetPattern = value;
            }
        }

        /// <inheritdoc />
        protected override bool Accepts(string targetString)
        {
            return Regex.IsMatch(targetString, TargetPattern);
        }
    }
}