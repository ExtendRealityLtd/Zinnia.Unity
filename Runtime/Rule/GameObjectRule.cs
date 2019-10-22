namespace Zinnia.Rule
{
    using UnityEngine;

    /// <summary>
    /// Simplifies implementing <see cref="IRule"/>s that only accept <see cref="GameObject"/>s.
    /// </summary>
    public abstract class GameObjectRule : Rule
    {
        /// <inheritdoc />
        public override bool Accepts(object target)
        {
            if (ShouldAutoRejectDueToState())
            {
                return false;
            }

            GameObject targetGameObject = target as GameObject;
            if (targetGameObject == null)
            {
                Component component = target as Component;
                if (component != null)
                {
                    targetGameObject = component.gameObject;
                }
            }

            return targetGameObject != null && Accepts(targetGameObject);
        }

        /// <summary>
        /// Determines whether a <see cref="GameObject"/> is accepted.
        /// </summary>
        /// <param name="targetGameObject">The <see cref="GameObject"/> to check.</param>
        /// <returns><see langword="true"/> if <paramref name="targetGameObject"/> is accepted, <see langword="false"/> otherwise.</returns>
        protected abstract bool Accepts(GameObject targetGameObject);
    }
}