namespace Zinnia.Data.Collection
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using Zinnia.Data.Collection.List;
    using Zinnia.Extension;

    /// <summary>
    /// Holds a collection of key/value relations between GameObjects and allows searching for a given key in the collection to emit the linked value.
    /// </summary>
    public class GameObjectRelations : MonoBehaviour
    {
        /// <summary>
        /// Defines the event for the output <see cref="GameObject"/>.
        /// </summary>
        [Serializable]
        public class GameObjectUnityEvent : UnityEvent<GameObject> { }

        [Tooltip("The collection of relations.")]
        [SerializeField]
        private GameObjectRelationObservableList relations;
        /// <summary>
        /// The collection of relations.
        /// </summary>
        public GameObjectRelationObservableList Relations
        {
            get
            {
                return relations;
            }
            set
            {
                relations = value;
            }
        }

        /// <summary>
        /// Emitted when a value is retrieved for a given key or relation index.
        /// </summary>
        public GameObjectUnityEvent ValueRetrieved = new GameObjectUnityEvent();
        /// <summary>
        /// Emitted when a no key can be found the given key or relation index.
        /// </summary>
        public UnityEvent KeyNotFound = new UnityEvent();

        /// <summary>
        /// Attempts to get the value in the list of relations for the given key.
        /// </summary>
        /// <param name="key">The key of the relation to get the value for.</param>
        /// <returns>The value for the given key.</returns>
        public virtual GameObject GetValue(GameObject key)
        {
            if (!this.IsValidState())
            {
                return null;
            }

            foreach (GameObjectRelationObservableList.Relation relation in Relations.NonSubscribableElements)
            {
                if (key.Equals(relation.Key))
                {
                    ValueRetrieved?.Invoke(relation.Value);
                    return relation.Value;
                }
            }
            KeyNotFound?.Invoke();
            return null;
        }

        /// <summary>
        /// Attempts to get the value in the list of relations for the given index.
        /// </summary>
        /// <param name="relationIndex">The index of the relation to get the value for.</param>
        /// <returns>The value for the given index.</returns>
        public virtual GameObject GetValue(int relationIndex)
        {
            if (!this.IsValidState())
            {
                return null;
            }

            for (int index = 0; index < Relations.NonSubscribableElements.Count; index++)
            {
                if (index == relationIndex)
                {
                    GameObject foundValue = Relations.NonSubscribableElements[index].Value;
                    ValueRetrieved?.Invoke(foundValue);
                    return foundValue;
                }
            }
            KeyNotFound?.Invoke();
            return null;
        }

        /// <summary>
        /// Attempts to get the value in the list of relations for the given key.
        /// </summary>
        /// <param name="key">The key of the relation to get the value for.</param>
        public virtual void DoGetValue(GameObject key)
        {
            GetValue(key);
        }

        /// <summary>
        /// Attempts to get the value in the list of relations for the given index.
        /// </summary>
        /// <param name="index">The index of the relation to get the value for.</param>
        public virtual void DoGetValue(int index)
        {
            GetValue(index);
        }
    }
}