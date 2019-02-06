namespace Zinnia.Action
{
    using Malimbe.BehaviourStateRequirementMethod;
    using Malimbe.XmlDocumentationAttribute;
    using UnityEngine;
    using Zinnia.Process;

    /// <summary>
    /// Re-emits the state of the given action whenever the process is run.
    /// </summary>
    public class StateEmitter : MonoBehaviour, IProcessable
    {
        /// <summary>
        /// The Action to re-emit the state for.
        /// </summary>
        [DocumentedByXml]
        public Action action;

        /// <summary>
        /// Re-emits the state of the Action.
        /// </summary>
        [RequiresBehaviourState]
        public virtual void Process()
        {
            if (action == null)
            {
                return;
            }

            action.EmitActivationState();
        }
    }
}