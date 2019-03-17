namespace Zinnia.Haptics
{
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;
    using Zinnia.Haptics.Collection;

    /// <summary>
    /// A proxy for managing the first active <see cref="HapticProcess"/> that is provided in the collection.
    /// </summary>
    public class HapticProcessor : HapticProcess
    {
        /// <summary>
        /// Process the first active <see cref="HapticProcess"/> found in the collection.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public HapticProcessObservableList HapticProcesses { get; set; }

        private HapticProcess _activeHapticProcess;
        /// <summary>
        /// The current active <see cref="HapticProcess"/> being utilized.
        /// </summary>
        public HapticProcess ActiveHapticProcess
        {
            get => _activeHapticProcess != null && _activeHapticProcess.IsActive() ? _activeHapticProcess : null;
            protected set
            {
                _activeHapticProcess = value;
            }
        }

        /// <summary>
        /// Starts the first active <see cref="HapticProcess"/> found.
        /// </summary>
        protected override void DoBegin()
        {
            HapticProcess firstActiveProcess = null;
            foreach (HapticProcess process in HapticProcesses.NonSubscribableElements)
            {
                if (process.IsActive())
                {
                    firstActiveProcess = process;
                    break;
                }
            }

            ActiveHapticProcess = firstActiveProcess;
            if (ActiveHapticProcess != null)
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