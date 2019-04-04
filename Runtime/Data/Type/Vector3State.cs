namespace Zinnia.Data.Type
{
    using UnityEngine;
    using System;
    using Malimbe.XmlDocumentationAttribute;

    /// <summary>
    /// Allows a boolean to be set per <see cref="UnityEngine.Vector3"/> element to provide a state reference.
    /// </summary>
    [Serializable]
    public struct Vector3State
    {
        /// <summary>
        /// The X State of the <see cref="UnityEngine.Vector3"/>.
        /// </summary>
        [DocumentedByXml]
        public bool xState;
        /// <summary>
        /// The Y State of the <see cref="UnityEngine.Vector3"/>.
        /// </summary>
        [DocumentedByXml]
        public bool yState;
        /// <summary>
        /// The Z State of the <see cref="UnityEngine.Vector3"/>.
        /// </summary>
        [DocumentedByXml]
        public bool zState;

        /// <summary>
        /// Shorthand for writing <c>Vector3State(false, false, false)</c>.
        /// </summary>
        public static readonly Vector3State False = new Vector3State(false, false, false);

        /// <summary>
        /// Shorthand for writing <c>Vector3State(true, true, true)</c>.
        /// </summary>
        public static readonly Vector3State True = new Vector3State(true, true, true);

        /// <summary>
        /// Shorthand for writing <c>Vector3State(true, false, false)</c>.
        /// </summary>
        public static readonly Vector3State XOnly = new Vector3State(true, false, false);

        /// <summary>
        /// Shorthand for writing <c>Vector3State(false, true, false)</c>.
        /// </summary>
        public static readonly Vector3State YOnly = new Vector3State(false, true, false);

        /// <summary>
        /// Shorthand for writing <c>Vector3State(false, false, true)</c>.
        /// </summary>
        public static readonly Vector3State ZOnly = new Vector3State(false, false, true);

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

        /// <summary>
        /// Returns the current state as a <see cref="Vector3"/> representation.
        /// </summary>
        /// <returns>The representation of the current state.</returns>
        public Vector3 ToVector3()
        {
            return new Vector3(xState ? 1f : 0f, yState ? 1f : 0f, zState ? 1f : 0f);
        }
    }
}