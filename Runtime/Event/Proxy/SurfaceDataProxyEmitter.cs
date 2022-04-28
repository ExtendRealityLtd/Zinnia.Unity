namespace Zinnia.Event.Proxy
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using Zinnia.Data.Type;
    using Zinnia.Extension;

    /// <summary>
    /// Emits a <see cref="UnityEvent"/> with a <see cref="SurfaceData"/> payload whenever <see cref="SingleEventProxyEmitter{TValue,TEvent}.Receive"/> is called.
    /// </summary>
    public class SurfaceDataProxyEmitter : RestrictableSingleEventProxyEmitter<SurfaceData, SurfaceDataProxyEmitter.UnityEvent>
    {
        /// <summary>
        /// The types of <see cref="GameObject"/> that can be used for the rule source.
        /// </summary>
        public enum RuleSourceType
        {
            /// <summary>
            /// Use the <see cref="TransformData.Transform"/> as the source for the rule.
            /// </summary>
            Source,
            /// <summary>
            /// Use the <see cref="SurfaceData.CollisionData.transform"/> as the target for the rule.
            /// </summary>
            Target
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
        public class UnityEvent : UnityEvent<SurfaceData> { }

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
                    return Payload?.Transform != null ? Payload?.Transform.gameObject : null;
                case RuleSourceType.Target:
                    return Payload?.CollisionData.transform != null ? Payload?.CollisionData.transform.gameObject : null;
            }

            return null;
        }
    }
}