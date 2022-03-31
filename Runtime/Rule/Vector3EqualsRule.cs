namespace Zinnia.Rule
{
    using UnityEngine;
    using Zinnia.Extension;

    /// <summary>
    /// Determines whether a given <see cref="Vector3"/> is equal to the <see cref="Target"/> within a <see cref="Tolerance"/>.
    /// </summary>
    public class Vector3EqualsRule : Vector3Rule
    {
        /// <summary>
        /// The <see cref="Vector3"/> to check equality against.
        /// </summary>
        [Tooltip("The Vector3 to check equality against.")]
        [SerializeField]
        private Vector3 _target;
        public Vector3 Target
        {
            get
            {
                return _target;
            }
            set
            {
                _target = value;
            }
        }
        /// <summary>
        /// The tolerance between the two <see cref="Vector3"/> values that can be considered equal.
        /// </summary>
        [Tooltip("The tolerance between the two Vector3 values that can be considered equal.")]
        [SerializeField]
        private float _tolerance = float.Epsilon;
        public float Tolerance
        {
            get
            {
                return _tolerance;
            }
            set
            {
                _tolerance = value;
            }
        }

        /// <inheritdoc />
        protected override bool Accepts(Vector3 targetVector3)
        {
            return targetVector3.ApproxEquals(Target, Tolerance);
        }
    }
}