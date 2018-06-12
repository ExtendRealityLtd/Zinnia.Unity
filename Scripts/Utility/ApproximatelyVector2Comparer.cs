namespace VRTK.Core.Utility
{
    using UnityEngine;
    using System.Collections.Generic;
    using VRTK.Core.Extension;

    /// <summary>
    /// Determines if two <see cref="Vector2"/> values are equal based on a given tolerance.
    /// </summary>
    public class ApproximatelyVector2Comparer : IEqualityComparer<Vector2>
    {
        /// <summary>
        /// The tolerance in which the two <see cref="Vector2"/> values can be within to be considered equal.
        /// </summary>
        public float tolerance;

        public ApproximatelyVector2Comparer(float tolerance = float.Epsilon)
        {
            this.tolerance = tolerance;
        }

        /// <inheritdoc/>
        public bool Equals(Vector2 x, Vector2 y)
        {
            return x.ApproxEquals(y, tolerance);
        }

        /// <inheritdoc/>
        public int GetHashCode(Vector2 obj)
        {
            return obj.GetHashCode();
        }
    }
}