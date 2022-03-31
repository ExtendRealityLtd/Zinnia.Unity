namespace Zinnia.Event
{
    using Malimbe.MemberChangeMethod;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.Events;
    using Zinnia.Data.Collection.List;

    /// <summary>
    /// Emits an event once a list of <see cref="Behaviour"/>s all are <see cref="Behaviour.isActiveAndEnabled"/>.
    /// </summary>
    public class BehaviourEnabledObserver : MonoBehaviour
    {
        /// <summary>
        /// The time between each <see cref="Behaviour.enabled"/> check.
        /// </summary>
        [Tooltip("The time between each Behaviour.enabled check.")]
        [SerializeField]
        private float _checkDelay = 0.000011f;
        public float CheckDelay
        {
            get
            {
                return _checkDelay;
            }
            set
            {
                _checkDelay = value;
            }
        }
        /// <summary>
        /// The maximum amount of time to perform the <see cref="Behaviour.enabled"/> check before ending.
        /// </summary>
        [Tooltip("The maximum amount of time to perform the Behaviour.enabled check before ending.")]
        [SerializeField]
        private float _maximumRunTime = float.PositiveInfinity;
        public float MaximumRunTime
        {
            get
            {
                return _maximumRunTime;
            }
            set
            {
                _maximumRunTime = value;
            }
        }

        /// <summary>
        /// The <see cref="Behaviour"/>s to observe.
        /// </summary>
        [Tooltip("The Behaviours to observe.")]
        [SerializeField]
        private BehaviourObservableList _behaviours;
        public BehaviourObservableList Behaviours
        {
            get
            {
                return _behaviours;
            }
            set
            {
                _behaviours = value;
            }
        }

        /// <summary>
        /// Emitted when all <see cref="Behaviours"/> are <see cref="Behaviour.isActiveAndEnabled"/>.
        /// </summary>
        public UnityEvent ActiveAndEnabled = new UnityEvent();

        /// <summary>
        /// A reference to the started routine.
        /// </summary>
        protected Coroutine behaviourCheckRoutine;
        /// <summary>
        /// Delays the <see cref="behaviourCheckRoutine"/> by <see cref="CheckDelay"/> seconds.
        /// </summary>
        protected WaitForSeconds checkDelayYieldInstruction;
        /// <summary>
        /// The amount of time until the <see cref="behaviourCheckRoutine"/> is cancelled.
        /// </summary>
        protected float timeUntilCheckIsCancelled;

        /// <summary>
        /// Initiates the check of the <see cref="Behaviours"/> state if no existing check is already running.
        /// </summary>
        public virtual void BeginCheck()
        {
            if (behaviourCheckRoutine == null)
            {
                behaviourCheckRoutine = StartCoroutine(Check());
            }
        }

        /// <summary>
        /// Cancels any running check of the <see cref="Behaviours"/> state.
        /// </summary>
        public virtual void EndCheck()
        {
            if (behaviourCheckRoutine == null)
            {
                return;
            }

            StopCoroutine(behaviourCheckRoutine);
            behaviourCheckRoutine = null;
        }

        protected virtual void OnEnable()
        {
            OnAfterCheckDelayChange();
            OnAfterMaximumRunTimeChange();
            BeginCheck();
        }

        protected virtual void OnDisable()
        {
            EndCheck();
        }

        /// <summary>
        /// Checks to see if the <see cref="Behaviours"/> specified have been enabled in the scene.
        /// </summary>
        /// <returns>An Enumerator to manage the running of the Coroutine.</returns>
        protected virtual IEnumerator Check()
        {
            timeUntilCheckIsCancelled = Time.time + MaximumRunTime;
            while (Time.time < timeUntilCheckIsCancelled)
            {
                if (AreBehavioursEnabled())
                {
                    break;
                }
                yield return checkDelayYieldInstruction;
            }
            behaviourCheckRoutine = null;
        }

        /// <summary>
        /// Checks whether all <see cref="Behaviours"/> are <see cref="Behaviour.isActiveAndEnabled"/> and emits <see cref="ActiveAndEnabled"/> if they are.
        /// </summary>
        /// <returns>Whether all <see cref="Behaviours"/> are active and enabled.</returns>
        protected virtual bool AreBehavioursEnabled()
        {
            if (Behaviours == null || Behaviours.NonSubscribableElements.Count == 0)
            {
                return false;
            }

            foreach (Behaviour behaviour in Behaviours.NonSubscribableElements)
            {
                if (!behaviour.isActiveAndEnabled)
                {
                    return false;
                }
            }

            ActiveAndEnabled?.Invoke();
            return true;
        }

        /// <summary>
        /// Called after <see cref="CheckDelay"/> has been changed.
        /// </summary>
        [CalledAfterChangeOf(nameof(CheckDelay))]
        protected virtual void OnAfterCheckDelayChange()
        {
            checkDelayYieldInstruction = new WaitForSeconds(CheckDelay);
        }

        /// <summary>
        /// Called after <see cref="MaximumRunTime"/> has been changed.
        /// </summary>
        [CalledAfterChangeOf(nameof(MaximumRunTime))]
        protected virtual void OnAfterMaximumRunTimeChange()
        {
            float remainingRunTime = timeUntilCheckIsCancelled - Time.time;
            timeUntilCheckIsCancelled = MaximumRunTime - remainingRunTime;
        }
    }
}
