namespace Zinnia.Haptics
{
    using UnityEngine;
    using System.Collections;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;

    /// <summary>
    /// Creates a haptic pattern based on the waveform of an <see cref="UnityEngine.AudioClip"/> and utilizes a <see cref="Haptics.HapticPulser"/> to create the effect.
    /// </summary>
    public class AudioClipHapticPulser : HapticProcess
    {
        /// <summary>
        /// The pulse process to utilize.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public HapticPulser HapticPulser { get; set; }
        /// <summary>
        /// The waveform to represent the haptic pattern.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public AudioClip AudioClip { get; set; }
        /// <summary>
        /// Multiplies the current audio peak to affect the wave intensity.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public float IntensityMultiplier { get; set; } = 1f;

        /// <summary>
        /// The size of the audio buffer.
        /// </summary>
        protected const int BufferSize = 8192;
        /// <summary>
        /// A reference to the started routine.
        /// </summary>
        protected Coroutine hapticRoutine;
        /// <summary>
        /// The original intensity of <see cref="HapticPulser"/> to reset back to after the process is complete.
        /// </summary>
        protected float cachedIntensity;
        /// <summary>
        /// The audio data buffer.
        /// </summary>
        protected readonly float[] audioData = new float[BufferSize];

        /// <inheritdoc />
        public override bool IsActive()
        {
            return base.IsActive() && HapticPulser != null && AudioClip != null && HapticPulser.IsActive();
        }

        /// <inheritdoc />
        protected override void DoBegin()
        {
            cachedIntensity = HapticPulser.Intensity;
            hapticRoutine = StartCoroutine(HapticProcessRoutine());
        }

        /// <inheritdoc />
        protected override void DoCancel()
        {
            if (hapticRoutine == null)
            {
                return;
            }

            StopCoroutine(hapticRoutine);
            hapticRoutine = null;
            HapticPulser.Cancel();
            ResetIntensity();
        }

        /// <summary>
        /// Resets the <see cref="Haptics.HapticPulser.Intensity"/> back to it's original value.
        /// </summary>
        protected virtual void ResetIntensity()
        {
            HapticPulser.Intensity = cachedIntensity;
        }

        /// <summary>
        /// Enumerates through <see cref="AudioClip"/> and pulses for each amplitude of the wave.
        /// </summary>
        /// <returns>An Enumerator to manage the running of the Coroutine.</returns>
        protected virtual IEnumerator HapticProcessRoutine()
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