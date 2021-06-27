namespace Zinnia.Process.Moment
{
    using UnityEngine;
    using System.Collections;
    using Malimbe.MemberChangeMethod;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;
    using Zinnia.Process.Moment.Collection;

    /// <summary>
    /// Iterates through a given <see cref="MomentProcess"/> collection and executes the <see cref="IProcessable.Process"/> method using Unity coroutine.
    /// </summary>
    public class CoroutineMomentProcessor : MonoBehaviour
    {
        /// <summary>
        /// The amount of time to pause after each process iteration.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public float Interval { get; set; } = 0.000011f;
        /// <summary>
        /// The maximum amount of time to perform the process before ending.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public float MaximumRunTime { get; set; } = float.PositiveInfinity;

        /// <summary>
        /// A collection of <see cref="MomentProcess"/> to process.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public MomentProcessObservableList Processes { get; set; }

        /// <summary>
        /// A reference to the started routine.
        /// </summary>
        protected Coroutine processRoutine;
        /// <summary>
        /// Delays the <see cref="processRoutine"/> by <see cref="Interval"/> seconds.
        /// </summary>
        protected WaitForSeconds delayYieldInstruction;
        /// <summary>
        /// The amount of time until the <see cref="processRoutine"/> is cancelled.
        /// </summary>
        protected float timeUntilProcessIsCancelled;

        /// <summary>
        /// Initiates the process if no existing process is already running.
        /// </summary>
        public virtual void BeginProcess()
        {
            if (processRoutine == null)
            {
                processRoutine = StartCoroutine(ProcessRoutine());
            }
        }

        /// <summary>
        /// Cancels any running process.
        /// </summary>
        public virtual void EndProcess()
        {
            if (processRoutine == null)
            {
                return;
            }

            StopCoroutine(processRoutine);
            processRoutine = null;
        }

        protected virtual void OnEnable()
        {
            OnAfterCheckDelayChange();
            OnAfterMaximumRunTimeChange();
            BeginProcess();
        }

        protected virtual void OnDisable()
        {
            EndProcess();
        }

        /// <summary>
        /// Calls <see cref="Process"/> on every frame.
        /// </summary>
        /// <returns>An Enumerator to manage the running of the Coroutine.</returns>
        protected virtual IEnumerator ProcessRoutine()
        {
            timeUntilProcessIsCancelled = Time.time + MaximumRunTime;
            while (Time.time < timeUntilProcessIsCancelled)
            {
                Process();
                yield return delayYieldInstruction;
            }
            processRoutine = null;
        }

        /// <summary>
        /// Iterates through the given <see cref="MomentProcess"/> and calls <see cref="MomentProcess.Process"/> on each one.
        /// </summary>
        protected virtual void Process()
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


        /// <summary>
        /// Called after <see cref="Interval"/> has been changed.
        /// </summary>
        [CalledAfterChangeOf(nameof(Interval))]
        protected virtual void OnAfterCheckDelayChange()
        {
            delayYieldInstruction = new WaitForSeconds(Interval);
        }

        /// <summary>
        /// Called after <see cref="MaximumRunTime"/> has been changed.
        /// </summary>
        [CalledAfterChangeOf(nameof(MaximumRunTime))]
        protected virtual void OnAfterMaximumRunTimeChange()
        {
            float remainingRunTime = timeUntilProcessIsCancelled - Time.time;
            timeUntilProcessIsCancelled = MaximumRunTime - remainingRunTime;
        }
    }
}