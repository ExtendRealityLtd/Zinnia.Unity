namespace Zinnia.Data.Type
{
    using System;
    using UnityEngine;
    using UnityObject = UnityEngine.Object;

    /// <summary>
    /// Provides a linkable reference to a <see cref="UnityObject"/>.
    /// </summary>
    [Serializable]

    public struct ObjectReference
    {
        /// <summary>
        /// The <see cref="UnityObject"/> to reference.
        /// </summary>
        [Tooltip("The UnityObject  to reference.")]
        public UnityObject linkedObject;
        /// <summary>
        /// The text to use for describing the linked reference.
        /// </summary>
        [Tooltip("The text to use for describing the linked reference.")]
        public string linkText;
        /// <summary>
        /// Whether the reference should be active.
        /// </summary>
        [Tooltip("Whether the reference should be active.")]
        public bool isActive;
    }
}