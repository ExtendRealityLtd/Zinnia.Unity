namespace Zinnia.Tracking.Modification
{
    using UnityEngine;
    using Zinnia.Process.Component;

    /// <summary>
    /// Mirrors the <see cref="GameObject.activeInHierarchy"/> state of the source <see cref="GameObject"/> across all of the given target <see cref="GameObject"/>s.
    /// </summary>
    public class GameObjectStateMirror : GameObjectSourceTargetProcessor
    {
        /// <summary>
        /// Applies the source <see cref="GameObject.activeInHierarchy"/> state to the target state.
        /// </summary>
        /// <param name="source">The source to take the active state from.</param>
        /// <param name="target">The target to apply the active state to.</param>
        protected override void ApplySourceToTarget(GameObject source, GameObject target)
        {
            target.gameObject.SetActive(source.gameObject.activeInHierarchy);
        }
    }
}