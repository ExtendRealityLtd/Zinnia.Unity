namespace VRTK.Core.Tracking.Collision.Active.Operation
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;

    /// <summary>
    /// Extracts the forward source container from a given <see cref="CollisionNotifier.EventData"/>.
    /// </summary>
    public class NotifierContainerExtractor : MonoBehaviour
    {
        /// <summary>
        /// Defines the event with the forward source container <see cref="GameObject"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<GameObject>
        {
        }

        /// <summary>
        /// Emitted when the forward source container extracted from the notifier.
        /// </summary>
        public UnityEvent Extracted = new UnityEvent();

        /// <summary>
        /// The currently extracted forward source container.
        /// </summary>
        public GameObject ForwardSourceContainer
        {
            get;
            protected set;
        }

        /// <summary>
        /// Extracts the forward source container from the given notifier event data.
        /// </summary>
        /// <param name="notifier">The notifier event data to extract from.</param>
        /// <returns>The forward source container within the notifier.</returns>
        public virtual GameObject Extract(CollisionNotifier.EventData notifier)
        {
            if (!isActiveAndEnabled || notifier == null)
            {
                ForwardSourceContainer = null;
                return null;
            }

            ForwardSourceContainer = notifier.forwardSource?.gameObject;
            Extracted?.Invoke(ForwardSourceContainer);
            return ForwardSourceContainer;
        }

        /// <summary>
        /// Extracts the forward source container from the given notifier event data.
        /// </summary>
        /// <param name="notifier">The notifier event data to extract from.</param>
        public virtual void DoExtract(CollisionNotifier.EventData notifier)
        {
            Extract(notifier);
        }
    }
}