namespace Zinnia.Rule
{
    using UnityEngine;
    using Zinnia.Extension;

    /// <summary>
    /// Determines whether a given <see cref="Vector3"/> is equal to the <see cref="Target"/> within a <see cref="Tolerance"/>.
    /// </summary>
    public class Vector3EqualsRule : Vector3Rule
    {
        [Tooltip("The Vector3 to check equality against.")]
        [SerializeField]
        private Vector3 target;
        /// <summary>
        /// The <see cref="Vector3"/> to check equality against.
        /// </summary>
        public Vector3 Target
        {
            get
            {
                return target;
            }
            set
            {
                target = value;
            }
        }
        [Tooltip("The tolerance between the two Vector3 values that can be considered equal.")]
        [SerializeField]
        private float tolerance = float.Epsilon;
        /// <summary>
        /// The tolerance between the two <see cref="Vector3"/> values that can be considered equal.
        /// </summary>
        public float Tolerance
        {
            get
            {
                return tolerance;
            }
            set
            {
                tolerance = value;
            }
        }

        /// <inheritdoc />
        protected override bool Accepts(Vector3 targetVector3)
        {
            return targetVector3.ApproxEquals(Target, Tolerance);
        }
    }
}