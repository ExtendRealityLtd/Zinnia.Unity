namespace Zinnia.Tracking.Velocity
{
    using UnityEngine;
    using System.Collections;
    using Malimbe.MemberClearanceMethod;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.BehaviourStateRequirementMethod;
    using Zinnia.Extension;

    /// <summary>
    /// Applies artificial velocities to the <see cref="Target"/> by changing the <see cref="Transform"/> properties.
    /// </summary>
    public class ArtificialVelocityApplier : MonoBehaviour
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
        /// The routine to handle the deceleration of the object based on the drag over time.
        /// </summary>
        private Coroutine decelerationRoutine;

        /// <summary>
        /// Applies the velocity data to the <see cref="Target"/>.
        /// </summary>
        [RequiresBehaviourState]
        public virtual void Apply()
        {
            CancelDeceleration();
            decelerationRoutine = StartCoroutine(BeginDeceleration());
        }

        /// <summary>
        /// Reset <see cref="Velocity"/> to <see cref="Vector3.zero"/>.
        /// </summary>
        public virtual void ClearVelocity()
        {
            Velocity = Vector3.zero;
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

        /// <summary>
        /// Begins decelerating the <see cref="Target"/> based on any opposing drag forces.
        /// </summary>
        /// <returns>An Enumerator to manage the running state of the Coroutine.</returns>
        protected virtual IEnumerator BeginDeceleration()
        {
            while (!Velocity.ApproxEquals(Vector3.zero, NilVelocityTolerance) || !AngularVelocity.ApproxEquals(Vector3.zero, NilAngularVelocityTolerance))
            {
                Velocity = Vector3.Slerp(Velocity, Vector3.zero, Drag * Time.deltaTime);
                AngularVelocity = Vector3.Slerp(AngularVelocity, Vector3.zero, AngularDrag * Time.deltaTime);
                Target.transform.localRotation *= Quaternion.Euler(AngularVelocity);
                Target.transform.localPosition += Velocity * Time.deltaTime;
                yield return null;
            }

            decelerationRoutine = null;
            Velocity = Vector3.zero;
            AngularVelocity = Vector3.zero;
        }

        /// <summary>
        /// Cancels the <see cref="decelerationRoutine"/>.
        /// </summary>
        protected virtual void CancelDeceleration()
        {
            if (decelerationRoutine == null)
            {
                return;
            }

            StopCoroutine(decelerationRoutine);
            decelerationRoutine = null;
        }
    }
}