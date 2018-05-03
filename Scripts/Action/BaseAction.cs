namespace VRTK.Core.Action
{
    using UnityEngine;

    /// <summary>
    /// The BaseAction forms as the basis for all action types.
    /// </summary>
    /// <typeparam name="T">The variable type the action will be utilising.</typeparam>
    public abstract class BaseAction<T> : MonoBehaviour
    {
        /// <summary>
        /// The state value of the action.
        /// </summary>
        public T Value
        {
            get;
            protected set;
        } = default(T);

        /// <summary>
        /// The previous state value of the action.
        /// </summary>
        protected T previousValue = default(T);
        /// <summary>
        /// The current state of the action.
        /// </summary>
        protected bool state;

        /// <summary>
        /// The Receive method allows an action to receive the payload from another action to enable action chaining.
        /// </summary>
        /// <param name="value">The value from the action.</param>
        /// <param name="sender">The sender of the action.</param>
        public abstract void Receive(T value, object sender = null);

        /// <summary>
        /// The OnActivated Method is used to call the appropriate Activated event.
        /// </summary>
        /// <param name="value">The value to pass to the event.</param>
        protected abstract void OnActivated(T value);

        /// <summary>
        /// The OnChanged Method is used to call the appropriate Changed event.
        /// </summary>
        /// <param name="value">The value to pass to the event.</param>
        protected abstract void OnChanged(T value);

        /// <summary>
        /// The OnDeactivated Method is used to call the appropriate Deactivated event.
        /// </summary>
        /// <param name="value">The value to pass to the event.</param>
        protected abstract void OnDeactivated(T value);

        /// <summary>
        /// The HasChanged method determines if the action state has changed from the previous state.
        /// </summary>
        /// <returns>Returns `true` if the action state has changed.</returns>
        protected virtual bool HasChanged()
        {
            return (!Value.Equals(previousValue));
        }

        /// <summary>
        /// The CanEmit method determines whether the event should be emitted.
        /// </summary>
        /// <returns>Returns `true` if the event should be emitted.</returns>
        protected virtual bool CanEmit()
        {
            return (isActiveAndEnabled);
        }
    }
}