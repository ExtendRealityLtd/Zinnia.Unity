namespace VRTK.Core.Rule
{
    using UnityEngine;
    using System.Collections.Generic;
    using System.Linq;
    using VRTK.Core.Data.Attribute;
    using VRTK.Core.Data.Type;
    using VRTK.Core.Extension;

    /// <summary>
    /// Determines whether a <see cref="GameObject"/> has any component found in a list.
    /// </summary>
    public class AnyComponentTypeRule : BaseGameObjectRule
    {
        /// <summary>
        /// The component types to look for.
        /// </summary>
        [TypePicker(typeof(Component))]
        [Tooltip("The component types to look for.")]
        public List<SerializableType> componentTypes = new List<SerializableType>();

        /// <inheritdoc/>
        protected override bool Accepts(GameObject targetGameObject)
        {
            return componentTypes.EmptyIfNull()
                .Where(serializedType => serializedType.ActualType != null)
                .Any(serializedType => targetGameObject.GetComponent(serializedType) != null);
        }
    }
}