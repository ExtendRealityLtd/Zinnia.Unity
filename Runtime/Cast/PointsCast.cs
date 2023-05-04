namespace Zinnia.Cast
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;
    using Zinnia.Data.Type;
    using Zinnia.Extension;
    using Zinnia.Process;
    using Zinnia.Rule;

    /// <summary>
    /// The base of casting components that result in points along the cast.
    /// </summary>
    public abstract class PointsCast : MonoBehaviour, IProcessable
    {
        /// <summary>
        /// Holds data about a <see cref="PointsCast"/> event.
        /// </summary>
        [Serializable]
        public class EventData
        {
            [Tooltip("The result of the most recent cast. null when the cast didn't hit anything.")]
            [SerializeField]
            private RaycastHit? hitData;
            /// <summary>
            /// The result of the most recent cast. <see langword="null"/> when the cast didn't hit anything.
            /// </summary>
            public RaycastHit? HitData
            {
                get
                {
                    return hitData;
                }
                set
                {
                    hitData = value;
                }
            }

            [Tooltip("The validity of the most recent HitData based on the TargetValidity rule.")]
            [SerializeField]
            private bool isValid;
            /// <summary>
            /// The validity of the most recent <see cref="HitData"/> based on the <see cref="TargetValidity"/> rule.
            /// </summary>
            public bool IsValid
            {
                get
                {
                    return isValid;
                }
                set
                {
                    isValid = value;
                }
            }

            /// <summary>
            /// The points along the most recent cast.
            /// </summary>
            public HeapAllocationFreeReadOnlyList<Vector3> Points { get; set; }

            public EventData Set(EventData source)
            {
                return Set(source.HitData, source.IsValid, source.Points);
            }

            public EventData Set(RaycastHit? targetHit, bool isValid, HeapAllocationFreeReadOnlyList<Vector3> points)
            {
                HitData = targetHit;
                IsValid = isValid;
                Points = points;
                return this;
            }

            /// <inheritdoc />
            public override string ToString()
            {
                if (HitData == null)
                {
                    return "";
                }

                RaycastHit rayData = (RaycastHit)HitData;

                string[] titles = new string[]
                {
                "HitData",
                "IsValid"
                };

                object[] values = new object[]
                {
                rayData.ToFormattedString(),
                IsValid
                };

                return StringExtensions.FormatForToString(titles, values);
            }

            public void Clear()
            {
                Set(default, default, default);
            }
        }

        /// <summary>
        /// Defines the event with the <see cref="EventData"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<EventData> { }

        [Header("Cast Settings")]
        [Tooltip("The origin point for the cast.")]
        [SerializeField]
        private GameObject origin;
        /// <summary>
        /// The origin point for the cast.
        /// </summary>
        public GameObject Origin
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
        [Tooltip("Allows to optionally affect the cast.")]
        [SerializeField]
        private PhysicsCast physicsCast;
        /// <summary>
        /// Allows to optionally affect the cast.
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
        [Tooltip("Allows to optionally determine targets based on the set rules.")]
        [SerializeField]
        private RuleContainer targetValidity;
        /// <summary>
        /// Allows to optionally determine targets based on the set rules.
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
        [Tooltip("Allows to optionally determine specific target point based on the set rules.")]
        [SerializeField]
        private RuleContainer targetPointValidity;
        /// <summary>
        /// Allows to optionally determine specific target point based on the set rules.
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
        [Tooltip("The amount of distance the cursor has to move before the destination of the cursor is updated to a new position.")]
        [SerializeField]
        private float cursorLockThreshold;
        /// <summary>
        /// The amount of distance the cursor has to move before the destination of the cursor is updated to a new position.
        /// </summary>
        public float CursorLockThreshold
        {
            get
            {
                return cursorLockThreshold;
            }
            set
            {
                cursorLockThreshold = value;
            }
        }
        [Tooltip("The duration it takes to transition previous destination point to the current actual target point.")]
        [SerializeField]
        private float transitionDuration;
        /// <summary>
        /// The duration it takes to transition previous destination point to the current actual target point.
        /// </summary>
        public float TransitionDuration
        {
            get
            {
                return transitionDuration;
            }
            set
            {
                transitionDuration = value;
            }
        }

        /// <summary>
        /// An override for the destination location point in world space.
        /// </summary>
        public Vector3? DestinationPointOverride { get; set; }

        /// <summary>
        /// Emitted whenever the cast result changes.
        /// </summary>
        [Header("Cast Events")]
        public UnityEvent ResultsChanged = new UnityEvent();

        /// <summary>
        /// The result of the most recent cast. <see langword="null"/> when the cast didn't hit anything or an invalid target according to <see cref="TargetValidity"/> or <see cref="TargetPointValidity"/> rules.
        /// </summary>
        private RaycastHit? targetHit;
        public RaycastHit? TargetHit
        {
            get
            {
                return targetHit;
            }
            protected set
            {
                targetHit = value;
                if (this.IsMemberChangeAllowed())
                {
                    OnAfterTargetHitChange();
                }
            }
        }
        /// <summary>
        /// Whether the current <see cref="TargetHit"/> is valid based on the <see cref="TargetValidity"/> and <see cref="TargetPointValidity"/> rules.
        /// </summary>
        public bool IsTargetHitValid { get; protected set; }
        /// <summary>
        /// The points along the most recent cast.
        /// </summary>
        public virtual HeapAllocationFreeReadOnlyList<Vector3> Points => points;

        /// <summary>
        /// The points along the most recent cast.
        /// </summary>
        protected readonly List<Vector3> points = new List<Vector3>();
        /// <summary>
        /// The data to emit with an event.
        /// </summary>
        protected readonly EventData eventData = new EventData();
        /// <summary>
        /// The origin forward that is being tracked by the drag delay.
        /// </summary>
        protected Vector3? trackedOriginForward = null;
        /// <summary>
        /// The reference to the output velocity.
        /// </summary>
        protected Vector3 outVelocity = Vector3.zero;

        /// <summary>
        /// Clears <see cref="Origin"/>.
        /// </summary>
        public virtual void ClearOrigin()
        {
            if (!this.IsValidState())
            {
                return;
            }

            Origin = default;
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
        /// Clears the <see cref="DestinationPointOverride"/>.
        /// </summary>
        public virtual void ClearDestinationPointOverride()
        {
            DestinationPointOverride = null;
        }

        /// <summary>
        /// Casts and creates points along the cast.
        /// </summary>
        public virtual void CastPoints()
        {
            if (!this.IsValidState() || Origin == null)
            {
                return;
            }

            DoCastPoints();
        }

        /// <inheritdoc />
        public virtual void Process()
        {
            CastPoints();
        }

        /// <summary>
        /// Performs the implemented way of casting points.
        /// </summary>
        protected abstract void DoCastPoints();

        protected virtual void OnEnable()
        {
            ClearState();
            OnAfterTargetHitChange();
        }

        protected virtual void OnDisable()
        {
            ClearState();
            ClearDestinationPointOverride();
        }

        /// <summary>
        /// Clears the pointer state.
        /// </summary>
        protected virtual void ClearState()
        {
            TargetHit = null;
            trackedOriginForward = null;
            IsTargetHitValid = false;
            points.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orignForward"></param>
        protected virtual Vector3 GetTrackedForward(Vector3 orignForward)
        {
            trackedOriginForward = TransitionDuration.ApproxEquals(0f) || trackedOriginForward == null ? orignForward : Vector3.SmoothDamp((Vector3)trackedOriginForward, orignForward, ref outVelocity, TransitionDuration, Mathf.Infinity, Time.deltaTime);
            return (Vector3)trackedOriginForward;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actualHitData"></param>
        /// <param name="hasCollided"></param>
        /// <returns></returns>
        protected virtual RaycastHit GetActualTargetHit(RaycastHit actualHitData, bool hasCollided)
        {
            RaycastHit newTargetHit = actualHitData;
            if (CursorLockThreshold > 0f && TargetHit != null)
            {
                RaycastHit existingTargetHit = (RaycastHit)TargetHit;
                if (Vector3.Distance(actualHitData.point, existingTargetHit.point) <= CursorLockThreshold)
                {
                    newTargetHit = existingTargetHit;
                }
            }
            TargetHit = hasCollided ? newTargetHit : (RaycastHit?)null;
            return newTargetHit;
        }

        /// <summary>
        /// Called after <see cref="TargetHit"/> has been changed.
        /// </summary>
        protected virtual void OnAfterTargetHitChange()
        {
            IsTargetHitValid = TargetHit != null && TargetValidity.Accepts(TargetHit.Value.transform.gameObject) && TargetPointValidity.Accepts(TargetHit.Value.point);
        }
    }
}