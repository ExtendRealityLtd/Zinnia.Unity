namespace Zinnia.Data.Type.Transformation.Conversion
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;

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

        [Tooltip("The current Vector2 representing the direction.")]
        [SerializeField]
        private Vector2 direction = new Vector2(0f, 1f);
        /// <summary>
        /// The current <see cref="Vector2"/> representing the direction.
        /// </summary>
        public Vector2 Direction
        {
            get
            {
                return direction;
            }
            set
            {
                direction = value;
            }
        }

        /// <summary>
        /// The current angle.
        /// </summary>
        protected float angle;
        /// <summary>
        /// A container to allow setting of the <see cref="Direction"/>.
        /// </summary>
        protected Vector2 outputAngle;

        /// <summary>
        /// Sets the <see cref="Direction"/> x value.
        /// </summary>
        /// <param name="value">The value to set to.</param>
        public virtual void SetDirectionX(float value)
        {
            Direction = new Vector2(value, Direction.y);
        }

        /// <summary>
        /// Sets the <see cref="Direction"/> y value.
        /// </summary>
        /// <param name="value">The value to set to.</param>
        public virtual void SetDirectionY(float value)
        {
            Direction = new Vector2(Direction.x, value);
        }

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