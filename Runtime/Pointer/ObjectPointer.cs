namespace Zinnia.Pointer
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using Malimbe.MemberClearanceMethod;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.BehaviourStateRequirementMethod;
    using Zinnia.Cast;
    using Zinnia.Data.Type;
    using Zinnia.Extension;
    using Zinnia.Visual;
    using Zinnia.Process;

    /// <summary>
    /// Allows pointing at objects and notifies when a target is hit, continues to be hit or stops being hit by listening to a <see cref="PointsCast"/>.
    /// </summary>
    public class ObjectPointer : MonoBehaviour, IProcessable
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
            [Serialized]
            [field: DocumentedByXml]
            public bool IsCurrentlyActive { get; set; }
            /// <summary>
            /// Whether the <see cref="ObjectPointer"/> is currently hovering over a target.
            /// </summary>
            [Serialized]
            [field: DocumentedByXml]
            public bool IsCurrentlyHovering { get; set; }
            /// <summary>
            /// The duration that the <see cref="ObjectPointer"/> has been hovering over it's current target.
            /// </summary>
            [Serialized]
            [field: DocumentedByXml]
            public float CurrentHoverDuration { get; set; }
            /// <summary>
            /// The points cast data given to the <see cref="ObjectPointer"/>.
            /// </summary>
            [Serialized]
            [field: DocumentedByXml]
            public PointsCast.EventData CurrentPointsCastData { get; set; }

            public EventData Set(EventData source)
            {
                return Set(source.IsCurrentlyActive, source.IsCurrentlyHovering, source.CurrentHoverDuration, source.CurrentPointsCastData);
            }

            public EventData Set(bool isActive, bool isHovering, float hoverDuration, PointsCast.EventData pointsCastData)
            {
                IsCurrentlyActive = isActive;
                IsCurrentlyHovering = isHovering;
                CurrentHoverDuration = hoverDuration;
                CurrentPointsCastData = pointsCastData;
                return this;
            }

            public override void Clear()
            {
                base.Clear();
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
        /// Represents the origin, i.e. the first rendered point.
        /// </summary>
        [Serialized, Cleared]
        [field: Header("Pointer Element Settings"), DocumentedByXml]
        public PointerElement Origin { get; set; }
        /// <summary>
        /// Represents the segments between <see cref="Origin"/> and <see cref="Destination"/>. This will get cloned to create all the segments.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public PointerElement RepeatedSegment { get; set; }
        /// <summary>
        /// Represents the destination, i.e. the last rendered point.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public PointerElement Destination { get; set; }

        /// <summary>
        /// Emitted when the <see cref="ObjectPointer"/> becomes active.
        /// </summary>
        [Header("Pointer Events"), DocumentedByXml]
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
        /// Whether the <see cref="ObjectPointer"/> is currently activated.
        /// </summary>
        public bool IsActivated { get; protected set; }
        /// <summary>
        /// Whether the <see cref="ObjectPointer"/> is currently hovering over a target.
        /// </summary>
        public bool IsHovering { get; protected set; }
        /// <summary>
        /// The duration that the <see cref="ObjectPointer"/> is hovering over the current target.
        /// </summary>
        public float HoverDuration { get; protected set; }
        /// <summary>
        /// The target that the <see cref="ObjectPointer"/> is currently hovering over. If there is no target then it is <see langword="null"/>.
        /// </summary>
        public EventData HoverTarget => IsHovering ? GetEventData(activePointsCastData) : null;
        /// <summary>
        /// The target that the <see cref="ObjectPointer"/> has most recently selected.
        /// </summary>
        public EventData SelectedTarget { get; protected set; }

        /// <summary>
        /// Whether any <see cref="PointerElement"/> of the <see cref="ObjectPointer"/> is currently visible.
        /// </summary>
        public bool IsVisible
        {
            get
            {
                if (!enabled)
                {
                    return false;
                }

                if (Origin.ElementVisibility == PointerElement.Visibility.AlwaysOff &&
                    RepeatedSegment.ElementVisibility == PointerElement.Visibility.AlwaysOff &&
                    Destination.ElementVisibility == PointerElement.Visibility.AlwaysOff)
                {
                    return false;
                }

                if (Origin.ElementVisibility == PointerElement.Visibility.AlwaysOn ||
                    RepeatedSegment.ElementVisibility == PointerElement.Visibility.AlwaysOn ||
                    Destination.ElementVisibility == PointerElement.Visibility.AlwaysOn)
                {
                    return true;
                }

                return IsActivated;
            }
        }

        /// <summary>
        /// The current active points in the cast.
        /// </summary>
        protected readonly PointsCast.EventData activePointsCastData = new PointsCast.EventData();
        /// <summary>
        /// The previous active points in the cast.
        /// </summary>
        protected readonly PointsCast.EventData previousPointsCastData = new PointsCast.EventData();
        /// <summary>
        /// Whether the pointer was visible in the previous frame.
        /// </summary>
        protected bool? wasPreviouslyVisible;
        /// <summary>
        /// The event data to emit on pointer events.
        /// </summary>
        protected readonly EventData eventData = new EventData();
        /// <summary>
        /// The points data to render the pointer with.
        /// </summary>
        protected readonly PointsRenderer.PointsData pointsData = new PointsRenderer.PointsData();

        /// <summary>
        /// The Activate method turns on the <see cref="ObjectPointer"/>.
        /// </summary>
        [RequiresBehaviourState]
        public virtual void Activate()
        {
            if (IsActivated)
            {
                return;
            }

            IsActivated = true;
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
            SelectedTarget = IsActivated && activePointsCastData.IsValid ? HoverTarget : null;
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
                if (data.HitData != null)
                {
                    Transform targetTransform = data.HitData.Value.transform;
                    if (targetTransform != null && targetTransform != activePointsCastData?.HitData?.transform)
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

        /// <summary>
        /// Handles the previous provided <see cref="PointsCast.EventData"/> once more.
        /// </summary>
        [RequiresBehaviourState]
        public virtual void RehandleData()
        {
            HandleData(activePointsCastData);
        }

        /// <summary>
        /// Processes the appearance of the pointer.
        /// </summary>
        [RequiresBehaviourState]
        public void Process()
        {
            TryEmitVisibilityEvent();
        }

        protected virtual void OnEnable()
        {
            TryEmitVisibilityEvent();
            UpdateRenderData();
        }

        protected virtual void OnDisable()
        {
            TryDeactivate(true);
        }

        /// <summary>
        /// Updates the <see cref="PointerElement"/>'s <see cref="GameObject"/>'s visibility and emits the <see cref="RenderDataChanged"/> event with the <see cref="GameObject"/>s used for the <see cref="PointerElement"/>s.
        /// </summary>
        protected virtual void UpdateRenderData()
        {
            pointsData.Points = activePointsCastData.Points;

            pointsData.StartPoint = GetElementRepresentation(Origin);
            pointsData.IsStartPointVisible = Origin.IsVisible;
            pointsData.RepeatedSegmentPoint = GetElementRepresentation(RepeatedSegment);
            pointsData.IsRepeatedSegmentPointVisible = RepeatedSegment.IsVisible;
            pointsData.EndPoint = GetElementRepresentation(Destination);
            pointsData.IsEndPointVisible = Destination.IsVisible;

            TryDeactivateElementObject(Origin.ValidElementContainer);
            TryDeactivateElementObject(Origin.InvalidElementContainer);
            TryDeactivateElementObject(RepeatedSegment.ValidElementContainer);
            TryDeactivateElementObject(RepeatedSegment.InvalidElementContainer);
            TryDeactivateElementObject(Destination.ValidElementContainer);
            TryDeactivateElementObject(Destination.InvalidElementContainer);

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
            if ((!isActiveAndEnabled && !ignoreBehaviourState) || !IsActivated)
            {
                return;
            }

            UpdateRenderData();
            TryEmitExit(previousPointsCastData);
            Deactivated?.Invoke(HoverTarget);
            IsActivated = false;
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
            if (activePointsCastData.HitData?.transform != null)
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
            if (IsVisible == wasPreviouslyVisible)
            {
                return;
            }

            wasPreviouslyVisible = IsVisible;

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
        /// Returns the <see cref="GameObject"/> used for a specific <see cref="PointerElement"/> based on the current <see cref="ObjectPointer"/> state.
        /// </summary>
        /// <param name="element">The <see cref="PointerElement"/> to return a <see cref="GameObject"/> for.</param>
        /// <returns>A <see cref="GameObject"/> to represent <paramref name="element"/>.</returns>
        protected virtual GameObject GetElementRepresentation(PointerElement element)
        {
            bool isValid = activePointsCastData.HitData != null && activePointsCastData.IsValid;
            bool showValidMesh = false;
            bool showInvalidMesh = false;

            switch (element.ElementVisibility)
            {
                case PointerElement.Visibility.OnWhenPointerActivated:
                    if (!IsActivated)
                    {
                        break;
                    }
                    showValidMesh = isValid;
                    showInvalidMesh = !isValid;
                    break;
                case PointerElement.Visibility.AlwaysOn:
                    showValidMesh = true;
                    showInvalidMesh = true;
                    break;
                case PointerElement.Visibility.AlwaysOff:
                    showValidMesh = false;
                    showInvalidMesh = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(element.ElementVisibility), element.ElementVisibility, null);
            }

            element.ValidMeshContainer.SetActive(showValidMesh);
            element.InvalidMeshContainer.SetActive(showInvalidMesh);
            element.IsVisible = showValidMesh || showInvalidMesh;
            return isValid ? element.ValidElementContainer : element.InvalidElementContainer;
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
            Transform validDestinationTransform = Destination == null || Destination.ValidElementContainer == null ? null : Destination.ValidElementContainer.transform;
            Transform pointerTransform = transform;

            eventData.Transform = pointerTransform;
            eventData.PositionOverride = validDestinationTransform == null ? data.HitData?.point : validDestinationTransform.position;
            eventData.RotationOverride = validDestinationTransform == null ? Quaternion.identity : validDestinationTransform.localRotation;
            eventData.ScaleOverride = validDestinationTransform == null ? Vector3.one : validDestinationTransform.lossyScale;
            eventData.Origin = pointerTransform.position;
            eventData.Direction = pointerTransform.forward;
            eventData.CollisionData = data?.HitData ?? default;
            return eventData.Set(IsActivated, IsHovering, HoverDuration, data);
        }
    }
}
