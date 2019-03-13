namespace Zinnia.Process.Moment
{
    using UnityEngine;
    using Malimbe.MemberChangeMethod;
    using Malimbe.MemberClearanceMethod;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;

    /// <summary>
    /// Wrapper for an <see cref="IProcessable"/> process that has a state to determine when it is to be processed.
    /// </summary>
    public class MomentProcess : MonoBehaviour, IProcessable
    {
        /// <summary>
        /// The source process to attach to the moment.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public ProcessContainer Source { get; set; }
        /// <summary>
        /// The process only executes if the <see cref="GameObject"/> is active and the <see cref="Component"/> is enabled.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public bool OnlyProcessOnActiveAndEnabled { get; set; } = true;
        /// <summary>
        /// A percentage defining how often to process the <see cref="Process"/>.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml, Range(0f, 1f)]
        public float Utilization { get; set; } = 1f;

        /// <summary>
        /// Keeps track of how often calls to <see cref="Process"/> were ignored because of <see cref="Utilization"/>.
        /// </summary>
        protected int counter;
        /// <summary>
        /// How many calls to <see cref="Process"/> to ignore because of <see cref="Utilization"/>.
        /// </summary>
        protected int delay;

        /// <summary>
        /// Calls <see cref="IProcessable.Process"/> on <see cref="Source"/> if <see cref="Utilization"/> allows.
        /// </summary>
        public virtual void Process()
        {
            if (Utilization < float.Epsilon)
            {
                return;
            }

            if (Source != null && (!OnlyProcessOnActiveAndEnabled || isActiveAndEnabled) && counter == delay)
            {
                Source.Interface.Process();
            }

            counter = (counter + 1) % (delay + 1);
        }

        /// <summary>
        /// This empty implementation tells Unity to draw the enabled checkbox for this component, allowing to disable the component at edit-time.
        /// </summary>
        protected virtual void OnEnable() { }

        /// <summary>
        /// Updates <see cref="delay"/> to adjust to the latest <see cref="Utilization"/>.
        /// </summary>
        protected virtual void UpdateDelay()
        {
            delay = (int)(1f / Utilization) - 1;
        }

        /// <summary>
        /// Called after <see cref="Utilization"/> has been changed.
        /// </summary>
        [CalledAfterChangeOf(nameof(Utilization))]
        protected virtual void OnAfterUtilizationChange()
        {
            if (Utilization < 0f || Utilization > 1f)
            {
                Utilization = Mathf.Clamp01(Utilization);
            }
            UpdateDelay();
        }
    }
}