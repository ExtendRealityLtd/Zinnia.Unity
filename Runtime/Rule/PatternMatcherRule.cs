namespace Zinnia.Rule
{
    using UnityEngine;
    using Zinnia.Pattern;
    using Zinnia.Pattern.Collection;

    /// <summary>
    /// Determines whether all of the given <see cref="PatternMatcher"/> components successfully match.
    /// </summary>
    public class PatternMatcherRule : Rule
    {
        [Tooltip("The patterns to attempt to match.")]
        [SerializeField]
        private PatternMatcherObservableList patterns;
        /// <summary>
        /// The patterns to attempt to match.
        /// </summary>
        public PatternMatcherObservableList Patterns
        {
            get
            {
                return patterns;
            }
            set
            {
                patterns = value;
            }
        }

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