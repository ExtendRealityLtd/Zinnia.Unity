namespace Zinnia.Data.Type
{
    using System;
    using UnityEngine;
    using Zinnia.Extension;

    /// <summary>
    /// Holds information about the located surface.
    /// </summary>
    [Serializable]
    public class SurfaceData : TransformData
    {
        [Tooltip("The origin in which the surface search came from.")]
        [SerializeField]
        private Vector3 origin;
        /// <summary>
        /// The origin in which the surface search came from.
        /// </summary>
        public Vector3 Origin
        {
            get
            {
                return origin;
            }
            set
            {
                origin = value;
            }
        }
        [Tooltip("The direction in which the surface search came from.")]
        [SerializeField]
        private Vector3 direction;
        /// <summary>
        /// The direction in which the surface search came from.
        /// </summary>
        public Vector3 Direction
        {
            get
            {
                return direction;
            }
            set
            {
                direction = value;
            }
        }

        /// <summary>
        /// Positional offset data that has been applied to the collision point.
        /// </summary>
        public Vector3 PositionalOffset { get; set; }

        /// <summary>
        /// <see cref="RaycastHit"/> data about the current collision.
        /// </summary>
        private RaycastHit collisionData;
        public RaycastHit CollisionData
        {
            get
            {
                return collisionData;
            }
            set
            {
                if (Application.isPlaying)
                {
                    OnBeforeCollisionDataChange();
                }
                collisionData = value;
            }
        }

        /// <summary>
        /// <see cref="RaycastHit"/> data about the previous collision.
        /// </summary>
        public RaycastHit PreviousCollisionData { get; protected set; }

        /// <summary>
        /// Creates a new <see cref="SurfaceData"/> with a default <see cref="Origin"/> and <see cref="Direction"/> for the collision search.
        /// </summary>
        public SurfaceData() { }

        /// <summary>
        /// Creates a new <see cref="SurfaceData"/> with the given <see cref="Transform"/> and a default <see cref="Origin"/> and <see cref="Direction"/> for the collision search.
        /// </summary>
        public SurfaceData(Transform transform) : base(transform) { }

        /// <summary>
        /// Creates a new <see cref="SurfaceData"/> with a given <see cref="Origin"/> and <see cref="Direction"/> for the collision search.
        /// </summary>
        /// <param name="origin">The given origin to instantiate the <see cref="SurfaceData"/> with.</param>
        /// <param name="direction">The given direction to instantiate the <see cref="SurfaceData"/> with.</param>
        public SurfaceData(Vector3 origin, Vector3 direction)
        {
            Origin = origin;
            Direction = direction;
        }

        /// <inheritdoc />
        public override void Clear()
        {
            base.Clear();
            Origin = Vector3.zero;
            Direction = Vector3.zero;
            PositionalOffset = Vector3.zero;
            CollisionData = new RaycastHit();
            PreviousCollisionData = new RaycastHit();
        }

        /// <inheritdoc />
        public override bool Equals(object other)
        {
            if (other == null)
            {
                return false;
            }

            SurfaceData data = other as SurfaceData;
            return Equals(data);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            string[] titles = new string[]
            {
                "Origin",
                "Direction",
                "CollisionData"
            };

            object[] values = new object[]
            {
                Origin,
                Direction,
                CollisionData.ToFormattedString()
            };

            return StringExtensions.FormatForToString(titles, values, base.ToString());
        }

        /// <summary>
        /// Checks to see if the given <see cref="SurfaceData"/> is equal to <see cref="this"/>.
        /// </summary>
        /// <param name="other">The instance to check equality with.</param>
        /// <returns>Whether the two instances are equal.</returns>
        public virtual bool Equals(SurfaceData other)
        {
            if (other == null || !GetType().Equals(other.GetType()))
            {
                return false;
            }

            return base.Equals(other) &&
                Origin.ApproxEquals(other.Origin) &&
                Direction.ApproxEquals(other.Direction) &&
                CollisionData.Equals(other.CollisionData) &&
                PreviousCollisionData.Equals(other.PreviousCollisionData);
        }

        /// <summary>
        /// Called before <see cref="CollisionData"/> has been changed.
        /// </summary>
        protected virtual void OnBeforeCollisionDataChange()
        {
            PreviousCollisionData = CollisionData;
        }
    }
}