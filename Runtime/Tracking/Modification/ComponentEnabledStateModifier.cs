namespace Zinnia.Tracking.Modification
{
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;
    using UnityEngine;
    using Zinnia.Data.Collection.List;
    using Zinnia.Data.Type;
    using Zinnia.Extension;

    /// <summary>
    /// Provides the ability to modify the enabled state of a <see cref="Behaviour"/> or <see cref="Renderer"/> component.
    /// </summary>
    public class ComponentEnabledStateModifier : MonoBehaviour
    {
        /// <summary>
        /// The <see cref="Object"/> types to manage the enabled state on.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public SerializableTypeComponentObservableList Types { get; set; }

        /// <summary>
        /// The target to modify the enabled states for the provided <see cref="Types"/>.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public GameObject Target { get; set; }

        /// <summary>
        /// Clears <see cref="Target"/>.
        /// </summary>
        public virtual void ClearTarget()
        {
            if (!this.IsValidState())
            {
                return;
            }

            Target = default;
        }

        /// <summary>
        /// Sets the enabled state of all matching <see cref="Types"/> found on <see cref="Target"/>.
        /// </summary>
        /// <param name="state">The enabled state to apply.</param>
        public virtual void SetEnabledState(bool state)
        {
            if (!this.IsValidState() || Types == null || Target == null)
            {
                return;
            }

            foreach (SerializableType serializableType in Types.NonSubscribableElements)
            {
                foreach (Component targetObject in Target.GetComponentsInChildren(serializableType, true))
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
}