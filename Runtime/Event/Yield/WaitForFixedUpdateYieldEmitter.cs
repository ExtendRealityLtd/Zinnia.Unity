namespace Zinnia.Event.Yield
{
    using System.Collections;
    using UnityEngine;

    /// <summary>
    /// Yields after the FixedUpdate moment.
    /// </summary>
    public class WaitForFixedUpdateYieldEmitter : YieldEmitter
    {
        /// <summary>
        /// The instruction to yield upon.
        /// </summary>
        protected WaitForFixedUpdate yieldInstruction = new WaitForFixedUpdate();

        /// <inheritdoc/>
        protected override IEnumerator YieldOn()
        {
            yield return yieldInstruction;
        }
    }
}