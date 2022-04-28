namespace Zinnia.Tracking.Velocity
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using Zinnia.Extension;

    /// <summary>
    /// Emits the velocities of a given <see cref="VelocityTracker"/>.
    /// </summary>
    public class VelocityEmitter : MonoBehaviour
    {
        /// <summary>
        /// Defines the event with the <see cref="Vector3"/>.
        /// </summary>
        [Serializable]
        public class Vector3UnityEvent : UnityEvent<Vector3> { }

        /// <summary>
        /// Defines the event with the <see cref="float"/>.
        /// </summary>
        [Serializable]
        public class FloatUnityEvent : UnityEvent<float> { }

        [Tooltip("The source VelocityTracker to receive the velocity data from.")]
        [SerializeField]
        private VelocityTracker source;
        /// <summary>
        /// The source <see cref="VelocityTracker"/> to receive the velocity data from.
        /// </summary>
        public VelocityTracker Source
        {
            get
            {
                return source;
            }
            set
            {
                source = value;
            }
        }

        /// <summary>
        /// Emitted when the Tracked Velocity is emitted.
        /// </summary>
        public Vector3UnityEvent VelocityEmitted = new Vector3UnityEvent();
        /// <summary>
        /// Emitted when the Tracked Speed is emitted.
        /// </summary>
        public FloatUnityEvent SpeedEmitted = new FloatUnityEvent();
        /// <summary>
        /// Emitted when the Tracked Angular Velocity is emitted.
        /// </summary>
        public Vector3UnityEvent AngularVelocityEmitted = new Vector3UnityEvent();
        /// <summary>
        /// Emitted when the Tracked Angular Speed is emitted.
        /// </summary>
        public FloatUnityEvent AngularSpeedEmitted = new FloatUnityEvent();

        /// <summary>
        /// Clears <see cref="Source"/>.
        /// </summary>
        public virtual void ClearSource()
        {
            if (!this.IsValidState())
            {
                return;
            }

            Source = default;
        }

        /// <summary>
        /// Emits the Velocity of the Tracked Velocity.
        /// </summary>
        public virtual void EmitVelocity()
        {
            if (!this.IsValidState() || Source == null)
            {
                return;
            }

            VelocityEmitted?.Invoke(Source.GetVelocity());
        }

        /// <summary>
        /// Emits the Speed of the Tracked Velocity.
        /// </summary>
        public virtual void EmitSpeed()
        {
            if (!this.IsValidState() || Source == null)
            {
                return;
            }

            SpeedEmitted?.Invoke(Source.GetVelocity().magnitude);
        }

        /// <summary>
        /// Emits the Angular Velocity of the Tracked Velocity.
        /// </summary>
        public virtual void EmitAngularVelocity()
        {
            if (!this.IsValidState() || Source == null)
            {
                return;
            }

            AngularVelocityEmitted?.Invoke(Source.GetAngularVelocity());
        }

        /// <summary>
        /// Emits the Angular Velocity of the Tracked Velocity.
        /// </summary>
        public virtual void EmitAngularSpeed()
        {
            if (!this.IsValidState() || Source == null)
            {
                return;
            }

            AngularSpeedEmitted?.Invoke(Source.GetAngularVelocity().magnitude);
        }

        /// <summary>
        /// Emits the Velocity, Speed, Angular Velocity and Angular Speed of the Tracked Velocity.
        /// </summary>
        public virtual void EmitAll()
        {
            EmitVelocity();
            EmitSpeed();
            EmitAngularVelocity();
            EmitAngularSpeed();
        }
    }
}