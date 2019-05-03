namespace Zinnia.Cast
{
    using UnityEngine;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;

    /// <summary>
    /// Casts a straight line and creates points at the origin and target.
    /// </summary>
    public class StraightLineCast : PointsCast
    {
        /// <summary>
        /// The maximum length to cast.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public float MaximumLength { get; set; } = 100f;

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

            points[0] = originPosition;
            points[1] = DestinationPointOverride != null ? (Vector3)DestinationPointOverride : destinationPosition;
        }
    }
}