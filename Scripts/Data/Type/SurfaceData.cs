namespace VRTK.Core.Data.Type
{
    using UnityEngine;

    /// <summary>
    /// The SurfaceData contains information about the located surface.
    /// </summary>
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
        /// RaycastHit data about the current collision.
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
        /// RaycastHit data about the previous collision.
        /// </summary>
        public RaycastHit PreviousCollisionData
        {
            get;
            protected set;
        }

        protected RaycastHit _collisionData;

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public SurfaceData()
        {
        }

        /// <summary>
        /// Creates a new SurfaceData with a default origin and direction for the collision search.
        /// </summary>
        /// <param name="defaultOrigin"></param>
        /// <param name="defaultDirection"></param>
        public SurfaceData(Vector3 defaultOrigin, Vector3 defaultDirection)
        {
            origin = defaultOrigin;
            direction = defaultDirection;
        }
    }
}