namespace VRTK.Core.Tracking
{
    using UnityEngine;
    using VRTK.Core.Process;

    /// <summary>
    /// The GameObjectStateMirror mirrors the active state of the source GameObject across all of the given target GameObjects.
    /// </summary>
    public class GameObjectStateMirror : SourceTargetProcessor
    {
        /// <summary>
        /// The Process method executes the relevant process.
        /// </summary>
        public override void Process()
        {
            ProcessAllComponents();
        }

        /// <summary>
        /// The ProcessComponent method sets all of the target GameObjects states to match the source GameObject state.
        /// </summary>
        /// <param name="source">The source Component that is a Transform.</param>
        /// <param name="target">The target Component that is a Transform.</param>
        protected override void ProcessComponent(Component source, Component target)
        {
            target.gameObject.SetActive(source.gameObject.activeInHierarchy);
        }
    }
}