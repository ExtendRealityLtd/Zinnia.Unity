namespace Zinnia.Haptics
{
    using UnityEngine;
    using System.Collections;
    using Malimbe.MemberChangeMethod;
    using Malimbe.MemberClearanceMethod;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;
    using Zinnia.Audio;

    /// <summary>
    /// Creates a haptic pattern based on the waveform of an <see cref="UnityEngine.AudioSource"/> and utilizes a <see cref="Haptics.HapticPulser"/> to create the effect.
    /// </summary>
    public class AudioSourceHapticPulser : RoutineHapticPulser
    {
        /// <summary>
        /// Observer that provides audio data from a <see cref="AudioSource"/>.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public AudioSourceDataObserver Observer { get; set; }

        /// <summary>
        /// A reused data instance.
        /// </summary>
        protected readonly AudioSourceDataObserver.EventData audioData = new AudioSourceDataObserver.EventData();

        /// <summary>
        /// Subscribes as a listener to the <see cref="Observer"/>.
        /// </summary>
        protected virtual void SubscribeToObserver()
        {
            if (Observer == null)
            {
                return;
            }

            Observer.DataObserved.AddListener(Receive);
        }

        /// <summary>
        /// Unsubscribes from listening to the <see cref="Observer"/>.
        /// </summary>
        protected virtual void UnsubscribeFromObserver()
        {
            if (Observer == null)
            {
                return;
            }

            Observer.DataObserved.RemoveListener(Receive);
        }

        /// <inheritdoc />
        public override bool IsActive()
        {
            return base.IsActive() && Observer != null;
        }

        /// <summary>
        /// Enumerates through <see cref="AudioSource"/> and pulses for each amplitude of the wave.
        /// </summary>
        /// <returns>An Enumerator to manage the running of the Coroutine.</returns>
        protected override IEnumerator HapticProcessRoutine()
        {
            SubscribeToObserver();
            int outputSampleRate = AudioSettings.outputSampleRate;
            while (Observer != null && Observer.IsAudioSourcePlaying())
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
            UnsubscribeFromObserver();
            ResetIntensity();
        }

        /// <summary>
        /// Receive audio data from <see cref="AudioSourceDataObserver"/>.
        /// </summary>
        protected virtual void Receive(AudioSourceDataObserver.EventData eventData)
        {
            audioData.Set(eventData);
        }

        /// <summary>
        /// Called before <see cref="Observer"/> has been changed.
        /// </summary>
        [CalledBeforeChangeOf(nameof(Observer))]
        protected virtual void OnBeforeObserverChange()
        {
            if (hapticRoutine == null)
            {
                return;
            }

            UnsubscribeFromObserver();
        }

        /// <summary>
        /// Called after <see cref="Observer"/> has been changed.
        /// </summary>
        [CalledAfterChangeOf(nameof(Observer))]
        protected virtual void OnAfterObserverChange()
        {
            if (hapticRoutine == null)
            {
                return;
            }

            SubscribeToObserver();
        }
    }
}