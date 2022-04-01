namespace Zinnia.Process.Moment
{
    using UnityEngine;
    using Zinnia.Process.Moment.Collection;

    /// <summary>
    /// Processes a list of <see cref="MomentProcess"/> as if it's a single <see cref="IProcessable"/> process.
    /// </summary>
    public class CompositeProcess : MonoBehaviour, IProcessable
    {
        [Tooltip("A collection of MomentProcess to process.")]
        [SerializeField]
        private MomentProcessObservableList processes;
        /// <summary>
        /// A collection of <see cref="MomentProcess"/> to process.
        /// </summary>
        public MomentProcessObservableList Processes
        {
            get
            {
                return processes;
            }
            set
            {
                processes = value;
            }
        }

        /// <summary>
        /// Iterates through <see cref="Processes"/> and calls <see cref="MomentProcess.Process"/> on each one.
        /// </summary>
        public void Process()
        {
            if (Processes == null)
            {
                return;
            }

            foreach (MomentProcess currentProcess in Processes.NonSubscribableElements)
            {
                currentProcess.Process();
            }
        }
    }
}
