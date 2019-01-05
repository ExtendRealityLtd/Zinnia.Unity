namespace Zinnia.Cast
{
    using UnityEngine;

    /// <summary>
    /// Casts a straight line in the direction of the origin for a fixed length.
    /// </summary>
    public class FixedLineCast : StraightLineCast
    {
        /// <summary>
        /// The current length of the cast.
        /// </summary>
        public float currentLength = 1f;

        /// <summary>
        /// Sets the current length of the cast to the given length.
        /// </summary>
        /// <param name="length">The new current length of the cast.</param>
        public virtual void SetCurrentLength(float length)
        {
            this.currentLength = length;
        }

        /// <summary>
        /// Sets the current length of the cast from the given event data.
        /// </summary>
        /// <param name="data">The data to extract the new current length from.</param>
        public virtual void SetCurrentLength(EventData data)
        {
            TargetHit = data?.targetHit;
            if (data?.points.Count >= 2)
            {
                currentLength = Vector3.Distance(data.points[0], data.points[1]);
            }
        }

        /// <inheritdoc />
        protected override void GeneratePoints()
        {
            points[0] = origin.transform.position;
            points[1] = origin.transform.position + origin.transform.forward * Mathf.Clamp(currentLength, 0f, maximumLength);
        }
    }
}