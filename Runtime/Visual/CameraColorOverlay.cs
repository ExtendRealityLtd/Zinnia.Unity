namespace Zinnia.Visual
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using System.Collections;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.BehaviourStateRequirementMethod;
    using Malimbe.MemberChangeMethod;
    using Zinnia.Rule;
    using Zinnia.Extension;

    /// <summary>
    /// Applies a color over the valid cameras and can be used to fade the screen view.
    /// </summary>
    public class CameraColorOverlay : MonoBehaviour
    {
        /// <summary>
        /// Holds data about a <see cref="CameraColorOverlay"/> event.
        /// </summary>
        [Serializable]
        public class EventData
        {
            /// <summary>
            /// The <see cref="Color"/> being applied to the camera overlay.
            /// </summary>
            [Serialized]
            [field: DocumentedByXml]
            public Color Color { get; set; }

            public EventData Set(EventData source)
            {
                return Set(source.Color);
            }

            public EventData Set(Color color)
            {
                Color = color;
                return this;
            }

            public void Clear()
            {
                Set(default(Color));
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
        /// The rules to determine which scene cameras to apply the overlay to.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public RuleContainer CameraValidity { get; set; }
        /// <summary>
        /// The <see cref="Color"/> of the overlay.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public Color OverlayColor { get; set; } = Color.black;
        /// <summary>
        /// The <see cref="Material"/> to use for the overlay.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public Material OverlayMaterial { get; set; }
        /// <summary>
        /// The duration of time to apply the overlay <see cref="Color"/>.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public float AddDuration { get; set; }
        /// <summary>
        /// The duration of time to remove the overlay <see cref="Color"/>.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public float RemoveDuration { get; set; } = 1f;
        /// <summary>
        /// The duration of time to wait once the overlay <see cref="Color"/> is applied before it is removed.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public float AppliedDuration { get; set; }

        /// <summary>
        /// Emitted when the <see cref="AddColorOverlay"/> method is called.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent Added = new UnityEvent();
        /// <summary>
        /// Emitted when the <see cref="AddColorOverlay"/> target overlay <see cref="Color"/> is reached.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent AddTransitioned = new UnityEvent();
        /// <summary>
        /// Emitted when the <see cref="RemoveColorOverlay"/> method is called.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent Removed = new UnityEvent();
        /// <summary>
        /// Emitted when the <see cref="RemoveColorOverlay"/> target overlay <see cref="Color"/> is reached.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent RemoveTransitioned = new UnityEvent();
        /// <summary>
        /// Emitted when an overlay <see cref="Color"/> has changed from the previous render frame.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent Changed = new UnityEvent();

        /// <summary>
        /// Whether an overlay add transition is in progress.
        /// </summary>
        public bool IsAddTransitioning { get; protected set; }
        /// <summary>
        /// Whether an overlay remove transition is in progress.
        /// </summary>
        public bool IsRemoveTransitioning { get; protected set; }

        /// <summary>
        /// The target duration to process the color change for.
        /// </summary>
        protected float targetDuration;
        /// <summary>
        /// A copy of the <see cref="OverlayMaterial"/> to apply the transition overlay color on.
        /// </summary>
        protected Material workingMaterial;
        /// <summary>
        /// The target color to apply to the camera overlay during the process.
        /// </summary>
        protected Color targetColor = new Color(0f, 0f, 0f, 0f);
        /// <summary>
        /// The current color of the camera overlay during the process.
        /// </summary>
        protected Color currentColor = new Color(0f, 0f, 0f, 0f);
        /// <summary>
        /// The difference in color of the camera overlay during the process.
        /// </summary>
        protected Color deltaColor = new Color(0f, 0f, 0f, 0f);
        /// <summary>
        /// The routine for handling the fade in and out of the camera overlay.
        /// </summary>
        protected Coroutine blinkRoutine;
        /// <summary>
        /// Delays the reset of <see cref="blinkRoutine"/> by <see cref="AddDuration"/> plus <see cref="AppliedDuration"/> seconds.
        /// </summary>
        protected WaitForSeconds resetDelayYieldInstruction;
        /// <summary>
        /// The event data to be emitted throughout the process.
        /// </summary>
        protected readonly EventData eventData = new EventData();

        /// <summary>
        /// Sets the current <see cref="OverlayColor"/> and <see cref="AddDuration"/> with the given parameters and applies the <see cref="OverlayColor"/> to the cameras via <see cref="CameraValidity"/> over the given <see cref="AddDuration"/>.
        /// </summary>
        /// <param name="overlayColor">The <see cref="Color"/> to apply to the overlay.</param>
        /// <param name="addDuration">The duration of time to apply the overlay <see cref="Color"/>.</param>
        [RequiresBehaviourState]
        public virtual void AddColorOverlay(Color overlayColor, float addDuration)
        {
            OverlayColor = overlayColor;
            AddDuration = addDuration;
            AddColorOverlay();
        }

        /// <summary>
        /// Applies the <see cref="OverlayColor"/> to the cameras via <see cref="CameraValidity"/> over the given <see cref="AddDuration"/>.
        /// </summary>
        [RequiresBehaviourState]
        public virtual void AddColorOverlay()
        {
            ApplyColorOverlay(OverlayColor, AddDuration);
        }

        /// <summary>
        /// Removes the <see cref="OverlayColor"/> to the cameras via <see cref="CameraValidity"/> over the given <see cref="RemoveDuration"/>.
        /// </summary>
        [RequiresBehaviourState]
        public virtual void RemoveColorOverlay()
        {
            ApplyColorOverlay(Color.clear, RemoveDuration);
            Removed?.Invoke(eventData.Set(Color.clear));
        }

        /// <summary>
        /// Applies the <see cref="OverlayColor"/> to the cameras via <see cref="CameraValidity"/> over the given <see cref="AddDuration"/> then waits for the given <see cref="AppliedDuration"/> then removes the <see cref="OverlayColor"/> over the given <see cref="RemoveDuration"/>.
        /// </summary>
        [RequiresBehaviourState]
        public virtual void Blink()
        {
            ApplyColorOverlay(OverlayColor, AddDuration);
            blinkRoutine = StartCoroutine(ResetBlink());
        }

        protected virtual void OnEnable()
        {
            CopyMaterialOverlayToWorking();
            OnAfterCheckDelayChange();
            Camera.onPostRender += PostRender;
        }

        protected virtual void OnDisable()
        {
            CancelBlinkRoutine();
            Camera.onPostRender -= PostRender;
        }

        /// <summary>
        /// Applies the given <see cref="Color"/> to the cameras via <see cref="CameraValidity"/> over the given duration.
        /// </summary>
        /// <param name="newColor"><see cref="Color"/> to apply to the overlay.</param>
        /// <param name="duration">The duration over which the <see cref="Color"/> is applied.</param>
        protected virtual void ApplyColorOverlay(Color newColor, float duration)
        {
            CancelBlinkRoutine();

            if (newColor != targetColor || !duration.ApproxEquals(targetDuration))
            {
                targetDuration = duration;
                targetColor = newColor;
                if (duration > 0.0f)
                {
                    deltaColor = (targetColor - currentColor) / duration;
                }
                else
                {
                    currentColor = newColor;
                }

                IsAddTransitioning = false;
                IsRemoveTransitioning = false;
                if (newColor != Color.clear)
                {
                    IsAddTransitioning = true;
                    Added?.Invoke(eventData.Set(newColor));
                }
                else
                {
                    IsRemoveTransitioning = true;
                }
            }
        }

        /// <summary>
        /// Waits for the given wait duration and then removes the <see cref="Color"/> overlay over the remove duration.
        /// </summary>
        /// <returns>An Enumerator to manage the running of the Coroutine.</returns>
        protected virtual IEnumerator ResetBlink()
        {
            yield return resetDelayYieldInstruction;
            RemoveColorOverlay();
        }

        /// <summary>
        /// Cancels any existing running <see cref="ResetBlink(float)"/> Coroutine.
        /// </summary>
        protected virtual void CancelBlinkRoutine()
        {
            if (blinkRoutine != null)
            {
                StopCoroutine(blinkRoutine);
            }
        }

        /// <summary>
        /// The moment before <see cref="Camera"/> render that will apply the <see cref="Color"/> overlay.
        /// </summary>
        /// <param name="sceneCamera">The <see cref="Camera"/> to apply onto.</param>
        protected virtual void PostRender(Camera sceneCamera)
        {
            if (!CameraValidity.Accepts(sceneCamera))
            {
                return;
            }

            if (currentColor != targetColor)
            {
                if (Mathf.Abs(currentColor.a - targetColor.a) < Mathf.Abs(deltaColor.a) * Time.deltaTime)
                {
                    currentColor = targetColor;
                    deltaColor = new Color(0, 0, 0, 0);
                }
                else
                {
                    currentColor += deltaColor * Time.deltaTime;
                }
                Changed?.Invoke(eventData.Set(currentColor));
            }
            else if (IsAddTransitioning)
            {
                AddTransitioned?.Invoke(eventData.Set(currentColor));
                IsAddTransitioning = false;
            }
            else if (IsRemoveTransitioning)
            {
                RemoveTransitioned?.Invoke(eventData.Set(currentColor));
                IsRemoveTransitioning = false;
            }

            if (currentColor.a > 0f && workingMaterial != null)
            {
                currentColor.a = (targetColor.a > currentColor.a && currentColor.a > 0.98f ? 1f : currentColor.a);
                workingMaterial.color = currentColor;
                workingMaterial.SetPass(0);
                GL.PushMatrix();
                GL.LoadOrtho();
                GL.Color(workingMaterial.color);
                GL.Begin(GL.QUADS);
                GL.Vertex3(0f, 0f, 0.9999f);
                GL.Vertex3(0f, 1f, 0.9999f);
                GL.Vertex3(1f, 1f, 0.9999f);
                GL.Vertex3(1f, 0f, 0.9999f);
                GL.End();
                GL.PopMatrix();
            }
        }

        /// <summary>
        /// Copies the <see cref="OverlayMaterial"/> data to the <see cref="workingMaterial"/> data.
        /// </summary>
        protected virtual void CopyMaterialOverlayToWorking()
        {
            if (Application.isPlaying)
            {
                Destroy(workingMaterial);
            }
            else
            {
                DestroyImmediate(workingMaterial);
            }

            if (OverlayMaterial != null)
            {
                workingMaterial = new Material(OverlayMaterial);
            }
        }

        /// <summary>
        /// Called after <see cref="OverlayMaterial"/> has been changed.
        /// </summary>
        [CalledAfterChangeOf(nameof(OverlayMaterial))]
        protected virtual void OnAfterOverlayMaterialChange()
        {
            CopyMaterialOverlayToWorking();
        }

        /// <summary>
        /// Called after <see cref="AddDuration"/> or <see cref="AppliedDuration"/> have been changed.
        /// </summary>
        [CalledAfterChangeOf(nameof(AddDuration)), CalledAfterChangeOf(nameof(AppliedDuration))]
        protected virtual void OnAfterCheckDelayChange()
        {
            resetDelayYieldInstruction = new WaitForSeconds(AddDuration + AppliedDuration);
        }
    }
}