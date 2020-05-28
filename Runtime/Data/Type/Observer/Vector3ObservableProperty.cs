namespace Zinnia.Data.Type.Observer
{
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using Zinnia.Extension;

    /// <summary>
    /// Allows observing changes of a <see cref="Vector3"/>.
    /// </summary>
    public class Vector3ObservableProperty : ObservableProperty<Vector3, Vector3ObservableProperty.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the <see cref="Vector3"/> state.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<Vector3> { }

        /// <summary>
        /// The tolerance to consider the current value and the cached value equal.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public float EqualityTolerance { get; set; } = float.Epsilon;

        /// <inheritdoc/>
        protected override bool Equals(Vector3 a, Vector3 b)
        {
            return a.ApproxEquals(b, EqualityTolerance);
        }
    }
}