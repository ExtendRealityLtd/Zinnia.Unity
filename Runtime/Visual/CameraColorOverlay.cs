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
    using Zinnia.Extension;
    using Zinnia.Data.Type;

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
        /// A <see cref="Camera"/> collection to apply the color overlay to.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public CameraList ValidCameras { get; set; }
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
        /// Emitted when an overlay <see cref="Color"/> is added.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent Added = new UnityEvent();
        /// <summary>
        /// Emitted when an overlay <see cref="Color"/> is removed.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent Removed = new UnityEvent();
        /// <summary>
        /// Emitted when an overlay <see cref="Color"/> has changed from the previous render frame.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent Changed = new UnityEvent();

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
        /// The event data to be emitted throughout the process.
        /// </summary>
        protected readonly EventData eventData = new EventData();

        /// <summary>
        /// Applies the <see cref="overlayColor"/> to the <see cref="validCameras"/> over the given <see cref="addDuration"/>.
        /// </summary>
        [RequiresBehaviourState]
        public virtual void AddColorOverlay()
        {
            AddColorOverlay(OverlayColor, AddDuration);
        }

        /// <summary>
        /// Removes the <see cref="overlayColor"/> to the <see cref="validCameras"/> over the given <see cref="removeDuration"/>.
        /// </summary>
        [RequiresBehaviourState]
        public virtual void RemoveColorOverlay()
        {
            AddColorOverlay(Color.clear, RemoveDuration);
            Removed?.Invoke(eventData.Set(Color.clear));
        }

        /// <summary>
        /// Applies the <see cref="overlayColor"/> to the <see cref="validCameras"/> over the given <see cref="addDuration"/> then waits for the given <see cref="appliedDuration"/> then removes the <see cref="overlayColor"/> over the given <see cref="removeDuration"/>.
        /// </summary>
        [RequiresBehaviourState]
        public virtual void Blink()
        {
            AddColorOverlay(OverlayColor, AddDuration);
            blinkRoutine = StartCoroutine(ResetBlink(AddDuration + AppliedDuration));
        }

        protected virtual void OnEnable()
        {
            CopyMaterialOverlayToWorking();
            Camera.onPostRender += PostRender;
        }

        protected virtual void OnDisable()
        {
            CancelBlinkRoutine();
            Camera.onPostRender -= PostRender;
        }

        /// <summary>
        /// Applies the given <see cref="Color"/> to the <see cref="validCameras"/> over the given duration.
        /// </summary>
        /// <param name="newColor"><see cref="Color"/> to apply to the overlay.</param>
        /// <param name="duration">The duration over which the <see cref="Color"/> is applied.</param>
        protected virtual void AddColorOverlay(Color newColor, float duration)
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

                if (newColor != Color.clear)
                {
                    Added?.Invoke(eventData.Set(newColor));
                }
            }
        }

        /// <summary>
        /// Waits for the given wait duration and then removes the <see cref="Color"/> overlay over the remove duration.
        /// </summary>
        /// <param name="givenWaitDuration">The duration in which to wait before removing the <see cref="Color"/> overlay.</param>
        /// <returns>An Enumerator to manage the running of the Coroutine.</returns>
        protected virtual IEnumerator ResetBlink(float givenWaitDuration)
        {
            yield return new WaitForSeconds(givenWaitDuration);
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
        /// <param name="cam">The <see cref="Camera"/> to apply onto.</param>
        protected virtual void PostRender(Camera cam)
        {
            if (ValidCameras == null || !ValidCameras.cameras.Contains(cam))
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
            Destroy(workingMaterial);
            workingMaterial = new Material(OverlayMaterial);
        }

        /// <summary>
        /// Called after <see cref="OverlayMaterial"/> has been changed.
        /// </summary>
        [CalledAfterChangeOf(nameof(OverlayMaterial)), RequiresBehaviourState]
        protected virtual void OnAfterOverlayMaterialChanged()
        {
            CopyMaterialOverlayToWorking();
        }
    }
}