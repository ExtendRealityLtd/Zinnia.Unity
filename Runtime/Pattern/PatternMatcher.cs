namespace Zinnia.Pattern
{
    using System.Text.RegularExpressions;
    using UnityEngine;

    /// <summary>
    /// A base to describe how to match a source string to a given pattern via a regular expression.
    /// </summary>
    public abstract class PatternMatcher : MonoBehaviour
    {
        [Tooltip("The pattern to match the source against.")]
        [SerializeField]
        private string pattern;
        /// <summary>
        /// The pattern to match the source against.
        /// </summary>
        public string Pattern
        {
            get
            {
                return pattern;
            }
            set
            {
                pattern = value;
            }
        }

        /// <summary>
        /// The current value of the source string.
        /// </summary>
        public string SourceValue { get; protected set; }

        /// <summary>
        /// Processes the source string to match against.
        /// </summary>
        /// <returns>The string to match against.</returns>
        public virtual string ProcessSourceString()
        {
            SourceValue = DefineSourceString();
            return SourceValue;
        }

        /// <summary>
        /// Determines whether the given <see cref="Pattern"/> matches against the source string.
        /// </summary>
        /// <returns></returns>
        public virtual bool DoMatch()
        {
            return Regex.IsMatch(ProcessSourceString(), Pattern);
        }

        /// <summary>
        /// Defindes the source string to match against.
        /// </summary>
        /// <returns>The string to match against.</returns>
        protected abstract string DefineSourceString();

        protected virtual void OnEnable()
        {
            ProcessSourceString();
        }
    }
}