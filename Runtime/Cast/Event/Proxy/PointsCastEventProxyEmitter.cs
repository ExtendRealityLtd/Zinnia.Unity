namespace Zinnia.Cast.Event.Proxy
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using Zinnia.Event.Proxy;
    using Zinnia.Extension;

    /// <summary>
    /// Emits a <see cref="UnityEvent"/> with a <see cref="PointsCast.EventData"/> payload whenever <see cref="SingleEventProxyEmitter{TValue,TEvent}.Receive"/> is called.
    /// </summary>
    public class PointsCastEventProxyEmitter : RestrictableSingleEventProxyEmitter<PointsCast.EventData, PointsCastEventProxyEmitter.UnityEvent>
    {
        /// <summary>
        /// The types of <see cref="GameObject"/> that can be used for the rule source.
        /// </summary>
        public enum RuleSourceType
        {
            /// <summary>
            /// Use the actual <see cref="Collider"/> hit as the source for the rule.
            /// </summary>
            Collider,
            /// <summary>
            /// Use the parent <see cref="Rigidbody"/> hit as the target for the rule.
            /// </summary>
            Rigidbody
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
        public class UnityEvent : UnityEvent<PointsCast.EventData> { }

        /// <inheritdoc />
        protected override object GetTargetToCheck()
        {
            if (Payload == null || Payload.HitData == null)
            {
                return null;
            }

            RaycastHit hitData = (RaycastHit)Payload.HitData;
            switch (RuleSource)
            {
                case RuleSourceType.Collider:
                    return hitData.collider.gameObject;
                case RuleSourceType.Rigidbody:
                    return hitData.collider.GetContainingTransform().gameObject;
            }

            return null;
        }
    }
}