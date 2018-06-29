namespace VRTK.Core.Process.Moment
{
    using UnityEngine;

    /// <summary>
    /// Wrapper for an <see cref="IProcessable"/> process that has a state to determine when it is to be processed.
    /// </summary>
    public sealed class MomentProcess : MonoBehaviour
    {
        /// <summary>
        /// The process to attach to the moment.
        /// </summary>
        [Tooltip("The process to attach to the moment.")]
        public ProcessContainer process;
        /// <summary>
        /// The process only executes if the <see cref="GameObject"/> is active and the <see cref="Component"/> is enabled.
        /// </summary>
        [Tooltip("Only run the process if the process is on an active GameObject and the Component is enabled.")]
        public bool onlyProcessOnActiveAndEnabled = true;

        private void OnEnable()
        {
            // This empty implementation tells Unity to draw the enabled checkbox for this component, allowing to disable the component at edit-time.
        }
    }
}