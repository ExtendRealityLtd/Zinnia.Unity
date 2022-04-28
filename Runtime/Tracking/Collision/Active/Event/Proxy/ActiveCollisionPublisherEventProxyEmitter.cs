namespace Zinnia.Tracking.Collision.Active.Event.Proxy
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using Zinnia.Event.Proxy;
    using Zinnia.Extension;
    using Zinnia.Tracking.Collision.Active;

    /// <summary>
    /// Emits a UnityEvent with a <see cref="ActiveCollisionPublisher.PayloadData"/> payload whenever the Receive method is called.
    /// </summary>
    public class ActiveCollisionPublisherEventProxyEmitter : RestrictableSingleEventProxyEmitter<ActiveCollisionPublisher.PayloadData, ActiveCollisionPublisherEventProxyEmitter.UnityEvent>
    {
        /// <summary>
        /// The types of <see cref="GameObject"/> that can be used for the rule source.
        /// </summary>
        public enum RuleSourceType
        {
            /// <summary>
            /// Use the <see cref="ActiveCollisionPublisher.EventData.SourceContainer"/> for the rule.
            /// </summary>
            SourceContainer,
            /// <summary>
            /// Use the <see cref="ActiveCollisionPublisher.EventData.PublisherContainer"/> for the rule.
            /// </summary>
            PublisherContainer
        }
        [Tooltip("The source GameObject to apply to the RestrictableSingleEventProxyEmitter.ReceiveValidity.")]
        [SerializeField]
        private RuleSourceType ruleSource;
        /// <summary>
        /// The source <see cref="GameObject"/> to apply to the <see cref="RestrictableSingleEventProxyEmitter.ReceiveValidity"/>.
        /// </summary>
        public RuleSourceType RuleSource
        {
            get
            {
                return ruleSource;
            }
            set
            {
                ruleSource = value;
            }
        }

        /// <summary>
        /// Defines the event with the specified state.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<ActiveCollisionPublisher.PayloadData> { }

        /// <summary>
        /// Sets the <see cref="RuleSource"/>.
        /// </summary>
        /// <param name="index">The index of the <see cref="RuleSourceType"/>.</param>
        public virtual void SetRuleSource(int index)
        {
            RuleSource = EnumExtensions.GetByIndex<RuleSourceType>(index);
        }

        /// <inheritdoc />
        protected override object GetTargetToCheck()
        {
            switch (RuleSource)
            {
                case RuleSourceType.PublisherContainer:
                    return Payload?.PublisherContainer;
                case RuleSourceType.SourceContainer:
                    return Payload?.SourceContainer;
            }
            return null;
        }
    }
}