namespace VRTK.Core.Data.Type
{
    using UnityEngine;
    using System;

    /// <summary>
    /// Holds information about the located surface.
    /// </summary>
    [Serializable]
    public class SurfaceData : TransformData
    {
        /// <summary>
        /// The origin in which the surface search came from.
        /// </summary>
        public Vector3 origin;
        /// <summary>
        /// The direction in which the surface search came from.
        /// </summary>
        public Vector3 direction;

        /// <summary>
        /// <see cref="RaycastHit"/> data about the current collision.
        /// </summary>
        public RaycastHit CollisionData
        {
            get
            {
                return _collisionData;
            }
            set
            {
                PreviousCollisionData = _collisionData;
                _collisionData = value;
            }
        }

        /// <summary>
        /// <see cref="RaycastHit"/> data about the previous collision.
        /// </summary>
        public RaycastHit PreviousCollisionData
        {
            get;
            protected set;
        }

        protected RaycastHit _collisionData;

        /// <summary>
        /// Creates a new <see cref="SurfaceData"/> with a default <see cref="origin"/> and <see cref="direction"/> for the collision search.
        /// </summary>
        public SurfaceData()
        {
        }

        /// <summary>
        /// Creates a new <see cref="SurfaceData"/> with a given <see cref="origin"/> and <see cref="direction"/> for the collision search.
        /// </summary>
        /// <param name="origin">The given origin to instantiate the <see cref="SurfaceData"/> with.</param>
        /// <param name="direction">The given direction to instantiate the <see cref="SurfaceData"/> with.</param>
        public SurfaceData(Vector3 origin, Vector3 direction)
        {
            this.origin = origin;
            this.direction = direction;
        }
    }
}