namespace Zinnia.Rule
{
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;
    using UnityEngine;
    using Zinnia.Data.Collection.List;
    using Zinnia.Data.Type;
    using Zinnia.Extension;

    /// <summary>
    /// Determines whether a <see cref="GameObject"/> has any component found in a list.
    /// </summary>
    public class AnyComponentTypeRule : GameObjectRule
    {
        /// <summary>
        /// The component types to look for.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public SerializableTypeComponentObservableList ComponentTypes { get; set; }

        /// <inheritdoc/>
        protected override bool Accepts(GameObject targetGameObject)
        {
            if (ComponentTypes == null)
            {
                return false;
            }

            foreach (SerializableType serializedType in ComponentTypes.NonSubscribableElements)
            {
                if (serializedType.ActualType != null && targetGameObject.TryGetComponent(serializedType) != null)
                {
                    return true;
                }
            }

            return false;
        }
    }
}