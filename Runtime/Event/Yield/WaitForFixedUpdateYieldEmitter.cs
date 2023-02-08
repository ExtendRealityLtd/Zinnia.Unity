namespace Zinnia.Event.Yield
{
    using System.Collections;
    using UnityEngine;

    /// <summary>
    /// Yields after the FixedUpdate moment.
    /// </summary>
    public class WaitForFixedUpdateYieldEmitter : YieldEmitter
    {
        #region Yield Settings
        [Header("Yield Settings")]
        [Tooltip("The number of fixed frames to wait before yielding.")]
        [SerializeField]
        private int framesUntilYield = 1;
        /// <summary>
        /// The number of fixed frames to wait before yielding.
        /// </summary>
        public int FramesUntilYield
        {
            get
            {
                return framesUntilYield;
            }
            set
            {
                framesUntilYield = value;
            }
        }
        #endregion

        /// <summary>
        /// The instruction to yield upon.
        /// </summary>
        protected WaitForFixedUpdate yieldInstruction = new WaitForFixedUpdate();

        /// <inheritdoc/>
        protected override IEnumerator YieldOn()
        {
            for (int i = 0; i < framesUntilYield; i++)
            {
                yield return yieldInstruction;
            }
        }
    }
}