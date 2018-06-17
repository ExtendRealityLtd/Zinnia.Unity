namespace VRTK.Core.Tracking.Follow.Modifier
{
    using UnityEngine;

    /// <summary>
    /// Modifies the source <see cref="Transform"/> data to equal the target <see cref="Transform"/> data.
    /// </summary>
    public class ModifyTransform : FollowModifier
    {
        /// <summary>
        /// Sets the source <see cref="Transform.position"/> to the target <see cref="Transform.position"/>.
        /// </summary>
        /// <param name="source">The source <see cref="Transform"/> to modify.</param>
        /// <param name="target">The target <see cref="Transform"/> to utilize in the modification.</param>
        protected override void DoUpdatePosition(Transform source, Transform target)
        {
            source.position = target.position;
        }

        /// <summary>
        /// Sets the source <see cref="Transform.rotation"/> to the target <see cref="Transform.rotation"/>.
        /// </summary>
        /// <param name="source">The source <see cref="Transform"/> to modify.</param>
        /// <param name="target">The target <see cref="Transform"/> to utilize in the modification.</param>
        protected override void DoUpdateRotation(Transform source, Transform target)
        {
            source.rotation = target.rotation;
        }

        /// <summary>
        /// Sets the source <see cref="Transform.localScale"/> to the target <see cref="Transform.localScale"/>.
        /// </summary>
        /// <param name="source">The source <see cref="Transform"/> to modify.</param>
        /// <param name="target">The target <see cref="Transform"/> to utilize in the modification.</param>
        protected override void DoUpdateScale(Transform source, Transform target)
        {
            source.localScale = target.localScale;
        }
    }
}