namespace Zinnia.Tracking
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;
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
        [Header("Search Settings")]
        [Tooltip("The origin of where to begin the cast to locate the nearest surface.")]
        [SerializeField]
        private GameObject searchOrigin;
        /// <summary>
        /// The origin of where to begin the cast to locate the nearest surface.
        /// </summary>
        public GameObject SearchOrigin
        {
            get
            {
                return searchOrigin;
            }
            set
            {
                searchOrigin = value;
            }
        }
        [Tooltip("The direction in which to cast to locate the nearest surface.")]
        [SerializeField]
        private Vector3 searchDirection;
        /// <summary>
        /// The direction in which to cast to locate the nearest surface.
        /// </summary>
        public Vector3 SearchDirection
        {
            get
            {
                return searchDirection;
            }
            set
            {
                searchDirection = value;
            }
        }
        [Tooltip("The distance to move the origin backwards through the SearchDirection to ensure the origin isn't clipping a surface.")]
        [SerializeField]
        private float originOffset = -0.01f;
        /// <summary>
        /// The distance to move the origin backwards through the <see cref="SearchDirection"/> to ensure the origin isn't clipping a surface.
        /// </summary>
        public float OriginOffset
        {
            get
            {
                return originOffset;
            }
            set
            {
                originOffset = value;
            }
        }
        [Tooltip("The maximum distance to cast the Ray.")]
        [SerializeField]
        private float maximumDistance = 50f;
        /// <summary>
        /// The maximum distance to cast the <see cref="Ray"/>.
        /// </summary>
        public float MaximumDistance
        {
            get
            {
                return maximumDistance;
            }
            set
            {
                maximumDistance = value;
            }
        }
        [Tooltip("The surface will only be located if the previous position has changed from the current position.")]
        [SerializeField]
        private bool mustChangePosition = true;
        /// <summary>
        /// The surface will only be located if the previous position has changed from the current position.
        /// </summary>
        public bool MustChangePosition
        {
            get
            {
                return mustChangePosition;
            }
            set
            {
                mustChangePosition = value;
            }
        }
        [Tooltip("The threshold difference between the previous point value and the current point value to be considered equal.")]
        [SerializeField]
        private float positionChangedEqualityThreshold = 0.0001f;
        /// <summary>
        /// The threshold difference between the previous point value and the current point value to be considered equal.
        /// </summary>
        public float PositionChangedEqualityThreshold
        {
            get
            {
                return positionChangedEqualityThreshold;
            }
            set
            {
                positionChangedEqualityThreshold = value;
            }
        }
        [Tooltip("The amount to offset the position of the destination point found on the located surface.")]
        [SerializeField]
        private Vector3 destinationOffset = Vector3.zero;
        /// <summary>
        /// The amount to offset the position of the destination point found on the located surface.
        /// </summary>
        public Vector3 DestinationOffset
        {
            get
            {
                return destinationOffset;
            }
            set
            {
                destinationOffset = value;
            }
        }
        #endregion

        #region Restriction Settings
        [Header("Restriction Settings")]
        [Tooltip("An optional RuleContainer to determine valid and invalid targets based on the set rules.")]
        [SerializeField]
        private RuleContainer targetValidity;
        /// <summary>
        /// An optional <see cref="RuleContainer"/> to determine valid and invalid targets based on the set rules.
        /// </summary>
        public RuleContainer TargetValidity
        {
            get
            {
                return targetValidity;
            }
            set
            {
                targetValidity = value;
            }
        }
        [Tooltip("An optional RuleContainer to determine specific target point based on the set rules.")]
        [SerializeField]
        private RuleContainer targetPointValidity;
        /// <summary>
        /// An optional <see cref="RuleContainer"/> to determine specific target point based on the set rules.
        /// </summary>
        public RuleContainer TargetPointValidity
        {
            get
            {
                return targetPointValidity;
            }
            set
            {
                targetPointValidity = value;
            }
        }
        [Tooltip("An optional RuleContainer to determine if the search for a valid surface should be terminated if the current found target matches the rule.")]
        [SerializeField]
        private RuleContainer locatorTermination;
        /// <summary>
        /// An optional <see cref="RuleContainer"/> to determine if the search for a valid surface should be terminated if the current found target matches the rule.
        /// </summary>
        public RuleContainer LocatorTermination
        {
            get
            {
                return locatorTermination;
            }
            set
            {
                locatorTermination = value;
            }
        }
        [Tooltip("An optional custom Cast.PhysicsCast object to affect the Ray.")]
        [SerializeField]
        private PhysicsCast physicsCast;
        /// <summary>
        /// An optional custom <see cref="Cast.PhysicsCast"/> object to affect the <see cref="Ray"/>.
        /// </summary>
        public PhysicsCast PhysicsCast
        {
            get
            {
                return physicsCast;
            }
            set
            {
                physicsCast = value;
            }
        }
        #endregion

        #region Location Events
        /// <summary>
        /// Emitted when a new surface is located.
        /// </summary>
        [Header("Location Events")]
        public UnityEvent SurfaceLocated = new UnityEvent();
        #endregion

        /// <summary>
        /// The located surface.
        /// </summary>
        public readonly SurfaceData surfaceData = new SurfaceData();

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
        /// Clears <see cref="SearchOrigin"/>.
        /// </summary>
        public virtual void ClearSearchOrigin()
        {
            if (!this.IsValidState())
            {
                return;
            }

            SearchOrigin = default;
        }

        /// <summary>
        /// Clears <see cref="TargetValidity"/>.
        /// </summary>
        public virtual void ClearTargetValidity()
        {
            if (!this.IsValidState())
            {
                return;
            }

            TargetValidity = default;
        }

        /// <summary>
        /// Clears <see cref="TargetPointValidity"/>.
        /// </summary>
        public virtual void ClearTargetPointValidity()
        {
            if (!this.IsValidState())
            {
                return;
            }

            TargetPointValidity = default;
        }

        /// <summary>
        /// Clears <see cref="LocatorTermination"/>.
        /// </summary>
        public virtual void ClearLocatorTermination()
        {
            if (!this.IsValidState())
            {
                return;
            }

            LocatorTermination = default;
        }

        /// <summary>
        /// Clears <see cref="PhysicsCast"/>.
        /// </summary>
        public virtual void ClearPhysicsCast()
        {
            if (!this.IsValidState())
            {
                return;
            }

            PhysicsCast = default;
        }

        /// <summary>
        /// Sets the <see cref="SearchDirection"/> x value.
        /// </summary>
        /// <param name="value">The value to set to.</param>
        public virtual void SetSearchDirectionX(float value)
        {
            SearchDirection = new Vector3(value, SearchDirection.y, SearchDirection.z);
        }

        /// <summary>
        /// Sets the <see cref="SearchDirection"/> y value.
        /// </summary>
        /// <param name="value">The value to set to.</param>
        public virtual void SetSearchDirectionY(float value)
        {
            SearchDirection = new Vector3(SearchDirection.x, value, SearchDirection.z);
        }

        /// <summary>
        /// Sets the <see cref="SearchDirection"/> z value.
        /// </summary>
        /// <param name="value">The value to set to.</param>
        public virtual void SetSearchDirectionZ(float value)
        {
            SearchDirection = new Vector3(SearchDirection.x, SearchDirection.y, value);
        }

        /// <summary>
        /// Sets the <see cref="DestinationOffset"/> x value.
        /// </summary>
        /// <param name="value">The value to set to.</param>
        public virtual void SetDestinationOffsetX(float value)
        {
            DestinationOffset = new Vector3(value, DestinationOffset.y, DestinationOffset.z);
        }

        /// <summary>
        /// Sets the <see cref="DestinationOffset"/> y value.
        /// </summary>
        /// <param name="value">The value to set to.</param>
        public virtual void SetDestinationOffsetY(float value)
        {
            DestinationOffset = new Vector3(DestinationOffset.x, value, DestinationOffset.z);
        }

        /// <summary>
        /// Sets the <see cref="DestinationOffset"/> z value.
        /// </summary>
        /// <param name="value">The value to set to.</param>
        public virtual void SetDestinationOffsetZ(float value)
        {
            DestinationOffset = new Vector3(DestinationOffset.x, DestinationOffset.y, value);
        }

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
        public virtual void Locate(TransformData givenOrigin)
        {
            if (!this.IsValidState() || givenOrigin == null || !givenOrigin.IsValid)
            {
                return;
            }

            if (CastRay(givenOrigin.Position, SearchDirection) && (!MustChangePosition || PositionChanged(PositionChangedEqualityThreshold)))
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
            givenOrigin += givenDirection.normalized * OriginOffset;
            surfaceData.Origin = givenOrigin;
            surfaceData.Direction = givenDirection;
            Ray tracerRaycast = new Ray(givenOrigin, givenDirection);
            return TargetValidity?.Interface == null && TargetPointValidity?.Interface == null ?
                FindFirstCollision(tracerRaycast) :
                FindAllCollisions(tracerRaycast);
        }

        /// <summary>
        /// Casts a ray to find the first collision.
        /// </summary>
        /// <param name="tracerRaycast">The ray to cast with.</param>
        /// <returns><see langword="true"/> if a valid surface is located.</returns>
        protected virtual bool FindFirstCollision(Ray tracerRaycast)
        {
            if (PhysicsCast.Raycast(PhysicsCast, tracerRaycast, out RaycastHit collision, MaximumDistance, Physics.IgnoreRaycastLayer) &&
                (LocatorTermination?.Interface == null || !CollisionMatchesRule(collision, LocatorTermination)))
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
                if (LocatorTermination?.Interface != null &&
                    CollisionMatchesRule(collision, LocatorTermination))
                {
                    break;
                }

                if (CollisionMatchesRule(collision, TargetValidity) && TargetPointValidity.Accepts(collision.point))
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
            surfaceData.PositionalOffset = DestinationOffset;
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