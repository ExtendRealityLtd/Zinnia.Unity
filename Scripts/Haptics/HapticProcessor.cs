namespace VRTK.Core.Haptics
{
    using UnityEngine;
    using System.Collections.Generic;
    using VRTK.Core.Extension;
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

        /// <summary>
        /// The current active <see cref="HapticProcess"/> being utilized.
        /// </summary>
        public HapticProcess ActiveProcess
        {
            get;
            protected set;
        }

        /// <inheritdoc />
        public override bool IsActive()
        {
            return (base.IsActive() && ActiveProcess != null && ActiveProcess.IsActive());
        }

        /// <summary>
        /// Starts the first active <see cref="HapticProcess"/> found.
        /// </summary>
        protected override void DoBegin()
        {
            ActiveProcess = hapticProcesses.EmptyIfNull().FirstOrDefault(process => process.IsActive());
            if (ActiveProcess != null)
            {
                ActiveProcess.Begin();
            }
        }

        /// <summary>
        /// Cancels the current <see cref="ActiveProcess"/> from running.
        /// </summary>
        protected override void DoCancel()
        {
            if (ActiveProcess != null)
            {
                ActiveProcess.Cancel();
            }
        }
    }
}