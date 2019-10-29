namespace Zinnia.Haptics
{
    using UnityEngine;
    using System.Collections;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;

    /// <summary>
    /// Creates a haptic pattern based on the waveform of an <see cref="UnityEngine.AudioClip"/> and utilizes a <see cref="Haptics.HapticPulser"/> to create the effect.
    /// </summary>
    public class AudioClipHapticPulser : RoutineHapticPulser
    {
        /// <summary>
        /// The waveform to represent the haptic pattern.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public AudioClip AudioClip { get; set; }

        /// <summary>
        /// The size of the audio buffer.
        /// </summary>
        protected const int BufferSize = 8192;
        /// <summary>
        /// The audio data buffer.
        /// </summary>
        protected readonly float[] audioData = new float[BufferSize];

        /// <inheritdoc />
        public override bool IsActive()
        {
            return base.IsActive() && AudioClip != null;
        }

        /// <summary>
        /// Enumerates through <see cref="AudioClip"/> and pulses for each amplitude of the wave.
        /// </summary>
        /// <returns>An Enumerator to manage the running of the Coroutine.</returns>
        protected override IEnumerator HapticProcessRoutine()
        {
            int sampleOffset = -BufferSize;
            float startTime = Time.time;
            float length = AudioClip.length;
            float endTime = startTime + length;
            float sampleRate = AudioClip.samples;
            while (Time.time <= endTime)
            {
                float lerpVal = (Time.time - startTime) / length;
                int sampleIndex = (int)(sampleRate * lerpVal);
                if (sampleIndex >= sampleOffset + BufferSize)
                {
                    AudioClip.GetData(audioData, sampleIndex);
                    sampleOffset = sampleIndex;
                }
                float currentSample = Mathf.Abs(audioData[sampleIndex - sampleOffset]);
                HapticPulser.Intensity = currentSample * IntensityMultiplier;
                HapticPulser.Begin();
                yield return null;
            }
            ResetIntensity();
        }
    }
}