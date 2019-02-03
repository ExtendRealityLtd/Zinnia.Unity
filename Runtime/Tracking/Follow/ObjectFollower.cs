namespace Zinnia.Tracking.Follow
{
    using UnityEngine;
    using UnityEngine.Events;
    using EmptyUnityEvent = UnityEngine.Events.UnityEvent;
    using System;
    using System.Collections.Generic;
    using Malimbe.XmlDocumentationAttribute;
    using Zinnia.Extension;
    using Zinnia.Process.Component;
    using Zinnia.Tracking.Follow.Modifier;

    /// <summary>
    /// Mirrors the <see cref="Transform"/> properties of another <see cref="Transform"/> based on the given <see cref="FollowModifier"/>.
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
            /// The source utilize within the <see cref="FollowModifier"/>.
            /// </summary>
            [DocumentedByXml]
            public GameObject source;
            /// <summary>
            /// The target to apply the <see cref="FollowModifier"/> on.
            /// </summary>
            [DocumentedByXml]
            public GameObject target;
            /// <summary>
            /// The optional offset the target follow against the source.
            /// </summary>
            [DocumentedByXml]
            public GameObject targetOffset;

            public EventData Set(EventData source)
            {
                return Set(source.source, source.target, source.targetOffset);
            }

            public EventData Set(GameObject source, GameObject target, GameObject targetOffset = null)
            {
                this.source = source;
                this.target = target;
                this.targetOffset = targetOffset;
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
        public class UnityEvent : UnityEvent<EventData>
        {
        }

        /// <summary>
        /// A <see cref="GameObject"/> collection of target offsets to offset the target against the source whilst following.
        /// </summary>
        [DocumentedByXml]
        public List<GameObject> targetOffsets = new List<GameObject>();
        /// <summary>
        /// The <see cref="FollowModifier"/> to apply.
        /// </summary>
        [Header("Follow Settings"), DocumentedByXml]
        public FollowModifier followModifier;

        /// <summary>
        /// The current <see cref="targetOffsets"/> collection index.
        /// </summary>
        public int CurrentTargetOffsetsIndex
        {
            get { return _currentTargetOffsetsIndex; }
            set { _currentTargetOffsetsIndex = targetOffsets.GetWrappedAndClampedIndex(value); }
        }
        private int _currentTargetOffsetsIndex;

        /// <summary>
        /// Emitted before any processing.
        /// </summary>
        [DocumentedByXml]
        public EmptyUnityEvent Preprocessed = new EmptyUnityEvent();
        /// <summary>
        /// Emitted after all processing is complete.
        /// </summary>
        [DocumentedByXml]
        public EmptyUnityEvent Processed = new EmptyUnityEvent();

        protected EventData eventData = new EventData();

        /// <inheritdoc />
        public override void Process()
        {
            if (!isActiveAndEnabled)
            {
                return;
            }

            Preprocessed?.Invoke();
            base.Process();
            Processed?.Invoke();
        }

        /// <summary>
        /// Adds the given targetOffset to the targetOffsets collection.
        /// </summary>
        /// <param name="targetOffset">The targetOffset to add.</param>
        public virtual void AddTargetOffset(GameObject targetOffset)
        {
            targetOffsets.Add(targetOffset);
        }

        /// <summary>
        /// Removes the given targetOffset from the targetOffsets collection.
        /// </summary>
        /// <param name="targetOffset">The targetOffset to remove.</param>
        public virtual void RemoveTargetOffset(GameObject targetOffset)
        {
            targetOffsets.Remove(targetOffset);
        }

        /// <summary>
        /// Sets the given targetOffset at the current targetOffsets index.
        /// </summary>
        /// <param name="targetOffset">The targetOffset to set.</param>
        public virtual void SetTargetOffsetAtCurrentIndex(GameObject targetOffset)
        {
            if (targetOffsets.Count == 0)
            {
                targetOffsets.Add(targetOffset);
            }
            else
            {
                targetOffsets[CurrentTargetsIndex] = targetOffset;
            }
        }

        /// <summary>
        /// Clears the targetOffsets collection.
        /// </summary>
        public virtual void ClearTargetOffsets()
        {
            targetOffsets.Clear();
        }

        /// <summary>
        /// Applies the follow modification of the given source to the given target.
        /// </summary>
        /// <param name="source">The source to take the follow data from.</param>
        /// <param name="target">The target to apply the follow data to.</param>
        protected override void ApplySourceToTarget(GameObject source, GameObject target)
        {
            GameObject followOffset = (targetOffsets.Count > 0 ? targetOffsets[targetOffsets.GetWrappedAndClampedIndex(CurrentTargetsIndex)] : null);
            if (followOffset != null && !followOffset.transform.IsChildOf(targets[CurrentTargetsIndex].transform))
            {
                throw new ArgumentException($"The `targetOffsets` at index [{CurrentTargetsIndex}] must be a child of the GameObject at `targets` index [{CurrentTargetsIndex}].");
            }
            followModifier.Modify(source, target, followOffset);
        }
    }
}