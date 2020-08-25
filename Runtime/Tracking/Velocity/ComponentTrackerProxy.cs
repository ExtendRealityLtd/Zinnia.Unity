namespace Zinnia.Tracking.Velocity
{
    using Malimbe.MemberChangeMethod;
    using Malimbe.MemberClearanceMethod;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;
    using UnityEngine;
    using Zinnia.Extension;

    /// <summary>
    /// Attempts to utilize the first <see cref="VelocityTracker"/> found on the given proxy <see cref="Component"/>.
    /// </summary>
    public class ComponentTrackerProxy : VelocityTracker
    {
        /// <summary>
        /// The <see cref="GameObject"/> that contains a <see cref="VelocityTracker"/>.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public GameObject ProxySource { get; set; }

        /// <summary>
        /// The cached <see cref="VelocityTracker"/> found on the proxy <see cref="Component"/>.
        /// </summary>
        protected VelocityTracker cachedVelocityTracker;

        protected virtual void OnEnable()
        {
            SetCachedVelocityTracker();
        }

        /// <inheritdoc />
        public override bool IsActive()
        {
            return base.IsActive() && cachedVelocityTracker != null && cachedVelocityTracker.isActiveAndEnabled;
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

        /// <summary>
        /// Called after <see cref="ProxySource"/> has been changed.
        /// </summary>
        [CalledAfterChangeOf(nameof(ProxySource))]
        protected virtual void OnAfterProxySourceChange()
        {
            SetCachedVelocityTracker();
        }

        /// <summary>
        /// Sets <see cref="cachedVelocityTracker"/> to the first found <see cref="VelocityTracker"> on the <see cref="ProxySource"> or any of its descendants.
        /// </summary>
        protected virtual void SetCachedVelocityTracker()
        {
            cachedVelocityTracker = ProxySource.TryGetComponent<VelocityTracker>(true);
        }
    }
}