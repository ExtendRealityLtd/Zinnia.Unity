namespace VRTK.Core.Tracking.Follow.Modifier.Property.Position
{
    using UnityEngine;
    using VRTK.Core.Extension;

    /// <summary>
    /// Updates the rigidbody velocity by moving towards a given source.
    /// </summary>
    public class RigidbodyVelocity : PropertyModifier
    {
        /// <summary>
        /// The maximum squared magnitude of velocity that can be applied to the source.
        /// </summary>
        [Tooltip("The maximum squared magnitude of velocity that can be applied to the source Transform.")]
        public float velocityLimit = float.PositiveInfinity;
        /// <summary>
        /// The maximum difference in distance to the tracked position.
        /// </summary>
        [Tooltip("The maximum difference in distance to the tracked position.")]
        public float maxDistanceDelta = 10f;

        protected Rigidbody cachedTargetRigidbody;
        protected GameObject cachedTarget;

        /// <summary>
        /// Modifies the target <see cref="Rigidbody"/> velocity to move towards the given source.
        /// </summary>
        /// <param name="source">The source to utilize in the modification.</param>
        /// <param name="target">The target to modify.</param>
        /// <param name="offset">The offset of the target against the source when modifying.</param>
        protected override void DoModify(GameObject source, GameObject target, GameObject offset = null)
        {
            cachedTargetRigidbody = (cachedTargetRigidbody == null || target != cachedTarget ? target.FindRigidbody() : cachedTargetRigidbody);
            cachedTarget = target;

            Vector3 positionDelta = source.transform.position - (offset != null ? offset.transform.position : target.transform.position);
            Vector3 velocityTarget = positionDelta / Time.fixedDeltaTime;
            Vector3 calculatedVelocity = Vector3.MoveTowards(cachedTargetRigidbody.velocity, velocityTarget, maxDistanceDelta);

            if (calculatedVelocity.sqrMagnitude < velocityLimit)
            {
                cachedTargetRigidbody.velocity = calculatedVelocity;
            }
        }
    }
}