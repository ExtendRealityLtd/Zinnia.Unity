namespace VRTK.Core.Event
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;

    /// <summary>
    /// Provides a base for allowing a <see cref="GameObject"/> to be extracted and emitted in an event.
    /// </summary>
    public abstract class BaseGameObjectEmitter : MonoBehaviour
    {
        /// <summary>
        /// Defines the event with the specified <see cref="GameObject"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<GameObject>
        {
        }

        /// <summary>
        /// Emitted when the <see cref="GameObject"/> is extracted.
        /// </summary>
        public UnityEvent Extracted = new UnityEvent();

        /// <summary>
        /// The extracted <see cref="GameObject"/>.
        /// </summary>
        public GameObject Result
        {
            get;
            protected set;
        }

        /// <summary>
        /// Extracts the <see cref="GameObject"/>/
        /// </summary>
        /// <returns>The extracted <see cref="GameObject"/>.</returns>
        public virtual GameObject Extract()
        {
            if (!isActiveAndEnabled || Result == null)
            {
                Result = null;
                return null;
            }

            Extracted?.Invoke(Result);
            return Result;
        }

        /// <summary>
        /// Extracts the <see cref="GameObject"/>.
        /// </summary>
        public virtual void DoExtract()
        {
            Extract();
        }
    }
}