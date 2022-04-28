namespace Zinnia.Rule
{
    using UnityEngine;
    using Zinnia.Data.Collection.List;
    using Zinnia.Data.Type;
    using Zinnia.Extension;

    /// <summary>
    /// Determines whether a <see cref="GameObject"/> has any component found in a list.
    /// </summary>
    public class AnyComponentTypeRule : GameObjectRule
    {
        [Tooltip("The component types to look for.")]
        [SerializeField]
        private SerializableTypeComponentObservableList componentTypes;
        /// <summary>
        /// The component types to look for.
        /// </summary>
        public SerializableTypeComponentObservableList ComponentTypes
        {
            get
            {
                return componentTypes;
            }
            set
            {
                componentTypes = value;
            }
        }

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