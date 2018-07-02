namespace VRTK.Core.Tracking.Velocity
{
    using UnityEngine;

    /// <summary>
    /// Attempts to utilise the first <see cref="VelocityTracker"/> found on the given proxy <see cref="GameObject"/>.
    /// </summary>
    public class VelocityTrackerGameObjectProxy : VelocityTracker
    {
        /// <summary>
        /// The <see cref="GameObject"/> that contains a <see cref="VelocityTracker"/>.
        /// </summary>
        [Tooltip("The GameObject that contains a VelocityTracker.")]
        public GameObject proxySource;

        protected VelocityTracker source;

        /// <inheritdoc />
        public override bool IsActive()
        {
            return (isActiveAndEnabled && source != null && source.isActiveAndEnabled);
        }

        /// <summary>
        /// Sets the source of the <see cref="VelocityTracker"/> proxy.
        /// </summary>
        /// <param name="proxySource">The <see cref="GameObject"/> that contains a <see cref="VelocityTracker"/>.</param>
        public virtual void SetProxySource(GameObject proxySource)
        {
            this.proxySource = proxySource;
            if (this.proxySource != null)
            {
                source = proxySource.GetComponentInChildren<VelocityTracker>();
            }
        }

        /// <summary>
        /// Clears the existing proxy source.
        /// </summary>
        public virtual void ClearProxySource()
        {
            proxySource = null;
            source = null;
        }

        /// <inheritdoc />
        protected override Vector3 DoGetVelocity()
        {
            return source.GetVelocity();
        }

        /// <inheritdoc />
        protected override Vector3 DoGetAngularVelocity()
        {
            return source.GetAngularVelocity();
        }
    }
}