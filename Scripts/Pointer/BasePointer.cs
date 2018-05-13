namespace VRTK.Core.Pointer
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using VRTK.Core.Utility;
    using VRTK.Core.Data.Type;

    /// <summary>
    /// The PointerData contains information about the current pointer state.
    /// </summary>
    public class PointerData : SurfaceData
    {
        /// <summary>
        /// Determines if the pointer is currently in it's activated state.
        /// </summary>
        public bool isActive;
        /// <summary>
        /// Determines if the pointer is in it's valid state.
        /// </summary>
        public bool isValid;
        /// <summary>
        /// The duration that the pointer has been hovering over it's current target.
        /// </summary>
        public float hoverDuration;
    }

    /// <summary>
    /// The BasePointer forms the basis for all pointer types.
    /// </summary>
    public abstract class BasePointer : MonoBehaviour
    {
        /// <summary>
        /// The visibility of the pointer element.
        /// </summary>
        public enum VisibilityType
        {
            /// <summary>
            /// The element will only be visible when the pointer is in the active state.
            /// </summary>
            OnWhenActive,
            /// <summary>
            /// The element will always be visible regardless of the pointer state.
            /// </summary>
            AlwaysOn,
            /// <summary>
            /// The element will never be visible regardless of the pointer state.
            /// </summary>
            AlwaysOff
        }

        [Header("Base Settings")]

        [Tooltip("An optional ExclusionRule to determine valid and invalid targets based on the set rules.")]
        public ExclusionRule exclusionRule;
        [Tooltip("An optional custom PhysicsCast object to affect the Cast of the pointer.")]
        public PhysicsCast physicsCast;

        /// <summary>
        /// The PointerUnityEvent emits an event with the PointerData payload and the sender object.
        /// </summary>
        [Serializable]
        public class PointerUnityEvent : UnityEvent<PointerData, object>
        {
        };

        [Header("Pointer Events")]

        /// <summary>
        /// The Activated event is emitted when the pointer becomes active.
        /// </summary>
        public PointerUnityEvent Activated = new PointerUnityEvent();
        /// <summary>
        /// The Deactivated event is emitted when the pointer becomes inactive.
        /// </summary>
        public PointerUnityEvent Deactivated = new PointerUnityEvent();
        /// <summary>
        /// The Entered event is emitted when the pointer collides with a new target.
        /// </summary>
        public PointerUnityEvent Entered = new PointerUnityEvent();
        /// <summary>
        /// The Exited event is emitted when the pointer stops colliding with an existing target.
        /// </summary>
        public PointerUnityEvent Exited = new PointerUnityEvent();
        /// <summary>
        /// The Hovering event is emitted when the pointer changes its hovering position ove an existing target.
        /// </summary>
        public PointerUnityEvent Hovering = new PointerUnityEvent();
        /// <summary>
        /// 
        /// </summary>
        public PointerUnityEvent Selected = new PointerUnityEvent();

        public abstract bool IsVisible();

        /// <summary>
        /// Reports the active state of the pointer.
        /// </summary>
        public bool Active
        {
            get;
            protected set;
        }

        /// <summary>
        /// Reports the valid state of the pointer.
        /// </summary>
        public bool Valid
        {
            get;
            protected set;
        }

        /// <summary>
        /// Reports hover duration of the pointer over the current target.
        /// </summary>
        public float HoverDuration
        {
            get;
            protected set;
        }

        /// <summary>
        /// Stores the RayCastHit information of the current pointer collision.
        /// </summary>
        public RaycastHit CollisionData
        {
            get;
            protected set;
        }

        /// <summary>
        /// Stores the Transform information of the cursor state.
        /// </summary>
        public Transform CursorData
        {
            protected get;
            set;
        }

        protected RaycastHit cachedCollision;

        /// <summary>
        /// The Activate method turns on the pointer.
        /// </summary>
        public virtual void Activate()
        {
            Active = true;
            OnActivated(GetPayload(CursorData, CollisionData));
        }

        /// <summary>
        /// The Deactivate method turns off the pointer.
        /// </summary>
        public virtual void Deactivate()
        {
            Active = false;
            TryEmitExit();
            OnDeactivated(GetPayload(CursorData, cachedCollision));
        }

        /// <summary>
        /// The Select method emits the selected event if the pointer is active and hovering over a valid target.
        /// </summary>
        public virtual void Select()
        {
            if (Active && Valid)
            {
                OnSelected(GetPayload(CursorData, CollisionData));
            }
        }

        /// <summary>
        /// The IsHovering method returns whether the pointer is currently hovering over a target.
        /// </summary>
        /// <returns>Returns `true` if the pointer is currently hovering over a target.</returns>
        public virtual bool IsHovering()
        {
            return (HoverDuration > 0f);
        }

        /// <summary>
        /// The IsElementVisible method determines if a given VisibilityType parameter should be considered visible.
        /// </summary>
        /// <param name="elementType">The VisibilityType to check for.</param>
        /// <returns>Returns `true` if the VisibilityType given is considered visible.</returns>
        public virtual bool IsElementVisible(VisibilityType elementType)
        {
            if (!enabled)
            {
                return false;
            }

            switch (elementType)
            {
                case VisibilityType.AlwaysOn:
                    return true;
                case VisibilityType.AlwaysOff:
                    return false;
            }
            return Active;
        }

        protected virtual void OnActivated(PointerData e)
        {
            Activated?.Invoke(e, this);
        }

        protected virtual void OnDeactivated(PointerData e)
        {
            Deactivated?.Invoke(e, this);
        }

        protected virtual void OnEntered(PointerData e)
        {
            Entered?.Invoke(e, this);
        }

        protected virtual void OnExited(PointerData e)
        {
            Exited?.Invoke(e, this);
        }

        protected virtual void OnHovering(PointerData e)
        {
            Hovering?.Invoke(e, this);
        }

        protected virtual void OnSelected(PointerData e)
        {
            Selected?.Invoke(e, this);
        }

        /// <summary>
        /// The CheckValidity method checks the valid state of the pointer collision and determines if the pointer is hovering over a valid target.
        /// </summary>
        protected virtual void CheckValidity()
        {
            if (Valid)
            {
                if (cachedCollision.transform == null || CollisionData.transform != cachedCollision.transform)
                {
                    TryEmitExit();
                    OnEntered(GetPayload(CursorData, CollisionData));
                }
                cachedCollision = CollisionData;
                HoverDuration += Time.deltaTime;
                OnHovering(GetPayload(CursorData, CollisionData));
            }
            else if (cachedCollision.transform != null)
            {
                TryEmitExit();
            }

            if (exclusionRule != null && CollisionData.transform != null)
            {
                Valid = !ExclusionRule.ShouldExclude(CollisionData.transform.gameObject, exclusionRule);
            }
        }

        /// <summary>
        /// The TryEmitExit method checks to see if the pointer is not currently colliding with a valid target and emits the Exit event.
        /// </summary>
        protected virtual void TryEmitExit()
        {
            if (cachedCollision.transform != null)
            {
                HoverDuration = 0f;
                OnExited(GetPayload(CursorData, cachedCollision));
                cachedCollision = new RaycastHit();
            }
        }

        /// <summary>
        /// The GetPayload method builds a valid Pointer Payload to use in the events.
        /// </summary>
        /// <param name="givenCursorData">A Transform containing information about the pointer cursor Transform.</param>
        /// <param name="givenCollisionData">A RaycastHit containing information about the current pointer collision target.</param>
        /// <returns>A PointerPayload of the pointer's current state.</returns>
        protected virtual PointerData GetPayload(Transform givenCursorData, RaycastHit givenCollisionData)
        {
            PointerData e = new PointerData
            {
                transform = transform,
                positionOverride = (givenCursorData != null ? (Vector3?)givenCursorData.position : null),
                rotationOverride = (givenCursorData != null ? (Quaternion?)givenCursorData.localRotation : null),
                localScaleOverride = (givenCursorData != null ? (Vector3?)givenCursorData.localScale : null),
                origin = transform.position,
                direction = transform.forward,
                CollisionData = givenCollisionData,
                isActive = Active,
                isValid = Valid,
                hoverDuration = HoverDuration
            };
            return e;
        }
    }
}