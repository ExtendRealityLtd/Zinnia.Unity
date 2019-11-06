namespace Zinnia.Tracking.Velocity
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using Malimbe.MemberClearanceMethod;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.BehaviourStateRequirementMethod;

    /// <summary>
    /// Emits the velocities of a given <see cref="VelocityTracker"/>.
    /// </summary>
    public class VelocityEmitter : MonoBehaviour
    {
        /// <summary>
        /// Defines the event with the <see cref="Vector3"/>.
        /// </summary>
        [Serializable]
        public class Vector3UnityEvent : UnityEvent<Vector3>
        {
        }

        /// <summary>
        /// Defines the event with the <see cref="float"/>.
        /// </summary>
        [Serializable]
        public class FloatUnityEvent : UnityEvent<float>
        {
        }

        /// <summary>
        /// The source <see cref="VelocityTracker"/> to receive the velocity data from.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public VelocityTracker Source { get; set; }

        /// <summary>
        /// Emitted when the Tracked Velocity is emitted.
        /// </summary>
        [DocumentedByXml]
        public Vector3UnityEvent VelocityEmitted = new Vector3UnityEvent();
        /// <summary>
        /// Emitted when the Tracked Speed is emitted.
        /// </summary>
        [DocumentedByXml]
        public FloatUnityEvent SpeedEmitted = new FloatUnityEvent();
        /// <summary>
        /// Emitted when the Tracked Angular Velocity is emitted.
        /// </summary>
        [DocumentedByXml]
        public Vector3UnityEvent AngularVelocityEmitted = new Vector3UnityEvent();
        /// <summary>
        /// Emitted when the Tracked Angular Speed is emitted.
        /// </summary>
        [DocumentedByXml]
        public FloatUnityEvent AngularSpeedEmitted = new FloatUnityEvent();

        /// <summary>
        /// Emits the Velocity of the Tracked Velocity.
        /// </summary>
        [RequiresBehaviourState]
        public virtual void EmitVelocity()
        {
            if (Source == null)
            {
                return;
            }

            VelocityEmitted?.Invoke(Source.GetVelocity());
        }

        /// <summary>
        /// Emits the Speed of the Tracked Velocity.
        /// </summary>
        [RequiresBehaviourState]
        public virtual void EmitSpeed()
        {
            if (Source == null)
            {
                return;
            }

            SpeedEmitted?.Invoke(Source.GetVelocity().magnitude);
        }

        /// <summary>
        /// Emits the Angular Velocity of the Tracked Velocity.
        /// </summary>
        [RequiresBehaviourState]
        public virtual void EmitAngularVelocity()
        {
            if (Source == null)
            {
                return;
            }

            AngularVelocityEmitted?.Invoke(Source.GetAngularVelocity());
        }

        /// <summary>
        /// Emits the Angular Velocity of the Tracked Velocity.
        /// </summary>
        [RequiresBehaviourState]
        public virtual void EmitAngularSpeed()
        {
            if (Source == null)
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