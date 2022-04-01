namespace Zinnia.Data.Operation.Extraction
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using Zinnia.Extension;

    /// <summary>
    /// Extracts and emits the <see cref="float"/> components of a <see cref="Vector2"/>.
    /// </summary>
    [Obsolete("Use `Zinnia.Data.Type.Transformation.Conversion.Vector2ToFloat` instead.")]
    public class Vector2ComponentExtractor : MonoBehaviour
    {
        /// <summary>
        /// Defines an event with a <see cref="float"/> value.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<float> { }

        /// <summary>
        /// The components of a <see cref="Vector2"/>
        /// </summary>
        public enum Vector2Component
        {
            /// <summary>
            /// The X component.
            /// </summary>
            X,
            /// <summary>
            /// The Y component.
            /// </summary>
            Y
        }

        [Tooltip("The source to extract from.")]
        [SerializeField]
        private Vector2 source;
        /// <summary>
        /// The source to extract from.
        /// </summary>
        public Vector2 Source
        {
            get
            {
                return source;
            }
            set
            {
                source = value;
            }
        }
        [Tooltip("The component to extract from the Vector2.")]
        [SerializeField]
        private Vector2Component componentToExtract = Vector2Component.X;
        /// <summary>
        /// The component to extract from the <see cref="Vector2"/>.
        /// </summary>
        public Vector2Component ComponentToExtract
        {
            get
            {
                return componentToExtract;
            }
            set
            {
                componentToExtract = value;
            }
        }

        /// <summary>
        /// Emitted when the <see cref="float"/> component from <see cref="Source"/> is extracted.
        /// </summary>
        public UnityEvent Extracted = new UnityEvent();

        /// <summary>
        /// The extracted <see cref="float"/> component.
        /// </summary>
        public float? Result { get; protected set; }

        /// <summary>
        /// Extracts the <see cref="float"/> component from the <see cref="Vector2"/>.
        /// </summary>
        /// <returns>The extracted <see cref="float"/>.</returns>
        public virtual float? Extract()
        {
            if (!isActiveAndEnabled)
            {
                Result = null;
                return null;
            }

            switch (ComponentToExtract)
            {
                case Vector2Component.X:
                    Result = Source.x;
                    break;
                case Vector2Component.Y:
                    Result = Source.y;
                    break;
            }

            Extracted?.Invoke(Result.Value);
            return Result;
        }

        /// <summary>
        /// Extracts the <see cref="float"/> component from the <see cref="Vector2"/>.
        /// </summary>
        public virtual void DoExtract()
        {
            if (!this.IsValidState())
            {
                return;
            }

            Extract();
        }

        /// <summary>
        /// Extracts the <see cref="float"/> component from the <see cref="Vector2"/>.
        /// </summary>
        /// <param name="source">The source to extract from.</param>
        /// <returns>The extracted <see cref="float"/>.</returns>
        public virtual float? Extract(Vector2 source)
        {
            if (!this.IsValidState())
            {
                return null;
            }

            Source = source;
            return Extract();
        }

        /// <summary>
        /// Extracts the <see cref="float"/> component from the <see cref="Vector2"/>.
        /// </summary>
        /// <param name="source">The source to extract from.</param>
        public virtual void DoExtract(Vector2 source)
        {
            if (!this.IsValidState())
            {
                return;
            }

            Extract(source);
        }
    }
}