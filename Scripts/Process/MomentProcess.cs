namespace VRTK.Core.Process
{
    using UnityEngine;

    /// <summary>
    /// The MomentProcess is a wrapper for an IProcessable process that has a state to determine when it is to be processed.
    /// </summary>
    public sealed class MomentProcess : MonoBehaviour
    {
        [Tooltip("The process to attach to the moment.")]
        public ProcessContainer process;
        [Tooltip("Only run the process if the process is on an active GameObject and the component is enabled.")]
        public bool onlyProcessOnActiveAndEnabled = true;
    }
}