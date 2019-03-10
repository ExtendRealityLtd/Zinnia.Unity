namespace Zinnia.Haptics
{
    using UnityEngine.XR;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;

    /// <summary>
    /// Creates a timed haptic pulse on an <see cref="XRNode"/>.
    /// </summary>
    public class XRNodeHapticPulser : HapticPulser
    {
        /// <summary>
        /// The node to pulse.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public XRNode Node { get; set; } = XRNode.LeftHand;
        /// <summary>
        /// The duration to pulse <see cref="Node"/> for.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public float Duration { get; set; } = 0.005f;

        /// <summary>
        /// The haptic capabilities of <see cref="Node"/>.
        /// </summary>
        protected HapticCapabilities nodeHapticCapabilities;

        /// <inheritdoc />
        protected override void DoBegin()
        {
            Pulse(Intensity, Duration);
        }

        /// <inheritdoc />
        protected override void DoCancel()
        {
            Pulse(0f, 0f);
        }

        /// <summary>
        /// Sends a pulse to <see cref="Node"/>.
        /// </summary>
        /// <param name="intensity">The intensity to pulse with.</param>
        /// <param name="duration">The duration to pulse for.</param>
        protected virtual void Pulse(float intensity, float duration)
        {
            InputDevice device = InputDevices.GetDeviceAtXRNode(Node);
            if (!device.TryGetHapticCapabilities(out nodeHapticCapabilities))
            {
                return;
            }

            if (nodeHapticCapabilities.supportsImpulse)
            {
                device.SendHapticImpulse(0, intensity, duration);
            }
            else if (nodeHapticCapabilities.supportsBuffer)
            {
                byte[] clip = GeneratePulseBuffer(duration, intensity);
                device.SendHapticBuffer(0, clip);
            }
        }

        /// <summary>
        /// Generates a pulse buffer array.
        /// </summary>
        /// <param name="intensity">The intensity to pulse with.</param>
        /// <param name="duration">The duration to pulse for.</param>
        /// <returns>The buffer array containing the pulse data.</returns>
        protected virtual byte[] GeneratePulseBuffer(float intensity, float duration)
        {
            int clipCount = (int)(nodeHapticCapabilities.bufferFrequencyHz * duration);
            byte[] clip = new byte[clipCount];
            for (int index = 0; index < clipCount; index++)
            {
                clip[index] = (byte)(byte.MaxValue * intensity);
            }
            return clip;
        }
    }
}