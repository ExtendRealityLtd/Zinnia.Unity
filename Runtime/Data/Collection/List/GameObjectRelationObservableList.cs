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
            [Tooltip("The GameObject acting as the value.")]
            [SerializeField]
            private GameObject value;
            /// <summary>
            /// The <see cref="GameObject"/> acting as the value.
            /// </summary>
            public GameObject Value
            {
                get
                {
                    return value;
                }
                set
                {
                    this.value = value;
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