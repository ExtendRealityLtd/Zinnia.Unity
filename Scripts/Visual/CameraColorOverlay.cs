﻿namespace VRTK.Core.Visual
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using System.Collections;
    using VRTK.Core.Extension;
    using VRTK.Core.Data.Type;

    /// <summary>
    /// Applies a color over the valid cameras and can be used to fade the screen view.
    /// </summary>
    public class CameraColorOverlay : MonoBehaviour
    {
        /// <summary>
        /// A <see cref="Camera"/> collection to apply the color overlay to.
        /// </summary>
        [Tooltip("A Camera collection to apply the color overlay to.")]
        public CameraList validCameras;
        /// <summary>
        /// The <see cref="Color"/> of the overlay.
        /// </summary>
        [Tooltip("The Color of the overlay.")]
        public Color overlayColor = Color.black;
        /// <summary>
        /// The <see cref="Material"/> to use for the overlay.
        /// </summary>
        [Tooltip("The Material to use for the overlay.")]
        public Material overlayMaterial;
        /// <summary>
        /// The duration of time to apply the overlay <see cref="Color"/>.
        /// </summary>
        [Tooltip("The duration of time to apply the overlay Color.")]
        public float addDuration = 0f;
        /// <summary>
        /// The duration of time to remove the overlay <see cref="Color"/>.
        /// </summary>
        [Tooltip("The duration of time to remove the overlay Color.")]
        public float removeDuration = 1f;
        /// <summary>
        /// The duration of time to wait once the overlay <see cref="Color"/> is applied before it is removed.
        /// </summary>
        [Tooltip("The duration of time to wait once the overlay Color is applied before it is removed.")]
        public float appliedDuration = 0f;

        /// <summary>
        /// Defines the event with the overlay <see cref="Color"/> and sender <see cref="object"/>.
        /// </summary>
        [Serializable]
        public class CameraColorOverlayUnityEvent : UnityEvent<Color, object>
        {
        }

        /// <summary>
        /// Emitted when an overlay <see cref="Color"/> is added.
        /// </summary>
        public CameraColorOverlayUnityEvent Added = new CameraColorOverlayUnityEvent();
        /// <summary>
        /// Emitted when an overlay <see cref="Color"/> is removed.
        /// </summary>
        public CameraColorOverlayUnityEvent Removed = new CameraColorOverlayUnityEvent();
        /// <summary>
        /// Emitted when an overlay <see cref="Color"/> has changed from the previous render frame.
        /// </summary>
        public CameraColorOverlayUnityEvent Changed = new CameraColorOverlayUnityEvent();

        protected float targetDuration;
        protected Color targetColor = new Color(0f, 0f, 0f, 0f);
        protected Color currentColor = new Color(0f, 0f, 0f, 0f);
        protected Color deltaColor = new Color(0f, 0f, 0f, 0f);
        protected Coroutine blinkRoutine;

        /// <summary>
        /// Applies the <see cref="overlayColor"/> to the <see cref="validCameras"/> over the given <see cref="addDuration"/>.
        /// </summary>
        public virtual void AddColorOverlay()
        {
            AddColorOverlay(overlayColor, addDuration);
        }

        /// <summary>
        /// Removes the <see cref="overlayColor"/> to the <see cref="validCameras"/> over the given <see cref="removeDuration"/>.
        /// </summary>
        public virtual void RemoveColorOverlay()
        {
            AddColorOverlay(Color.clear, removeDuration);
            OnRemoved(Color.clear, this);
        }

        /// <summary>
        /// Applies the <see cref="overlayColor"/> to the <see cref="validCameras"/> over the given <see cref="addDuration"/> then waits for the given <see cref="appliedDuration"/> then removes the <see cref="overlayColor"/> over the given <see cref="removeDuration"/>.
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

        protected virtual void OnAdded(Color color, object sender)
        {
            if (isActiveAndEnabled)
            {
                Added?.Invoke(color, sender);
            }
        }

        protected virtual void OnRemoved(Color color, object sender)
        {
            if (isActiveAndEnabled)
            {
                Removed?.Invoke(color, sender);
            }
        }

        protected virtual void OnChanged(Color color, object sender)
        {
            if (isActiveAndEnabled)
            {
                Changed?.Invoke(color, sender);
            }
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
                    OnAdded(overlayColor, this);
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
            if (validCameras == null || !validCameras.cameras.Contains(cam))
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
                OnChanged(currentColor, this);
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