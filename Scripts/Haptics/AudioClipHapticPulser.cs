namespace VRTK.Core.Haptics
{
    using UnityEngine;
    using System.Collections;

    /// <summary>
    /// Creates a haptic pattern based on the waveform of an <see cref="UnityEngine.AudioClip"/> and utilizes a <see cref="HapticPulser"/> to create the effect.
    /// </summary>
    public class AudioClipHapticPulser : HapticProcess
    {
        /// <summary>
        /// The pulse process to utilize.
        /// </summary>
        [Tooltip("The pulse process to utilize.")]
        public HapticPulser hapticPulser;
        /// <summary>
        /// The waveform to represent the haptic pattern.
        /// </summary>
        [Tooltip("The waveform to represent the haptic pattern.")]
        public AudioClip audioClip;
        /// <summary>
        /// Multiplies the current audio peak to affect the wave intensity.
        /// </summary>
        [Tooltip("Multiplies the current audio peak to affect the wave intensity.")]
        public float intensityMultiplier = 1f;

        /// <summary>
        /// A reference to the started routine.
        /// </summary>
        protected Coroutine hapticRoutine;
        /// <summary>
        /// The original intensity of the provided <see cref="HapticPulser"/> to reset back to after the process is complete.
        /// </summary>
        protected float cachedIntensity;
        /// <summary>
        /// The size of the audio buffer.
        /// </summary>
        protected int bufferSize = 8192;

        /// <inheritdoc />
        public override bool IsActive()
        {
            return (base.IsActive() && hapticPulser != null && audioClip != null && hapticPulser.IsActive());
        }

        /// <inheritdoc />
        protected override void DoBegin()
        {
            cachedIntensity = hapticPulser.Intensity;
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
            hapticPulser.Cancel();
            ResetIntensity();
        }

        /// <summary>
        /// Resets the <see cref="HapticPulse.Intensity"/> back to it's original value.
        /// </summary>
        protected virtual void ResetIntensity()
        {
            hapticPulser.Intensity = cachedIntensity;
        }

        /// <summary>
        /// Enumerates through the given <see cref="AudioClip"/> and pulses for each amplitude of the wave.
        /// </summary>
        /// <returns>An Enumerator to manage the running of the Coroutine.</returns>
        protected virtual IEnumerator HapticProcessRoutine()
        {
            float[] audioData = new float[bufferSize];
            int sampleOffset = -bufferSize;
            float startTime = Time.time;
            float length = audioClip.length;
            float endTime = startTime + length;
            float sampleRate = audioClip.samples;
            while (Time.time <= endTime)
            {
                float lerpVal = (Time.time - startTime) / length;
                int sampleIndex = (int)(sampleRate * lerpVal);
                if (sampleIndex >= sampleOffset + bufferSize)
                {
                    audioClip.GetData(audioData, sampleIndex);
                    sampleOffset = sampleIndex;
                }
                float currentSample = Mathf.Abs(audioData[sampleIndex - sampleOffset]);
                hapticPulser.Intensity = currentSample * intensityMultiplier;
                hapticPulser.Begin();
                yield return null;
            }
            ResetIntensity();
        }
    }
}