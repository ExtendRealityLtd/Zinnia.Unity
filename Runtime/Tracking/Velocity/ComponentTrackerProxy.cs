namespace Zinnia.Tracking.Velocity
{
    using UnityEngine;
    using Zinnia.Extension;

    /// <summary>
    /// Attempts to utilize the first <see cref="VelocityTracker"/> found on the given proxy <see cref="Component"/>.
    /// </summary>
    public class ComponentTrackerProxy : VelocityTracker
    {
        [Tooltip("The GameObject that contains a VelocityTracker.")]
        [SerializeField]
        private GameObject proxySource;
        /// <summary>
        /// The <see cref="GameObject"/> that contains a <see cref="VelocityTracker"/>.
        /// </summary>
        public GameObject ProxySource
        {
            get
            {
                return proxySource;
            }
            set
            {
                proxySource = value;
                if (this.IsMemberChangeAllowed())
                {
                    OnAfterProxySourceChange();
                }
            }
        }

        /// <summary>
        /// The cached <see cref="VelocityTracker"/> found on the proxy <see cref="Component"/>.
        /// </summary>
        protected VelocityTracker cachedVelocityTracker;

        /// <summary>
        /// Clears <see cref="ProxySource"/>.
        /// </summary>
        public virtual void ClearProxySource()
        {
            if (!this.IsValidState())
            {
                return;
            }

            ProxySource = default;
        }

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