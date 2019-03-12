﻿namespace Zinnia.Pointer
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using Malimbe.BehaviourStateRequirementMethod;
    using Malimbe.XmlDocumentationAttribute;
    using Zinnia.Cast;
    using Zinnia.Data.Type;
    using Zinnia.Extension;
    using Zinnia.Visual;

    /// <summary>
    /// Allows pointing at objects and notifies when a target is hit, continues to be hit or stops being hit by listening to a <see cref="PointsCast"/>.
    /// </summary>
    public class ObjectPointer : MonoBehaviour
    {
        /// <summary>
        /// Holds data about a <see cref="ObjectPointer"/> event.
        /// </summary>
        [Serializable]
        public class EventData : SurfaceData
        {
            /// <summary>
            /// Whether the <see cref="ObjectPointer"/> is currently activated.
            /// </summary>
            [DocumentedByXml]
            public bool isActive;
            /// <summary>
            /// Whether the <see cref="ObjectPointer"/> is currently hovering over a target.
            /// </summary>
            [DocumentedByXml]
            public bool isHovering;
            /// <summary>
            /// The duration that the <see cref="ObjectPointer"/> has been hovering over it's current target.
            /// </summary>
            [DocumentedByXml]
            public float hoverDuration;
            /// <summary>
            /// The points cast data given to the <see cref="ObjectPointer"/>.
            /// </summary>
            [DocumentedByXml]
            public PointsCast.EventData pointsCastData;

            public EventData Set(EventData source)
            {
                return Set(source.isActive, source.isHovering, source.hoverDuration, source.pointsCastData);
            }

            public EventData Set(bool isActive, bool isHovering, float hoverDuration, PointsCast.EventData pointsCastData)
            {
                this.isActive = isActive;
                this.isHovering = isHovering;
                this.hoverDuration = hoverDuration;
                this.pointsCastData = pointsCastData;
                return this;
            }

            public void Clear()
            {
                Set(default, default, default, default);
            }
        }

        /// <summary>
        /// Defines the event with the <see cref="EventData"/>.
        /// </summary>
        /// <remarks>The <see cref="ObjectPointer"/> data is <see langword="null"/> in case the <see cref="ObjectPointer"/> isn't hitting any target.</remarks>
        [Serializable]
        public class UnityEvent : UnityEvent<EventData>
        {
        }

        /// <summary>
        /// Defines the event with the <see cref="PointsRenderer.PointsData"/> state.
        /// </summary>
        [Serializable]
        public class PointsRendererUnityEvent : UnityEvent<PointsRenderer.PointsData>
        {
        }

        /// <summary>
        /// Describes an element of the rendered <see cref="ObjectPointer"/>.
        /// </summary>
        [Serializable]
        public class Element
        {
            /// <summary>
            /// The visibility of an <see cref="Element"/>.
            /// </summary>
            public enum Visibility
            {
                /// <summary>
                /// The <see cref="Element"/> will only be visible when the <see cref="ObjectPointer"/> is activated.
                /// </summary>
                OnWhenPointerActivated,
                /// <summary>
                /// The <see cref="Element"/> will always be visible regardless of the <see cref="ObjectPointer"/> state.
                /// </summary>
                AlwaysOn,
                /// <summary>
                /// The <see cref="Element"/> will never be visible regardless of the <see cref="ObjectPointer"/> state.
                /// </summary>
                AlwaysOff
            }

            /// <summary>
            /// Represents the <see cref="Element"/> when it's colliding with a valid object.
            /// </summary>
            [DocumentedByXml]
            public GameObject validObject;
            /// <summary>
            /// Represents the <see cref="Element"/> when it's colliding with an invalid object or not colliding at all.
            /// </summary>
            [DocumentedByXml]
            public GameObject invalidObject;
            /// <summary>
            /// Determines when the <see cref="Element"/> is visible.
            /// </summary>
            [DocumentedByXml]
            public Visibility visibility = Visibility.OnWhenPointerActivated;
        }

        /// <summary>
        /// Determines if the <see cref="ObjectPointer"/> should be automatically activated when the script is enabled.
        /// </summary>
        [DocumentedByXml]
        public bool activateOnEnable;

        /// <summary>
        /// Represents the origin, i.e. the first rendered point.
        /// </summary>
        [DocumentedByXml]
        public Element origin = new Element();
        /// <summary>
        /// Represents the segments between <see cref="origin"/> and <see cref="destination"/>. This will get cloned to create all the segments.
        /// </summary>
        [DocumentedByXml]
        public Element repeatedSegment = new Element();
        /// <summary>
        /// Represents the destination, i.e. the last rendered point.
        /// </summary>
        [DocumentedByXml]
        public Element destination = new Element();

        /// <summary>
        /// Emitted when the <see cref="ObjectPointer"/> becomes active.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent Activated = new UnityEvent();
        /// <summary>
        /// Emitted when the <see cref="ObjectPointer"/> becomes inactive.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent Deactivated = new UnityEvent();
        /// <summary>
        /// Emitted when the <see cref="ObjectPointer"/> collides with a new target.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent Entered = new UnityEvent();
        /// <summary>
        /// Emitted when the <see cref="ObjectPointer"/> stops colliding with an existing target.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent Exited = new UnityEvent();
        /// <summary>
        /// Emitted when the <see cref="ObjectPointer"/> changes its hovering position over an existing target.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent Hovering = new UnityEvent();
        /// <summary>
        /// Emitted whenever <see cref="Select"/> is called.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent Selected = new UnityEvent();
        /// <summary>
        /// Emitted when the <see cref="ObjectPointer"/> appears and becomes visible.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent Appeared = new UnityEvent();
        /// <summary>
        /// Emitted when the <see cref="ObjectPointer"/> disappears and becomes hidden.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent Disappeared = new UnityEvent();
        /// <summary>
        /// Emitted when the <see cref="ObjectPointer"/> elements change.
        /// </summary>
        [DocumentedByXml]
        public PointsRendererUnityEvent RenderDataChanged = new PointsRendererUnityEvent();

        /// <summary>
        /// The activation state of the <see cref="ObjectPointer"/>.
        /// </summary>
        public bool ActivationState
        {
            get;
            protected set;
        }

        /// <summary>
        /// Reports hover duration of the <see cref="ObjectPointer"/> over the current target.
        /// </summary>
        public float HoverDuration
        {
            get;
            protected set;
        }

        /// <summary>
        /// The target that the <see cref="ObjectPointer"/> is currently hovering over. If there is no target then it is <see langword="null"/>.
        /// </summary>
        public EventData HoverTarget => (IsHovering ? GetEventData(activePointsCastData) : null);

        /// <summary>
        /// The target that the <see cref="ObjectPointer"/> has most recently selected.
        /// </summary>
        public EventData SelectedTarget
        {
            get;
            protected set;
        }

        /// <summary>
        /// Whether the <see cref="ObjectPointer"/> is currently hovering over a target.
        /// </summary>
        /// <returns><see langword="true"/> if the <see cref="ObjectPointer"/> is currently hovering over a target, <see langword="false"/> otherwise.</returns>
        public bool IsHovering
        {
            get;
            protected set;
        }

        /// <summary>
        /// Whether any <see cref="Element"/> of the <see cref="ObjectPointer"/> is currently visible.
        /// </summary>
        public bool IsVisible
        {
            get
            {
                if (!enabled)
                {
                    return false;
                }

                if (origin.visibility == Element.Visibility.AlwaysOff &&
                    repeatedSegment.visibility == Element.Visibility.AlwaysOff &&
                    destination.visibility == Element.Visibility.AlwaysOff)
                {
                    return false;
                }

                if (origin.visibility == Element.Visibility.AlwaysOn ||
                    repeatedSegment.visibility == Element.Visibility.AlwaysOn ||
                    destination.visibility == Element.Visibility.AlwaysOn)
                {
                    return true;
                }

                return ActivationState;
            }
        }

        protected readonly PointsCast.EventData activePointsCastData = new PointsCast.EventData();
        protected readonly PointsCast.EventData previousPointsCastData = new PointsCast.EventData();
        protected bool? previousVisibility;
        protected readonly EventData eventData = new EventData();
        protected readonly PointsRenderer.PointsData pointsData = new PointsRenderer.PointsData();

        /// <summary>
        /// The Activate method turns on the <see cref="ObjectPointer"/>.
        /// </summary>
        [RequiresBehaviourState]
        public virtual void Activate()
        {
            if (ActivationState)
            {
                return;
            }

            ActivationState = true;
            UpdateRenderData();
            Activated?.Invoke(HoverTarget);
        }

        /// <summary>
        /// The Deactivate method turns off the <see cref="ObjectPointer"/>.
        /// </summary>
        public virtual void Deactivate()
        {
            TryDeactivate(false);
        }

        /// <summary>
        /// Gets the current <see cref="ObjectPointer"/> state and emits it through <see cref="Selected"/>.
        /// </summary>
        [RequiresBehaviourState]
        public virtual void Select()
        {
            SelectedTarget = (ActivationState ? HoverTarget : null);
            Selected?.Invoke(SelectedTarget);
        }

        /// <summary>
        /// Handles the provided data to transition state and emit the <see cref="ObjectPointer"/> events.
        /// </summary>
        /// <param name="data">The data describing the results of the most recent cast.</param>
        [RequiresBehaviourState]
        public virtual void HandleData(PointsCast.EventData data)
        {
            if (IsVisible)
            {
                previousPointsCastData.Set(activePointsCastData);
                if (data.TargetHit != null)
                {
                    Transform targetTransform = data.TargetHit.Value.transform;
                    if (targetTransform != null && targetTransform != activePointsCastData?.TargetHit?.transform)
                    {
                        TryEmitExit(previousPointsCastData);
                        Entered?.Invoke(GetEventData(data));
                    }

                    HoverDuration += Time.deltaTime;
                    IsHovering = true;
                    Hovering?.Invoke(GetEventData(data));
                }
                else
                {
                    TryEmitExit(previousPointsCastData);
                }

                activePointsCastData.Set(data);
            }
            else
            {
                activePointsCastData.Clear();
                previousPointsCastData.Clear();
            }

            UpdateRenderData();

        }

        protected virtual void OnEnable()
        {
            if (activateOnEnable)
            {
                Activate();
            }
            ActivationState = activateOnEnable;
            TryEmitVisibilityEvent();
            UpdateRenderData();
        }

        protected virtual void OnDisable()
        {
            TryDeactivate(true);
        }

        protected virtual void Update()
        {
            TryEmitVisibilityEvent();
        }

        /// <summary>
        /// Updates the <see cref="Element"/>'s <see cref="GameObject"/>'s visibility and emits the <see cref="RenderDataChanged"/> event with the <see cref="GameObject"/>s used for the <see cref="Element"/>s.
        /// </summary>
        protected virtual void UpdateRenderData()
        {
            pointsData.Points = (activePointsCastData.Points ?? Array.Empty<Vector3>());
            pointsData.StartPoint = GetElementRepresentation(origin);
            pointsData.RepeatedSegmentPoint = GetElementRepresentation(repeatedSegment);
            pointsData.EndPoint = GetElementRepresentation(destination);

            TryDeactivateElementObject(origin.validObject);
            TryDeactivateElementObject(origin.invalidObject);
            TryDeactivateElementObject(repeatedSegment.validObject);
            TryDeactivateElementObject(repeatedSegment.invalidObject);
            TryDeactivateElementObject(destination.validObject);
            TryDeactivateElementObject(destination.invalidObject);

            pointsData.StartPoint.TrySetActive(true);
            pointsData.RepeatedSegmentPoint.TrySetActive(true);
            pointsData.EndPoint.TrySetActive(true);

            RenderDataChanged?.Invoke(pointsData);
            TryEmitVisibilityEvent();
        }

        /// <summary>
        /// Attempts to deactivate the <see cref="ObjectPointer"/>.
        /// </summary>
        /// <param name="ignoreBehaviourState">Determines if the events should be emitted based on the <see cref="Behaviour.enabled"/> state.</param>
        protected virtual void TryDeactivate(bool ignoreBehaviourState)
        {
            if ((!isActiveAndEnabled && !ignoreBehaviourState) || !ActivationState)
            {
                return;
            }

            UpdateRenderData();
            TryEmitExit(previousPointsCastData);
            Deactivated?.Invoke(HoverTarget);
            ActivationState = false;
            activePointsCastData.Clear();
            previousPointsCastData.Clear();
            UpdateRenderData();
        }

        /// <summary>
        /// Checks to see if the <see cref="ObjectPointer"/> is not currently colliding with a valid target and emits the <see cref="Exited"/> event.
        /// </summary>
        /// <param name="data">The current points cast data.</param>
        protected virtual void TryEmitExit(PointsCast.EventData data)
        {
            if (activePointsCastData.TargetHit?.transform != null)
            {
                Exited?.Invoke(GetEventData(data));
                IsHovering = false;
                HoverDuration = 0f;
            }
        }

        /// <summary>
        /// Emits the <see cref="Appeared"/> or <see cref="Disappeared"/> event for the current <see cref="IsVisible"/> state in case that state changed.
        /// </summary>
        protected virtual void TryEmitVisibilityEvent()
        {
            if (IsVisible == previousVisibility)
            {
                return;
            }

            previousVisibility = IsVisible;

            if (IsVisible)
            {
                Appeared?.Invoke(HoverTarget);
            }
            else
            {
                Disappeared?.Invoke(HoverTarget);
            }
        }

        /// <summary>
        /// Returns the <see cref="GameObject"/> used for a specific <see cref="Element"/> based on the current <see cref="ObjectPointer"/> state.
        /// </summary>
        /// <param name="element">The <see cref="Element"/> to return a <see cref="GameObject"/> for.</param>
        /// <returns>A <see cref="GameObject"/> to represent <paramref name="element"/>.</returns>
        protected virtual GameObject GetElementRepresentation(Element element)
        {
            bool isValid = (activePointsCastData.TargetHit != null);

            switch (element.visibility)
            {
                case Element.Visibility.OnWhenPointerActivated:
                    return (ActivationState ? (isValid ? element.validObject : element.invalidObject) : null);
                case Element.Visibility.AlwaysOn:
                    return (isValid ? element.validObject : element.invalidObject);
                case Element.Visibility.AlwaysOff:
                    return null;
                default:
                    throw new ArgumentOutOfRangeException(nameof(element.visibility), element.visibility, null);
            }
        }

        /// <summary>
        /// Attempts to deactivate an object that is an element of this <see cref="ObjectPointer"/> if it's not one of the elements that should stay activated.
        /// </summary>
        /// <param name="elementObject">The object to check and deactivate if needed.</param>
        protected virtual void TryDeactivateElementObject(GameObject elementObject)
        {
            if (elementObject != null
                && elementObject != pointsData.StartPoint
                && elementObject != pointsData.RepeatedSegmentPoint
                && elementObject != pointsData.EndPoint)
            {
                elementObject.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Builds a valid <see cref="ObjectPointer"/> payload to use in the events.
        /// </summary>
        /// <returns>A <see cref="EventData"/> object of the <see cref="ObjectPointer"/>'s current state.</returns>
        protected virtual EventData GetEventData(PointsCast.EventData data)
        {
            Transform validDestinationTransform = destination == null || destination.validObject == null ? null : destination.validObject.transform;
            Transform pointerTransform = transform;

            eventData.Transform = pointerTransform;
            eventData.PositionOverride = validDestinationTransform == null ? data.TargetHit?.point : validDestinationTransform.position;
            eventData.RotationOverride = validDestinationTransform == null ? Quaternion.identity : validDestinationTransform.localRotation;
            eventData.ScaleOverride = validDestinationTransform == null ? Vector3.one : validDestinationTransform.lossyScale;
            eventData.Origin = pointerTransform.position;
            eventData.Direction = pointerTransform.forward;
            eventData.CollisionData = data?.TargetHit ?? default;
            return eventData.Set(ActivationState, IsHovering, HoverDuration, data);
        }
    }
}
