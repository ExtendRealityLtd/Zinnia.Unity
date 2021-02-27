namespace Zinnia.Rule
{
    /// <summary>
    /// Simplifies implementing <see cref="IRule"/>s that only accept <see cref="int"/>s.
    /// </summary>
    public abstract class IntRule : Rule
    {
        /// <inheritdoc />
        public override bool Accepts(object target)
        {
            if (ShouldAutoRejectDueToState())
            {
                return false;
            }

            int? targetInt = target as int?;

            return targetInt != null && Accepts((int)targetInt);
        }

        /// <summary>
        /// Determines whether a <see cref="int"/> is accepted.
        /// </summary>
        /// <param name="targetInt">The <see cref="int"/> to check.</param>
        /// <returns><see langword="true"/> if <paramref name="targetInt"/> is accepted, <see langword="false"/> otherwise.</returns>
        protected abstract bool Accepts(int targetInt);
    }
}