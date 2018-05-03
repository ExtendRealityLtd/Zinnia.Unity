namespace VRTK.Core.Visual
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// The CameraColorOverlay applies a color over the valid cameras and can be used to fade the screen view.
    /// </summary>
    public class CameraColorOverlay : MonoBehaviour
    {
        [Tooltip("An array of cameras which to apply the color overlay to.")]
        public List<Camera> validCameras = new List<Camera>();
        [Tooltip("The color of the overlay.")]
        public Color overlayColor = Color.black;
        [Tooltip("The material to use for the overlay.")]
        public Material overlayMaterial;
        [Tooltip("The duration of time to apply the overlay color.")]
        public float addDuration = 0f;
        [Tooltip("The duration of time to remove the overlay color.")]
        public float removeDuration = 1f;
        [Tooltip("The duration of time to wait once the overlay color is applied before it is removed.")]
        public float appliedDuration = 0f;

        /// <summary>
        /// The CameraColorOverlayUnityEvent emits an event with the current color being overlaid on the valid cameras along with the sender object.
        /// </summary>
        [Serializable]
        public class CameraColorOverlayUnityEvent : UnityEvent<Color, object>
        {
        };

        /// <summary>
        /// The ColorOverlayAdded event is emitted when the `AddColorOverlay` method is called.
        /// </summary>
        public CameraColorOverlayUnityEvent ColorOverlayAdded;
        /// <summary>
        /// The ColorOverlayRemoved event is emitted when the `RemoveColorOverlay` method is called.
        /// </summary>
        public CameraColorOverlayUnityEvent ColorOverlayRemoved;
        /// <summary>
        /// The ColorOverlayChanged event is emitted during the color overlay cycle.
        /// </summary>
        public CameraColorOverlayUnityEvent ColorOverlayChanged;

        protected Color targetColor = new Color(0f, 0f, 0f, 0f);
        protected Color currentColor = new Color(0f, 0f, 0f, 0f);
        protected Color deltaColor = new Color(0f, 0f, 0f, 0f);
        protected Coroutine blinkRoutine;

        /// <summary>
        /// The AddColorOverlay method applies the `overlayColor` to the `validCameras` over the given `addDuration`.
        /// </summary>
        public virtual void AddColorOverlay()
        {
            CancelBlinkRoutine();
            AddColorOverlay(overlayColor, addDuration);
            OnColorOverlayAdded(overlayColor);
        }

        /// <summary>
        /// The RemoveColorOverlay method removes the `overlayColor` from the `validCameras` over the given `removeDuration`.
        /// </summary>
        public virtual void RemoveColorOverlay()
        {
            AddColorOverlay(Color.clear, removeDuration);
            OnColorOverlayRemoved(Color.clear);
        }

        /// <summary>
        /// The Blink method adds the `overlayColor` to the `validCameras` over the given `addDuration` then waits for the given `appliedDuration` then removes the `overlayColor` over the given `removeDuration`.
        /// </summary>
        public virtual void Blink()
        {
            AddColorOverlay(overlayColor, addDuration);
            blinkRoutine = StartCoroutine(ResetBlink(addDuration + appliedDuration));
        }

        protected virtual void OnEnable()
        {
            Camera.onPostRender += PostRender;
        }

        protected virtual void OnDisable()
        {
            CancelBlinkRoutine();
            Camera.onPostRender -= PostRender;
        }

        protected virtual void OnColorOverlayAdded(Color color)
        {
            ColorOverlayAdded?.Invoke(color, this);
        }

        protected virtual void OnColorOverlayRemoved(Color color)
        {
            ColorOverlayRemoved?.Invoke(color, this);
        }

        protected virtual void OnColorOverlayChanged(Color color)
        {
            ColorOverlayChanged?.Invoke(color, this);
        }

        /// <summary>
        /// The AddOverlayColor method applies the given color to the `validCameras` over the given duration.
        /// </summary>
        /// <param name="newColor">Color to apply to the overlay.</param>
        /// <param name="duration">The duration over which the color is applied.</param>
        protected virtual void AddColorOverlay(Color newColor, float duration)
        {
            targetColor = newColor;
            if (duration > 0.0f)
            {
                deltaColor = (targetColor - currentColor) / duration;
            }
            else
            {
                currentColor = newColor;
            }
        }

        /// <summary>
        /// The ResetBlink method waits for the given wait duration and then removes the color overlay over the remove duration.
        /// </summary>
        /// <param name="givenWaitDuration">The duration in which to wait before removing the color overlay.</param>
        /// <returns>An Enumerator to manage the running of the Coroutine.</returns>
        protected virtual IEnumerator ResetBlink(float givenWaitDuration)
        {
            yield return new WaitForSeconds(givenWaitDuration);
            RemoveColorOverlay();
        }

        /// <summary>
        /// The CancelBlinkRoutine cancels any existing running `ResetBlink` Coroutine.
        /// </summary>
        protected virtual void CancelBlinkRoutine()
        {
            if (blinkRoutine != null)
            {
                StopCoroutine(blinkRoutine);
            }
        }

        /// <summary>
        /// The PostRender event on the camera that will apply the color overlay.
        /// </summary>
        /// <param name="cam">The camera to apply onto.</param>
        protected virtual void PostRender(Camera cam)
        {
            if (!validCameras.Contains(cam))
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
                OnColorOverlayChanged(currentColor);
            }

            if (currentColor.a > 0f && overlayMaterial != null)
            {
                currentColor.a = (targetColor.a > currentColor.a && currentColor.a > 0.98f ? 1f : currentColor.a);
                overlayMaterial.color = currentColor;
                overlayMaterial.SetPass(0);
                GL.PushMatrix();
                GL.LoadOrtho();
                GL.Color(overlayMaterial.color);
                GL.Begin(GL.QUADS);
                GL.Vertex3(0f, 0f, 0.9999f);
                GL.Vertex3(0f, 1f, 0.9999f);
                GL.Vertex3(1f, 1f, 0.9999f);
                GL.Vertex3(1f, 0f, 0.9999f);
                GL.End();
                GL.PopMatrix();
            }
        }
    }
}