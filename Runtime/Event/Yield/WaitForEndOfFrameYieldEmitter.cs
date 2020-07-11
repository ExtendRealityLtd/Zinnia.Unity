namespace Zinnia.Event.Yield
{
    using System.Collections;
    using UnityEngine;

    /// <summary>
    /// Yields at the End of the Frame.
    /// </summary>
    public class WaitForEndOfFrameYieldEmitter : YieldEmitter
    {
        /// <summary>
        /// The instruction to yield upon.
        /// </summary>
        protected WaitForEndOfFrame yieldInstruction = new WaitForEndOfFrame();

        /// <inheritdoc/>
        protected override IEnumerator YieldOn()
        {
            yield return yieldInstruction;
        }
    }
}