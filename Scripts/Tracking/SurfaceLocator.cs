namespace VRTK.Core.Tracking
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using VRTK.Core.Cast;
    using VRTK.Core.Utility;
    using VRTK.Core.Data.Type;
    using VRTK.Core.Process;
    using VRTK.Core.Extension;

    /// <summary>
    /// Casts a <see cref="Ray"/> in a given direction and looks for the nearest valid surface.
    /// </summary>
    public class SurfaceLocator : MonoBehaviour, IProcessable
    {
        /// <summary>
        /// The origin of where to begin the cast to locate the nearest surface.
        /// </summary>
        [Tooltip("The origin of where to begin the cast to locate the nearest surface.")]
        public Transform searchOrigin;
        /// <summary>
        /// The direction in which to cast to locate the nearest surface.
        /// </summary>
        [Tooltip("The direction in which to cast to locate the nearest surface.")]
        public Vector3 searchDirection;
        /// <summary>
        /// The distance to move the origin backwards through the <see cref="searchDirection"/> to ensure the origin isn't clipping a surface.
        /// </summary>
        [Tooltip("The distance to move the origin backwards through the `searchDirection` to ensure the origin isn't clipping a surface.")]
        public float originOffset = -0.01f;
        /// <summary>
        /// The maximum distance to cast the <see cref="Ray"/>.
        /// </summary>
        [Tooltip("The maximum distance to cast the ray.")]
        public float maximumDistance = 50f;
        /// <summary>
        /// An optional <see cref="ExclusionRule"/> to determine valid and invalid targets based on the set rules.
        /// </summary>
        [Tooltip("An optional ExclusionRule to determine valid and invalid targets based on the set rules.")]
        public ExclusionRule targetValidity;
        /// <summary>
        /// An optional custom <see cref="PhysicsCast"/> object to affect the <see cref="Ray"/>.
        /// </summary>
        [Tooltip("An optional custom PhysicsCast object to affect the Ray.")]
        public PhysicsCast physicsCast;

        protected const float DISTANCE_VARIANCE = 0.0001f;

        /// <summary>
        /// Defines the event with the <see cref="SurfaceData"/> and sender <see cref="object"/>.
        /// </summary>
        [Serializable]
        public class SurfaceLocatorUnityEvent : UnityEvent<SurfaceData, object>
        {
        }

        /// <summary>
        /// Emitted when a new surface is located.
        /// </summary>
        public SurfaceLocatorUnityEvent SurfaceLocated = new SurfaceLocatorUnityEvent();

        /// <summary>
        /// The located surface.
        /// </summary>
        public SurfaceData SurfaceData
        {
            get;
            protected set;
        }

        /// <summary>
        /// Locates the nearest available surface upon a <see cref="MomentProcess"/>.
        /// </summary>
        public virtual void Process()
        {
            Locate();
        }

        /// <summary>
        /// Locates the nearest available surface with the specified <see cref="searchOrigin"/> <see cref="Transform"/>.
        /// </summary>
        public virtual void Locate()
        {
            Locate(new TransformData(searchOrigin));
        }

        /// <summary>
        /// Locates the nearest available surface with the given <see cref="TransformData"/>.
        /// </summary>
        /// <param name="givenOrigin">The <see cref="TransformData"/> object to use as the origin for the surface search.</param>
        /// <param name="initiator">The <see cref="object"/> which initiated the method.</param>
        public virtual void Locate(TransformData givenOrigin, object initiator = null)
        {
            if (givenOrigin != null && givenOrigin.Valid && CastRay(givenOrigin.Position, searchDirection) && PositionChanged(DISTANCE_VARIANCE))
            {
                SurfaceData.rotationOverride = givenOrigin.Rotation;
                OnSurfaceLocated(SurfaceData);
            }
        }

        protected virtual void OnSurfaceLocated(SurfaceData e)
        {
            if (isActiveAndEnabled)
            {
                SurfaceLocated?.Invoke(e, this);
            }
        }

        /// <summary>
        /// Checks to see if the surface position has changed between frames.
        /// </summary>
        /// <param name="checkDistance">The distance to consider a position change.</param>
        /// <returns><see langword="true"/> if the surface position has changed or no previous surface data is found.</returns>
        protected virtual bool PositionChanged(float checkDistance)
        {
            return (SurfaceData.PreviousCollisionData.transform == null || !SurfaceData.Position.ApproxEquals(SurfaceData.PreviousCollisionData.point, checkDistance));
        }

        /// <summary>
        /// Casts a <see cref="Ray"/> in the given direction from the given origin to search the nearest surface.
        /// </summary>
        /// <param name="givenOrigin">The origin to begin the <see cref="Ray"/> from.</param>
        /// <param name="givenDirection">The direction in which to cast the <see cref="Ray"/>.</param>
        /// <returns><see langword="true"/> if a valid surface is located.</returns>
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
        /// Determines whether the <see cref="RaycastHit"/> data contains a valid surface.
        /// </summary>
        /// <param name="collisionData">The <see cref="RaycastHit"/> data to check for validity on.</param>
        /// <returns><see langword="true"/> if the <see cref="RaycastHit"/> data contains a valid surface.</returns>
        protected virtual bool ValidSurface(RaycastHit collisionData)
        {
            if (targetValidity != null && collisionData.transform != null)
            {
                return !ExclusionRule.ShouldExclude(collisionData.transform.gameObject, targetValidity);
            }
            return true;
        }
    }
}