namespace Test.Zinnia.Utility.Mock
{
    /// <summary>
    /// The UnityEventListenerMock creates a simple mechanism of registering a listener with a UnityEvent and checking if the event was emitted.
    /// </summary>
    public class UnityEventListenerMock
    {
        /// <summary>
        /// The state of whether the event emitted was received by the listener.
        /// </summary>
        public bool Received
        {
            get;
            private set;
        }

        /// <summary>
        /// The Reset method resets the listener state.
        /// </summary>
        public virtual void Reset()
        {
            Received = false;
        }

        public virtual void Listen()
        {
            Received = true;
        }

        public virtual void Listen<T0>(T0 a)
        {
            Received = true;
        }

        public virtual void Listen<T0, T1>(T0 a, T1 b)
        {
            Received = true;
        }

        public virtual void Listen<T0, T1, T2>(T0 a, T1 b, T2 c)
        {
            Received = true;
        }

        public virtual void Listen<T0, T1, T2, T3>(T0 a, T1 b, T2 c, T3 d)
        {
            Received = true;
        }
    }
}