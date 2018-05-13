namespace VRTK.Core.Tracking
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using VRTK.Core.Utility;
    using VRTK.Core.Data.Type;
    using VRTK.Core.Process;
    using VRTK.Core.Extension;

    /// <summary>
    /// The SurfaceLocator casts a ray in a given direction and looks for the nearest valid surface.
    /// </summary>
    public class SurfaceLocator : MonoBehaviour, IProcessable
    {
        [Tooltip("The origin of where to begin the cast to locate the nearest surface.")]
        public Transform searchOrigin;
        [Tooltip("The direction in which to cast to locate the nearest surface.")]
        public Vector3 searchDirection;
        [Tooltip("The distance to move the origin backwards through the `searchDirection` to ensure the origin isn't clipping a surface.")]
        public float originOffset = -0.01f;
        [Tooltip("The maximum distance to cast the ray.")]
        public float maximumDistance = 50f;
        [Tooltip("An optional ExclusionRule to determine valid and invalid targets based on the set rules.")]
        public ExclusionRule exclusionRule;
        [Tooltip("An optional custom PhysicsCast object to affect the Cast of the pointer.")]
        public PhysicsCast physicsCast;

        protected const float DISTANCE_VARIANCE = 0.0001f;

        /// <summary>
        /// The SurfaceLocatorUnityEvent emits an event with the SurfaceData payload and the sender object.
        /// </summary>
        [Serializable]
        public class SurfaceLocatorUnityEvent : UnityEvent<SurfaceData, object>
        {
        };

        /// <summary>
        /// The SurfaceLocated event is emitted when a new surface is located.
        /// </summary>
        public SurfaceLocatorUnityEvent SurfaceLocated = new SurfaceLocatorUnityEvent();

        /// <summary>
        /// The SurfaceData about the located surface.
        /// </summary>
        public SurfaceData SurfaceData
        {
            get;
            protected set;
        }

        /// <summary>
        /// The Process method attempts to locate the nearest available surface upon a MomentProcess.
        /// </summary>
        public virtual void Process()
        {
            Locate();
        }

        /// <summary>
        /// The Locate method attempts to locate the nearest available surface with the specified `searchOrigin` Transform.
        /// </summary>
        public virtual void Locate()
        {
            Locate(new TransformData(searchOrigin));
        }

        /// <summary>
        /// The Locate method attempts to locate the nearest available surface with the given TransformData.
        /// </summary>
        /// <param name="givenOrigin">The TransformData object to use as the origin for the surface search.</param>
        /// <param name="initiator">The object which initiated the method.</param>
        public virtual void Locate(TransformData givenOrigin, object initiator = null)
        {
            if (givenOrigin != null && givenOrigin.Valid && CastRay(givenOrigin.Position, searchDirection) && PositionChanged(DISTANCE_VARIANCE))
            {
                OnSurfaceLocated(SurfaceData);
            }
        }

        protected virtual void OnSurfaceLocated(SurfaceData e)
        {
            SurfaceLocated?.Invoke(e, this);
        }

        /// <summary>
        /// The PositionChanged method checks to see if the surface position has changed between frames.
        /// </summary>
        /// <param name="checkDistance">The distance to consider a position change.</param>
        /// <returns>Returns `true` if the surface position has changed or no previous surface data is found.</returns>
        protected virtual bool PositionChanged(float checkDistance)
        {
            return (SurfaceData.PreviousCollisionData.transform == null || !SurfaceData.Position.Compare(SurfaceData.PreviousCollisionData.point, checkDistance));
        }

        /// <summary>
        /// The CastRay method casts a ray in the given direction from the given origin to search the nearest surface.
        /// </summary>
        /// <param name="givenOrigin">The origin to begin the RayCast from.</param>
        /// <param name="givenDirection">The direction in which to cast the ray.</param>
        /// <returns>Returns `true` if a valid surface is located.</returns>
        protected virtual bool CastRay(Vector3 givenOrigin, Vector3 givenDirection)
        {
            givenOrigin = givenOrigin + (givenDirection.normalized * originOffset);
            if (SurfaceData == null)
            {
                SurfaceData = new SurfaceData();
            }
            SurfaceData.origin = givenOrigin;
            SurfaceData.direction = givenDirection;
            Ray tracerRaycast = new Ray(givenOrigin, givenDirection);
            RaycastHit collision;
            bool collided = PhysicsCast.Raycast(physicsCast, tracerRaycast, out collision, maximumDistance, Physics.IgnoreRaycastLayer);
            SurfaceData.CollisionData = collision;

            if (collided && ValidSurface(SurfaceData.CollisionData))
            {
                SurfaceData.transform = SurfaceData.CollisionData.transform;
                SurfaceData.positionOverride = SurfaceData.CollisionData.point;
                return true;
            }
            return false;
        }

        /// <summary>
        /// The ValidSurface method determines whether the RaycastHit data contains a valid surface.
        /// </summary>
        /// <param name="collisionData">The RaycastHit data to check for validity on.</param>
        /// <returns>Returns `true` if the RaycastHit data contains a valid surface.</returns>
        protected virtual bool ValidSurface(RaycastHit collisionData)
        {
            if (exclusionRule != null && collisionData.transform != null)
            {
                return !ExclusionRule.ShouldExclude(collisionData.transform.gameObject, exclusionRule);
            }
            return true;
        }
    }
}