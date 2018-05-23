namespace VRTK.Core.Pointer
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using VRTK.Core.Cast;
    using VRTK.Core.Data.Type;
    using VRTK.Core.Visual;

    /// <summary>
    /// Contains information about the current <see cref="ObjectPointer"/> state.
    /// </summary>
    public class PointerData : SurfaceData
    {
        /// <summary>
        /// Whether the <see cref="ObjectPointer"/> is currently activated.
        /// </summary>
        public bool isActive;
        /// <summary>
        /// Whether the <see cref="ObjectPointer"/> is currently hovering over a target.
        /// </summary>
        public bool isHovering;
        /// <summary>
        /// The duration that the <see cref="ObjectPointer"/> has been hovering over it's current target.
        /// </summary>
        public float hoverDuration;
        /// <summary>
        /// The points cast data given to the <see cref="ObjectPointer"/>.
        /// </summary>
        public PointsCastData pointsCastData;
    }

    /// <summary>
    /// Allows pointing at objects and notifies when a target is hit, continues to be hit or stops being hit by listening to a <see cref="PointsCast"/>.
    /// </summary>
    public class ObjectPointer : MonoBehaviour
    {
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
            [Tooltip("Represents the Element when it's colliding with a valid object.")]
            public GameObject validObject;
            /// <summary>
            /// Represents the <see cref="Element"/> when it's colliding with an invalid object or not colliding at all.
            /// </summary>
            [Tooltip("Represents the Element when it's colliding with an invalid object or not colliding at all.")]
            public GameObject invalidObject;
            /// <summary>
            /// Determines when the <see cref="Element"/> is visible.
            /// </summary>
            [Tooltip("Determines when the Element is visible.")]
            public Visibility visibility = Visibility.OnWhenPointerActivated;
        }

        /// <summary>
        /// Determines if the <see cref="ObjectPointer"/> should be automatically activated when the script is enabled.
        /// </summary>
        [Tooltip("Determines if the ObjectPointer should be automatically activated when the script is enabled.")]
        public bool activateOnEnable;

        /// <summary>
        /// Represents the origin, i.e. the first rendered point.
        /// </summary>
        [Tooltip("Represents the origin, i.e. the first rendered point.")]
        public Element origin = new Element();
        /// <summary>
        /// Represents the segments between <see cref="origin"/> and <see cref="destination"/>. This will get cloned to create all the segments.
        /// </summary>
        [Tooltip("Represents the segments between origin and destination. This will get cloned to create all the segments.")]
        public Element repeatedSegment = new Element();
        /// <summary>
        /// Represents the destination, i.e. the last rendered point.
        /// </summary>
        [Tooltip("Represents the destination, i.e. the last rendered point.")]
        public Element destination = new Element();

        /// <summary>
        /// Defines the event with the <see cref="PointerData"/> state and sender <see cref="object"/>.
        /// </summary>
        /// <remarks>The <see cref="ObjectPointer"/> data is <see langword="null"/> in case the <see cref="ObjectPointer"/> isn't hitting any target.</remarks>
        [Serializable]
        public class PointerUnityEvent : UnityEvent<PointerData, object>
        {
        }

        /// <summary>
        /// Defines the event with the <see cref="PointsRendererData"/> state and sender <see cref="object"/>.
        /// </summary>
        [Serializable]
        public class PointsRendererUnityEvent : UnityEvent<PointsRendererData, object>
        {
        }

        /// <summary>
        /// Emitted when the <see cref="ObjectPointer"/> becomes active.
        /// </summary>
        public PointerUnityEvent Activated = new PointerUnityEvent();
        /// <summary>
        /// Emitted when the <see cref="ObjectPointer"/> becomes inactive.
        /// </summary>
        public PointerUnityEvent Deactivated = new PointerUnityEvent();
        /// <summary>
        /// Emitted when the <see cref="ObjectPointer"/> collides with a new target.
        /// </summary>
        public PointerUnityEvent Entered = new PointerUnityEvent();
        /// <summary>
        /// Emitted when the <see cref="ObjectPointer"/> stops colliding with an existing target.
        /// </summary>
        public PointerUnityEvent Exited = new PointerUnityEvent();
        /// <summary>
        /// Emitted when the <see cref="ObjectPointer"/> changes its hovering position over an existing target.
        /// </summary>
        public PointerUnityEvent Hovering = new PointerUnityEvent();
        /// <summary>
        /// Emitted whenever <see cref="Select"/> is called.
        /// </summary>
        public PointerUnityEvent Selected = new PointerUnityEvent();
        /// <summary>
        /// Emitted when the <see cref="ObjectPointer"/> elements change.
        /// </summary>
        public PointsRendererUnityEvent RenderDataChanged = new PointsRendererUnityEvent();
        /// <summary>
        /// Emitted when the <see cref="ObjectPointer"/> becomes visible.
        /// </summary>
        public PointerUnityEvent BecameVisible = new PointerUnityEvent();
        /// <summary>
        /// Emitted when the <see cref="ObjectPointer"/> becomes hidden.
        /// </summary>
        public PointerUnityEvent BecameHidden = new PointerUnityEvent();

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
        public PointerData HoverTarget => (previousPointsCastData == null ? null : GetPayload(previousPointsCastData));

        /// <summary>
        /// The target that the <see cref="ObjectPointer"/> has most recently selected.
        /// </summary>
        public PointerData SelectedTarget
        {
            get;
            protected set;
        }

        /// <summary>
        /// Whether the <see cref="ObjectPointer"/> is currently hovering over a target.
        /// </summary>
        /// <returns><see langword="true"/> if the <see cref="ObjectPointer"/> is currently hovering over a target, <see langword="false"/> otherwise.</returns>
        public bool IsHovering => (HoverDuration > 0f);

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

        protected PointsCastData previousPointsCastData;
        protected bool? previousVisibility;

        /// <summary>
        /// The Activate method turns on the <see cref="ObjectPointer"/>.
        /// </summary>
        public virtual void Activate()
        {
            if (isActiveAndEnabled)
            {
                ActivationState = true;
                UpdateRenderData();
                Activated?.Invoke(HoverTarget, this);
            }
        }

        /// <summary>
        /// The Deactivate method turns off the <see cref="ObjectPointer"/>.
        /// </summary>
        public virtual void Deactivate()
        {
            ActivationState = false;
            TryEmitExit(previousPointsCastData);
            UpdateRenderData();
            Deactivated?.Invoke(HoverTarget, this);
        }

        /// <summary>
        /// Gets the current <see cref="ObjectPointer"/> state and emits it through <see cref="Selected"/>.
        /// </summary>
        public virtual void Select()
        {
            if (isActiveAndEnabled)
            {
                SelectedTarget = (ActivationState ? HoverTarget : null);
                Selected?.Invoke(SelectedTarget, this);
            }
        }

        /// <summary>
        /// Handles the provided data to transition state and emit the <see cref="ObjectPointer"/> events.
        /// </summary>
        /// <param name="data">The data describing the results of the most recent cast.</param>
        /// <param name="initiator">The initiator of this method.</param>
        public virtual void HandleData(PointsCastData data, object initiator = null)
        {
            if (isActiveAndEnabled)
            {
                if (data.targetHit != null)
                {
                    Transform targetTransform = data.targetHit.Value.transform;
                    if (targetTransform != null && targetTransform != previousPointsCastData?.targetHit?.transform)
                    {
                        TryEmitExit(data);
                        OnEntered(data);
                    }

                    HoverDuration += Time.deltaTime;
                    OnHovering(data);
                    previousPointsCastData = data;
                }
                else
                {
                    TryEmitExit(data);
                }
                UpdateRenderData();
            }
        }

        protected virtual void OnEnable()
        {
            if (activateOnEnable)
            {
                Activate();
            }
            TryEmitVisibilityEvent();
            UpdateRenderData();
        }

        protected virtual void OnDisable()
        {
            Deactivate();
        }

        protected virtual void Update()
        {
            TryEmitVisibilityEvent();
        }

        protected virtual void OnEntered(PointsCastData data)
        {
            if (isActiveAndEnabled)
            {
                Entered?.Invoke(GetPayload(data), this);
            }
        }

        protected virtual void OnExited(PointsCastData data)
        {
            if (isActiveAndEnabled)
            {
                Exited?.Invoke(GetPayload(data), this);
            }
        }

        protected virtual void OnHovering(PointsCastData data)
        {
            if (isActiveAndEnabled)
            {
                Hovering?.Invoke(GetPayload(data), this);
            }
        }

        /// <summary>
        /// Updates the <see cref="Element"/>'s <see cref="GameObject"/>'s visibility and emits the <see cref="RenderDataChanged"/> event with the <see cref="GameObject"/>s used for the <see cref="Element"/>s.
        /// </summary>
        protected virtual void UpdateRenderData()
        {
            PointsRendererData data = new PointsRendererData
            {
                points = previousPointsCastData?.points ?? Array.Empty<Vector3>(),
                start = GetElementRepresentation(origin),
                repeatedSegment = GetElementRepresentation(repeatedSegment),
                end = GetElementRepresentation(destination)
            };

            GameObject[] gameObjects =
            {
                origin.validObject,
                origin.invalidObject,
                repeatedSegment.validObject,
                repeatedSegment.invalidObject,
                destination.validObject,
                destination.invalidObject
            };

            IEnumerable<GameObject> unusedGameObjects = gameObjects.Except(
                    new[]
                    {
                        data.start,
                        data.repeatedSegment,
                        data.end
                    })
                .Concat(
                    new[]
                    {
                        repeatedSegment.validObject,
                        repeatedSegment.invalidObject
                    })
                .Where(@object => @object != null);

            foreach (GameObject unusedGameObject in unusedGameObjects)
            {
                unusedGameObject.SetActive(false);
            }

            IEnumerable<GameObject> usedGameObjects = new[]
            {
                data.start,
                data.repeatedSegment,
                data.end
            }.Where(@object => @object != null);

            foreach (GameObject usedGameObject in usedGameObjects)
            {
                usedGameObject.SetActive(true);
            }

            RenderDataChanged?.Invoke(data, this);
            TryEmitVisibilityEvent();
        }

        /// <summary>
        /// Checks to see if the <see cref="ObjectPointer"/> is not currently colliding with a valid target and emits the <see cref="Exited"/> event.
        /// </summary>
        /// <param name="data">The current points cast data.</param>
        protected virtual void TryEmitExit(PointsCastData data)
        {
            if (previousPointsCastData?.targetHit?.transform == null)
            {
                return;
            }
            HoverDuration = 0f;
            OnExited(data);
            previousPointsCastData = null;
        }

        /// <summary>
        /// Emits the <see cref="BecameVisible"/> or <see cref="BecameHidden"/> event for the current <see cref="IsVisible"/> state in case that state changed.
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
                BecameVisible?.Invoke(HoverTarget, this);
            }
            else
            {
                BecameHidden?.Invoke(HoverTarget, this);
            }
        }

        /// <summary>
        /// Returns the <see cref="GameObject"/> used for a specific <see cref="Element"/> based on the current <see cref="ObjectPointer"/> state.
        /// </summary>
        /// <param name="element">The <see cref="Element"/> to return a <see cref="GameObject"/> for.</param>
        /// <returns>A <see cref="GameObject"/> to represent <paramref name="element"/>.</returns>
        protected virtual GameObject GetElementRepresentation(Element element)
        {
            bool isValid = (previousPointsCastData?.targetHit != null);

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
        /// Builds a valid <see cref="ObjectPointer"/> payload to use in the events.
        /// </summary>
        /// <returns>A <see cref="PointerData"/> object of the <see cref="ObjectPointer"/>'s current state.</returns>
        protected virtual PointerData GetPayload(PointsCastData data)
        {
            Transform targetTransform = data.targetHit?.transform;
            Transform validDestinationTransform = (destination != null && destination.validObject != null ? destination.validObject.transform : null);

            return new PointerData
            {
                transform = transform,
                positionOverride = (validDestinationTransform != null ? (Vector3?)validDestinationTransform.position : data.targetHit?.point),
                rotationOverride = (validDestinationTransform != null ? validDestinationTransform.localRotation : Quaternion.identity),
                localScaleOverride = (validDestinationTransform != null ? validDestinationTransform.localScale : Vector3.one),
                origin = transform.position,
                direction = transform.forward,
                CollisionData = data.targetHit ?? default(RaycastHit),
                isActive = ActivationState,
                isHovering = IsHovering,
                hoverDuration = HoverDuration,
                pointsCastData = data
            };
        }
    }
}