namespace VRTK.Core.Prefabs.Pointers
{
    using UnityEngine;
    using VRTK.Core.Extension;
    using VRTK.Core.Tracking.Follow;

    /// <summary>
    /// Provides internal settings for the pointer prefabs.
    /// </summary>
    public class PointerInternalSetup : MonoBehaviour
    {
        /// <summary>
        /// The <see cref="PointerUserSetup"/> for providing required follow target information.
        /// </summary>
        [Tooltip("The Pointer User Setup for providing required follow target information.")]
        public PointerUserSetup userSetup;
        /// <summary>
        /// The <see cref="ObjectFollow"/> component to set the target <see cref="Transform"/> on.
        /// </summary>
        [Tooltip("The Object Follow component to set the target Transform on.")]
        public ObjectFollow objectFollow;

        protected virtual void OnEnable()
        {
            if (objectFollow == null || userSetup == null || userSetup.followTarget == null)
            {
                return;
            }

            objectFollow.targetComponents.Clear();
            objectFollow.targetComponents.Add(userSetup.followTarget.TryGetComponent());
        }
    }
}