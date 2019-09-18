namespace Zinnia.Process.Moment
{
    using UnityEngine;
    using Malimbe.MemberChangeMethod;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;
    using Zinnia.Process.Moment.Collection;
    using UnityEngine.Rendering;

    /// <summary>
    /// Iterates through a given <see cref="MomentProcess"/> collection and executes the <see cref="IProcessable.Process"/> method on the given Unity game loop moment.
    /// </summary>
    public class MomentProcessor : MonoBehaviour
    {
        /// <summary>
        /// The point in the Unity game loop when to execute the processes.
        /// </summary>
        public enum Moment
        {
            /// <summary>
            /// Never execute the processes.
            /// </summary>
            None,
            /// <summary>
            /// Execute the processes in the FixedUpdate physics part of the Unity game loop.
            /// </summary>
            FixedUpdate,
            /// <summary>
            /// Executes the processes in the Update game logic part of the Unity game loop.
            /// </summary>
            Update,
            /// <summary>
            /// Executes the processes in the LateUpdate game logic part of the Unity game loop.
            /// </summary>
            LateUpdate,
            /// <summary>
            /// Executes the processes in the camera PreCull scene rendering part of the Unity game loop.
            /// </summary>
            PreCull,
            /// <summary>
            /// Executes the processes in the camera PreRender scene rendering part of the Unity game loop.
            /// </summary>
            PreRender
        }

        /// <summary>
        /// The moment in which to process the given processes.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public Moment ProcessMoment { get; set; } = Moment.PreRender;
        /// <summary>
        /// A collection of <see cref="MomentProcess"/> to process.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public MomentProcessObservableList Processes { get; set; }

        protected virtual void OnEnable()
        {
            SubscribeMoment();
        }

        protected virtual void OnDisable()
        {
            UnsubscribeMoment();
        }

        protected virtual void FixedUpdate()
        {
            if (ProcessMoment == Moment.FixedUpdate)
            {
                Process();
            }
        }

        protected virtual void Update()
        {
            if (ProcessMoment == Moment.Update)
            {
                Process();
            }
        }

        protected virtual void LateUpdate()
        {
            if (ProcessMoment == Moment.LateUpdate)
            {
                Process();
            }
        }

#if UNITY_2019_1_OR_NEWER
        protected virtual void OnSrpCameraPreRender(ScriptableRenderContext context, Camera givenCamera)
        {
            Process();
        }
#endif

        protected virtual void OnCameraPreRender(Camera givenCamera)
        {
            Process();
        }

        protected virtual void OnCameraPreCull(Camera givenCamera)
        {
            Process();
        }

        /// <summary>
        /// Handles unsubscribing to the chosen subscribed moment event.
        /// </summary>
        protected virtual void UnsubscribeMoment()
        {
            switch (ProcessMoment)
            {
                case Moment.PreRender:
                    if (GraphicsSettings.renderPipelineAsset != null)
                    {
#if UNITY_2019_1_OR_NEWER
                        RenderPipelineManager.beginCameraRendering -= OnSrpCameraPreRender;
#endif
                    }
                    else
                    {
                        Camera.onPreRender -= OnCameraPreRender;
                    }
                    break;
                case Moment.PreCull:
                    Camera.onPreCull -= OnCameraPreCull;
                    break;
            }
        }

        /// <summary>
        /// Handles subscribing to the chosen moment to process event.
        /// </summary>
        protected virtual void SubscribeMoment()
        {
            switch (ProcessMoment)
            {
                case Moment.PreRender:
                    if (GraphicsSettings.renderPipelineAsset != null)
                    {
#if UNITY_2019_1_OR_NEWER
                        RenderPipelineManager.beginCameraRendering += OnSrpCameraPreRender;
#else
                        Debug.LogWarning("SRP is only supported on Unity 2019.1 or above");
#endif
                    }
                    else
                    {
                        Camera.onPreRender += OnCameraPreRender;
                    }
                    break;
                case Moment.PreCull:
                    Camera.onPreCull += OnCameraPreCull;
                    break;
            }
        }

        /// <summary>
        /// Iterates through the given <see cref="MomentProcess"/> and calls <see cref="MomentProcess.Process"/> on each one.
        /// </summary>
        protected virtual void Process()
        {
            if (Processes == null)
            {
                return;
            }

            foreach (MomentProcess currentProcess in Processes.NonSubscribableElements)
            {
                currentProcess.Process();
            }
        }

        /// <summary>
        /// Called before <see cref="ProcessMoment"/> has been changed.
        /// </summary>
        [CalledBeforeChangeOf(nameof(ProcessMoment))]
        protected virtual void OnBeforeProcessMomentChange()
        {
            UnsubscribeMoment();
        }

        /// <summary>
        /// Called after <see cref="ProcessMoment"/> has been changed.
        /// </summary>
        [CalledAfterChangeOf(nameof(ProcessMoment))]
        protected virtual void OnAfterProcessMomentChange()
        {
            SubscribeMoment();
        }
    }
}