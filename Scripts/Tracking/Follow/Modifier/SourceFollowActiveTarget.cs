namespace VRTK.Core.Tracking.Follow.Modifier
{
    using UnityEngine;

    /// <summary>
    /// Forces the transformations of the source <see cref="Transform"/> to match those of the target <see cref="Transform"/> by applying the given <see cref="FollowModifier"/>.
    /// </summary>
    public class SourceFollowActiveTarget : FollowModifier
    {
        /// <summary>
        /// The <see cref="FollowModifier"/> to apply to the source <see cref="Transform"/> that is following the active target <see cref="Transform"/>.
        /// </summary>
        [Tooltip("The FollowModifier to apply to the source Transform that is following the active target Transform.")]
        public FollowModifier appliedModifier;

        /// <summary>
        /// Updates the source <see cref="Transform"/> position using the first active target <see cref="Transform"/>.
        /// </summary>
        /// <param name="source">The source <see cref="Transform"/> to modify.</param>
        /// <param name="target">The target <see cref="Transform"/> to utilize in the modification.</param>
        public override void UpdatePosition(Transform source, Transform target)
        {
            if (appliedModifier != null && isActiveAndEnabled)
            {
                appliedModifier.UpdatePosition(source, target);
                CachedSource = appliedModifier.CachedSource;
                CachedTarget = appliedModifier.CachedTarget;
            }
        }

        /// <summary>
        /// Updates the source <see cref="Transform"/> rotation using the first active target <see cref="Transform"/>.
        /// </summary>
        /// <param name="source">The source <see cref="Transform"/> to modify.</param>
        /// <param name="target">The target <see cref="Transform"/> to utilize in the modification.</param>
        public override void UpdateRotation(Transform source, Transform target)
        {
            if (appliedModifier != null && isActiveAndEnabled)
            {
                appliedModifier.UpdateRotation(source, target);
                CachedSource = appliedModifier.CachedSource;
                CachedTarget = appliedModifier.CachedTarget;
            }
        }

        /// <summary>
        /// Updates the source <see cref="Transform"/> scale using the first active target <see cref="Transform"/>.
        /// </summary>
        /// <param name="source">The source <see cref="Transform"/> to modify.</param>
        /// <param name="target">The target <see cref="Transform"/> to utilize in the modification.</param>
        public override void UpdateScale(Transform source, Transform target)
        {
            if (appliedModifier != null && isActiveAndEnabled)
            {
                appliedModifier.UpdateScale(source, target);
                CachedSource = appliedModifier.CachedSource;
                CachedTarget = appliedModifier.CachedTarget;
            }
        }

        protected virtual void Awake()
        {
            ProcessType = ProcessTarget.FirstActive;
        }
    }
}