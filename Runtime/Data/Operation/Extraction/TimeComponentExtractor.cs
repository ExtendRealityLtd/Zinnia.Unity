namespace Zinnia.Data.Operation.Extraction
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using Zinnia.Extension;

    /// <summary>
    /// Extracts and emits the elements from <see cref="Time"/>.
    /// </summary>
    public class TimeComponentExtractor : FloatExtractor<TimeComponentExtractor.TimeComponent, TimeComponentExtractor.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the specified <see cref="float"/>.
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
            /// The timeScale-independent time for this frame. This is the time in seconds since the start of the game.
            /// </summary>
            UnscaledTime,
            /// <summary>
            /// The TimeScale-independent time the latest MonoBehaviour.FixedUpdate has started. This is the time in seconds since the start of the game.
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
            /// Slows game playback time to allow screen shots to be saved between frames.
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
        /// Sets the <see cref="Source"/>.
        /// </summary>
        /// <param name="index">The index of the <see cref="TimeComponent"/>.</param>
        public virtual void SetSource(int index)
        {
            Source = EnumExtensions.GetByIndex<TimeComponent>(index);
        }

        /// <inheritdoc />
        protected override float? ExtractValue()
        {
            switch (Source)
            {
                case TimeComponent.Time:
                    return Time.time;
                case TimeComponent.FixedTime:
                    return Time.fixedTime;
                case TimeComponent.TimeStepTypeTime:
                    return Time.inFixedTimeStep ? Time.fixedTime : Time.time;
                case TimeComponent.DeltaTime:
                    return Time.deltaTime;
                case TimeComponent.FixedDeltaTime:
                    return Time.fixedDeltaTime;
                case TimeComponent.TimeStepTypeDeltaTime:
                    return Time.inFixedTimeStep ? Time.fixedDeltaTime : Time.deltaTime;
                case TimeComponent.UnscaledTime:
                    return Time.unscaledTime;
                case TimeComponent.FixedUnscaledTime:
                    return Time.fixedUnscaledTime;
                case TimeComponent.TimeStepTypeUnscaledTime:
                    return Time.inFixedTimeStep ? Time.fixedUnscaledTime : Time.unscaledTime;
                case TimeComponent.UnscaledDeltaTime:
                    return Time.unscaledDeltaTime;
                case TimeComponent.FixedUnscaledDeltaTime:
                    return Time.fixedUnscaledDeltaTime;
                case TimeComponent.TimeStepTypeUnscaledDeltaTime:
                    return Time.inFixedTimeStep ? Time.fixedUnscaledDeltaTime : Time.unscaledDeltaTime;
                case TimeComponent.CaptureFrameRate:
                    return Time.captureFramerate;
                case TimeComponent.FrameCount:
                    return Time.frameCount;
                case TimeComponent.MaximumDeltaTime:
                    return Time.maximumDeltaTime;
                case TimeComponent.MaximumParticleDeltaTime:
                    return Time.maximumParticleDeltaTime;
                case TimeComponent.RealTimeSinceStartUp:
                    return Time.realtimeSinceStartup;
                case TimeComponent.RenderedFrameCount:
                    return Time.renderedFrameCount;
                case TimeComponent.SmoothDeltaTime:
                    return Time.smoothDeltaTime;
                case TimeComponent.TimeScale:
                    return Time.timeScale;
                case TimeComponent.TimeSinceLevelLoad:
                    return Time.timeSinceLevelLoad;
                default:
                    return null;
            }
        }
    }
}