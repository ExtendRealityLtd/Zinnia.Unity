namespace Zinnia.Haptics
{
    using UnityEngine;

    /// <summary>
    /// The basis of a haptic process that can be started or cancelled.
    /// </summary>
    public abstract class HapticProcess : MonoBehaviour
    {
        /// <summary>
        /// The state of whether the <see cref="Component"/> is active.
        /// </summary>
        /// <returns><see langword="true"/> if the <see cref="Component"/> is considered active.</returns>
        public virtual bool IsActive()
        {
            return isActiveAndEnabled;
        }

        /// <summary>
        /// Starts the haptic process.
        /// </summary>
        public virtual void Begin()
        {
            if (!IsActive())
            {
                return;
            }

            DoBegin();
        }

        /// <summary>
        /// Cancels the existing haptic process.
        /// </summary>
        public virtual void Cancel()
        {
            DoCancel();
        }

        /// <summary>
        /// Cancels any existing haptic process and then starts a new one.
        /// </summary>
        public virtual void Restart()
        {
            Cancel();
            Begin();
        }

        /// <summary>
        /// Starts the haptic process.
        /// </summary>
        protected abstract void DoBegin();
        /// <summary>
        /// Cancels the existing haptic process.
        /// </summary>
        protected abstract void DoCancel();

        protected virtual void OnDisable()
        {
            Cancel();
        }
    }
}