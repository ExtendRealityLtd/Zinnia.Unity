namespace VRTK.Core.Tracking.Collision.ListProcess.Action
{
    using UnityEngine;
    using System.Collections.Generic;

    /// <summary>
    /// The base for providing an action that can be executed from a give collision.
    /// </summary>
    public abstract class BaseListAction : MonoBehaviour
    {
        /// <summary>
        /// Executes an action on the given collision collection with the given state.
        /// </summary>
        /// <param name="collisionList">The collision collection at the time the action was executed.</param>
        /// <param name="state">The state for the action.</param>
        public abstract void Execute(List<CollisionNotifier.EventData> collisionList, bool state);
    }
}