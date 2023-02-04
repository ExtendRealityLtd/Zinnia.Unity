namespace Zinnia.Extension
{
    using UnityEngine;

    /// <summary>
    /// Extended methods for the <see cref="Quaternion"/> Type.
    /// </summary>
    public static class QuaternionExtensions
    {
        /// <summary>
        /// Determines if two <see cref="Quaternion"/> values are equal based on a given tolerance.
        /// </summary>
        /// <param name="a">The <see cref="Quaternion"/> to compare against.</param>
        /// <param name="b">The <see cref="Quaternion"/> to compare with.</param>
        /// <param name="tolerance">The tolerance in which the two <see cref="Quaternion"/> values can be within to be considered equal.</param>
        /// <returns><see langword="true"/> if the two <see cref="Quaternion"/> values are considered equal.</returns>
        public static bool ApproxEquals(this Quaternion a, Quaternion b, float tolerance = float.Epsilon)
        {
            return a.x.ApproxEquals(b.x, tolerance) && a.y.ApproxEquals(b.y, tolerance) && a.z.ApproxEquals(b.z, tolerance) && a.w.ApproxEquals(b.w, tolerance);
        }

        /// <summary>
        /// Gradually changes a <see cref="Quaternion"/> towards a desired goal over time.
        /// </summary>
        /// <param name="current">The current position.</param>
        /// <param name="target">The position we are trying to reach.</param>
        /// <param name="derivative">The current rotational derivative, this value is modified by the function every time you call it.</param>
        /// <param name="smoothTime">Approximately the time it will take to reach the target. A smaller value will reach the target faster.</param>
        /// <returns></returns>
        public static Quaternion SmoothDamp(Quaternion current, Quaternion target, ref Quaternion derivative, float smoothTime)
        {
            if (Time.deltaTime < Mathf.Epsilon)
            {
                return current;
            }

            float signedFlip = Quaternion.Dot(current, target) > 0f ? 1f : -1f;
            target.x *= signedFlip;
            target.y *= signedFlip;
            target.z *= signedFlip;
            target.w *= signedFlip;

            Vector4 result = new Vector4(
                Mathf.SmoothDamp(current.x, target.x, ref derivative.x, smoothTime),
                Mathf.SmoothDamp(current.y, target.y, ref derivative.y, smoothTime),
                Mathf.SmoothDamp(current.z, target.z, ref derivative.z, smoothTime),
                Mathf.SmoothDamp(current.w, target.w, ref derivative.w, smoothTime)
            ).normalized;

            Vector4 derivativeError = Vector4.Project(new Vector4(derivative.x, derivative.y, derivative.z, derivative.w), result);
            derivative.x -= derivativeError.x;
            derivative.y -= derivativeError.y;
            derivative.z -= derivativeError.z;
            derivative.w -= derivativeError.w;

            return new Quaternion(result.x, result.y, result.z, result.w);
        }
    }
}