namespace Zinnia.Visual
{
    using Malimbe.BehaviourStateRequirementMethod;
    using Malimbe.MemberChangeMethod;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;
    using System;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.Rendering;
    using Zinnia.Extension;
    using Zinnia.Rule;

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

            /// <inheritdoc />
            public override string ToString()
            {
                string[] titles = new string[]
                {
                "Color"
                };

                object[] values = new object[]
                {
                Color
                };

                return StringExtensions.FormatForToString(titles, values);
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
        public class UnityEvent : UnityEvent<EventData> { }

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
        /// A container for holding the Universal Render Pipeline fade overlay mesh.
        /// </summary>
        protected GameObject urpFadeOverlay;
        /// <summary>
        /// The mesh for use with the Universal Render Pipeline fade overlay.
        /// </summary>
        protected MeshRenderer fadeRenderer;
        /// <summary>
        /// The last used camera in the fade routine.
        /// </summary>
        protected Camera lastUsedCamera;

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

        /// <summary>
        /// Destroys the fade mesh used in the Universal Render Pipeline. To recreate it, the component must be disabled then re-enabled.
        /// </summary>
        public virtual void DestroyFadeMesh()
        {
            if (urpFadeOverlay == null)
            {
                return;
            }

            urpFadeOverlay.transform.SetParent(null);
            Destroy(urpFadeOverlay);
        }

        protected virtual void OnEnable()
        {
            lastUsedCamera = null;
            CopyMaterialOverlayToWorking();
            OnAfterCheckDelayChange();
            if (GraphicsSettings.renderPipelineAsset != null)
            {
                CreateFadeMesh();
#if UNITY_2019_1_OR_NEWER
                RenderPipelineManager.beginCameraRendering += UrpPreRender;
#endif
            }
            else
            {
                Camera.onPostRender += PostRender;
            }
        }

        protected virtual void OnDisable()
        {
            CancelBlinkRoutine();
            if (GraphicsSettings.renderPipelineAsset != null)
            {
                if (fadeRenderer != null)
                {
                    fadeRenderer.enabled = false;
                }
#if UNITY_2019_1_OR_NEWER
                RenderPipelineManager.beginCameraRendering -= UrpPreRender;
#endif
            }
            else
            {
                Camera.onPostRender -= PostRender;
            }
        }

        /// <summary>
        /// Creates the fade mesh used in the Universal Render Pipeline.
        /// </summary>
        protected virtual void CreateFadeMesh()
        {
            if (urpFadeOverlay != null)
            {
                return;
            }

            urpFadeOverlay = new GameObject("Zinnia.URPFadeOverlay." + name);
            MeshFilter fadeMesh = urpFadeOverlay.AddComponent<MeshFilter>();
            fadeRenderer = urpFadeOverlay.AddComponent<MeshRenderer>();
            Mesh mesh = new Mesh();
            fadeMesh.mesh = mesh;

            Vector3[] vertices = new Vector3[4];

            float width = 2f;
            float height = 2f;
            float depth = 1f;

            vertices[0] = new Vector3(-width, -height, depth);
            vertices[1] = new Vector3(width, -height, depth);
            vertices[2] = new Vector3(-width, height, depth);
            vertices[3] = new Vector3(width, height, depth);

            mesh.vertices = vertices;

            int[] tri = new int[6] { 0, 2, 1, 2, 3, 1 };

            mesh.triangles = tri;

            Vector3[] normals = new Vector3[4] { -Vector3.forward, -Vector3.forward, -Vector3.forward, -Vector3.forward };

            mesh.normals = normals;

            Vector2[] uv = new Vector2[4];

            uv[0] = new Vector2(0, 0);
            uv[1] = new Vector2(1, 0);
            uv[2] = new Vector2(0, 1);
            uv[3] = new Vector2(1, 1);

            mesh.uv = uv;
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
        /// Processes the transition of the current color to the target color.
        /// </summary>
        protected virtual void ProcessColorTransition()
        {
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
        }

        /// <summary>
        /// Determines whether a transition should occur and sets the transition color if required.
        /// </summary>
        /// <returns>Whether the transition should occur.</returns>
        protected virtual bool ShouldTransition()
        {
            if (currentColor.a > 0f && workingMaterial != null)
            {
                currentColor.a = (targetColor.a > currentColor.a && currentColor.a > 0.98f ? 1f : currentColor.a);
                workingMaterial.color = currentColor;
                return true;
            }

            return false;
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

            ProcessColorTransition();

            if (ShouldTransition())
            {
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
        /// The Universal Render Pipeline moment before the camera render that will apply the <see cref="Color"/> overlay.
        /// </summary>
        /// <param name="context">The render context.</param>
        /// <param name="sceneCamera">The <see cref="Camera"/> to apply onto.</param>
#if UNITY_2019_1_OR_NEWER
        protected virtual void UrpPreRender(ScriptableRenderContext context, Camera sceneCamera)
        {
            ProcessColorTransition();

            if (urpFadeOverlay == null || fadeRenderer == null)
            {
                return;
            }

            if (ShouldTransition())
            {
                if (lastUsedCamera != sceneCamera)
                {
                    urpFadeOverlay.transform.SetParent(sceneCamera.transform);
                    urpFadeOverlay.transform.localPosition = Vector3.forward;
                    urpFadeOverlay.transform.localRotation = Quaternion.identity;
                    urpFadeOverlay.transform.localScale = Vector3.one;
                }
                lastUsedCamera = sceneCamera;
                fadeRenderer.material = workingMaterial;
                fadeRenderer.enabled = true;
            }
            else
            {
                fadeRenderer.enabled = false;
            }
        }
#endif

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