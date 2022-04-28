namespace Zinnia.Tracking.Velocity
{
    using System.Collections;
    using UnityEngine;

    /// <summary>
    /// Derived from <see cref="ArtificialVelocityApplierProcess"/> to apply artificial velocities to the <see cref="Target"/> by changing the <see cref="Transform"/> properties.
    /// </summary>
    /// <remarks>
    /// This applier uses coroutine.
    /// </remarks>
    public class ArtificialVelocityApplier : ArtificialVelocityApplierProcess
    {
        /// <summary>
        /// The routine to handle the deceleration of the object based on the drag over time.
        /// </summary>
        protected Coroutine decelerationRoutine;

        /// <inheritdoc />
        public override void Apply()
        {
            CancelDeceleration();
            canProcess = true;
            decelerationRoutine = StartCoroutine(BeginDeceleration());
        }

        /// <summary>
        /// Cancels the <see cref="decelerationRoutine"/>.
        /// </summary>
        public override void CancelDeceleration()
        {
            canProcess = false;
            if (decelerationRoutine != null)
            {
                StopCoroutine(decelerationRoutine);
                decelerationRoutine = null;
            }
        }

        /// <summary>
        /// Begins decelerating the <see cref="Target"/> based on any opposing drag forces.
        /// </summary>
        /// <returns>An Enumerator to manage the running state of the Coroutine.</returns>
        protected virtual IEnumerator BeginDeceleration()
        {
            while (canProcess)
            {
                Process();
                yield return null;
            }

            decelerationRoutine = null;
        }
    }
}