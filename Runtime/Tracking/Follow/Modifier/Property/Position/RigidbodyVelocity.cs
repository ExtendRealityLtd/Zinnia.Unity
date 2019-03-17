namespace Zinnia.Tracking.Follow.Modifier.Property.Position
{
    using UnityEngine;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;
    using Zinnia.Extension;

    /// <summary>
    /// Updates the rigidbody velocity by moving towards a given source.
    /// </summary>
    public class RigidbodyVelocity : PropertyModifier
    {
        /// <summary>
        /// The maximum squared magnitude of velocity that can be applied to the source.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public float VelocityLimit { get; set; } = float.PositiveInfinity;
        /// <summary>
        /// The maximum difference in distance to the tracked position.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public float MaxDistanceDelta { get; set; } = 10f;

        /// <summary>
        /// A cached version of the target rigidbody.
        /// </summary>
        protected Rigidbody cachedTargetRigidbody;
        /// <summary>
        /// A cached version of the target.
        /// </summary>
        protected GameObject cachedTarget;

        /// <summary>
        /// Modifies the target <see cref="Rigidbody"/> velocity to move towards the given source.
        /// </summary>
        /// <param name="source">The source to utilize in the modification.</param>
        /// <param name="target">The target to modify.</param>
        /// <param name="offset">The offset of the target against the source when modifying.</param>
        protected override void DoModify(GameObject source, GameObject target, GameObject offset = null)
        {
            cachedTargetRigidbody = cachedTargetRigidbody == null || target != cachedTarget ? target.TryGetComponent<Rigidbody>(true) : cachedTargetRigidbody;
            cachedTarget = target;

            Vector3 positionDelta = source.transform.position - (offset != null ? offset.transform.position : target.transform.position);
            Vector3 velocityTarget = positionDelta / Time.deltaTime;
            Vector3 calculatedVelocity = Vector3.MoveTowards(cachedTargetRigidbody.velocity, velocityTarget, MaxDistanceDelta);

            if (calculatedVelocity.sqrMagnitude < VelocityLimit)
            {
                cachedTargetRigidbody.velocity = calculatedVelocity;
            }
        }
    }
}