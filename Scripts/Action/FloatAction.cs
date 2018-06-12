namespace VRTK.Core.Action
{
    using UnityEngine.Events;
    using System;
    using VRTK.Core.Utility;

    /// <summary>
    /// Emits a <see cref="float"/> value.
    /// </summary>
    public class FloatAction : BaseAction<float, FloatAction.FloatActionUnityEvent>
    {
        /// <summary>
        /// Defines the event with the <see cref="float"/> value and sender <see cref="object"/>.
        /// </summary>
        [Serializable]
        public class FloatActionUnityEvent : UnityEvent<float, object>
        {
        }

        public float equalityTolerance = float.Epsilon;

        protected override void Awake()
        {
            equalityComparer = new ApproximatelyFloatComparer(equalityTolerance);
        }

        /// <inheritdoc/>
        public override void Receive(float value, object sender = null)
        {
            ApproximatelyFloatComparer approximatelyComparer = equalityComparer as ApproximatelyFloatComparer;
            if (approximatelyComparer != null)
            {
                approximatelyComparer.tolerance = equalityTolerance;
            }

            base.Receive(value, sender);
        }
    }
}