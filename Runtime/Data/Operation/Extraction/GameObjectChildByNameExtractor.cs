namespace Zinnia.Data.Operation.Extraction
{
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;
    using System;
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// Extracts a child <see cref="GameObject"/> by the given named path.
    /// </summary>
    public class GameObjectChildByNameExtractor : GameObjectExtractor<GameObject, ComponentGameObjectExtractor.UnityEvent>
    {
        /// <summary>
        /// The path name to the child <see cref="GameObject"/>.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public string ChildNamePath { get; set; }

        /// <summary>
        /// Defines the event with the specified <see cref="GameObject"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<GameObject> { }

        /// <inheritdoc />
        protected override GameObject ExtractValue()
        {
            if (Source == null)
            {
                return null;
            }

            Transform found = Source.transform.Find(ChildNamePath);
            return found != null ? found.gameObject : null;
        }
    }
}