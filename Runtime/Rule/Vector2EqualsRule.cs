namespace Zinnia.Rule
{
    using UnityEngine;
    using Zinnia.Extension;

    /// <summary>
    /// Determines whether a given <see cref="Vector2"/> is equal to the <see cref="Target"/> within a <see cref="Tolerance"/>.
    /// </summary>
    public class Vector2EqualsRule : Vector2Rule
    {
        [Tooltip("The Vector2 to check equality against.")]
        [SerializeField]
        private Vector2 _target;
        /// <summary>
        /// The <see cref="Vector2"/> to check equality against.
        /// </summary>
        public Vector2 Target
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
        [Tooltip("The tolerance between the two Vector2 values that can be considered equal.")]
        [SerializeField]
        private float _tolerance = float.Epsilon;
        /// <summary>
        /// The tolerance between the two <see cref="Vector2"/> values that can be considered equal.
        /// </summary>
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
        protected override bool Accepts(Vector2 targetVector2)
        {
            return targetVector2.ApproxEquals(Target, Tolerance);
        }
    }
}