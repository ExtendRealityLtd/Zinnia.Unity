namespace VRTK.Core.Tracking.Follow.Modifier
{
    using UnityEngine;

    /// <summary>
    /// Forces the transformations of all the <see cref="Transform"/> targets to match those of the source <see cref="Transform"/> by applying the given <see cref="FollowModifier"/>.
    /// </summary>
    public class TargetsFollowSource : FollowModifier
    {
        /// <summary>
        /// The <see cref="FollowModifier"/> to apply to the <see cref="Transform"/> targets that are following the source <see cref="Transform"/>.
        /// </summary>
        [Tooltip("The FollowModifier to apply to the Transform targets that are following the source Transform.")]
        public FollowModifier appliedModifier;

        /// <summary>
        /// Updates the target <see cref="Transform"/> position using the source <see cref="Transform"/>.
        /// </summary>
        /// <param name="source">The source to utilize in the modification.</param>
        /// <param name="target">The target to modify.</param>
        /// <param name="offset">The offset of the target against the source when modifying.</param>
        protected override void DoUpdatePosition(Transform source, Transform target, Transform offset = null)
        {
            if (appliedModifier != null)
            {
                appliedModifier.UpdatePosition(source, target, offset);
            }
        }

        /// <summary>
        /// Updates the target <see cref="Transform"/> rotation using the source <see cref="Transform"/>.
        /// </summary>
        /// <param name="source">The source to utilize in the modification.</param>
        /// <param name="target">The target to modify.</param>
        /// <param name="offset">The offset of the target against the source when modifying.</param>
        protected override void DoUpdateRotation(Transform source, Transform target, Transform offset = null)
        {
            if (appliedModifier != null)
            {
                appliedModifier.UpdateRotation(source, target, offset);
            }
        }

        /// <summary>
        /// Updates the target <see cref="Transform"/> scale using the source <see cref="Transform"/>.
        /// </summary>
        /// <param name="source">The source to utilize in the modification.</param>
        /// <param name="target">The target to modify.</param>
        /// <param name="offset">The offset of the target against the source when modifying.</param>
        protected override void DoUpdateScale(Transform source, Transform target, Transform offset = null)
        {
            if (appliedModifier != null)
            {
                appliedModifier.UpdateScale(source, target, offset);
            }
        }

        protected virtual void Awake()
        {
            ProcessType = ProcessTarget.All;
        }
    }
}