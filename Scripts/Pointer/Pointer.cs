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
    /// Contains information about the current <see cref="Pointer"/> state.
    /// </summary>
    public class PointerData : SurfaceData
    {
        /// <summary>
        /// Whether the <see cref="Pointer"/> is currently activated.
        /// </summary>
        public bool isActive;
        /// <summary>
        /// Whether the <see cref="Pointer"/> is currently hovering over a target.
        /// </summary>
        public bool isHovering;
        /// <summary>
        /// The duration that the <see cref="Pointer"/> has been hovering over it's current target.
        /// </summary>
        public float hoverDuration;
        /// <summary>
        /// The points cast data given to the <see cref="Pointer"/>.
        /// </summary>
        public PointsCastData pointsCastData;
    }

    /// <summary>
    /// Allows pointing at objects and notifies when a target is hit, continues to be hit or stops being hit by listening to a <see cref="PointsCast"/>.
    /// </summary>
    public class Pointer : MonoBehaviour
    {
        /// <summary>
        /// Describes an element of the rendered <see cref="Pointer"/>.
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
                /// The <see cref="Element"/> will only be visible when the <see cref="Pointer"/> is activated.
                /// </summary>
                OnWhenPointerActivated,
                /// <summary>
                /// The <see cref="Element"/> will always be visible regardless of the <see cref="Pointer"/> state.
                /// </summary>
                AlwaysOn,
                /// <summary>
                /// The <see cref="Element"/> will never be visible regardless of the <see cref="Pointer"/> state.
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
        /// Defines the event with the <see cref="PointerData"/> state and sender <see cref="object"/>.
        /// </summary>
        /// <remarks>The <see cref="Pointer"/> data is <see langword="null"/> in case the <see cref="Pointer"/> isn't hitting any target.</remarks>
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
        /// Determines if the <see cref="Pointer"/> should be automatically activated when the script is enabled.
        /// </summary>
        [Tooltip("Determines if the pointer should be automatically activated when the script is enabled.")]
        public bool activateOnEnable;

        /// <summary>
        /// Represents the origin, i.e. the first rendered point.
        /// </summary>
        [Tooltip("Represents the origin, i.e. the first rendered point.")]
        public Element origin;
        /// <summary>
        /// Represents the segments between <see cref="origin"/> and <see cref="destination"/>. This will get cloned to create all the segments.
        /// </summary>
        [Tooltip("Represents the segments between origin and destination. This will get cloned to create all the segments.")]
        public Element repeatedSegment;
        /// <summary>
        /// Represents the destination, i.e. the last rendered point.
        /// </summary>
        [Tooltip("Represents the destination, i.e. the last rendered point.")]
        public Element destination;

        /// <summary>
        /// The Activated event is emitted when the <see cref="Pointer"/> becomes active.
        /// </summary>
        public PointerUnityEvent Activated = new PointerUnityEvent();
        /// <summary>
        /// The Deactivated event is emitted when the <see cref="Pointer"/> becomes inactive.
        /// </summary>
        public PointerUnityEvent Deactivated = new PointerUnityEvent();
        /// <summary>
        /// Emitted when the <see cref="Pointer"/> collides with a new target.
        /// </summary>
        public PointerUnityEvent Entered = new PointerUnityEvent();
        /// <summary>
        /// Emitted when the <see cref="Pointer"/> stops colliding with an existing target.
        /// </summary>
        public PointerUnityEvent Exited = new PointerUnityEvent();
        /// <summary>
        /// Emitted when the <see cref="Pointer"/> changes its hovering position over an existing target.
        /// </summary>
        public PointerUnityEvent Hovering = new PointerUnityEvent();
        /// <summary>
        /// Emitted whenever <see cref="Select"/> is called.
        /// </summary>
        public PointerUnityEvent Selected = new PointerUnityEvent();
        /// <summary>
        /// Emitted when the <see cref="Pointer"/> elements change.
        /// </summary>
        public PointsRendererUnityEvent RenderDataChanged = new PointsRendererUnityEvent();
        /// <summary>
        /// Emitted when the <see cref="Pointer"/> becomes visible.
        /// </summary>
        public PointerUnityEvent BecameVisible = new PointerUnityEvent();
        /// <summary>
        /// Emitted when the <see cref="Pointer"/> becomes hidden.
        /// </summary>
        public PointerUnityEvent BecameHidden = new PointerUnityEvent();

        /// <summary>
        /// The activation state of the <see cref="Pointer"/>.
        /// </summary>
        public bool ActivationState
        {
            get;
            protected set;
        }

        /// <summary>
        /// Reports hover duration of the <see cref="Pointer"/> over the current target.
        /// </summary>
        public float HoverDuration
        {
            get;
            protected set;
        }

        /// <summary>
        /// Whether the <see cref="Pointer"/> is currently hovering over a target.
        /// </summary>
        /// <returns><see langword="true"/> if the <see cref="Pointer"/> is currently hovering over a target, <see langword="false"/> otherwise.</returns>
        public bool IsHovering => HoverDuration > 0f;

        /// <summary>
        /// Whether any <see cref="Element"/> of the <see cref="Pointer"/> is currently visible.
        /// </summary>
        public bool IsVisible
        {
            get
            {
                if (!enabled)
                {
                    return false;
                }

                if (origin.visibility == Element.Visibility.AlwaysOff && repeatedSegment.visibility == Element.Visibility.AlwaysOff && destination.visibility == Element.Visibility.AlwaysOff)
                {
                    return false;
                }

                if (origin.visibility == Element.Visibility.AlwaysOn || repeatedSegment.visibility == Element.Visibility.AlwaysOn || destination.visibility == Element.Visibility.AlwaysOn)
                {
                    return true;
                }

                return ActivationState;
            }
        }

        protected PointsCastData previousPointsCastData;
        protected bool? previousVisibility;

        /// <summary>
        /// The current state of the <see cref="Pointer"/>.
        /// </summary>
        /// <returns>A <see cref="PointerData"/> object if the <see cref="Pointer"/> is currently hitting a target, <see langword="null"/> otherwise.</returns>
        public PointerData GetCurrentState()
        {
            return previousPointsCastData == null ? null : GetPayload(previousPointsCastData);
        }

        /// <summary>
        /// The Activate method turns on the <see cref="Pointer"/>.
        /// </summary>
        public virtual void Activate()
        {
            ActivationState = true;
            UpdateRenderData();
            Activated?.Invoke(GetCurrentState(), this);
        }

        /// <summary>
        /// The Deactivate method turns off the <see cref="Pointer"/>.
        /// </summary>
        public virtual void Deactivate()
        {
            ActivationState = false;
            TryEmitExit(previousPointsCastData);
            UpdateRenderData();
            Deactivated?.Invoke(GetCurrentState(), this);
        }

        /// <summary>
        /// Gets the current <see cref="Pointer"/> state and emits it through <see cref="Selected"/>.
        /// </summary>
        public virtual void Select()
        {
            Selected?.Invoke(ActivationState ? GetCurrentState() : null, this);
        }

        /// <summary>
        /// Handles the provided data to transition state and emit the <see cref="Pointer"/> events.
        /// </summary>
        /// <param name="data">The data describing the results of the most recent cast.</param>
        /// <param name="initiator">The initiator of this method.</param>
        public virtual void HandleData(PointsCastData data, object initiator = null)
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
            }
            else
            {
                TryEmitExit(data);
            }

            previousPointsCastData = data;
            UpdateRenderData();
        }

        protected virtual void OnEnable()
        {
            ActivationState = activateOnEnable;
            TryEmitVisibilityEvent();
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
        /// Checks to see if the <see cref="Pointer"/> is not currently colliding with a valid target and emits the <see cref="Exited"/> event.
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
                BecameVisible?.Invoke(GetCurrentState(), this);
            }
            else
            {
                BecameHidden?.Invoke(GetCurrentState(), this);
            }
        }

        /// <summary>
        /// Returns the <see cref="GameObject"/> used for a specific <see cref="Element"/> based on the current <see cref="Pointer"/> state.
        /// </summary>
        /// <param name="element">The <see cref="Element"/> to return a <see cref="GameObject"/> for.</param>
        /// <returns>A <see cref="GameObject"/> to represent <paramref name="element"/>.</returns>
        protected virtual GameObject GetElementRepresentation(Element element)
        {
            bool isValid = previousPointsCastData?.targetHit != null;

            switch (element.visibility)
            {
                case Element.Visibility.OnWhenPointerActivated:
                    return ActivationState ? (isValid ? element.validObject : element.invalidObject) : null;
                case Element.Visibility.AlwaysOn:
                    return isValid ? element.validObject : element.invalidObject;
                case Element.Visibility.AlwaysOff:
                    return null;
                default:
                    throw new ArgumentOutOfRangeException(nameof(element.visibility), element.visibility, null);
            }
        }

        /// <summary>
        /// Builds a valid <see cref="Pointer"/> payload to use in the events.
        /// </summary>
        /// <returns>A <see cref="PointerData"/> object of the <see cref="Pointer"/>'s current state.</returns>
        protected virtual PointerData GetPayload(PointsCastData data)
        {
            Transform targetTransform = data.targetHit?.transform;
            return new PointerData
            {
                transform = transform,
                positionOverride = targetTransform == null ? (Vector3?)null : targetTransform.position,
                rotationOverride = targetTransform == null ? (Quaternion?)null : targetTransform.rotation,
                localScaleOverride = targetTransform == null ? (Vector3?)null : targetTransform.localScale,
                origin = transform.position,
                direction = transform.forward,
                CollisionData = data.targetHit ?? default(RaycastHit),
                isActive = ActivationState,
                isHovering = IsHovering,
                hoverDuration = HoverDuration,
                pointsCastData = data
            };
        }

        protected virtual void OnEntered(PointsCastData data)
        {
            Entered?.Invoke(GetPayload(data), this);
        }

        protected virtual void OnExited(PointsCastData data)
        {
            Exited?.Invoke(GetPayload(data), this);
        }

        protected virtual void OnHovering(PointsCastData data)
        {
            Hovering?.Invoke(GetPayload(data), this);
        }
    }
}