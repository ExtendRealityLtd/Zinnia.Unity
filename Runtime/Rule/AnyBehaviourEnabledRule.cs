namespace Zinnia.Rule
{
    using UnityEngine;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;
    using Zinnia.Extension;
    using Zinnia.Data.Type;
    using Zinnia.Data.Collection.List;

    /// <summary>
    /// Determines whether a <see cref="GameObject"/> has any <see cref="Behaviour"/> that is enabled and found in a list.
    /// </summary>
    public class AnyBehaviourEnabledRule : GameObjectRule
    {
        /// <summary>
        /// The behaviour types to look for.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public SerializableTypeBehaviourObservableList BehaviourTypes { get; set; }

        /// <inheritdoc/>
        protected override bool Accepts(GameObject targetGameObject)
        {
            if (BehaviourTypes == null)
            {
                return false;
            }

            foreach (SerializableType serializedType in BehaviourTypes.NonSubscribableElements)
            {
                if (serializedType.ActualType != null && IsEnabled(targetGameObject.TryGetComponent(serializedType)))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Determines whether the given component is enabled.
        /// </summary>
        /// <param name="component">The component to check the enabled state on.</param>
        /// <returns>Whether the component is enabled or not.</returns>
        protected virtual bool IsEnabled(Component component)
        {
            if (component == null)
            {
                return false;
            }

            Behaviour checkBehaviour = component as Behaviour;
            return checkBehaviour != null && checkBehaviour.enabled;
        }
    }
}