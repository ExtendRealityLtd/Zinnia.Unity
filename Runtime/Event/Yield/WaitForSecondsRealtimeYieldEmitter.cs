namespace Zinnia.Event.Yield
{
    using System.Collections;
    using UnityEngine;
    using Zinnia.Extension;

    /// <summary>
    /// Yields after the <see cref="SecondsToWait"/> have passed in unscaled time.
    /// </summary>
    public class WaitForSecondsRealtimeYieldEmitter : YieldEmitter
    {
        [Tooltip("The number of seconds to wait in unscaled time before yielding.")]
        [SerializeField]
        private float secondsToWait;
        /// <summary>
        /// The number of seconds to wait in unscaled time before yielding.
        /// </summary>
        public float SecondsToWait
        {
            get
            {
                return secondsToWait;
            }
            set
            {
                secondsToWait = value;
                if (this.IsMemberChangeAllowed())
                {
                    OnAfterSecondsToWaitChange();
                }
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
        protected virtual void OnAfterSecondsToWaitChange()
        {
            yieldInstruction = new WaitForSecondsRealtime(SecondsToWait);
        }
    }
}