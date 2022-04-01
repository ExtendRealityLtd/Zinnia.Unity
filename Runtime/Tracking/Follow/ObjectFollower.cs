namespace Zinnia.Tracking.Follow
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using Zinnia.Data.Collection.List;
    using Zinnia.Extension;
    using Zinnia.Process.Component;
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
            [Tooltip("The source utilize within the Modifier.FollowModifier.")]
            [SerializeField]
            private GameObject eventSource;
            /// <summary>
            /// The source utilize within the <see cref="Modifier.FollowModifier"/>.
            /// </summary>
            public GameObject EventSource
            {
                get
                {
                    return eventSource;
                }
                set
                {
                    eventSource = value;
                }
            }
            [Tooltip("The target to apply the Modifier.FollowModifier on.")]
            [SerializeField]
            private GameObject eventTarget;
            /// <summary>
            /// The target to apply the <see cref="Modifier.FollowModifier"/> on.
            /// </summary>
            public GameObject EventTarget
            {
                get
                {
                    return eventTarget;
                }
                set
                {
                    eventTarget = value;
                }
            }
            [Tooltip("The optional offset the target follow against the source.")]
            [SerializeField]
            private GameObject eventTargetOffset;
            /// <summary>
            /// The optional offset the target follow against the source.
            /// </summary>
            public GameObject EventTargetOffset
            {
                get
                {
                    return eventTargetOffset;
                }
                set
                {
                    eventTargetOffset = value;
                }
            }

            /// <summary>
            /// Clears <see cref="EventSource"/>.
            /// </summary>
            public virtual void ClearEventSource()
            {
                EventSource = default;
            }

            /// <summary>
            /// Clears <see cref="EventTarget"/>.
            /// </summary>
            public virtual void ClearEventTarget()
            {
                EventTarget = default;
            }

            /// <summary>
            /// Clears <see cref="EventTargetOffset"/>.
            /// </summary>
            public virtual void ClearEventTargetOffset()
            {
                EventTargetOffset = default;
            }

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
        public class FollowEvent : UnityEvent<EventData> { }

        [Tooltip("A GameObject collection of target offsets to offset the GameObjectSourceTargetProcessor.Targets against the source whilst following. The GameObject for the target offset must be a child of the corresponding target.")]
        [SerializeField]
        private GameObjectObservableList targetOffsets;
        /// <summary>
        /// A <see cref="GameObject"/> collection of target offsets to offset the <see cref="GameObjectSourceTargetProcessor.Targets"/> against the source whilst following. The <see cref="GameObject"/> for the target offset must be a child of the corresponding target.
        /// </summary>
        public GameObjectObservableList TargetOffsets
        {
            get
            {
                return targetOffsets;
            }
            set
            {
                targetOffsets = value;
            }
        }
        [Header("Follow Settings")]
        [Tooltip("The Modifier.FollowModifier to apply.")]
        [SerializeField]
        private FollowModifier followModifier;
        /// <summary>
        /// The <see cref="Modifier.FollowModifier"/> to apply.
        /// </summary>
        public FollowModifier FollowModifier
        {
            get
            {
                return followModifier;
            }
            set
            {
                followModifier = value;
            }
        }

        /// <summary>
        /// Emitted before any processing.
        /// </summary>
        public UnityEvent Preprocessed = new UnityEvent();
        /// <summary>
        /// Emitted after all processing is complete.
        /// </summary>
        public UnityEvent Processed = new UnityEvent();

        /// <summary>
        /// Clears <see cref="TargetOffsets"/>.
        /// </summary>
        public virtual void ClearTargetOffsets()
        {
            if (!this.IsValidState())
            {
                return;
            }

            TargetOffsets = default;
        }

        /// <summary>
        /// Clears <see cref="FollowModifier"/>.
        /// </summary>
        public virtual void ClearFollowModifier()
        {
            if (!this.IsValidState())
            {
                return;
            }

            FollowModifier = default;
        }

        /// <inheritdoc />
        public override void Process()
        {
            if (!this.IsValidState())
            {
                return;
            }

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
