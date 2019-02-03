namespace Zinnia.Tracking.Velocity
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using Malimbe.XmlDocumentationAttribute;

    /// <summary>
    /// Emits the velocities of a given <see cref="VelocityTracker"/>.
    /// </summary>
    public class VelocityEmitter : MonoBehaviour
    {
        /// <summary>
        /// Defines the event with the <see cref="Vector3"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<Vector3>
        {
        }

        /// <summary>
        /// The source <see cref="VelocityTracker "/> to receive the velocity data from.
        /// </summary>
        [DocumentedByXml]
        public VelocityTracker source;

        /// <summary>
        /// Emitted when the Tracked Velocity is emitted.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent VelocityEmitted = new UnityEvent();
        /// <summary>
        /// Emitted when the Tracked Angular Velocity is emitted.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent AngularVelocityEmitted = new UnityEvent();

        /// <summary>
        /// Emits the Velocity of the Tracked Velocity.
        /// </summary>
        public virtual void EmitVelocity()
        {
            if (!isActiveAndEnabled || source == null)
            {
                return;
            }

            VelocityEmitted?.Invoke(source.GetVelocity());
        }

        /// <summary>
        /// Emits the Angular Velocity of the Tracked Velocity.
        /// </summary>
        public virtual void EmitAngularVelocity()
        {
            if (!isActiveAndEnabled || source == null)
            {
                return;
            }

            AngularVelocityEmitted?.Invoke(source.GetAngularVelocity());
        }

        /// <summary>
        /// Emits the Velocity and Angular Velocity of the Tracked Velocity.
        /// </summary>
        public virtual void EmitAll()
        {
            EmitVelocity();
            EmitAngularVelocity();
        }
    }
}