namespace Zinnia.Action
{
    using UnityEngine;
    using Zinnia.Extension;
    using Zinnia.Process;

    /// <summary>
    /// Re-emits the state of the given action whenever the process is run.
    /// </summary>
    public class StateEmitter : MonoBehaviour, IProcessable
    {
        [Tooltip("The Action to re-emit the state for.")]
        [SerializeField]
        private Action _action;
        /// <summary>
        /// The Action to re-emit the state for.
        /// </summary>
        public Action Action
        {
            get
            {
                return _action;
            }
            set
            {
                _action = value;
            }
        }

        /// <summary>
        /// Re-emits the state of the Action.
        /// </summary>
        public virtual void Process()
        {
            if (!this.IsValidState() || Action == null)
            {
                return;
            }

            Action.EmitActivationState();
        }
    }
}