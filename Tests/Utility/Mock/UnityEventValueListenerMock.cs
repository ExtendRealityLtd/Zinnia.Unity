namespace Test.Zinnia.Utility.Mock
{
    /// <summary>
    /// Allows listening to a UnityEvent that emits a value and checking if the event was emitted as well as the value that was emitted.
    /// </summary>
    /// <typeparam name="T">The type of the emitted value.</typeparam>
    public class UnityEventValueListenerMock<T>
    {
        /// <summary>
        /// Whether the event emitted was received by the listener.
        /// </summary>
        public bool Received { get; private set; }
        /// <summary>
        /// The received event value.
        /// </summary>
        public T Value { get; private set; }

        /// <summary>
        /// The Reset method resets the listener state.
        /// </summary>
        public virtual void Reset()
        {
            Received = false;
            Value = default;
        }

        /// <summary>
        /// The callback for the event.
        /// </summary>
        /// <param name="value">The event's value.</param>
        public virtual void Listen(T value)
        {
            Received = true;
            Value = value;
        }
    }
}