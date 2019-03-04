namespace Zinnia.Action
{
    using Malimbe.BehaviourStateRequirementMethod;
    using Malimbe.PropertySerializationAttribute;
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
        [Serialized]
        [field: DocumentedByXml]
        public Action Action { get; set; }

        /// <summary>
        /// Re-emits the state of the Action.
        /// </summary>
        [RequiresBehaviourState]
        public virtual void Process()
        {
            if (Action == null)
            {
                return;
            }

            Action.EmitActivationState();
        }
    }
}