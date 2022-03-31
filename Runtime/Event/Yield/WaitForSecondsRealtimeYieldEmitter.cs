namespace Zinnia.Event.Yield
{
    using Malimbe.MemberChangeMethod;
    using System.Collections;
    using UnityEngine;

    /// <summary>
    /// Yields after the <see cref="SecondsToWait"/> have passed in unscaled time.
    /// </summary>
    public class WaitForSecondsRealtimeYieldEmitter : YieldEmitter
    {
        /// <summary>
        /// The number of seconds to wait in unscaled time before yielding.
        /// </summary>
        [Tooltip("The number of seconds to wait in unscaled time before yielding.")]
        [SerializeField]
        private float _secondsToWait;
        public float SecondsToWait
        {
            get
            {
                return _secondsToWait;
            }
            set
            {
                _secondsToWait = value;
            }
        }

        /// <summary>
        /// The instruction to yield upon.
        /// </summary>
        protected WaitForSecondsRealtime yieldInstruction;

        /// <inheritdoc/>
        protected override IEnumerator YieldOn()
        {
            yield return yieldInstruction;
        }

        protected virtual void OnEnable()
        {
            OnAfterSecondsToWaitChange();
        }

        /// <summary>
        /// Called after <see cref="SecondsToWait"/> has been changed.
        /// </summary>
        [CalledAfterChangeOf(nameof(SecondsToWait))]
        protected virtual void OnAfterSecondsToWaitChange()
        {
            yieldInstruction = new WaitForSecondsRealtime(SecondsToWait);
        }
    }
}