namespace Zinnia.Tracking.Collision.Event.Proxy
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;
    using Zinnia.Extension;
    using Zinnia.Event.Proxy;

    /// <summary>
    /// Emits a UnityEvent with a <see cref="ActiveCollisionsContainer.EventData"/> payload whenever <see cref="SingleEventProxyEmitter{TValue,TEvent}.Receive"/> is called.
    /// </summary>
    public class CollisionNotifierEventProxyEmitter : RestrictableSingleEventProxyEmitter<CollisionNotifier.EventData, CollisionNotifierEventProxyEmitter.UnityEvent>
    {
        /// <summary>
        /// The types of <see cref="GameObject"/> that can be used for the rule source.
        /// </summary>
        public enum RuleSourceType
        {
            /// <summary>
            /// Use the <see cref="CollisionNotifier.EventData.ForwardSource"/> for the rule.
            /// </summary>
            ForwardSource,
            /// <summary>
            /// Use the <see cref="CollisionNotifier.EventData.ColliderData"/> containing <see cref="Transform"/> for the rule.
            /// </summary>
            CollidingSource
        }

        /// <summary>
        /// The source <see cref="GameObject"/> to apply to the <see cref="RestrictableSingleEventProxyEmitter.ReceiveValidity"/>.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public RuleSourceType RuleSource { get; set; }

        /// <summary>
        /// Defines the event with the specified state.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<CollisionNotifier.EventData>
        {
        }

        /// <inheritdoc />
        protected override object GetTargetToCheck()
        {
            switch (RuleSource)
            {
                case RuleSourceType.ForwardSource:
                    return Payload.ForwardSource.gameObject;
                case RuleSourceType.CollidingSource:
                    Transform containingTransform = Payload.ColliderData.GetContainingTransform();
                    return containingTransform != null ? containingTransform.gameObject : null;
            }
            return null;
        }
    }
}