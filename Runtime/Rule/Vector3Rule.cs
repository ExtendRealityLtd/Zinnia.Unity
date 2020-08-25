namespace Zinnia.Rule
{
    using UnityEngine;

    /// <summary>
    /// Simplifies implementing <see cref="IRule"/>s that only accept <see cref="Vector3"/>s.
    /// </summary>
    public abstract class Vector3Rule : Rule
    {
        /// <inheritdoc />
        public override bool Accepts(object target)
        {
            if (ShouldAutoRejectDueToState())
            {
                return false;
            }

            Vector3? targetVector3 = target as Vector3?;

            return targetVector3 != null && Accepts((Vector3)targetVector3);
        }

        /// <summary>
        /// Determines whether a <see cref="Vector3"/> is accepted.
        /// </summary>
        /// <param name="targetVector3">The <see cref="Vector3"/> to check.</param>
        /// <returns><see langword="true"/> if <paramref name="targetVector3"/> is accepted, <see langword="false"/> otherwise.</returns>
        protected abstract bool Accepts(Vector3 targetVector3);
    }
}