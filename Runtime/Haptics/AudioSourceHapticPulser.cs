namespace Zinnia.Haptics
{
    using UnityEngine;
    using System.Collections;
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
        [Serialized]
        [field: DocumentedByXml]
        public AudioSource AudioSource { get; set; }

        /// <summary>
        /// <see cref="AudioSettings.dspTime"/> of the last <see cref="OnAudioFilterRead"/>.
        /// </summary>
        protected double filterReadDspTime;
        /// <summary>
        /// Audio data array of the last <see cref="OnAudioFilterRead"/>.
        /// </summary>
        protected float[] filterReadData;
        /// <summary>
        /// Number of channels of the last <see cref="OnAudioFilterRead"/>.
        /// </summary>
        protected int filterReadChannels;

        /// <inheritdoc />
        public override bool IsActive()
        {
            return base.IsActive() && AudioSource != null;
        }

        /// <summary>
        /// Enumerates through <see cref="AudioSource"/> and pulses for each amplitude of the wave.
        /// </summary>
        /// <returns>An Enumerator to manage the running of the Coroutine.</returns>
        protected override IEnumerator HapticProcessRoutine()
        {
            int outputSampleRate = AudioSettings.outputSampleRate;
            while (AudioSource.isPlaying)
            {
                int sampleIndex = (int)((AudioSettings.dspTime - filterReadDspTime) * outputSampleRate);
                float currentSample = 0;
                if (filterReadData != null && sampleIndex * filterReadChannels < filterReadData.Length)
                {
                    for (int i = 0; i < filterReadChannels; ++i)
                    {
                        currentSample += filterReadData[sampleIndex + i];
                    }
                    currentSample /= filterReadChannels;
                }
                HapticPulser.Intensity = currentSample * IntensityMultiplier;
                HapticPulser.Begin();
                yield return null;
            }
            ResetIntensity();
        }

        /// <summary>
        /// Store currently playing audio data and additional data.
        /// </summary>
        /// <param name="data">An array of floats comprising the audio data.</param>
        /// <param name="channels">An int that stores the number of channels of audio data passed to this delegate.</param>
        protected virtual void OnAudioFilterRead(float[] data, int channels)
        {
            filterReadDspTime = AudioSettings.dspTime;
            filterReadData = data;
            filterReadChannels = channels;
        }
    }
}