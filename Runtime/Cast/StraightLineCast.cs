namespace Zinnia.Cast
{
    using System.Collections.Generic;
    using UnityEngine;
    using Zinnia.Extension;
    using Zinnia.Utility;

    /// <summary>
    /// Casts a straight line and creates points at the origin and target.
    /// </summary>
    public class StraightLineCast : PointsCast
    {
        [Header("Line Settings")]
        [Tooltip("The maximum length to cast.")]
        [SerializeField]
        private float maximumLength = 100f;
        /// <summary>
        /// The maximum length to cast.
        /// </summary>
        public float MaximumLength
        {
            get
            {
                return maximumLength;
            }
            set
            {
                maximumLength = value;
                if (this.IsMemberChangeAllowed())
                {
                    OnAfterMaximumLengthChange();
                }
            }
        }
        [Header("Fixed Settings")]
        [Tooltip("Whether to fix the length of the pointer to the specified FixedLength.")]
        [SerializeField]
        private bool shouldFixLength;
        /// <summary>
        /// Whether to fix the length of the pointer to the specified <see cref="FixedLength"/>.
        /// </summary>
        public bool ShouldFixLength
        {
            get
            {
                return shouldFixLength;
            }
            set
            {
                shouldFixLength = value;
            }
        }
        [Tooltip("The fixed length of the cast.")]
        [SerializeField]
        private float fixedLength = 1f;
        /// <summary>
        /// The fixed length of the cast.
        /// </summary>
        public float FixedLength
        {
            get
            {
                return fixedLength;
            }
            set
            {
                fixedLength = value;
                if (this.IsMemberChangeAllowed())
                {
                    OnAfterFixedLengthChange();
                }
            }
        }
        [Tooltip("Whether the fixed length cast should find and update the current target.")]
        [SerializeField]
        private bool shouldFixedFindTarget;
        /// <summary>
        /// Whether the fixed length cast should find and update the current target.
        /// </summary>
        public bool ShouldFixedFindTarget
        {
            get
            {
                return shouldFixedFindTarget;
            }
            set
            {
                shouldFixedFindTarget = value;
            }
        }

        [Header("Drag Settings")]
        [Tooltip("The number of segments to generate when creating a curved drag effect on the line.")]
        [SerializeField]
        private int dragEffectDensity;
        /// <summary>
        /// The number of segments to generate when creating a curved drag effect on the line.
        /// </summary>
        public int DragEffectDensity
        {
            get
            {
                return dragEffectDensity;
            }
            set
            {
                dragEffectDensity = value;
                if (this.IsMemberChangeAllowed())
                {
                    OnAfterIsDragEffectDensity();
                }
            }
        }
        [Tooltip("The amount of height offset to apply to the curved drag line.")]
        [SerializeField]
        private float dragCurveOffset;
        /// <summary>
        /// The amount of height offset to apply to the curved drag line.
        /// </summary>
        public float DragCurveOffset
        {
            get
            {
                return dragCurveOffset;
            }
            set
            {
                dragCurveOffset = value;
            }
        }

        /// <summary>
        /// Whether the caster is using the drag effect.
        /// </summary>
        public virtual bool UsingDragEffect => DragEffectDensity + dragEffectDensityOffset >= 4f;

        /// <summary>
        /// The offset to apply to the given <see cref="DragEffectDensity"/>
        /// </summary>
        protected const int dragEffectDensityOffset = 3;
        /// <summary>
        /// The additional threshold to apply to the fixed length ray to ensure it touches its target.
        /// </summary>
        protected const float fixedLengthThreshold = 0.001f;
        /// <summary>
        /// A reusable collection of <see cref="Vector3"/>s.
        /// </summary>
        protected readonly List<Vector3> curvePoints = new List<Vector3>();

        /// <summary>
        /// Sets the fixed length of the cast from the given event data.
        /// </summary>
        /// <param name="data">The data to extract the new fixed length from.</param>
        public virtual void SetFixedLength(EventData data)
        {
            TargetHit = data?.HitData;
            if (data?.Points.Count >= 2)
            {
                FixedLength = Vector3.Distance(data.Points[0], data.Points[data.Points.Count - 1]);
            }
        }

        /// <summary>
        /// Increments the fixed length of the cast by the given value.
        /// </summary>
        /// <param name="value">The value to increment the length by.</param>
        public virtual void IncrementFixedLength(float value)
        {
            if (!this.IsValidState())
            {
                return;
            }

            FixedLength += value;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            SetupPoints();
        }

        /// <inheritdoc />
        protected override void DoCastPoints()
        {
            GeneratePoints();
            ResultsChanged?.Invoke(eventData.Set(TargetHit, IsTargetHitValid, Points));
        }

        /// <summary>
        /// Sets the default number of points for the caster.
        /// </summary>
        protected virtual void SetupPoints()
        {
            points.Clear();
            for (int i = 0; i < (UsingDragEffect ? 4 : 2); i++)
            {
                points.Add(default);
            }
        }

        /// <summary>
        /// Generates the points for the cast.
        /// </summary>
        protected virtual void GeneratePoints()
        {
            Vector3 originPosition = Origin.transform.position;
            Vector3 originForward = Origin.transform.forward;

            if (UsingDragEffect)
            {
                GenerateDraggedPoints(originPosition, originForward);
            }
            else
            {
                GenerateStraightPoints(originPosition, originForward);
            }
        }

        /// <summary>
        /// Generates points based down a simple straight line.
        /// </summary>
        /// <param name="originPosition">The position of the line origin.</param>
        /// <param name="originForward">The forward direction from the origin.</param>
        protected virtual void GenerateStraightPoints(Vector3 originPosition, Vector3 originForward)
        {
            if (points.Count < 2)
            {
                return;
            }

            points[0] = originPosition;
            points[1] = DestinationPointOverride != null ? (Vector3)DestinationPointOverride : GetDestination(originPosition, originForward, out _);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="originPosition"></param>
        /// <param name="originForward"></param>
        protected virtual void GenerateDraggedPoints(Vector3 originPosition, Vector3 originForward)
        {
            if (points.Count < 4)
            {
                return;
            }

            Vector3 destinationPoint = GetDestination(originPosition, originForward, out Vector3 midPoint);
            GeneratePoints(originPosition, midPoint, DestinationPointOverride != null ? (Vector3)DestinationPointOverride : destinationPoint);
        }

        /// <summary>
        /// Gets the destination position of the cast based on whether its a fixed or varied length.
        /// </summary>
        /// <param name="originPosition">The origin of the cast.</param>
        /// <param name="originForward">The forward direction of the origin.</param>
        /// <param name="midPoint">The generated mid point of the cast.</param>
        /// <returns>The destination position of the cast.</returns>
        protected virtual Vector3 GetDestination(Vector3 originPosition, Vector3 originForward, out Vector3 midPoint)
        {
            Vector3 actualForward = GetTrackedForward(Origin.transform.forward);
            Vector3 destinationPoint = ShouldFixLength ? GetFixedDestination(originPosition, actualForward) : GetRayDestination(originPosition, actualForward);
            midPoint = originPosition + originForward * Vector3.Distance(originPosition, destinationPoint);
            return destinationPoint;
        }

        /// <summary>
        /// Gets the destination position based on a fixed length cast.
        /// </summary>
        /// <param name="originPosition">The origin of the cast.</param>
        /// <param name="originForward">The forward direction of the origin.</param>
        /// <returns>The destination position of the cast.</returns>
        protected virtual Vector3 GetFixedDestination(Vector3 originPosition, Vector3 originForward)
        {
            if (ShouldFixedFindTarget)
            {
                GetTargetHit(originPosition, originForward, FixedLength + fixedLengthThreshold, out bool _);
            }

            Vector3 destinationPoint = originPosition + originForward * FixedLength;
            return destinationPoint;
        }

        /// <summary>
        /// Gets the destination position based on the target of a RayCast.
        /// </summary>
        /// <param name="originPosition">The origin of the cast.</param>
        /// <param name="originForward">The forward direction of the origin.</param>
        /// <returns>The destination position of the cast.</returns>
        protected virtual Vector3 GetRayDestination(Vector3 originPosition, Vector3 originForward)
        {
            RaycastHit actualHitData = GetTargetHit(originPosition, originForward, MaximumLength, out bool hasCollided);
            Vector3 destinationPoint = hasCollided ? actualHitData.point : originPosition + originForward * MaximumLength;

            return destinationPoint;
        }

        /// <summary>
        /// Gets the hit target data.
        /// </summary>
        /// <param name="originPosition">The origin of the cast.</param>
        /// <param name="originForward">The forward direction of the origin.</param>
        /// <param name="maxLength">The max length of the cast.</param>
        /// <param name="hasCollided">Whether the ray has collided.</param>
        /// <returns>The target hit data.</returns>
        protected virtual RaycastHit GetTargetHit(Vector3 originPosition, Vector3 originForward, float maxLength, out bool hasCollided)
        {
            Ray forwardCast = new Ray(originPosition, originForward);
            hasCollided = PhysicsCast.Raycast(PhysicsCast, forwardCast, out RaycastHit actualHitData, maxLength, Physics.IgnoreRaycastLayer);

            return GetActualTargetHit(actualHitData, hasCollided);
        }

        /// <summary>
        /// Generates a curved set of points based on the given origin, mid and destination point.
        /// </summary>
        /// <param name="originPoint">The origin point of the line.</param>
        /// <param name="midPoint">The mid point of the line.</param>
        /// <param name="destinationPoint">The destination point of the line.</param>
        protected virtual void GeneratePoints(Vector3 originPoint, Vector3 midPoint, Vector3 destinationPoint)
        {
            curvePoints.Clear();
            curvePoints.Add(originPoint);
            curvePoints.Add(midPoint + (Vector3.back * DragCurveOffset));
            curvePoints.Add(destinationPoint);
            curvePoints.Add(destinationPoint);

            points.Clear();
            foreach (Vector3 generatedPoint in BezierCurveGenerator.GeneratePoints(DragEffectDensity + dragEffectDensityOffset, curvePoints))
            {
                points.Add(generatedPoint);
            }
        }

        /// <summary>
        /// Retrieves <see cref="FixedLength"/> clamped between `0f` and <see cref="MaximumLength"/>.
        /// </summary>
        /// <returns>The clamped value.</returns>
        protected virtual float GetClampedFixedLength()
        {
            return Mathf.Clamp(FixedLength, 0f, MaximumLength);
        }

        /// <summary>
        /// Called after <see cref="MaximumLength"/> has been changed.
        /// </summary>
        protected virtual void OnAfterMaximumLengthChange()
        {
            fixedLength = GetClampedFixedLength();
        }

        /// <summary>
        /// Called after <see cref="FixedLength"/> has been changed.
        /// </summary>
        protected virtual void OnAfterFixedLengthChange()
        {
            fixedLength = GetClampedFixedLength();
        }

        /// <summary>
        /// Called after <see cref="DragEffectDensity"/> has been changed.
        /// </summary>
        protected virtual void OnAfterIsDragEffectDensity()
        {
            SetupPoints();
        }
    }
}