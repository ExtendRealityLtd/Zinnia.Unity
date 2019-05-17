namespace Zinnia.Data.Type.Transformation.Conversion
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;

    /// <summary>
    /// Transforms a <see cref="float"/> angle value to a <see cref="Vector2"/> direction.
    /// </summary>
    public class AngleToVector2Direction : Transformer<float, Vector2, AngleToVector2Direction.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the transformed <see cref="Vector2"/> value.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<Vector2> { }

        /// <summary>
        /// The current <see cref="Vector2"/> representing the direction.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public Vector2 Direction { get; set; } = new Vector2(0f, 1f);

        /// <summary>
        /// The current angle.
        /// </summary>
        protected float angle;
        /// <summary>
        /// A container to allow setting of the <see cref="Direction"/>.
        /// </summary>
        protected Vector2 outputAngle;

        /// <summary>
        /// Transforms the given <see cref="float"/> angle into a <see cref="Vector2"/> direction.
        /// </summary>
        /// <param name="input">The value to transform.</param>
        /// <returns>The transformed value.</returns>
        protected override Vector2 Process(float input)
        {
            angle += input;
            angle = Mathf.Repeat(angle, 360f);
            outputAngle.x = Mathf.Cos(angle * Mathf.Deg2Rad);
            outputAngle.y = Mathf.Sin(angle * Mathf.Deg2Rad);
            Direction = outputAngle;
            Direction *= 1f / Mathf.Max(Mathf.Abs(Direction.x), Mathf.Abs(Direction.y));

            return Direction;
        }
    }
}