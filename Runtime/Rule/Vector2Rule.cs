namespace Zinnia.Rule
{
    using UnityEngine;

    /// <summary>
    /// Simplifies implementing <see cref="IRule"/>s that only accept <see cref="Vector2"/>s.
    /// </summary>
    public abstract class Vector2Rule : Rule
    {
        /// <inheritdoc />
        public override bool Accepts(object target)
        {
            if (ShouldAutoRejectDueToState())
            {
                return false;
            }

            Vector2? targetVector2 = target as Vector2?;

            return targetVector2 != null && Accepts((Vector2)targetVector2);
        }

        /// <summary>
        /// Determines whether a <see cref="Vector2"/> is accepted.
        /// </summary>
        /// <param name="targetVector2">The <see cref="Vector2"/> to check.</param>
        /// <returns><see langword="true"/> if <paramref name="targetVector2"/> is accepted, <see langword="false"/> otherwise.</returns>
        protected abstract bool Accepts(Vector2 targetVector2);
    }
}