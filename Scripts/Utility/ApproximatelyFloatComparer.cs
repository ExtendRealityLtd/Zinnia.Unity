namespace VRTK.Core.Utility
{
    using System.Collections.Generic;
    using VRTK.Core.Extension;

    /// <summary>
    /// Determines if two <see cref="float"/> values are equal based on a given tolerance.
    /// </summary>
    public class ApproximatelyFloatComparer : IEqualityComparer<float>
    {
        /// <summary>
        /// The tolerance in which the two <see cref="float"/> values can be within to be considered equal.
        /// </summary>
        public float tolerance;

        public ApproximatelyFloatComparer(float tolerance = float.Epsilon)
        {
            this.tolerance = tolerance;
        }

        /// <inheritdoc/>
        public bool Equals(float x, float y)
        {
            return x.ApproxEquals(y, tolerance);
        }

        /// <inheritdoc/>
        public int GetHashCode(float obj)
        {
            return obj.GetHashCode();
        }
    }
}
