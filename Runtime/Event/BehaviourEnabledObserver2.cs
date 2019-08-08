namespace Zinnia.Event
{
    using UnityEngine;
    using UnityEngine.Events;
    using Malimbe.BehaviourStateRequirementMethod;
    using Malimbe.MemberClearanceMethod;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;
    using Zinnia.Data.Collection.List;
    using Zinnia.Extension;
    using Zinnia.Process;
    using Zinnia.Rule;

    /// <summary>
    /// Emits an event once a list of <see cref="Behaviour"/>s all are <see cref="Behaviour.isActiveAndEnabled"/>.
    /// </summary>
    public class BehaviourEnabledObserver2 : MonoBehaviour, IProcessable
    {
        /// <summary>
        /// The <see cref="Behaviour"/>s to observe.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public BehaviourObservableList Behaviours { get; set; }

        /// <summary>
        /// The rule to match against the Behaviours.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public RuleContainer Rule { get; set; }

        /// <summary>
        /// Emitted when all <see cref="Behaviours"/> are <see cref="Behaviour.isActiveAndEnabled"/>.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent ActiveAndEnabled = new UnityEvent();

        /// <summary>
        /// Checks whether all <see cref="Behaviours"/> are <see cref="Behaviour.isActiveAndEnabled"/> and emits <see cref="ActiveAndEnabled"/> if they are.
        /// </summary>
        /// <returns>Whether all <see cref="Behaviours"/> are active and enabled.</returns>
        [RequiresBehaviourState]
        public virtual void Process()
        {
            if (Behaviours == null || Behaviours.NonSubscribableElements.Count == 0)
            {
                return;
            }

            foreach (Behaviour behaviour in Behaviours.NonSubscribableElements)
            {
                if (!Rule.Accepts(behaviour))
                {
                    return;
                }
            }

            ActiveAndEnabled?.Invoke();
        }
    }
}
