namespace Zinnia.Data.Collection.List
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// Allows observing changes to a <see cref="List{T}"/> of <see cref="Relation"/>s.
    /// </summary>
    public class GameObjectRelationObservableList : DefaultObservableList<GameObjectRelationObservableList.Relation, GameObjectRelationObservableList.UnityEvent>
    {
        /// <summary>
        /// A relationship between a key <see cref="Relation"/> and its associated value <see cref="Relation"/>.
        /// </summary>
        [Serializable]
        public class Relation
        {
            /// <summary>
            /// The <see cref="GameObject"/> acting as the key.
            /// </summary>
            [Tooltip("The GameObject acting as the key.")]
            [SerializeField]
            private GameObject _key;
            public GameObject Key
            {
                get
                {
                    return _key;
                }
                set
                {
                    _key = value;
                }
            }
            /// <summary>
            /// The <see cref="GameObject"/> acting as the value.
            /// </summary>
            [Tooltip("The GameObject acting as the value.")]
            [SerializeField]
            private GameObject _value;
            public GameObject Value
            {
                get
                {
                    return _value;
                }
                set
                {
                    _value = value;
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
            /// Clears <see cref="Value"/>.
            /// </summary>
            public virtual void ClearValue()
            {
                Value = default;
            }
        }

        /// <summary>
        /// Defines the event with the <see cref="Relation"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<Relation> { }
    }
}