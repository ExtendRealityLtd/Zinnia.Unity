namespace Zinnia.Tracking.Collision.Event.Proxy
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using Zinnia.Event.Proxy;
    using Zinnia.Extension;

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
        public class UnityEvent : UnityEvent<CollisionNotifier.EventData> { }

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