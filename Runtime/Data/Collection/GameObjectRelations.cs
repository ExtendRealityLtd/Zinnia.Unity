namespace Zinnia.Data.Collection
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.BehaviourStateRequirementMethod;
    using Zinnia.Data.Collection.List;

    /// <summary>
    /// Holds a collection of key/value relations between GameObjects and allows searching for a given key in the collection to emit the linked value.
    /// </summary>
    public class GameObjectRelations : MonoBehaviour
    {
        /// <summary>
        /// Defines the event for the output <see cref="GameObject"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<GameObject>
        {
        }

        /// <summary>
        /// The collection of relations.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public GameObjectRelationObservableList Relations { get; set; }

        /// <summary>
        /// Emitted when a value is retrieved for a given key.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent ValueRetrieved = new UnityEvent();

        /// <summary>
        /// Attempts to get the value in the list of relations for the given key.
        /// </summary>
        /// <param name="key">The key of the relation to get the value for.</param>
        /// <returns>The value for the given key.</returns>
        [RequiresBehaviourState]
        public virtual GameObject GetValue(GameObject key)
        {
            foreach (GameObjectRelationObservableList.Relation relation in Relations.NonSubscribableElements)
            {
                if (key.Equals(relation.Key))
                {
                    ValueRetrieved?.Invoke(relation.Value);
                    return relation.Value;
                }
            }
            return null;
        }

        /// <summary>
        /// Attempts to get the value in the list of relations for the given index.
        /// </summary>
        /// <param name="relationIndex">The index of the relation to get the value for.</param>
        /// <returns>The value for the given index.</returns>
        [RequiresBehaviourState]
        public virtual GameObject GetValue(int relationIndex)
        {
            for (int index = 0; index < Relations.NonSubscribableElements.Count; index++)
            {
                if (index == relationIndex)
                {
                    GameObject foundValue = Relations.NonSubscribableElements[index].Value;
                    ValueRetrieved?.Invoke(foundValue);
                    return foundValue;
                }
            }
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