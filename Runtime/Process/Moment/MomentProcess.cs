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
        /// The interval in seconds defining how often to process the <see cref="Process"/>. Negative values will be clamped to zero.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public float Interval { get; set; }
        /// <summary>
        /// When to call <see cref="Process"/> the next time. Updated automatically based on <see cref="Interval"/> after <see cref="Process"/> has been called.
        /// </summary>
        public float NextProcessTime { get; set; }

        /// <summary>
        /// Calls <see cref="IProcessable.Process"/> on <see cref="Source"/> if <see cref="NextProcessTime"/> allows.
        /// </summary>
        public virtual void Process()
        {
            if (NextProcessTime <= Time.time)
            {
                ProcessNow();
            }
        }

        /// <summary>
        /// Calls <see cref="IProcessable.Process"/> on <see cref="Source"/>, ignoring whether <see cref="NextProcessTime"/> allows.
        /// </summary>
        public virtual void ProcessNow()
        {
            if (Source == null || (OnlyProcessOnActiveAndEnabled && !isActiveAndEnabled))
            {
                return;
            }

            Source.Interface.Process();
            UpdateNextProcessTime();
        }

        /// <summary>
        /// Sets <see cref="NextProcessTime"/> to a random time between now and now plus <see cref="Interval"/>.
        /// </summary>
        public virtual void RandomizeNextProcessTime()
        {
            NextProcessTime = Time.time + (Random.value * Interval);
        }

        protected virtual void Awake()
        {
            RandomizeNextProcessTime();
        }

        protected virtual void OnEnable()
        {
            OnAfterIntervalChange();
        }

        /// <summary>
        /// Updates <see cref="NextProcessTime"/> to adjust to the latest <see cref="Interval"/>.
        /// </summary>
        protected virtual void UpdateNextProcessTime()
        {
            NextProcessTime = Time.time + Interval;
        }

        /// <summary>
        /// Called after <see cref="Interval"/> has been changed.
        /// </summary>
        [CalledAfterChangeOf(nameof(Interval))]
        protected virtual void OnAfterIntervalChange()
        {
            Interval = Mathf.Max(0f, Interval);
        }
    }
}