namespace Zinnia.Event
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// Emits an event once a list of <see cref="Behaviour"/>s all are <see cref="Behaviour.isActiveAndEnabled"/>.
    /// </summary>
    public class BehaviourEnabledObserver : MonoBehaviour
    {
        /// <summary>
        /// The minimum time allowed to be passed to <see cref="MonoBehaviour.InvokeRepeating"/>.
        /// </summary>
        public const float MinimumRepeatRate = 0.000011f;

        /// <summary>
        /// The <see cref="Behaviour"/>s to observe.
        /// </summary>
        public List<Behaviour> behaviours = new List<Behaviour>();
        /// <summary>
        /// Emitted when all <see cref="behaviours"/> are <see cref="Behaviour.isActiveAndEnabled"/>.
        /// </summary>
        public UnityEvent ActiveAndEnabled = new UnityEvent();

        protected virtual void OnEnable()
        {
            InvokeRepeating(nameof(Check), MinimumRepeatRate, MinimumRepeatRate);
        }

        protected virtual void OnDisable()
        {
            CancelInvoke(nameof(Check));
        }

        /// <summary>
        /// Checks whether all <see cref="behaviours"/> are <see cref="Behaviour.isActiveAndEnabled"/> and emits <see cref="ActiveAndEnabled"/> if they are.
        /// </summary>
        protected virtual void Check()
        {
            if (!behaviours.TrueForAll(behaviour => behaviour.isActiveAndEnabled))
            {
                return;
            }

            CancelInvoke(nameof(Check));
            ActiveAndEnabled?.Invoke();
        }
    }
}
