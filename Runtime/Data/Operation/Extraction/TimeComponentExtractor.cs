namespace Zinnia.Data.Operation.Extraction
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;

    /// <summary>
    /// Extracts and emits the elements from <see cref="Time"/>.
    /// </summary>
    public class TimeComponentExtractor : MonoBehaviour
    {
        /// <summary>
        /// Defines an event with a <see cref="float"/> value.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<float> { }

        /// <summary>
        /// The components of <see cref="Time"/>
        /// </summary>
        public enum TimeComponent
        {
            /// <summary>
            /// The time at the beginning of this frame. This is the time in seconds since the start of the game.
            /// </summary>
            Time,
            /// <summary>
            /// The time the latest <see cref="MonoBehaviour.FixedUpdate"/> has started. This is the time in seconds since the start of the game.
            /// </summary>
            FixedTime,
            /// <summary>
            /// The <see cref="TimeComponent.FixedTime"/> if in <see cref="Time.inFixedTimeStep"/> otherwise the <see cref="TimeComponent.Time"/>.
            /// </summary>
            TimeStepTypeTime,
            /// <summary>
            /// The completion time in seconds since the last frame.
            /// </summary>
            DeltaTime,
            /// <summary>
            /// The interval in seconds at which physics and other fixed frame rate updates are performed.
            /// </summary>
            FixedDeltaTime,
            /// <summary>
            /// The <see cref="TimeComponent.FixedDeltaTime"/> if in <see cref="Time.inFixedTimeStep"/> otherwise the <see cref="TimeComponent.DeltaTime"/>.
            /// </summary>
            TimeStepTypeDeltaTime,
            /// <summary>
            /// The timeScale-independant time for this frame. This is the time in seconds since the start of the game.
            /// </summary>
            UnscaledTime,
            /// <summary>
            /// The TimeScale-independant time the latest MonoBehaviour.FixedUpdate has started. This is the time in seconds since the start of the game.
            /// </summary>
            FixedUnscaledTime,
            /// <summary>
            /// The <see cref="TimeComponent.FixedUnscaledTime"/> if in <see cref="Time.inFixedTimeStep"/> otherwise the <see cref="TimeComponent.UnscaledTime"/>.
            /// </summary>
            TimeStepTypeUnscaledTime,
            /// <summary>
            /// The timeScale-independent interval in seconds from the last frame to the current one.
            /// </summary>
            UnscaledDeltaTime,
            /// <summary>
            /// The timeScale-independent interval in seconds from the last fixed frame to the current one.
            /// </summary>
            FixedUnscaledDeltaTime,
            /// <summary>
            /// The <see cref="TimeComponent.FixedUnscaledDeltaTime"/> if in <see cref="Time.inFixedTimeStep"/> otherwise the <see cref="TimeComponent.UnscaledDeltaTime"/>.
            /// </summary>
            TimeStepTypeUnscaledDeltaTime,
            /// <summary>
            /// Slows game playback time to allow screenshots to be saved between frames.
            /// </summary>
            CaptureFrameRate,
            /// <summary>
            /// The total number of frames that have passed.
            /// </summary>
            FrameCount,
            /// <summary>
            /// The maximum time a frame can take.
            /// </summary>
            MaximumDeltaTime,
            /// <summary>
            /// The maximum time a frame can spend on particle updates.
            /// </summary>
            MaximumParticleDeltaTime,
            /// <summary>
            /// The real time in seconds since the game started.
            /// </summary>
            RealTimeSinceStartUp,
            /// <summary>
            /// The total number of rendered frames that have passed.
            /// </summary>
            RenderedFrameCount,
            /// <summary>
            /// A smoothed out Time.deltaTime.
            /// </summary>
            SmoothDeltaTime,
            /// <summary>
            /// The scale at which the time is passing. This can be used for slow motion effects.
            /// </summary>
            TimeScale,
            /// <summary>
            /// The time in seconds since the last level has been loaded.
            /// </summary>
            TimeSinceLevelLoad
        }

        /// <summary>
        /// The component to extract from <see cref="Time"/>.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public TimeComponent ComponentToExtract { get; set; } = TimeComponent.Time;

        /// <summary>
        /// Emitted when the <see cref="float"/> component from <see cref="Time"/> is extracted.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent Extracted = new UnityEvent();

        /// <summary>
        /// The extracted <see cref="float"/> component.
        /// </summary>
        public float? Result { get; protected set; }

        /// <summary>
        /// Extracts the <see cref="float"/> component from <see cref="Time"/>.
        /// </summary>
        /// <returns>The extracted <see cref="float"/>.</returns>
        public virtual float? Extract()
        {
            if (!isActiveAndEnabled)
            {
                Result = null;
                return null;
            }

            switch (ComponentToExtract)
            {
                case TimeComponent.Time:
                    Result = Time.time;
                    break;
                case TimeComponent.FixedTime:
                    Result = Time.fixedTime;
                    break;
                case TimeComponent.TimeStepTypeTime:
                    Result = Time.inFixedTimeStep ? Time.fixedTime : Time.time;
                    break;
                case TimeComponent.DeltaTime:
                    Result = Time.deltaTime;
                    break;
                case TimeComponent.FixedDeltaTime:
                    Result = Time.fixedDeltaTime;
                    break;
                case TimeComponent.TimeStepTypeDeltaTime:
                    Result = Time.inFixedTimeStep ? Time.fixedDeltaTime : Time.deltaTime;
                    break;
                case TimeComponent.UnscaledTime:
                    Result = Time.unscaledTime;
                    break;
                case TimeComponent.FixedUnscaledTime:
                    Result = Time.fixedUnscaledTime;
                    break;
                case TimeComponent.TimeStepTypeUnscaledTime:
                    Result = Time.inFixedTimeStep ? Time.fixedUnscaledTime : Time.unscaledTime;
                    break;
                case TimeComponent.UnscaledDeltaTime:
                    Result = Time.unscaledDeltaTime;
                    break;
                case TimeComponent.FixedUnscaledDeltaTime:
                    Result = Time.fixedUnscaledDeltaTime;
                    break;
                case TimeComponent.TimeStepTypeUnscaledDeltaTime:
                    Result = Time.inFixedTimeStep ? Time.fixedUnscaledDeltaTime : Time.unscaledDeltaTime;
                    break;
                case TimeComponent.CaptureFrameRate:
                    Result = Time.captureFramerate;
                    break;
                case TimeComponent.FrameCount:
                    Result = Time.frameCount;
                    break;
                case TimeComponent.MaximumDeltaTime:
                    Result = Time.maximumDeltaTime;
                    break;
                case TimeComponent.MaximumParticleDeltaTime:
                    Result = Time.maximumParticleDeltaTime;
                    break;
                case TimeComponent.RealTimeSinceStartUp:
                    Result = Time.realtimeSinceStartup;
                    break;
                case TimeComponent.RenderedFrameCount:
                    Result = Time.renderedFrameCount;
                    break;
                case TimeComponent.SmoothDeltaTime:
                    Result = Time.smoothDeltaTime;
                    break;
                case TimeComponent.TimeScale:
                    Result = Time.timeScale;
                    break;
                case TimeComponent.TimeSinceLevelLoad:
                    Result = Time.timeSinceLevelLoad;
                    break;
            }

            Extracted?.Invoke(Result.Value);
            return Result;
        }

        /// <summary>
        /// Extracts the <see cref="float"/> component from <see cref="Time"/>.
        /// </summary>
        public virtual void DoExtract()
        {
            Extract();
        }
    }
}