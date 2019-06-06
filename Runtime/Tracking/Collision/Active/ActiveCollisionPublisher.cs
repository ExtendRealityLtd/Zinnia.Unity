namespace Zinnia.Tracking.Collision.Active
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using System.Collections.Generic;
    using Malimbe.MemberClearanceMethod;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.BehaviourStateRequirementMethod;
    using Zinnia.Extension;

    /// <summary>
    /// Publishes itself to all <see cref="ActiveCollisionConsumer"/> components found within the current <see cref="PayloadData.ActiveCollisions"/> collection.
    /// </summary>
    public class ActiveCollisionPublisher : MonoBehaviour
    {
        /// <summary>
        /// Holds data about a <see cref="ActiveCollisionPublisher"/> payload.
        /// </summary>
        [Serializable]
        public class PayloadData
        {
            /// <summary>
            /// The container of the source that is initiating the collision.
            /// </summary>
            [Serialized, Cleared]
            [field: DocumentedByXml]
            public GameObject SourceContainer { get; set; }

            /// <summary>
            /// The active collisions.
            /// </summary>
            public List<CollisionNotifier.EventData> ActiveCollisions
            {
                get;
                protected set;
            } = new List<CollisionNotifier.EventData>();

            /// <summary>
            /// The <see cref="GameObject"/> that this is residing on.
            /// </summary>
            public GameObject PublisherContainer { get; set; }
        }

        /// <summary>
        /// Defines the event for the output <see cref="PayloadData"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<PayloadData>
        {
        }

        /// <summary>
        /// The data to publish to any available consumers.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public PayloadData Payload { get; set; } = new PayloadData();

        /// <summary>
        /// Emitted the collision data is published.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent Published = new UnityEvent();

        /// <summary>
        /// A reused instance to use when looking up <see cref="ActiveCollisionConsumer"/> components.
        /// </summary>
        protected readonly List<ActiveCollisionConsumer> activeCollisionConsumers = new List<ActiveCollisionConsumer>();
        /// <summary>
        /// A reused instance to use when looking up the Active Collisions in a Payload.
        /// </summary>
        protected List<CollisionNotifier.EventData> activeCollisions = new List<CollisionNotifier.EventData>();

        /// <summary>
        /// Sets the active collisions by copying it from given <see cref="ActiveCollisionsContainer.EventData"/>.
        /// </summary>
        /// <param name="data">The data to copy from.</param>
        [RequiresBehaviourState]
        public virtual void SetActiveCollisions(ActiveCollisionsContainer.EventData data)
        {
            if (data == null || data.ActiveCollisions == null)
            {
                return;
            }

            Payload.ActiveCollisions.Clear();
            Payload.ActiveCollisions.AddRange(data.ActiveCollisions);
        }

        /// <summary>
        /// Sets the active collision data by copying it from given <see cref="PayloadData"/> as long as the component is active and enabled.
        /// </summary>
        /// <param name="payload">The data to copy from.</param>
        [RequiresBehaviourState]
        public virtual void SetActiveCollisions(PayloadData payload)
        {
            SetActiveCollisionsEvenWhenDisabled(payload);
        }

        /// <summary>
        /// Sets the active collision data by copying it from given <see cref="PayloadData"/> even if the component is disabled.
        /// </summary>
        /// <param name="payload">The data to copy from.</param>
        public virtual void SetActiveCollisionsEvenWhenDisabled(PayloadData payload)
        {
            if (payload == null)
            {
                return;
            }

            Payload.ActiveCollisions.Clear();
            Payload.ActiveCollisions.AddRange(payload.ActiveCollisions);
        }

        /// <summary>
        /// Clears the existing active collision data.
        /// </summary>
        public virtual void ClearActiveCollisions()
        {
            Payload.ActiveCollisions.Clear();
        }

        /// <summary>
        /// Publishes itself and the current collision to all <see cref="ActiveCollisionConsumer"/> components found on any of the active collisions as long as the component is active and enabled.
        /// </summary>
        [RequiresBehaviourState]
        public virtual void Publish()
        {
            ForcePublish();
        }

        /// <summary>
        /// Publishes itself and the current collision to all <see cref="ActiveCollisionConsumer"/> components found on any of the active collisions as long as the component.
        /// </summary>
        public virtual void ForcePublish()
        {
            Payload.PublisherContainer = gameObject;
            activeCollisions.Clear();
            activeCollisions.AddRange(Payload.ActiveCollisions);
            foreach (CollisionNotifier.EventData currentCollision in activeCollisions)
            {
                Transform reference = currentCollision.ColliderData.GetContainingTransform();
                foreach (ActiveCollisionConsumer consumer in GetConsumers(reference))
                {
                    if (consumer.Container == null || consumer.Container == reference.gameObject)
                    {
                        consumer.Consume(Payload, currentCollision);
                    }
                }
            }
            Published?.Invoke(Payload);
        }

        /// <summary>
        /// Gets a valid <see cref="ActiveCollisionConsumer"/> collection.
        /// </summary>
        /// <param name="reference">The reference to start searching for <see cref="ActiveCollisionConsumer"/> components in.</param>
        /// <returns>The obtained collection.</returns>
        protected virtual List<ActiveCollisionConsumer> GetConsumers(Transform reference)
        {
            if (reference == null || transform.IsChildOf(reference))
            {
                activeCollisionConsumers.Clear();
            }
            else
            {
                reference.GetComponentsInChildren(activeCollisionConsumers);
            }

            return activeCollisionConsumers;
        }
    }
}