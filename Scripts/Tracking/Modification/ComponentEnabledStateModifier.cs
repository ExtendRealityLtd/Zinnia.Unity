namespace VRTK.Core.Tracking.Modification
{
    using UnityEngine;
    using System.Linq;
    using System.Collections.Generic;
    using VRTK.Core.Extension;
    using VRTK.Core.Data.Attribute;
    using VRTK.Core.Data.Type;

    /// <summary>
    /// Provides the ability to modify the enabled state of a <see cref="Behaviour"/> or <see cref="Renderer"/> component.
    /// </summary>
    public class ComponentEnabledStateModifier : MonoBehaviour
    {
        /// <summary>
        /// The <see cref="Object"/> types to manage the enabled state on.
        /// </summary>
        [Tooltip("The Object types to manage the enabled state on."), TypePicker(typeof(Object))]
        public List<SerializableType> types = new List<SerializableType>();

        /// <summary>
        /// The target to modify the enabled states for the provided <see cref="types"/>.
        /// </summary>
        [Tooltip("The target to modify the enabled states for the provided types")]
        public GameObject target;

        /// <summary>
        /// Sets the current <see cref="target"/>.
        /// </summary>
        /// <param name="target">The new target.</param>
        public virtual void SetTarget(GameObject target)
        {
            this.target = target;
        }

        /// <summary>
        /// Clears the current <see cref="target"/>.
        /// </summary>
        public virtual void ClearTarget()
        {
            target = null;
        }

        /// <summary>
        /// Sets the enabled state of all matching <see cref="types"/> found on <see cref="target"/>.
        /// </summary>
        /// <param name="state">The enabled state to apply.</param>
        public virtual void SetEnabledState(bool state)
        {
            if (!isActiveAndEnabled || target == null)
            {
                return;
            }

            IEnumerable<Object> targetObjects = types.EmptyIfNull()
                .SelectMany(serializableType => target.GetComponentsInChildren(serializableType, true));

            foreach (Object targetObject in targetObjects)
            {
                Behaviour potentialBehaviour = targetObject as Behaviour;
                if (potentialBehaviour != null)
                {
                    potentialBehaviour.enabled = state;
                    continue;
                }

                Renderer potentialRenderer = targetObject as Renderer;
                if (potentialRenderer != null)
                {
                    potentialRenderer.enabled = state;
                }
            }
        }
    }
}