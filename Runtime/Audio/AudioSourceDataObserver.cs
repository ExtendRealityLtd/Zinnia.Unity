namespace Zinnia.Audio
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;

    /// <summary>
    /// Observes the <see cref="AudioSource"/> and emits the audio data.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
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
        /// Returns whether the <see cref="AudioSource"/> is player.
        /// </summary>
        public virtual bool IsAudioSourcePlaying() => audioSource != null && audioSource.isPlaying;

        /// <summary>
        /// The <see cref="AudioSource"/> to observe.
        /// </summary>
        protected AudioSource audioSource;

        /// <summary>
        /// Caches the <see cref="AudioSource"/>.
        /// </summary>
        protected virtual void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

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