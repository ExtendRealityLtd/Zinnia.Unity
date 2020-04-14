namespace Zinnia.Data.Operation.Cache
{
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;
    using System;
    using UnityEngine.Events;
    using Zinnia.Extension;

    public class FloatCache : ValueCache<float, FloatCache.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the specified <see cref="float"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<float>
        {
        }

        /// <summary>
        /// The tolerance to consider the current value and the cached value equal.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public float EqualityTolerance { get; set; } = float.Epsilon;

        /// <inheritdoc/>
        protected override bool AreEqual(float a, float b)
        {
            return a.ApproxEquals(b, EqualityTolerance);
        }
    }
}