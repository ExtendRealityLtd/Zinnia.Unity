namespace Zinnia.Tracking.Velocity
{
    using Malimbe.BehaviourStateRequirementMethod;
    using Malimbe.MemberClearanceMethod;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;
    using UnityEngine;
    using Zinnia.Extension;
    using Zinnia.Process;

    /// <summary>
    /// Applies artificial velocities to the <see cref="Target"/> by changing the <see cref="Transform"/> properties.
    /// </summary>
    public class ArtificialVelocityApplierProcess : MonoBehaviour, IProcessable
    {
        /// <summary>
        /// The object to apply the artificial velocities to.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public GameObject Target { get; set; }

        /// <summary>
        /// The velocity to apply.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public Vector3 Velocity { get; set; }

        /// <summary>
        /// The angular velocity to apply.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public Vector3 AngularVelocity { get; set; }

        /// <summary>
        /// The drag to apply to reduce the directional velocity over time and to slow down <see cref="Target"/>.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public float Drag { get; set; } = 1f;

        /// <summary>
        /// The angular drag to apply to reduce the rotational velocity over time and to slow down <see cref="Target"/>.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public float AngularDrag { get; set; } = 0.5f;

        /// <summary>
        /// The tolerance the velocity can be within zero to be considered nil.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public float NilVelocityTolerance { get; set; } = 0.001f;

        /// <summary>
        /// The tolerance the angular velocity can be within zero to be considered nil.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public float NilAngularVelocityTolerance { get; set; } = 0.001f;

        /// <summary>
        /// Determine if we can process.
        /// </summary>
        protected bool canProcess = false;

        /// <summary>
        /// Applies the velocity data to the <see cref="Target"/>.
        /// </summary>
        [RequiresBehaviourState]
        public virtual void Apply()
        {
            canProcess = true;
        }

        /// <summary>
        /// Sets the <see cref="Velocity"/> x value.
        /// </summary>
        /// <param name="value">The value to set to.</param>
        public virtual void SetVelocityX(float value)
        {
            Velocity = new Vector3(value, Velocity.y, Velocity.z);
        }

        /// <summary>
        /// Sets the <see cref="Velocity"/> y value.
        /// </summary>
        /// <param name="value">The value to set to.</param>
        public virtual void SetVelocityY(float value)
        {
            Velocity = new Vector3(Velocity.x, value, Velocity.z);
        }

        /// <summary>
        /// Sets the <see cref="Velocity"/> z value.
        /// </summary>
        /// <param name="value">The value to set to.</param>
        public virtual void SetVelocityZ(float value)
        {
            Velocity = new Vector3(Velocity.x, Velocity.y, value);
        }

        /// <summary>
        /// Increments the <see cref="Velocity"/> by the given value.
        /// </summary>
        /// <param name="value">The value to increment by.</param>
        public virtual void IncrementVelocity(Vector3 value)
        {
            Velocity += value;
        }

        /// <summary>
        /// Reset <see cref="Velocity"/> to <see cref="Vector3.zero"/>.
        /// </summary>
        public virtual void ClearVelocity()
        {
            Velocity = Vector3.zero;
        }

        /// <summary>
        /// Sets the <see cref="AngularVelocity"/> x value.
        /// </summary>
        /// <param name="value">The value to set to.</param>
        public virtual void SetAngularVelocityX(float value)
        {
            AngularVelocity = new Vector3(value, AngularVelocity.y, AngularVelocity.z);
        }

        /// <summary>
        /// Sets the <see cref="AngularVelocity"/> y value.
        /// </summary>
        /// <param name="value">The value to set to.</param>
        public virtual void SetAngularVelocityY(float value)
        {
            AngularVelocity = new Vector3(AngularVelocity.x, value, AngularVelocity.z);
        }

        /// <summary>
        /// Sets the <see cref="AngularVelocity"/> z value.
        /// </summary>
        /// <param name="value">The value to set to.</param>
        public virtual void SetAngularVelocityZ(float value)
        {
            AngularVelocity = new Vector3(AngularVelocity.x, AngularVelocity.y, value);
        }

        /// <summary>
        /// Increments the <see cref="AngularVelocity"/> by the given value.
        /// </summary>
        /// <param name="value">The value to increment by.</param>
        public virtual void IncrementAngularVelocity(Vector3 value)
        {
            AngularVelocity += value;
        }

        /// <summary>
        /// Reset <see cref="AngularVelocity"/> to <see cref="Vector3.zero"/>.
        /// </summary>
        public virtual void ClearAngularVelocity()
        {
            AngularVelocity = Vector3.zero;
        }

        protected virtual void OnDisable()
        {
            CancelDeceleration();
        }

        /// <inheritdoc />
        public virtual void Process()
        {
            if (!canProcess)
            {
                return;
            }

            if (!Velocity.ApproxEquals(Vector3.zero, NilVelocityTolerance) || !AngularVelocity.ApproxEquals(Vector3.zero, NilAngularVelocityTolerance))
            {
                float deltaTime = Time.inFixedTimeStep ? Time.fixedDeltaTime : Time.deltaTime;
                Velocity = Vector3.Lerp(Velocity, Vector3.zero, Drag * deltaTime);
                AngularVelocity = Vector3.Lerp(AngularVelocity, Vector3.zero, AngularDrag * deltaTime);
                Target.transform.localRotation *= Quaternion.Euler(AngularVelocity);
                Target.transform.localPosition += Velocity * deltaTime;
            }
            else
            {
                Velocity = Vector3.zero;
                AngularVelocity = Vector3.zero;
                canProcess = false;
            }
        }

        /// <summary>
        /// Cancels the <see cref="decelerationRoutine"/>.
        /// </summary>
        protected virtual void CancelDeceleration()
        {
            canProcess = false;
        }
    }
}