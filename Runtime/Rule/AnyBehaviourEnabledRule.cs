namespace Zinnia.Rule
{
    using UnityEngine;
    using Zinnia.Data.Collection.List;
    using Zinnia.Data.Type;
    using Zinnia.Extension;

    /// <summary>
    /// Determines whether a <see cref="GameObject"/> has any <see cref="Behaviour"/> that is enabled and found in a list.
    /// </summary>
    public class AnyBehaviourEnabledRule : GameObjectRule
    {
        [Tooltip("The behaviour types to look for.")]
        [SerializeField]
        private SerializableTypeBehaviourObservableList behaviourTypes;
        /// <summary>
        /// The behaviour types to look for.
        /// </summary>
        public SerializableTypeBehaviourObservableList BehaviourTypes
        {
            get
            {
                return behaviourTypes;
            }
            set
            {
                behaviourTypes = value;
            }
        }

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