namespace Zinnia.Data.Collection.List
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using System.Collections.Generic;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.MemberClearanceMethod;
    using Malimbe.XmlDocumentationAttribute;

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
            [Serialized, Cleared]
            [field: DocumentedByXml]
            public GameObject Key { get; set; }
            /// <summary>
            /// The <see cref="GameObject"/> acting as the value.
            /// </summary>
            [Serialized, Cleared]
            [field: DocumentedByXml]
            public GameObject Value { get; set; }
        }

        /// <summary>
        /// Defines the event with the <see cref="Relation"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<Relation>
        {
        }
    }
}