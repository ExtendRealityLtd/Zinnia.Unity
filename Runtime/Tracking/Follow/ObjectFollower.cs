namespace Zinnia.Tracking.Follow
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using Malimbe.MemberClearanceMethod;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.BehaviourStateRequirementMethod;
    using Zinnia.Extension;
    using Zinnia.Process.Component;
    using Zinnia.Data.Collection.List;
    using Zinnia.Tracking.Follow.Modifier;

    /// <summary>
    /// Mirrors the <see cref="Transform"/> properties of another <see cref="Transform"/> based on the given <see cref="Modifier.FollowModifier"/>.
    /// </summary>
    public class ObjectFollower : GameObjectSourceTargetProcessor
    {
        /// <summary>
        /// Holds data about a <see cref="ObjectFollower"/> event.
        /// </summary>
        [Serializable]
        public class EventData
        {
            /// <summary>
            /// The source utilize within the <see cref="Modifier.FollowModifier"/>.
            /// </summary>
            [Serialized, Cleared]
            [field: DocumentedByXml]
            public GameObject EventSource { get; set; }
            /// <summary>
            /// The target to apply the <see cref="Modifier.FollowModifier"/> on.
            /// </summary>
            [Serialized, Cleared]
            [field: DocumentedByXml]
            public GameObject EventTarget { get; set; }
            /// <summary>
            /// The optional offset the target follow against the source.
            /// </summary>
            [Serialized, Cleared]
            [field: DocumentedByXml]
            public GameObject EventTargetOffset { get; set; }

            public EventData Set(EventData source)
            {
                return Set(source.EventSource, source.EventTarget, source.EventTargetOffset);
            }

            public EventData Set(GameObject source, GameObject target, GameObject targetOffset = null)
            {
                EventSource = source;
                EventTarget = target;
                EventTargetOffset = targetOffset;
                return this;
            }

            public void Clear()
            {
                Set(default, default, default);
            }
        }

        /// <summary>
        /// Defines the event with the <see cref="EventData"/>.
        /// </summary>
        [Serializable]
        public class FollowEvent : UnityEvent<EventData>
        {
        }

        /// <summary>
        /// A <see cref="GameObject"/> collection of target offsets to offset the <see cref="GameObjectSourceTargetProcessor.Targets"/> against the source whilst following. The <see cref="GameObject"/> for the target offset must be a child of the corresponding target.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public GameObjectObservableList TargetOffsets { get; set; }
        /// <summary>
        /// The <see cref="Modifier.FollowModifier"/> to apply.
        /// </summary>
        [Serialized, Cleared]
        [field: Header("Follow Settings"), DocumentedByXml]
        public FollowModifier FollowModifier { get; set; }

        /// <summary>
        /// Emitted before any processing.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent Preprocessed = new UnityEvent();
        /// <summary>
        /// Emitted after all processing is complete.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent Processed = new UnityEvent();

        /// <inheritdoc />
        [RequiresBehaviourState]
        public override void Process()
        {
            Preprocessed?.Invoke();
            base.Process();
            Processed?.Invoke();
        }

        /// <summary>
        /// Applies the follow modification of the given source to the given target.
        /// </summary>
        /// <param name="source">The source to take the follow data from.</param>
        /// <param name="target">The target to apply the follow data to.</param>
        protected override void ApplySourceToTarget(GameObject source, GameObject target)
        {
            GameObject followOffset = GetFollowOffset();
            if (followOffset != null && !followOffset.transform.IsChildOf(Targets.NonSubscribableElements[Targets.CurrentIndex].transform))
            {
                throw new ArgumentException($"The `TargetOffsets` at index [{Targets.CurrentIndex}] must be a child of the GameObject at `Targets` index [{Targets.CurrentIndex}].");
            }
            FollowModifier.Modify(source, target, followOffset);
        }

        /// <summary>
        /// Gets the Follow Offset for the current target offset based on the current target index.
        /// </summary>
        /// <returns></returns>
        protected virtual GameObject GetFollowOffset()
        {
            if (Targets == null || TargetOffsets == null || Targets.NonSubscribableElements.Count == 0 || TargetOffsets.NonSubscribableElements.Count == 0)
            {
                return null;
            }

            int currentIndexTargets = TargetOffsets.NonSubscribableElements.ClampIndex(Targets.CurrentIndex);
            return TargetOffsets.NonSubscribableElements[currentIndexTargets];
        }
    }
}
