namespace Zinnia.Event
{
    using UnityEngine;

    /// <summary>
    /// Extracts the <see cref="GameObject"/> from the <see cref="source"/> and emits an event containing the result.
    /// </summary>
    public class ComponentGameObjectEmitter : GameObjectEmitter
    {
        /// <summary>
        /// The source to extract from.
        /// </summary>
        [Tooltip("The source to extract from.")]
        public Component source;

        /// <summary>
        /// Sets the current <see cref="source"/>.
        /// </summary>
        /// <param name="source">The new source.</param>
        public virtual void SetSource(Component source)
        {
            this.source = source;
        }

        /// <summary>
        /// Clears the current <see cref="source"/>.
        /// </summary>
        public virtual void ClearSource()
        {
            source = null;
        }

        /// <inheritdoc />
        public override GameObject Extract()
        {
            if (source == null)
            {
                Result = null;
                return null;
            }

            Result = source.gameObject;
            return base.Extract();
        }
    }
}