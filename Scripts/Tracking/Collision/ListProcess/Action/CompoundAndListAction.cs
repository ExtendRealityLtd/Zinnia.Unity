namespace VRTK.Core.Tracking.Collision.ListProcess.Action
{
    using UnityEngine;
    using System.Collections.Generic;

    /// <summary>
    /// Allows multiple <see cref="BaseListAction"/> types to be executed sequentially.
    /// </summary>
    public class CompoundAndListAction : BaseListAction
    {
        /// <summary>
        /// A collection of actions to execute.
        /// </summary>
        [Tooltip("A collection of actions to execute.")]
        public List<BaseListAction> actions = new List<BaseListAction>();

        /// <summary>
        /// Executes every action in the collection based on the collision process.
        /// </summary>
        /// <param name="collisionList">The collision collection at the time the action was executed.</param>
        /// <param name="state">The state for the action.</param>
        public override void Execute(List<CollisionNotifier.EventData> collisionList, bool state)
        {
            actions.ForEach(action => action.Execute(collisionList, state));
        }
    }
}