namespace VRTK.Core.Tracking.Collision.ListProcess.Action
{
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Sets the <see cref="GameObject"/> components in the collision collection to either active or inactive.
    /// </summary>
    public class SetActiveStateListAction : BaseListAction
    {
        /// <summary>
        /// The <see cref="GameObject"/>s to affect.
        /// </summary>
        [Tooltip("The GameObjects to affect.")]
        public List<GameObject> targets;

        /// <summary>
        /// Activates or deactivaes each <see cref="GameObject"/> in the <see cref="targets"/> collection based on the collision process state.
        /// </summary>
        /// <param name="collisionList">The collision collection at the time the action was executed.</param>
        /// <param name="state">The state for the action.</param>
        public override void Execute(List<CollisionNotifier.EventData> collisionList, bool state)
        {
            targets.ForEach(target => target.SetActive(state));
        }
    }
}