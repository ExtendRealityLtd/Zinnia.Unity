namespace Zinnia.Tracking.Collision.Active
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using System.Collections.Generic;
    using Malimbe.BehaviourStateRequirementMethod;
    using Malimbe.XmlDocumentationAttribute;
    using Zinnia.Extension;

    /// <summary>
    /// Publishes itself to all <see cref="ActiveCollisionConsumer"/> components found within the current <see cref="PayloadData.ActiveCollisions"/> collection.
    /// </summary>
    public class ActiveCollisionPublisher : MonoBehaviour
    {
        /// <summary>
        /// Defines the event for the output <see cref="PayloadData"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<PayloadData>
        {
        }

        /// <summary>
        /// Holds data about a <see cref="ActiveCollisionPublisher"/> payload.
        /// </summary>
        [Serializable]
        public class PayloadData
        {
            /// <summary>
            /// The container of the source that is initiating the collision.
            /// </summary>
            [DocumentedByXml]
            public GameObject sourceContainer;

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
            public GameObject PublisherContainer
            {
                get;
                set;
            }
        }

        /// <summary>
        /// The data to publish to any available consumers.
        /// </summary>
        [DocumentedByXml]
        public PayloadData payload = new PayloadData();

        /// <summary>
        /// Emitted the collision data is published.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent Published = new UnityEvent();

        /// <summary>
        /// Sets the active collisions by copying it from given <see cref="ActiveCollisionsContainer.EventData"/>.
        /// </summary>
        /// <param name="data">The data to copy from.</param>
        [RequiresBehaviourState]
        public virtual void SetActiveCollisions(ActiveCollisionsContainer.EventData data)
        {
            if (data == null || data.activeCollisions == null)
            {
                return;
            }

            payload.ActiveCollisions.Clear();
            payload.ActiveCollisions.AddRange(data.activeCollisions);
        }

        /// <summary>
        /// Sets the active collision data by copying it from given <see cref="PayloadData"/> as long as the component is active and enabled.
        /// </summary>
        /// <param name="payload">The data to copy from.</param>
        [RequiresBehaviourState]
        public virtual void SetActiveCollisions(PayloadData payload)
        {
            ForceSetActiveCollisions(payload);
        }

        /// <summary>
        /// Sets the active collision data by copying it from given <see cref="PayloadData"/>.
        /// </summary>
        /// <param name="payload">The data to copy from.</param>
        public virtual void ForceSetActiveCollisions(PayloadData payload)
        {
            if (payload == null)
            {
                return;
            }

            this.payload.ActiveCollisions.Clear();
            this.payload.ActiveCollisions.AddRange(payload.ActiveCollisions);
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
            payload.PublisherContainer = gameObject;
            foreach (CollisionNotifier.EventData currentCollision in payload.ActiveCollisions)
            {
                Transform reference = currentCollision.collider.GetContainingTransform();
                foreach (ActiveCollisionConsumer consumer in GetConsumers(reference))
                {
                    if (consumer.container == null || consumer.container == reference.gameObject)
                    {
                        consumer.Consume(payload, currentCollision);
                    }
                }
            }
            Published?.Invoke(payload);
        }

        /// <summary>
        /// Gets a valid <see cref="ActiveCollisionConsumer"/> collection.
        /// </summary>
        /// <param name="reference">The reference to start searching for <see cref="ActiveCollisionConsumer"/> components in.</param>
        /// <returns>The obtained collection.</returns>
        protected virtual ActiveCollisionConsumer[] GetConsumers(Transform reference)
        {
            if (reference == null || transform.IsChildOf(reference))
            {
                return Array.Empty<ActiveCollisionConsumer>();
            }

            return reference.GetComponentsInChildren<ActiveCollisionConsumer>();
        }
    }
}