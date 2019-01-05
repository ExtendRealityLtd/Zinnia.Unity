namespace Zinnia.Process.Moment
{
    using UnityEngine;
    using System.Collections.Generic;
    using System.Linq;
    using Zinnia.Extension;

    /// <summary>
    /// Iterates through the given <see cref="MomentProcess"/> array and executes the <see cref="MomentProcess.process"/> method on the given Unity game loop moment.
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
        public Moment momentToProcess = Moment.PreRender;
        /// <summary>
        /// A collection of <see cref="MomentProcess"/> to process.
        /// </summary>
        public List<MomentProcess> processes = new List<MomentProcess>();

        protected Moment subscribedMoment;

        protected virtual void OnEnable()
        {
            subscribedMoment = Moment.None;
            ManageSubscriptions();
        }

        protected virtual void OnDisable()
        {
            UnsubscribeMoment();
        }

        protected virtual void FixedUpdate()
        {
            if (momentToProcess == Moment.FixedUpdate)
            {
                Process();
            }
        }

        protected virtual void Update()
        {
            if (momentToProcess == Moment.Update)
            {
                Process();
            }
        }

        protected virtual void LateUpdate()
        {
            if (momentToProcess == Moment.LateUpdate)
            {
                Process();
            }
            ManageSubscriptions();
        }

        protected virtual void OnCameraPreRender(Camera givenCamera)
        {
            Process();
        }

        protected virtual void OnCameraPreCull(Camera givenCamera)
        {
            Process();
        }

        /// <summary>
        /// Handles subscribing and unsubscribing to the relevant camera events.
        /// </summary>
        protected virtual void ManageSubscriptions()
        {
            if (subscribedMoment != momentToProcess)
            {
                UnsubscribeMoment();
                SubscribeMoment();
            }
        }

        /// <summary>
        /// Handles unsubscribing to the chosen subscribed moment event.
        /// </summary>
        protected virtual void UnsubscribeMoment()
        {
            switch (subscribedMoment)
            {
                case Moment.PreRender:
                    Camera.onPreRender -= OnCameraPreRender;
                    break;
                case Moment.PreCull:
                    Camera.onPreCull -= OnCameraPreCull;
                    break;
            }
            subscribedMoment = Moment.None;
        }

        /// <summary>
        /// Handles subscribing to the chosen moment to process event.
        /// </summary>
        protected virtual void SubscribeMoment()
        {
            switch (momentToProcess)
            {
                case Moment.PreRender:
                    Camera.onPreRender += OnCameraPreRender;
                    break;
                case Moment.PreCull:
                    Camera.onPreCull += OnCameraPreCull;
                    break;
            }
            subscribedMoment = momentToProcess;
        }

        /// <summary>
        /// Iterates through the given <see cref="MomentProcess"/> and calls <see cref="MomentProcess.Process"/> on each one.
        /// </summary>
        protected virtual void Process()
        {
            foreach (MomentProcess currentProcess in processes.EmptyIfNull().Where(process => process != null))
            {
                currentProcess.Process();
            }
        }
    }
}