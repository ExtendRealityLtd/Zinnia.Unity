namespace Zinnia.Tracking.Follow.Operation.Extraction
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using Zinnia.Data.Operation.Extraction;

    /// <summary>
    /// Extracts the <see cref="GameObject"/> of the EventSource from a given <see cref="ObjectFollower.EventData"/>.
    /// </summary>
    public class ObjectFollowerEventDataSourceExtractor : GameObjectExtractor<ObjectFollower.EventData, ObjectFollowerEventDataSourceExtractor.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the specified <see cref="GameObject"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<GameObject> { }

        /// <inheritdoc />
        protected override GameObject ExtractValue()
        {
            return Source != null ? Source.EventSource : null;
        }
    }
}
