namespace Zinnia.Tracking.Follow.Operation.Extraction
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using Zinnia.Data.Operation.Extraction;

    /// <summary>
    /// Extracts the <see cref="GameObject"/> of the EventTargetOffset from a given <see cref="ObjectFollower.EventData"/>.
    /// </summary>
    public class ObjectFollowerEventDataTargetOffsetExtractor : GameObjectExtractor<ObjectFollower.EventData, ObjectFollowerEventDataTargetOffsetExtractor.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the specified <see cref="GameObject"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<GameObject> { }

        /// <inheritdoc />
        protected override GameObject ExtractValue()
        {
            return Source != null ? Source.EventTargetOffset : null;
        }
    }
}
