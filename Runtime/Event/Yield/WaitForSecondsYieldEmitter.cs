namespace Zinnia.Event.Yield
{
    using Malimbe.MemberChangeMethod;
    using System.Collections;
    using UnityEngine;

    /// <summary>
    /// Yields after the <see cref="SecondsToWait"/> have passed in scaled time by <see cref="Time.timeScale"/>.
    /// </summary>
    public class WaitForSecondsYieldEmitter : YieldEmitter
    {
        /// <summary>
        /// The number of seconds to wait before yielding.
        /// </summary>
        [Tooltip("The number of seconds to wait before yielding.")]
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
        protected WaitForSeconds yieldInstruction;

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
            yieldInstruction = new WaitForSeconds(SecondsToWait);
        }
    }
}