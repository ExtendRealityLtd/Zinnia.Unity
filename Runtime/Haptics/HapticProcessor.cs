namespace Zinnia.Haptics
{
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;
    using Zinnia.Haptics.Collection;

    /// <summary>
    /// Processes each active <see cref="HapticProcess"/> in the given <see cref="HapticProcessObservableList"/> and can optionally cease after the first valid process.
    /// </summary>
    public class HapticProcessor : HapticProcess
    {
        /// <summary>
        /// The <see cref="HapticProcess"/> collection to attempt to process.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public HapticProcessObservableList HapticProcesses { get; set; }
        /// <summary>
        /// Whether to cease the processing of the collection after the first valid <see cref="HapticProcess"/> is processed.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public bool CeaseAfterFirstSourceProcessed { get; set; } = true;

        /// <summary>
        /// The current active <see cref="HapticProcess"/> being utilized.
        /// </summary>
        public HapticProcess ActiveHapticProcess
        {
            get => activeHapticProcess != null && activeHapticProcess.IsActive() ? activeHapticProcess : null;
            protected set
            {
                activeHapticProcess = value;
            }
        }
        /// <summary>
        /// The backing field for holding the value of <see cref="ActiveHapticProcess"/>.
        /// </summary>
        private HapticProcess activeHapticProcess;

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