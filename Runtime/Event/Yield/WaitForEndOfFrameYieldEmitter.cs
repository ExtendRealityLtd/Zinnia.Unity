namespace Zinnia.Event.Yield
{
    using System.Collections;
    using UnityEngine;

    /// <summary>
    /// Yields at the End of the Frame.
    /// </summary>
    public class WaitForEndOfFrameYieldEmitter : YieldEmitter
    {
        #region Yield Settings
        [Header("Yield Settings")]
        [Tooltip("The number of frames to wait before yielding.")]
        [SerializeField]
        private int framesUntilYield = 1;
        /// <summary>
        /// The number of frames to wait before yielding.
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
        protected WaitForEndOfFrame yieldInstruction = new WaitForEndOfFrame();

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