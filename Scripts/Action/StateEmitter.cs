namespace VRTK.Core.Action
{
    using UnityEngine;
    using VRTK.Core.Process;

    /// <summary>
    /// Re-emits the state of the given action whenever the process is run.
    /// </summary>
    public class StateEmitter : MonoBehaviour, IProcessable
    {
        /// <summary>
        /// The Action to re-emit the state for.
        /// </summary>
        [Tooltip("The Action to re-emit the state for.")]
        public Action action;

        /// <summary>
        /// Re-emits the state of the Action. 
        /// </summary>
        public virtual void Process()
        {
            if (!isActiveAndEnabled || action == null)
            {
                return;
            }

            action.EmitActivationState();
        }
    }
}