namespace Zinnia.Data.Collection
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using System.Collections.Generic;

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
        /// A relationship between a key <see cref="GameObject"/> and it's associated value <see cref="GameObject"/>.
        /// </summary>
        [Serializable]
        public class Relation
        {
            /// <summary>
            /// The <see cref="GameObject"/> to use as the key for the relation.
            /// </summary>
            public GameObject key;
            /// <summary>
            /// The <see cref="GameObject"/> to use as the value for the relation.
            /// </summary>
            public GameObject value;
        }

        /// <summary>
        /// The collection of relations.
        /// </summary>
        public List<Relation> relations = new List<Relation>();

        /// <summary>
        /// Emitted when a value is retrieved for a given key.
        /// </summary>
        public UnityEvent ValueRetrieved = new UnityEvent();

        /// <summary>
        /// Attempts to get the value in the list of relations for the given key.
        /// </summary>
        /// <param name="key">The key of the relation to get the value for.</param>
        /// <returns>The value for the given key.</returns>
        public virtual GameObject GetValue(GameObject key)
        {
            if (!isActiveAndEnabled)
            {
                return null;
            }

            foreach (Relation relation in relations)
            {
                if (key.Equals(relation.key))
                {
                    ValueRetrieved?.Invoke(relation.value);
                    return relation.value;
                }
            }
            return null;
        }

        /// <summary>
        /// Attempts to get the value in the list of relations for the given index.
        /// </summary>
        /// <param name="relationIndex">The index of the relation to get the value for.</param>
        /// <returns>The value for the given index.</returns>
        public virtual GameObject GetValue(int relationIndex)
        {
            if (!isActiveAndEnabled)
            {
                return null;
            }

            for (int index = 0; index < relations.Count; index++)
            {
                if (index == relationIndex)
                {
                    GameObject foundValue = relations[index].value;
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