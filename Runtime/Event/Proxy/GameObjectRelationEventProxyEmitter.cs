namespace Zinnia.Event.Proxy
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using Zinnia.Data.Collection.List;

    /// <summary>
    /// Emits a <see cref="UnityEvent"/> with a <see cref="GameObjectRelationObservableList.Relation"/> payload whenever the <see cref="SingleEventProxyEmitter{TValue,TEvent}.Receive"/> method is called.
    /// </summary>
    public class GameObjectRelationEventProxyEmitter : RestrictableSingleEventProxyEmitter<GameObjectRelationObservableList.Relation, GameObjectRelationEventProxyEmitter.UnityEvent>
    {
        /// <summary>
        /// The types of <see cref="GameObject"/> that can be used for the rule source.
        /// </summary>
        public enum RuleSourceType
        {
            /// <summary>
            /// Use the Relation Key as the source for the rule.
            /// </summary>
            Key,
            /// <summary>
            /// Use the Relation Value as the target for the rule.
            /// </summary>
            Value
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
        public class UnityEvent : UnityEvent<GameObjectRelationObservableList.Relation> { }

        /// <inheritdoc />
        protected override object GetTargetToCheck()
        {
            switch (RuleSource)
            {
                case RuleSourceType.Key:
                    return Payload != null ? Payload?.Key : null;
                case RuleSourceType.Value:
                    return Payload != null ? Payload?.Value : null;
            }

            return null;
        }
    }
}