namespace Zinnia.Event.Yield
{
    using Malimbe.MemberChangeMethod;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;
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
        [Serialized]
        [field: DocumentedByXml]
        public float SecondsToWait { get; set; }

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