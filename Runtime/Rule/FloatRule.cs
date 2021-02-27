namespace Zinnia.Rule
{
    /// <summary>
    /// Simplifies implementing <see cref="IRule"/>s that only accept <see cref="float"/>s.
    /// </summary>
    public abstract class FloatRule : Rule
    {
        /// <inheritdoc />
        public override bool Accepts(object target)
        {
            if (ShouldAutoRejectDueToState())
            {
                return false;
            }

            float? targetFloat = target as float?;

            return targetFloat != null && Accepts((float)targetFloat);
        }

        /// <summary>
        /// Determines whether a <see cref="float"/> is accepted.
        /// </summary>
        /// <param name="targetFloat">The <see cref="float"/> to check.</param>
        /// <returns><see langword="true"/> if <paramref name="targetFloat"/> is accepted, <see langword="false"/> otherwise.</returns>
        protected abstract bool Accepts(float targetFloat);
    }
}