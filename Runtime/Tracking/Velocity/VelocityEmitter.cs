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
        public class UnityEvent : UnityEvent<Vector3>
        {
        }

        /// <summary>
        /// The source <see cref="VelocityTracker "/> to receive the velocity data from.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public VelocityTracker Source { get; set; }

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
        /// Emits the Velocity and Angular Velocity of the Tracked Velocity.
        /// </summary>
        public virtual void EmitAll()
        {
            EmitVelocity();
            EmitAngularVelocity();
        }
    }
}