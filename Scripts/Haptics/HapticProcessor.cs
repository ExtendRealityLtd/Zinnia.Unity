namespace Zinnia.Haptics
{
    using UnityEngine;
    using Zinnia.Extension;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A proxy for managing the first active <see cref="HapticProcess"/> that is provided in the collection.
    /// </summary>
    public class HapticProcessor : HapticProcess
    {
        /// <summary>
        /// Process the first active <see cref="HapticProcess"/> found in the collection.
        /// </summary>
        [Tooltip("Process the first active HapticProcess found in the collection.")]
        public List<HapticProcess> hapticProcesses = new List<HapticProcess>();

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
            ActiveHapticProcess = hapticProcesses.EmptyIfNull().FirstOrDefault(process => process.IsActive());
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