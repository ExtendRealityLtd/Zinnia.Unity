namespace Zinnia.Haptics
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using System.Collections;
    using Malimbe.MemberChangeMethod;
    using Malimbe.MemberClearanceMethod;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;

    /// <summary>
    /// Creates a haptic pattern based on the waveform of an <see cref="UnityEngine.AudioSource"/> and utilizes a <see cref="Haptics.HapticPulser"/> to create the effect.
    /// </summary>
    public class AudioSourceHapticPulser : RoutineHapticPulser
    {
        /// <summary>
        /// The waveform to represent the haptic pattern.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public AudioSource AudioSource { get; set; }

        /// <summary>
        /// Observer added to <see cref="AudioSource"/>.
        /// </summary>
        protected AudioSourceDataObserver observer;
        /// <summary>
        /// The observed audio data.
        /// </summary>
        protected readonly AudioSourceDataObserver.EventData audioData = new AudioSourceDataObserver.EventData();

        /// <inheritdoc />
        public override bool IsActive()
        {
            return base.IsActive() && AudioSource != null;
        }

        /// <inheritdoc />
        protected override void DoCancel()
        {
            RemoveDataObserver();
            base.DoCancel();
        }
        
        /// <summary>
        /// Enumerates through <see cref="AudioSource"/> and pulses for each amplitude of the wave.
        /// </summary>
        /// <returns>An Enumerator to manage the running of the Coroutine.</returns>
        protected override IEnumerator HapticProcessRoutine()
        {
            AddDataObserver();
            int outputSampleRate = AudioSettings.outputSampleRate;
            while (AudioSource != null && AudioSource.isPlaying)
            {
                float currentSample = 0;
                if (audioData.Data != null)
                {
                    int sampleIndex = (int)((AudioSettings.dspTime - audioData.DspTime) * outputSampleRate) * audioData.Channels;
                    sampleIndex = Mathf.Min(sampleIndex, audioData.Data.Length - audioData.Channels);
                    for (int i = 0; i < audioData.Channels; ++i)
                    {
                        currentSample += Mathf.Abs(audioData.Data[sampleIndex + i]);
                    }
                    currentSample /= audioData.Channels;
                }
                HapticPulser.Intensity = currentSample * IntensityMultiplier;
                HapticPulser.Begin();
                yield return null;
            }
            RemoveDataObserver();
            ResetIntensity();
        }

        /// <summary>
        /// Adds a <see cref="AudioSourceHapticPulserDataObserver"/> to the <see cref="AudioSource"/>.
        /// </summary>
        protected virtual void AddDataObserver()
        {
            if (AudioSource == null)
            {
                return;
            }

            observer = AudioSource.gameObject.AddComponent<AudioSourceDataObserver>();
            observer.DataObserved.AddListener(Receive);
        }

        /// <summary>
        /// Remove the <see cref="AudioSourceHapticPulserDataObserver"/> from the <see cref="AudioSource"/>.
        /// </summary>
        protected virtual void RemoveDataObserver()
        {
            if (observer == null)
            {
                return;
            }

            observer.DataObserved.RemoveListener(Receive);
            Destroy(observer);
            observer = null;
        }

        /// <summary>
        /// Receive audio data from <see cref="AudioSourceHapticPulserDataObserver"/>.
        /// </summary>
        protected virtual void Receive(AudioSourceDataObserver.EventData eventData)
        {
            audioData.Set(eventData);
        }

        /// <summary>
        /// Called before <see cref="AudioSource"/> has been changed.
        /// </summary>
        [CalledBeforeChangeOf(nameof(AudioSource))]
        protected virtual void OnBeforeAudioSourceChange()
        {
            if (hapticRoutine == null)
            {
                return;
            }

            RemoveDataObserver();
        }

        /// <summary>
        /// Called after <see cref="AudioSource"/> has been changed.
        /// </summary>
        [CalledAfterChangeOf(nameof(AudioSource))]
        protected virtual void OnAfterAudioSourceChange()
        {
            if (hapticRoutine == null)
            {
                return;
            }

            AddDataObserver();
        }
    }

    /// <summary>
    /// Observes the <see cref="AudioSource"/> and emits the audio data.
    /// </summary>
    public class AudioSourceDataObserver : MonoBehaviour
    {
        /// <summary>
        /// Holds data about a <see cref="AudioSourceDataObserver"/> event.
        /// </summary>
        [Serializable]
        public class EventData
        {
            /// <summary>
            /// <see cref="AudioSettings.dspTime"/> of the last <see cref="OnAudioFilterRead"/>.
            /// </summary>
            [Serialized]
            [field: DocumentedByXml]
            public double DspTime { get; set; }
            /// <summary>
            /// Audio data array of the last <see cref="OnAudioFilterRead"/>.
            /// </summary>
            [Serialized]
            [field: DocumentedByXml]
            public float[] Data { get; set; }
            /// <summary>
            /// Number of channels of the last <see cref="OnAudioFilterRead"/>.
            /// </summary>
            [Serialized]
            [field: DocumentedByXml]
            public int Channels { get; set; }

            public EventData Set(EventData source)
            {
                return Set(source.DspTime, source.Data, source.Channels);
            }

            public EventData Set(double dspTime, float[] data, int channels)
            {
                DspTime = dspTime;
                Data = data;
                Channels = channels;
                return this;
            }

            public void Clear()
            {
                Set(default, default, default);
            }
        }

        /// <summary>
        /// Defines the event with the <see cref="EventData"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<EventData> { }

        /// <summary>
        /// Emitted whenever the audio data is observed.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent DataObserved = new UnityEvent();
        /// <summary>
        /// The data to emit with an event.
        /// </summary>
        protected readonly EventData eventData = new EventData();

        /// <summary>
        /// Emits audio data.
        /// </summary>
        /// <param name="data">An array of floats comprising the audio data.</param>
        /// <param name="channels">An int that stores the number of channels of audio data passed to this delegate.</param>
        protected virtual void OnAudioFilterRead(float[] data, int channels)
        {
            DataObserved?.Invoke(eventData.Set(AudioSettings.dspTime, data, channels));
        }
    }
}