namespace VRTK.Core.Prefabs.Pointer
{
    using UnityEngine;
    using VRTK.Core.Utility;

    /// <summary>
    /// The StraightPointer is a pointer implementation that casts a direct line from the point of origin until it collides with a valid target or reaches it's maximum length.
    /// </summary>
    public class StraightPointer : BasePointer
    {
        [Header("Pointer Settings")]

        [Tooltip("The maximum length to cast the pointer.")]
        public float maximumLength = 100f;
        [Tooltip("The GameObject to represent the pointer's tracer when it is colliding with a valid object.")]
        public GameObject validTracer;
        [Tooltip("The GameObject to represent the pointer's cursor when it is colliding with a valid object.")]
        public GameObject validCursor;
        [Tooltip("The GameObject to represent the pointer's tracer when it is colliding with an invalid object or not colliding at all.")]
        public GameObject invalidTracer;
        [Tooltip("The GameObject to represent the pointer's cursor when it is colliding with an invalid object or not colliding at all.")]
        public GameObject invalidCursor;
        [Tooltip("Determines when the pointer tracer GameObject should be seen.")]
        public VisibilityType tracerVisibility = VisibilityType.OnWhenActive;
        [Tooltip("Determines when the pointer cursor GameObject should be seen.")]
        public VisibilityType cursorVisibility = VisibilityType.OnWhenActive;
        [Tooltip("Determines if the pointer should be automatically activated when the script is enabled.")]
        public bool activateOnEnable;

        /// <summary>
        /// The IsVisible method determines if the pointer is currently visible regardless of active state.
        /// </summary>
        /// <returns>Returns `true` if the pointer is currently visible.</returns>
        public override bool IsVisible()
        {
            if (!enabled)
            {
                return false;
            }

            if (tracerVisibility == VisibilityType.AlwaysOn || cursorVisibility == VisibilityType.AlwaysOn)
            {
                return true;
            }

            if (tracerVisibility == VisibilityType.AlwaysOff && cursorVisibility == VisibilityType.AlwaysOff)
            {
                return false;
            }

            return Active;
        }

        /// <summary>
        /// The Deactivate method turns off the pointer.
        /// </summary>
        public override void Deactivate()
        {
            base.Deactivate();
            SetElementState();
        }

        protected virtual void OnEnable()
        {
            Active = activateOnEnable;
            if (Active)
            {
                Activate();
            }
            SetElementState();
        }

        protected virtual void OnDisable()
        {
            SetElementState();
        }

        protected virtual void Update()
        {
            if (IsVisible())
            {
                float tracerLength = CalculateTracerLength();
                FormatElements(tracerLength);
                CheckValidity();
            }
            SetElementState();
            CursorData = (validCursor.transform != null ? validCursor.transform : null);
        }

        /// <summary>
        /// The SetElementState method determines the active state of the pointer elements.
        /// </summary>
        protected virtual void SetElementState()
        {
            SetElementVisible(tracerVisibility, validTracer, invalidTracer);
            SetElementVisible(cursorVisibility, validCursor, invalidCursor);
        }

        /// <summary>
        /// The SetElementVisible method determines the visibility state of the given element.
        /// </summary>
        /// <param name="elementType">The VisibilityType parameter of the element to determine visibility for.</param>
        /// <param name="validElement">The GameObject to set active is the element should be valid.</param>
        /// <param name="invalidElement">The GameObject to set active is the element should be invalid.</param>
        protected virtual void SetElementVisible(VisibilityType elementType, GameObject validElement, GameObject invalidElement)
        {
            if (IsElementVisible(elementType))
            {
                SetElementActive(validElement, Valid);
                SetElementActive(invalidElement, !Valid);
            }
            else
            {
                SetElementActive(validElement, false);
                SetElementActive(invalidElement, false);
            }
        }

        /// <summary>
        /// The SetElementActive method determines the active state of the given element.
        /// </summary>
        /// <param name="element">The GameObject of which to set it's active state.</param>
        /// <param name="state">The state to set on the GameObject.</param>
        protected virtual void SetElementActive(GameObject element, bool state)
        {
            if (element != null)
            {
                element.SetActive(state);
            }
        }

        /// <summary>
        /// The CalculateTracerLength method determines the length of the pointer by casting a ray in the forward direction of the Transform.
        /// </summary>
        /// <returns>A float representing the distance the length has reached.</returns>
        protected virtual float CalculateTracerLength()
        {
            Ray tracerRaycast = new Ray(transform.position, transform.forward);
            RaycastHit collision;
            Valid = PhysicsCast.Raycast(physicsCast, tracerRaycast, out collision, maximumLength, Physics.IgnoreRaycastLayer);
            CollisionData = collision;
            return (Valid && CollisionData.distance < maximumLength ? CollisionData.distance : maximumLength);
        }

        /// <summary>
        /// The FormatElements method formats the pointer's tracer and cursor shape and position based on the desired length.
        /// </summary>
        /// <param name="tracerLength">The length of the pointer to format the relevant elements to.</param>
        protected virtual void FormatElements(float tracerLength)
        {
            SetElementsPosition(validTracer, validCursor, tracerLength);
            SetElementsPosition(invalidTracer, invalidCursor, tracerLength);
        }

        /// <summary>
        /// The SetElementsPosition formats the relevant elements of the pointer in local space.
        /// </summary>
        /// <param name="tracer">The GameObject representing the pointer's tracer.</param>
        /// <param name="cursor">The GameObject representing the pointer's cursor.</param>
        /// <param name="tracerLength">The length of the pointer tracer.</param>
        protected virtual void SetElementsPosition(GameObject tracer, GameObject cursor, float tracerLength)
        {
            float tracerPosition = tracerLength / 2f;
            if (tracer != null)
            {
                tracer.transform.localScale = new Vector3(tracer.transform.localScale.x, tracer.transform.localScale.y, tracerLength);
                tracer.transform.localPosition = Vector3.forward * tracerPosition;
            }

            if (cursor != null)
            {
                cursor.transform.localPosition = Vector3.forward * tracerLength;
            }
        }
    }
}