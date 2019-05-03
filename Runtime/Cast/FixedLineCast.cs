namespace Zinnia.Cast
{
    using UnityEngine;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.MemberChangeMethod;

    /// <summary>
    /// Casts a straight line in the direction of the origin for a fixed length.
    /// </summary>
    public class FixedLineCast : StraightLineCast
    {
        /// <summary>
        /// The current length of the cast.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public float CurrentLength { get; set; } = 1f;

        /// <summary>
        /// Sets the current length of the cast from the given event data.
        /// </summary>
        /// <param name="data">The data to extract the new current length from.</param>
        public virtual void SetCurrentLength(EventData data)
        {
            TargetHit = data?.HitData;
            if (data?.Points.Count >= 2)
            {
                CurrentLength = Vector3.Distance(data.Points[0], data.Points[1]);
            }
        }

        /// <inheritdoc />
        protected override void GeneratePoints()
        {
            Vector3 originPosition = Origin.transform.position;
            Vector3 destinationPosition = originPosition + Origin.transform.forward * CurrentLength;

            points[0] = originPosition;
            points[1] = DestinationPointOverride != null ? (Vector3)DestinationPointOverride : destinationPosition;
        }

        /// <summary>
        /// Retrieves <see cref="CurrentLength"/> clamped between `0f` and <see cref="StraightLineCast.MaximumLength"/>.
        /// </summary>
        /// <returns>The clamped value.</returns>
        protected virtual float GetClampedCurrentLength()
        {
            return Mathf.Clamp(CurrentLength, 0f, MaximumLength);
        }

        /// <summary>
        /// Called after <see cref="StraightLineCast.MaximumLength"/> has been changed.
        /// </summary>
        [CalledAfterChangeOf(nameof(MaximumLength))]
        protected virtual void OnAfterMaximumLengthChange()
        {
            CurrentLength = GetClampedCurrentLength();
        }

        /// <summary>
        /// Called after <see cref="CurrentLength"/> has been changed.
        /// </summary>
        [CalledAfterChangeOf(nameof(CurrentLength))]
        protected virtual void OnAfterCurrentLengthChange()
        {
            CurrentLength = GetClampedCurrentLength();
        }
    }
}