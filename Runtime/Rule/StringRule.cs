namespace Zinnia.Rule
{
    /// <summary>
    /// Simplifies implementing <see cref="IRule"/>s that only accept <see cref="string"/>s.
    /// </summary>
    public abstract class StringRule : Rule
    {
        /// <inheritdoc />
        public override bool Accepts(object target)
        {
            if (ShouldAutoRejectDueToState())
            {
                return false;
            }

            string targetString = target as string;

            return targetString != null && Accepts(targetString);
        }

        /// <summary>
        /// Determines whether a <see cref="string"/> is accepted.
        /// </summary>
        /// <param name="targetString">The <see cref="string"/> to check.</param>
        /// <returns><see langword="true"/> if <paramref name="targetString"/> is accepted, <see langword="false"/> otherwise.</returns>
        protected abstract bool Accepts(string targetString);
    }
}