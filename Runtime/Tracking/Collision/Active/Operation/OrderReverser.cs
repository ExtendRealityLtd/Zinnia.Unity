namespace Zinnia.Tracking.Collision.Active.Operation
{
    using Malimbe.XmlDocumentationAttribute;
    using UnityEngine;

    /// <summary>
    /// Reverses the order of the given collision collection.
    /// </summary>
    public class OrderReverser : MonoBehaviour
    {
        /// <summary>
        /// Emitted when the collection is reversed.
        /// </summary>
        [DocumentedByXml]
        public ActiveCollisionsContainer.ActiveCollisionUnityEvent Reversed = new ActiveCollisionsContainer.ActiveCollisionUnityEvent();

        protected readonly ActiveCollisionsContainer.EventData reversedList = new ActiveCollisionsContainer.EventData();

        /// <summary>
        /// Reverses the given collision collection.
        /// </summary>
        /// <param name="originalList">The original collision collection.</param>
        public virtual void DoReverse(ActiveCollisionsContainer.EventData originalList)
        {
            Reverse(originalList);
        }

        /// <summary>
        /// Reverses the given collision collection.
        /// </summary>
        /// <param name="originalList">The original collision collection.</param>
        /// <returns>The reversed collision collection.</returns>
        public virtual ActiveCollisionsContainer.EventData Reverse(ActiveCollisionsContainer.EventData originalList)
        {
            if (!isActiveAndEnabled)
            {
                return originalList;
            }

            reversedList.Set(originalList);
            reversedList.ActiveCollisions.Reverse();

            Reversed?.Invoke(reversedList);
            return reversedList;
        }
    }
}