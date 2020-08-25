namespace Zinnia.Data.Type.Observer
{
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using Zinnia.Extension;

    /// <summary>
    /// Allows observing changes of a <see cref="Vector2"/>.
    /// </summary>
    public class Vector2ObservableProperty : ObservableProperty<Vector2, Vector2ObservableProperty.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the <see cref="Vector2"/> state.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<Vector2> { }

        /// <summary>
        /// The tolerance to consider the current value and the cached value equal.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public float EqualityTolerance { get; set; } = float.Epsilon;

        /// <inheritdoc/>
        protected override bool Equals(Vector2 a, Vector2 b)
        {
            return a.ApproxEquals(b, EqualityTolerance);
        }
    }
}