namespace VRTK.Core.Data.Type.Transformation
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;

    /// <summary>
    /// Transforms a <see cref="Vector3"/> by multiplying each coordinate by a given multiplier.
    /// </summary>
    /// <example>
    /// (2f,3f,4f) * [3f,4f,5f] = (6f,12f,20f)
    /// </example>
    public class Vector3Multiplier : Transformer<Vector3, Vector3, Vector3Multiplier.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the multiplied <see cref="Vector3"/> value.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<Vector3>
        {
        }

        /// <summary>
        /// The value to multiply the input by.
        /// </summary>
        [Tooltip("The value to multiply the input by."), SerializeField]
        protected Vector3 multiplier = Vector3.one;

        /// <summary>
        /// Sets the value to multiply the input by.
        /// </summary>
        /// <param name="multiplier">The new multiplier value.</param>
        public virtual void SetMultiplier(Vector3 multiplier)
        {
            this.multiplier = multiplier;
        }

        /// <summary>
        /// Sets the x value to multiply the input x by.
        /// </summary>
        /// <param name="xMultiplier">The new x multiplier value.</param>
        public virtual void SetXMultiplier(float xMultiplier)
        {
            multiplier.x = xMultiplier;
        }

        /// <summary>
        /// Sets the y value to multiply the input y by.
        /// </summary>
        /// <param name="yMultiplier">The new y multiplier value.</param>
        public virtual void SetYMultiplier(float yMultiplier)
        {
            multiplier.y = yMultiplier;
        }

        /// <summary>
        /// Sets the z value to multiply the input z by.
        /// </summary>
        /// <param name="zMultiplier">The new z multiplier value.</param>
        public virtual void SetZMultiplier(float zMultiplier)
        {
            multiplier.z = zMultiplier;
        }

        /// <summary>
        /// Multiplies the input by the multipliers.
        /// </summary>
        /// <param name="input">The value to transform.</param>
        /// <returns>The transformed value.</returns>
        protected override Vector3 Process(Vector3 input)
        {
            return Vector3.Scale(input, multiplier);
        }
    }
}