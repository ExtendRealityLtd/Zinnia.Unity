namespace Zinnia.Rule
{
    using UnityEngine;
    using System.Collections.Generic;
    using Malimbe.XmlDocumentationAttribute;
    using Zinnia.Data.Attribute;
    using Zinnia.Data.Type;

    /// <summary>
    /// Determines whether a <see cref="GameObject"/> has any component found in a list.
    /// </summary>
    public class AnyComponentTypeRule : GameObjectRule
    {
        /// <summary>
        /// The component types to look for.
        /// </summary>
        [TypePicker(typeof(Component)), DocumentedByXml]
        public List<SerializableType> componentTypes = new List<SerializableType>();

        /// <inheritdoc/>
        protected override bool Accepts(GameObject targetGameObject)
        {
            foreach (SerializableType serializedType in componentTypes)
            {
                if (serializedType.ActualType != null && targetGameObject.GetComponent(serializedType) != null)
                {
                    return true;
                }
            }

            return false;
        }
    }
}