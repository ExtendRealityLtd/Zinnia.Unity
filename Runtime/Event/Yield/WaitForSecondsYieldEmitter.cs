namespace Zinnia.Event.Yield
{
    using System.Collections;
    using UnityEngine;
    using Zinnia.Extension;

    /// <summary>
    /// Yields after the <see cref="SecondsToWait"/> have passed in scaled time by <see cref="Time.timeScale"/>.
    /// </summary>
    public class WaitForSecondsYieldEmitter : YieldEmitter
    {
        [Tooltip("The number of seconds to wait before yielding.")]
        [SerializeField]
        private float secondsToWait;
        /// <summary>
        /// The number of seconds to wait before yielding.
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
        protected virtual void OnAfterSecondsToWaitChange()
        {
            yieldInstruction = new WaitForSeconds(SecondsToWait);
        }
    }
}