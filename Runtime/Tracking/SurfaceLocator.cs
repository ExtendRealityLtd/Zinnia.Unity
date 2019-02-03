namespace Zinnia.Tracking
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using System.Linq;
    using Malimbe.BehaviourStateRequirementMethod;
    using Malimbe.MemberClearanceMethod;
    using Malimbe.XmlDocumentationAttribute;
    using Zinnia.Cast;
    using Zinnia.Data.Type;
    using Zinnia.Extension;
    using Zinnia.Process;
    using Zinnia.Process.Moment;
    using Zinnia.Rule;

    /// <summary>
    /// Casts a <see cref="Ray"/> in a given direction and looks for the nearest valid surface.
    /// </summary>
    public class SurfaceLocator : MonoBehaviour, IProcessable
    {
        /// <summary>
        /// Defines the event with the <see cref="SurfaceData"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<SurfaceData>
        {
        }

        /// <summary>
        /// The origin of where to begin the cast to locate the nearest surface.
        /// </summary>
        [DocumentedByXml, Cleared]
        public GameObject searchOrigin;
        /// <summary>
        /// The direction in which to cast to locate the nearest surface.
        /// </summary>
        [DocumentedByXml]
        public Vector3 searchDirection;
        /// <summary>
        /// The distance to move the origin backwards through the <see cref="searchDirection"/> to ensure the origin isn't clipping a surface.
        /// </summary>
        [DocumentedByXml]
        public float originOffset = -0.01f;
        /// <summary>
        /// The maximum distance to cast the <see cref="Ray"/>.
        /// </summary>
        [DocumentedByXml]
        public float maximumDistance = 50f;
        /// <summary>
        /// An optional <see cref="RuleContainer"/> to determine valid and invalid targets based on the set rules.
        /// </summary>
        [DocumentedByXml]
        public RuleContainer targetValidity;
        /// <summary>
        /// An optional custom <see cref="PhysicsCast"/> object to affect the <see cref="Ray"/>.
        /// </summary>
        [DocumentedByXml]
        public PhysicsCast physicsCast;

        /// <summary>
        /// Emitted when a new surface is located.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent SurfaceLocated = new UnityEvent();

        /// <summary>
        /// The located surface.
        /// </summary>
        public SurfaceData SurfaceData
        {
            get;
            protected set;
        }

        protected const float DISTANCE_VARIANCE = 0.0001f;

        /// <summary>
        /// Sets the <see cref="searchOrigin"/> parameter.
        /// </summary>
        /// <param name="searchOrigin">The new searchOrigin value.</param>
        public virtual void SetSearchOrigin(GameObject searchOrigin)
        {
            this.searchOrigin = searchOrigin;
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
        [RequiresBehaviourState]
        public virtual void Locate(TransformData givenOrigin)
        {
            if (givenOrigin == null || !givenOrigin.Valid)
            {
                return;
            }

            if (CastRay(givenOrigin.Position, searchDirection) && PositionChanged(DISTANCE_VARIANCE))
            {
                SurfaceData.rotationOverride = givenOrigin.Rotation;
                SurfaceLocated?.Invoke(SurfaceData);
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
            return (targetValidity == null ? FindFirstCollision(tracerRaycast) : FindAllCollisions(tracerRaycast));
        }

        /// <summary>
        /// Casts a ray to find the first collision.
        /// </summary>
        /// <param name="tracerRaycast">The ray to cast with.</param>
        /// <returns><see langword="true"/> if a valid surface is located.</returns>
        protected virtual bool FindFirstCollision(Ray tracerRaycast)
        {
            RaycastHit collision;
            if (PhysicsCast.Raycast(physicsCast, tracerRaycast, out collision, maximumDistance, Physics.IgnoreRaycastLayer))
            {
                SetSurfaceData(collision);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Casts a ray to find all collisions.
        /// </summary>
        /// <param name="tracerRaycast">The ray to cast with.</param>
        /// <returns><see langword="true"/> if a valid surface is located.</returns>
        protected virtual bool FindAllCollisions(Ray tracerRaycast)
        {
            foreach (RaycastHit collision in PhysicsCast.RaycastAll(physicsCast, tracerRaycast, maximumDistance, Physics.IgnoreRaycastLayer).OrderBy(hit => hit.distance))
            {
                if (ValidSurface(collision))
                {
                    SetSurfaceData(collision);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Sets the <see cref="SurfaceData"/> collision information.
        /// </summary>
        /// <param name="collision">The data to use when setting the SurfaceData.</param>
        protected virtual void SetSurfaceData(RaycastHit collision)
        {
            SurfaceData.CollisionData = collision;
            SurfaceData.transform = SurfaceData.CollisionData.transform;
            SurfaceData.positionOverride = SurfaceData.CollisionData.point;
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
                return targetValidity.Accepts(collisionData.transform.gameObject);
            }
            return true;
        }
    }
}