namespace Zinnia.Tracking
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using System.Collections.Generic;
    using Malimbe.MemberClearanceMethod;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.BehaviourStateRequirementMethod;
    using Zinnia.Cast;
    using Zinnia.Rule;
    using Zinnia.Process;
    using Zinnia.Process.Moment;
    using Zinnia.Data.Type;
    using Zinnia.Extension;

    /// <summary>
    /// Casts a <see cref="Ray"/> in a given direction and looks for the nearest valid surface.
    /// </summary>
    public class SurfaceLocator : MonoBehaviour, IProcessable
    {
        /// <summary>
        /// Compares two instances of <see cref="RaycastHit"/>.
        /// </summary>
        protected class RayCastHitComparer : IComparer<RaycastHit>
        {
            /// <inheritdoc />
            public virtual int Compare(RaycastHit x, RaycastHit y)
            {
                return x.distance.CompareTo(y.distance);
            }
        }

        /// <summary>
        /// Defines the event with the <see cref="SurfaceData"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<SurfaceData> { }

        #region Search Settings
        /// <summary>
        /// The origin of where to begin the cast to locate the nearest surface.
        /// </summary>
        [Serialized, Cleared]
        [field: Header("Search Settings"), DocumentedByXml]
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
        /// The amount to offset the position of the destination point found on the located surface.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public Vector3 DestinationOffset { get; set; } = Vector3.zero;
        #endregion

        #region Restriction Settings
        /// <summary>
        /// An optional <see cref="RuleContainer"/> to determine valid and invalid targets based on the set rules.
        /// </summary>
        [Serialized, Cleared]
        [field: Header("Restriction Settings"), DocumentedByXml]
        public RuleContainer TargetValidity { get; set; }
        /// <summary>
        /// An optional <see cref="RuleContainer"/> to determine if the search for a valid surface should be terminated if the current found target matches the rule.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public RuleContainer LocatorTermination { get; set; }
        /// <summary>
        /// An optional custom <see cref="Cast.PhysicsCast"/> object to affect the <see cref="Ray"/>.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public PhysicsCast PhysicsCast { get; set; }
        #endregion

        #region Location Events
        /// <summary>
        /// Emitted when a new surface is located.
        /// </summary>
        [Header("Location Events"), DocumentedByXml]
        public UnityEvent SurfaceLocated = new UnityEvent();
        #endregion

        /// <summary>
        /// The located surface.
        /// </summary>
        public readonly SurfaceData surfaceData = new SurfaceData();

        /// <summary>
        /// The distance to consider a position change.
        /// </summary>
        protected const float DISTANCE_VARIANCE = 0.0001f;
        /// <summary>
        /// A reused comparer instance.
        /// </summary>
        protected static readonly RayCastHitComparer Comparer = new RayCastHitComparer();
        /// <summary>
        /// The comparison <see cref="Comparer"/> does.
        /// </summary>
        protected static readonly Comparison<RaycastHit> Comparison = Comparer.Compare;
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
            transformData.Transform = SearchOrigin == null ? null : SearchOrigin.transform;
            Locate(transformData);
        }

        /// <summary>
        /// Locates the nearest available surface with the given <see cref="TransformData"/>.
        /// </summary>
        /// <param name="givenOrigin">The <see cref="TransformData"/> object to use as the origin for the surface search.</param>
        [RequiresBehaviourState]
        public virtual void Locate(TransformData givenOrigin)
        {
            if (givenOrigin == null || !givenOrigin.IsValid)
            {
                return;
            }

            if (CastRay(givenOrigin.Position, SearchDirection) && PositionChanged(DISTANCE_VARIANCE))
            {
                surfaceData.RotationOverride = givenOrigin.Rotation;
                surfaceData.ScaleOverride = givenOrigin.Scale;
                SurfaceLocated?.Invoke(surfaceData);
            }
        }

        protected virtual void OnEnable()
        {
            surfaceData.Clear();
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
            surfaceData.Origin = givenOrigin;
            surfaceData.Direction = givenDirection;
            Ray tracerRaycast = new Ray(givenOrigin, givenDirection);
            return TargetValidity?.Interface == null ? FindFirstCollision(tracerRaycast) : FindAllCollisions(tracerRaycast);
        }

        /// <summary>
        /// Casts a ray to find the first collision.
        /// </summary>
        /// <param name="tracerRaycast">The ray to cast with.</param>
        /// <returns><see langword="true"/> if a valid surface is located.</returns>
        protected virtual bool FindFirstCollision(Ray tracerRaycast)
        {
            if (PhysicsCast.Raycast(PhysicsCast, tracerRaycast, out RaycastHit collision, MaximumDistance, Physics.IgnoreRaycastLayer) && (LocatorTermination?.Interface == null || !CollisionMatchesRule(collision, LocatorTermination)))
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
            ArraySegment<RaycastHit> raycastHits = PhysicsCast.RaycastAll(
                PhysicsCast,
                tracerRaycast,
                MaximumDistance,
                Physics.IgnoreRaycastLayer);
            ArraySortExtensions<RaycastHit>.Sort(raycastHits.Array, raycastHits.Offset, raycastHits.Count, Comparer, Comparison);

            foreach (RaycastHit collision in (HeapAllocationFreeReadOnlyList<RaycastHit>)raycastHits)
            {
                if (LocatorTermination?.Interface != null && CollisionMatchesRule(collision, LocatorTermination))
                {
                    break;
                }

                if (CollisionMatchesRule(collision, TargetValidity))
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
            surfaceData.Transform = surfaceData.CollisionData.transform;
            surfaceData.PositionOverride = surfaceData.CollisionData.point + DestinationOffset;
        }

        /// <summary>
        /// Determines whether the <see cref="RaycastHit"/> data contains a valid collision against the given rule.
        /// </summary>
        /// <param name="collisionData">The <see cref="RaycastHit"/> data to check for validity on.</param>
        /// <param name="rule">The <see cref="RuleContainer"/> to check the validity against.</param>
        /// <returns>Whether the <see cref="RaycastHit"/> data contains a valid collision.</returns>
        protected virtual bool CollisionMatchesRule(RaycastHit collisionData, RuleContainer rule)
        {
            return collisionData.transform != null && rule.Accepts(collisionData.transform.gameObject);
        }
    }
}