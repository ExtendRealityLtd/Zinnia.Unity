namespace Zinnia.Haptics
{
    using UnityEngine;
    using Zinnia.Haptics.Collection;

    /// <summary>
    /// Processes each active <see cref="HapticProcess"/> in the given <see cref="HapticProcessObservableList"/> and can optionally cease after the first valid process.
    /// </summary>
    public class HapticProcessor : HapticProcess
    {
        [Tooltip("The HapticProcess collection to attempt to process.")]
        [SerializeField]
        private HapticProcessObservableList hapticProcesses;
        /// <summary>
        /// The <see cref="HapticProcess"/> collection to attempt to process.
        /// </summary>
        public HapticProcessObservableList HapticProcesses
        {
            get
            {
                return hapticProcesses;
            }
            set
            {
                hapticProcesses = value;
            }
        }
        [Tooltip("Whether to cease the processing of the collection after the first valid HapticProcess is processed.")]
        [SerializeField]
        private bool ceaseAfterFirstSourceProcessed = true;
        /// <summary>
        /// Whether to cease the processing of the collection after the first valid <see cref="HapticProcess"/> is processed.
        /// </summary>
        public bool CeaseAfterFirstSourceProcessed
        {
            get
            {
                return ceaseAfterFirstSourceProcessed;
            }
            set
            {
                ceaseAfterFirstSourceProcessed = value;
            }
        }

        /// <summary>
        /// The backing field for holding the value of <see cref="ActiveHapticProcess"/>.
        /// </summary>
        private HapticProcess activeHapticProcess;
        /// <summary>
        /// The current active <see cref="HapticProcess"/> being utilized.
        /// </summary>
        public virtual HapticProcess ActiveHapticProcess
        {
            get => activeHapticProcess != null && activeHapticProcess.IsActive() ? activeHapticProcess : null;
            protected set
            {
                activeHapticProcess = value;
            }
        }

        /// <summary>
        /// Starts the first active <see cref="HapticProcess"/> found.
        /// </summary>
        protected override void DoBegin()
        {
            ActiveHapticProcess = null;
            if (HapticProcesses == null)
            {
                return;
            }

            foreach (HapticProcess process in HapticProcesses.NonSubscribableElements)
            {
                if (process.IsActive())
                {
                    ActiveHapticProcess = process;
                    if (CeaseAfterFirstSourceProcessed)
                    {
                        break;
                    }
                    else
                    {
                        ActiveHapticProcess.Begin();
                    }
                }
            }

            if (CeaseAfterFirstSourceProcessed && ActiveHapticProcess != null)
            {
                ActiveHapticProcess.Begin();
            }
        }

        /// <summary>
        /// Cancels the current <see cref="ActiveHapticProcess"/> from running.
        /// </summary>
        protected override void DoCancel()
        {
            if (ActiveHapticProcess != null)
            {
                ActiveHapticProcess.Cancel();
            }
        }
    }
}