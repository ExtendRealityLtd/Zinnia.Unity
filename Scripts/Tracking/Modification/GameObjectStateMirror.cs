namespace VRTK.Core.Tracking.Modification
{
    using UnityEngine;
    using VRTK.Core.Process.Component;

    /// <summary>
    /// Mirrors the <see cref="GameObject.activeInHierarchy"/> state of the source <see cref="GameObject"/> across all of the given target <see cref="GameObject"/>s.
    /// </summary>
    public class GameObjectStateMirror : SourceTargetProcessor
    {
        /// <summary>
        /// Executes the relevant process.
        /// </summary>
        public override void Process()
        {
            ProcessAllComponents();
        }

        /// <summary>
        /// Sets all of the target <see cref="GameObject"/>s states to match the source <see cref="GameObject"/> state.
        /// </summary>
        /// <param name="source">The source <see cref="Component"/> that is a <see cref="GameObject"/>.</param>
        /// <param name="target">The target <see cref="Component"/> that is a <see cref="GameObject"/>.</param>
        protected override void ProcessComponent(Component source, Component target)
        {
            target.gameObject.SetActive(source.gameObject.activeInHierarchy);
        }
    }
}