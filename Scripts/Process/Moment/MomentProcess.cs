namespace VRTK.Core.Process.Moment
{
    using UnityEngine;

    /// <summary>
    /// Wrapper for an <see cref="IProcessable"/> process that has a state to determine when it is to be processed.
    /// </summary>
    public class MomentProcess : MonoBehaviour, IProcessable
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
        /// <summary>
        /// A percentage defining how often to process the <see cref="process"/>.
        /// </summary>
        [Tooltip("A percentage defining how often to process the process."), Range(0f, 1f), SerializeField]
        private float utilization = 1f;

        /// <summary>
        /// A percentage defining how often to process the <see cref="process"/>.
        /// </summary>
        public float Utilization
        {
            get
            {
                return utilization;
            }
            set
            {
                utilization = Mathf.Clamp01(value);
                UpdateDelay();
            }
        }

        /// <summary>
        /// Keeps track of how often calls to <see cref="Process"/> were ignored because of <see cref="Utilization"/>.
        /// </summary>
        protected int counter;
        /// <summary>
        /// How many calls to <see cref="Process"/> to ignore because of <see cref="Utilization"/>.
        /// </summary>
        protected int delay;

        /// <summary>
        /// Calls <see cref="IProcessable.Process"/> on <see cref="process"/> if <see cref="Utilization"/> allows.
        /// </summary>
        public virtual void Process()
        {
            if (Utilization < float.Epsilon)
            {
                return;
            }

            if (process != null && (!onlyProcessOnActiveAndEnabled || isActiveAndEnabled) && counter == delay)
            {
                process.Interface.Process();
            }

            counter = (counter + 1) % (delay + 1);
        }

        protected virtual void OnEnable()
        {
            // This empty implementation tells Unity to draw the enabled checkbox for this component, allowing to disable the component at edit-time.
        }

        protected virtual void OnValidate()
        {
            UpdateDelay();
        }

        /// <summary>
        /// Updates <see cref="delay"/> to adjust to the latest <see cref="Utilization"/>.
        /// </summary>
        protected virtual void UpdateDelay()
        {
            delay = (int)(1f / Utilization) - 1;
        }
    }
}