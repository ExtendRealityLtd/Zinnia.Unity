namespace Zinnia.Cast
{
    using UnityEngine;
    using Zinnia.Extension;

    /// <summary>
    /// Casts a straight line and creates points at the origin and target.
    /// </summary>
    public class StraightLineCast : PointsCast
    {
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

        protected override void OnEnable()
        {
            base.OnEnable();
            points.Add(Vector3.zero);
            points.Add(Vector3.zero);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            points.Clear();
        }

        /// <inheritdoc />
        protected override void DoCastPoints()
        {
            GeneratePoints();
            ResultsChanged?.Invoke(eventData.Set(TargetHit, IsTargetHitValid, Points));
        }

        /// <summary>
        /// Generates the points for the cast.
        /// </summary>
        protected virtual void GeneratePoints()
        {
            Vector3 originPosition = Origin.transform.position;
            Vector3 originForward = Origin.transform.forward;

            Ray ray = new Ray(originPosition, originForward);
            bool hasCollided = PhysicsCast.Raycast(PhysicsCast, ray, out RaycastHit hitData, MaximumLength, Physics.IgnoreRaycastLayer);
            TargetHit = hasCollided ? hitData : (RaycastHit?)null;

            Vector3 destinationPosition = hasCollided ? hitData.point : originPosition + originForward * MaximumLength;

            if (points.Count >= 2)
            {
                points[0] = originPosition;
                points[1] = DestinationPointOverride != null ? (Vector3)DestinationPointOverride : destinationPosition;
            }
        }

        /// <summary>
        /// Called after <see cref="MaximumLength"/> has been changed.
        /// </summary>
        protected virtual void OnAfterMaximumLengthChange() { }
    }
}