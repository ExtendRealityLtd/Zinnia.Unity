namespace Zinnia.Tracking.Follow.Event.Proxy
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using Zinnia.Event.Proxy;
    using Zinnia.Extension;

    public class ObjectFollowerEventProxyEmitter : RestrictableSingleEventProxyEmitter<ObjectFollower.EventData, ObjectFollowerEventProxyEmitter.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the specified state.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<ObjectFollower.EventData> { }

        /// <summary>
        /// The types of <see cref="GameObject"/> that can be used for the rule source.
        /// </summary>
        public enum RuleSourceType
        {
            /// <summary>
            /// Use the <see cref="ObjectFollower.EventData.EventSource"/> for the rule.
            /// </summary>
            Source,
            /// <summary>
            /// Use the <see cref="ObjectFollower.EventData.EventTarget"/> for the rule.
            /// </summary>
            Target,
            /// <summary>
            /// Use the <see cref="ObjectFollower.EventData.EventTargetOffset"/> for the rule.
            /// </summary>
            TargetOffset
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
                case RuleSourceType.Source:
                    return Payload.EventSource.gameObject;
                case RuleSourceType.Target:
                    return Payload.EventTarget.gameObject;
                case RuleSourceType.TargetOffset:
                    return Payload.EventTargetOffset.gameObject;
                default:
                    return null;
            }
        }
    }
}