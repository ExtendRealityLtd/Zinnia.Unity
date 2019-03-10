﻿namespace Zinnia.Tracking
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using Malimbe.MemberClearanceMethod;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.BehaviourStateRequirementMethod;
    using Zinnia.Cast;
    using Zinnia.Rule;
    using Zinnia.Process;
    using Zinnia.Process.Moment;
    using Zinnia.Extension;
    using Zinnia.Data.Type;

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
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public GameObject SearchOrigin { get; set; }
        /// <summary>
        /// The direction in which to cast to locate the nearest surface.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public Vector3 SearchDirection { get; set; }
        /// <summary>
        /// The distance to move the origin backwards through the <see cref="SearchDirection"/> to ensure the origin isn't clipping a surface.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public float OriginOffset { get; set; } = -0.01f;
        /// <summary>
        /// The maximum distance to cast the <see cref="Ray"/>.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public float MaximumDistance { get; set; } = 50f;
        /// <summary>
        /// An optional <see cref="RuleContainer"/> to determine valid and invalid targets based on the set rules.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public RuleContainer TargetValidity { get; set; }
        /// <summary>
        /// An optional custom <see cref="Cast.PhysicsCast"/> object to affect the <see cref="Ray"/>.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public PhysicsCast PhysicsCast { get; set; }

        /// <summary>
        /// Emitted when a new surface is located.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent SurfaceLocated = new UnityEvent();

        /// <summary>
        /// The located surface.
        /// </summary>
        public readonly SurfaceData surfaceData = new SurfaceData();

        /// <summary>
        /// The distance to consider a position change.
        /// </summary>
        protected const float DISTANCE_VARIANCE = 0.0001f;
        /// <summary>
        /// A reused data instance.
        /// </summary>
        protected readonly TransformData transformData = new TransformData();

        /// <summary>
        /// Locates the nearest available surface upon a <see cref="MomentProcess"/>.
        /// </summary>
        public virtual void Process()
        {
            Locate();
        }

        /// <summary>
        /// Locates the nearest available surface with the specified <see cref="SearchOrigin"/> <see cref="Transform"/>.
        /// </summary>
        public virtual void Locate()
        {
            transformData.transform = SearchOrigin == null ? null : SearchOrigin.transform;
            Locate(transformData);
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

            if (CastRay(givenOrigin.Position, SearchDirection) && PositionChanged(DISTANCE_VARIANCE))
            {
                surfaceData.rotationOverride = givenOrigin.Rotation;
                SurfaceLocated?.Invoke(surfaceData);
            }
        }

        /// <summary>
        /// Checks to see if the surface position has changed between frames.
        /// </summary>
        /// <param name="checkDistance">The distance to consider a position change.</param>
        /// <returns><see langword="true"/> if the surface position has changed or no previous surface data is found.</returns>
        protected virtual bool PositionChanged(float checkDistance)
        {
            return surfaceData.PreviousCollisionData.transform == null || !surfaceData.Position.ApproxEquals(surfaceData.PreviousCollisionData.point, checkDistance);
        }

        /// <summary>
        /// Casts a <see cref="Ray"/> in the given direction from the given origin to search the nearest surface.
        /// </summary>
        /// <param name="givenOrigin">The origin to begin the <see cref="Ray"/> from.</param>
        /// <param name="givenDirection">The direction in which to cast the <see cref="Ray"/>.</param>
        /// <returns><see langword="true"/> if a valid surface is located.</returns>
        protected virtual bool CastRay(Vector3 givenOrigin, Vector3 givenDirection)
        {
            givenOrigin = givenOrigin + (givenDirection.normalized * OriginOffset);
            surfaceData.origin = givenOrigin;
            surfaceData.direction = givenDirection;
            Ray tracerRaycast = new Ray(givenOrigin, givenDirection);
            return TargetValidity == null ? FindFirstCollision(tracerRaycast) : FindAllCollisions(tracerRaycast);
        }

        /// <summary>
        /// Casts a ray to find the first collision.
        /// </summary>
        /// <param name="tracerRaycast">The ray to cast with.</param>
        /// <returns><see langword="true"/> if a valid surface is located.</returns>
        protected virtual bool FindFirstCollision(Ray tracerRaycast)
        {
            RaycastHit collision;
            if (PhysicsCast.Raycast(PhysicsCast, tracerRaycast, out collision, MaximumDistance, Physics.IgnoreRaycastLayer))
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
            RaycastHit[] raycastHits = PhysicsCast.RaycastAll(
                PhysicsCast,
                tracerRaycast,
                MaximumDistance,
                Physics.IgnoreRaycastLayer);
            Array.Sort(raycastHits, (x, y) => (int)(x.distance - y.distance));
            foreach (RaycastHit collision in raycastHits)
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
        /// Sets the <see cref="surfaceData"/> collision information.
        /// </summary>
        /// <param name="collision">The data to use when setting <see cref="surfaceData"/>.</param>
        protected virtual void SetSurfaceData(RaycastHit collision)
        {
            surfaceData.CollisionData = collision;
            surfaceData.transform = surfaceData.CollisionData.transform;
            surfaceData.positionOverride = surfaceData.CollisionData.point;
        }

        /// <summary>
        /// Determines whether the <see cref="RaycastHit"/> data contains a valid surface.
        /// </summary>
        /// <param name="collisionData">The <see cref="RaycastHit"/> data to check for validity on.</param>
        /// <returns><see langword="true"/> if the <see cref="RaycastHit"/> data contains a valid surface.</returns>
        protected virtual bool ValidSurface(RaycastHit collisionData)
        {
            if (TargetValidity != null && collisionData.transform != null)
            {
                return TargetValidity.Accepts(collisionData.transform.gameObject);
            }
            return true;
        }
    }
}