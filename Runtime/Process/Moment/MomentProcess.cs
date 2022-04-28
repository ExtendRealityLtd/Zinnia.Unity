namespace Zinnia.Process.Moment
{
    using UnityEngine;
    using Zinnia.Extension;

    /// <summary>
    /// Wrapper for an <see cref="IProcessable"/> process that has a state to determine when it is to be processed.
    /// </summary>
    public class MomentProcess : MonoBehaviour, IProcessable
    {
        [Tooltip("The source process to attach to the moment.")]
        [SerializeField]
        private ProcessContainer source;
        /// <summary>
        /// The source process to attach to the moment.
        /// </summary>
        public ProcessContainer Source
        {
            get
            {
                return source;
            }
            set
            {
                source = value;
            }
        }
        [Tooltip("The process only executes if the GameObject is active and the Component is enabled.")]
        [SerializeField]
        private bool onlyProcessOnActiveAndEnabled = true;
        /// <summary>
        /// The process only executes if the <see cref="GameObject"/> is active and the <see cref="Component"/> is enabled.
        /// </summary>
        public bool OnlyProcessOnActiveAndEnabled
        {
            get
            {
                return onlyProcessOnActiveAndEnabled;
            }
            set
            {
                onlyProcessOnActiveAndEnabled = value;
            }
        }
        [Tooltip("The interval in seconds defining how often to process the Process. Negative values will be clamped to zero.")]
        [SerializeField]
        private float interval;
        /// <summary>
        /// The interval in seconds defining how often to process the <see cref="Process"/>. Negative values will be clamped to zero.
        /// </summary>
        public float Interval
        {
            get
            {
                return interval;
            }
            set
            {
                interval = value;
                if (this.IsMemberChangeAllowed())
                {
                    OnAfterIntervalChange();
                }
            }
        }
        /// <summary>
        /// When to call <see cref="Process"/> the next time. Updated automatically based on <see cref="Interval"/> after <see cref="Process"/> has been called.
        /// </summary>
        public float NextProcessTime { get; set; }

        /// <summary>
        /// Clears <see cref="Source"/>.
        /// </summary>
        public virtual void ClearSource()
        {
            if (!this.IsValidState())
            {
                return;
            }

            Source = default;
        }

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
        protected virtual void OnAfterIntervalChange()
        {
            interval = Mathf.Max(0f, Interval);
        }
    }
}