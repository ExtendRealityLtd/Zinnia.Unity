namespace Zinnia.Data.Operation.Cache
{
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using Zinnia.Extension;

    public class Vector2Cache : ValueCache<Vector2, Vector2Cache.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the specified <see cref="Vector2"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<Vector2>
        {
        }

        /// <summary>
        /// The tolerance to consider the current value and the cached value equal.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public float EqualityTolerance { get; set; } = float.Epsilon;

        /// <inheritdoc/>
        protected override bool AreEqual(Vector2 a, Vector2 b)
        {
            return a.ApproxEquals(b, EqualityTolerance);
        }
    }
}