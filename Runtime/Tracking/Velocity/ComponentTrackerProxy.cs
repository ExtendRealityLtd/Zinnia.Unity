namespace Zinnia.Tracking.Velocity
{
    using Malimbe.MemberClearanceMethod;
    using Malimbe.XmlDocumentationAttribute;
    using UnityEngine;

    /// <summary>
    /// Attempts to utilize the first <see cref="VelocityTracker"/> found on the given proxy <see cref="Component"/>.
    /// </summary>
    public class ComponentTrackerProxy : VelocityTracker
    {
        /// <summary>
        /// The <see cref="Component"/> that contains a <see cref="VelocityTracker"/>.
        /// </summary>
        [DocumentedByXml, Cleared]
        public Component proxySource;

        /// <summary>
        /// The cached <see cref="VelocityTracker"/> found on the proxy <see cref="Component"/>.
        /// </summary>
        protected VelocityTracker cachedVelocityTracker;

        /// <inheritdoc />
        public override bool IsActive()
        {
            return (base.IsActive() && cachedVelocityTracker != null && cachedVelocityTracker.isActiveAndEnabled);
        }

        /// <summary>
        /// Sets the source of the <see cref="VelocityTracker"/> proxy.
        /// </summary>
        /// <param name="proxySource">The <see cref="Component"/> that contains a <see cref="VelocityTracker"/>.</param>
        public virtual void SetProxySource(Component proxySource)
        {
            this.proxySource = proxySource;
            cachedVelocityTracker = (proxySource == null ? null : proxySource.GetComponentInChildren<VelocityTracker>());
        }

        /// <summary>
        /// Sets the source of the <see cref="VelocityTracker"/> proxy.
        /// </summary>
        /// <param name="proxySource">The <see cref="GameObject"/> that contains a <see cref="VelocityTracker"/>.</param>
        public virtual void SetProxySource(GameObject proxySource)
        {
            SetProxySource(proxySource == null ? null : proxySource.GetComponent<Component>());
        }

        /// <summary>
        /// Clears the existing proxy source.
        /// </summary>
        public virtual void ClearProxySource()
        {
            cachedVelocityTracker = null;
        }

        /// <inheritdoc />
        protected override Vector3 DoGetVelocity()
        {
            return cachedVelocityTracker.GetVelocity();
        }

        /// <inheritdoc />
        protected override Vector3 DoGetAngularVelocity()
        {
            return cachedVelocityTracker.GetAngularVelocity();
        }
    }
}