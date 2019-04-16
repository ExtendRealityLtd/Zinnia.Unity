namespace Zinnia.Rule
{
    using UnityEngine;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;
    using Zinnia.Extension;
    using Zinnia.Data.Type;
    using Zinnia.Data.Collection.List;

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