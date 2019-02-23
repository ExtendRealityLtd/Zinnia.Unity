namespace Zinnia.Cast
{
    using UnityEngine;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.PropertyValidationMethod;
    using Malimbe.XmlDocumentationAttribute;

    /// <summary>
    /// Casts a straight line and creates points at the origin and target.
    /// </summary>
    public class StraightLineCast : PointsCast
    {
        /// <summary>
        /// The maximum length to cast.
        /// </summary>
        [Serialized, Validated]
        [field: DocumentedByXml]
        public float MaximumLength { get; set; } = 100f;

        protected virtual void OnEnable()
        {
            points.Add(Vector3.zero);
            points.Add(Vector3.zero);
        }

        protected virtual void OnDisable()
        {
            points.Clear();
        }

        /// <inheritdoc />
        protected override void DoCastPoints()
        {
            GeneratePoints();
            ResultsChanged?.Invoke(eventData.Set(TargetHit, Points));
        }

        /// <summary>
        /// Generates the points for the cast.
        /// </summary>
        protected virtual void GeneratePoints()
        {
            Ray ray = new Ray(origin.transform.position, origin.transform.forward);
            RaycastHit hitData;
            bool hasCollided = PhysicsCast.Raycast(physicsCast, ray, out hitData, MaximumLength, Physics.IgnoreRaycastLayer);
            TargetHit = (hasCollided ? hitData : (RaycastHit?)null);

            points[0] = origin.transform.position;
            points[1] = (hasCollided ? hitData.point : origin.transform.position + origin.transform.forward * MaximumLength);
        }
    }
}