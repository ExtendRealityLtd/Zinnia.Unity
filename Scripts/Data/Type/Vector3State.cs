namespace VRTK.Core.Data.Type
{
    using System;

    /// <summary>
    /// Allows a boolean to be set per <see cref="UnityEngine.Vector3"/> element to provide a state reference.
    /// </summary>
    [Serializable]
    public class Vector3State
    {
        /// <summary>
        /// The X State of the <see cref="UnityEngine.Vector3"/>.
        /// </summary>
        public bool xState;
        /// <summary>
        /// The Y State of the <see cref="UnityEngine.Vector3"/>.
        /// </summary>
        public bool yState;
        /// <summary>
        /// The Z State of the <see cref="UnityEngine.Vector3"/>.
        /// </summary>
        public bool zState;

        /// <summary>
        /// Default Vector3State of all false.
        /// </summary>
        public static readonly Vector3State False = new Vector3State(false, false, false);

        /// <summary>
        /// Default Vector3State of all true.
        /// </summary>
        public static readonly Vector3State True = new Vector3State(true, true, true);

        /// <summary>
        /// The Constructor that allows setting the individual states at instantiation.
        /// </summary>
        /// <param name="x">The X State.</param>
        /// <param name="y">The Y State.</param>
        /// <param name="z">The Z State.</param>
        public Vector3State(bool x, bool y, bool z)
        {
            xState = x;
            yState = y;
            zState = z;
        }
    }
}