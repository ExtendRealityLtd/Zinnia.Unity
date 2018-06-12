namespace VRTK.Core.Action
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using VRTK.Core.Utility;

    /// <summary>
    /// Emits a <see cref="Vector2"/> value.
    /// </summary>
    public class Vector2Action : BaseAction<Vector2, Vector2Action.Vector2ActionUnityEvent>
    {
        /// <summary>
        /// Defines the event with the <see cref="Vector2"/> value and sender <see cref="object"/>.
        /// </summary>
        [Serializable]
        public class Vector2ActionUnityEvent : UnityEvent<Vector2, object>
        {
        }

        public float equalityTolerance = float.Epsilon;

        protected override void Awake()
        {
            equalityComparer = new ApproximatelyVector2Comparer(equalityTolerance);
        }

        /// <inheritdoc/>
        public override void Receive(Vector2 value, object sender = null)
        {
            ApproximatelyVector2Comparer approximatelyComparer = equalityComparer as ApproximatelyVector2Comparer;
            if (approximatelyComparer != null)
            {
                approximatelyComparer.tolerance = equalityTolerance;
            }

            base.Receive(value, sender);
        }
    }
}