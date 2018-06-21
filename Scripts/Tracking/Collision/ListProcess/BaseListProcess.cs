namespace VRTK.Core.Tracking.Collision.ListProcess
{
    using UnityEngine;
    using System.Linq;
    using System.Collections.Generic;
    using VRTK.Core.Tracking.Collision.ListProcess.Action;
    using VRTK.Core.Extension;

    /// <summary>
    /// The base for providing a processor to a <see cref="CollisionList"/>.
    /// </summary>
    public abstract class BaseListProcess : MonoBehaviour
    {
        /// <summary>
        /// Executes any <see cref="BaseListAction"/> components found on the collider.
        /// </summary>
        [Tooltip("Executes any List Action components found on the collider.")]
        public bool executeColliderActions = false;

        /// <summary>
        /// Emitted when the <see cref="CollisionList"/> collection has been processed.
        /// </summary>
        public CollisionList.UnityEvent Processed = new CollisionList.UnityEvent();

        /// <summary>
        /// Processes the given collision collection.
        /// </summary>
        /// <param name="collisionList">The collision collection to process.</param>
        public virtual void Process(List<CollisionNotifier.EventData> collisionList)
        {
            Processed?.Invoke(DoProcess(collisionList));
        }

        /// <summary>
        /// Processes the given collision collection using a specifically defined method.
        /// </summary>
        /// <param name="collisionList">The collision collection to process.</param>
        /// <returns>The processed collision collection.</returns>
        protected abstract List<CollisionNotifier.EventData> DoProcess(List<CollisionNotifier.EventData> collisionList);

        /// <summary>
        /// Returns a collection of actions on the given collider that can be executed.
        /// </summary>
        /// <param name="collisionData">The current collider to check for actions on.</param>
        /// <returns>A collection of actions.</returns>
        protected virtual List<BaseListAction> GetListActions(CollisionNotifier.EventData collisionData)
        {
            return collisionData.collider.GetContainingTransform().GetComponents<BaseListAction>().ToList();
        }

        /// <summary>
        /// Attempts to execute all found actions on the current collider if the <see cref="executeColliderActions"/> is <see langword="true"/>.
        /// </summary>
        /// <param name="collisionList">The complete collision collection.</param>
        /// <param name="currentCollider">The current collider to search for actions on.</param>
        /// <param name="state">The state to pass to the executed action.</param>
        protected virtual void ExecuteCollisionActions(List<CollisionNotifier.EventData> collisionList, CollisionNotifier.EventData currentCollider, bool state)
        {
            if (executeColliderActions)
            {
                GetListActions(currentCollider).ForEach(action => action.Execute(collisionList, state));
            }
        }
    }
}