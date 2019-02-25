namespace Zinnia.Cast
{
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.PropertySetterMethod;
    using Malimbe.PropertyValidationMethod;
    using Malimbe.XmlDocumentationAttribute;
    using UnityEngine;

    /// <summary>
    /// Casts a straight line in the direction of the origin for a fixed length.
    /// </summary>
    public class FixedLineCast : StraightLineCast
    {
        /// <summary>
        /// The current length of the cast.
        /// </summary>
        [Serialized, Validated]
        [field: DocumentedByXml]
        public float CurrentLength { get; set; } = 1f;

        /// <summary>
        /// Sets the current length of the cast from the given event data.
        /// </summary>
        /// <param name="data">The data to extract the new current length from.</param>
        public virtual void SetCurrentLength(EventData data)
        {
            TargetHit = data?.targetHit;
            if (data?.points.Count >= 2)
            {
                CurrentLength = Vector3.Distance(data.points[0], data.points[1]);
            }
        }

        /// <inheritdoc />
        protected override void GeneratePoints()
        {
            Vector3 originPosition = origin.transform.position;
            points[0] = originPosition;
            points[1] = originPosition + origin.transform.forward * CurrentLength;
        }

        /// <summary>
        /// Handles changes to <see cref="StraightLineCast.MaximumLength"/>.
        /// </summary>
        /// <param name="previousValue">The previous value.</param>
        /// <param name="newValue">The new value.</param>
        [CalledBySetter(nameof(MaximumLength))]
        protected virtual void OnMaximumLengthChange(float previousValue, ref float newValue)
        {
            CurrentLength = Mathf.Clamp(CurrentLength, 0f, newValue);
        }

        /// <summary>
        /// Handles changes to <see cref="CurrentLength"/>.
        /// </summary>
        /// <param name="previousValue">The previous value.</param>
        /// <param name="newValue">The new value.</param>
        [CalledBySetter(nameof(CurrentLength))]
        protected virtual void OnCurrentLengthChange(float previousValue, ref float newValue)
        {
            newValue = Mathf.Clamp(newValue, 0f, MaximumLength);
        }
    }
}