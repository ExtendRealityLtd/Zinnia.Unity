namespace Zinnia.Data.Collection.List
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;
    using Zinnia.Extension;

    /// <summary>
    /// Allows observing changes to a <see cref="List{T}"/> of <see cref="MultiRelation"/>s.
    /// </summary>
    public class GameObjectMultiRelationObservableList : DefaultObservableList<GameObjectMultiRelationObservableList.MultiRelation, GameObjectMultiRelationObservableList.MultiRelationUnityEvent>
    {
        /// <summary>
        /// A relationship between a key <see cref="GameObject"/> and its associated <see cref="GameObject"/> collection.
        /// </summary>
        [Serializable]
        public class MultiRelation
        {
            [Tooltip("The GameObject acting as the key.")]
            [SerializeField]
            private GameObject key;
            /// <summary>
            /// The <see cref="GameObject"/> acting as the key.
            /// </summary>
            public GameObject Key
            {
                get
                {
                    return key;
                }
                set
                {
                    key = value;
                }
            }
            [Tooltip("The GameObject collection of relation values.")]
            [SerializeField]
            private List<GameObject> values;
            /// <summary>
            /// The <see cref="GameObject"/> collection of relation values.
            /// </summary>
            public List<GameObject> Values
            {
                get
                {
                    return values;
                }
                set
                {
                    values = value;
                }
            }

            /// <summary>
            /// Clears <see cref="Key"/>.
            /// </summary>
            public virtual void ClearKey()
            {
                Key = default;
            }

            /// <summary>
            /// Clears <see cref="Values"/>.
            /// </summary>
            public virtual void ClearValues()
            {
                Values.Clear();
            }
        }

        /// <summary>
        /// Defines the event with the <see cref="MultiRelation"/>.
        /// </summary>
        [Serializable]
        public class MultiRelationUnityEvent : UnityEvent<MultiRelation> { }

        /// <summary>
        /// Defines the event with the <see cref="<see cref="Collection.List{GameObject}"/>"/>.
        /// </summary>
        [Serializable]
        public class ListUnityEvent : UnityEvent<List<GameObject>> { }

        #region Relationship Events
        /// <summary>
        /// Emitted when the given key is matched with a relationship.
        /// </summary>
        [Header("Relationship Events")]
        public ListUnityEvent RelationshipFound = new ListUnityEvent();
        /// <summary>
        /// Emitted when the given key not is matched with a relationship.
        /// </summary>
        public UnityEvent RelationshipNotFound = new UnityEvent();
        #endregion

        /// <summary>
        /// Whether the <see cref="MultiRelation"/> contains a relationship between the given key and provides the related values.
        /// </summary>
        /// <param name="key">The key to check relationship for.</param>
        /// <param name="values">The values associated with any found key.</param>
        /// <returns>Whether there are any relationships for the given key.</returns>
        public virtual bool HasRelationship(GameObject key, out List<GameObject> values)
        {
            foreach (MultiRelation element in Elements)
            {
                if (element.Key == key)
                {
                    values = element.Values;
                    if (this.IsValidState())
                    {
                        RelationshipFound?.Invoke(values);
                    }
                    return true;
                }
            }

            values = null;
            if (this.IsValidState())
            {
                RelationshipNotFound.Invoke();
            }
            return false;
        }
    }
}