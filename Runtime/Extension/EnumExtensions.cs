namespace Zinnia.Extension
{
    using System;
    using UnityEngine;

    /// <summary>
    /// Static methods for the <see cref="Enum"/> Type.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Gets the <see cref="Enum"/> value based on the given index.
        /// </summary>
        /// <typeparam name="T">The <see cref="Enum"/> type.</typeparam>
        /// <param name="index">The index to retrieve from. In case this index is out of bounds for the <see cref="Enum"/> it will be clamped within the valid bounds.</param>
        /// <returns>The value for the index.</returns>
        public static T GetByIndex<T>(int index)
        {
            return (T)(object)Mathf.Clamp(index, 0, Enum.GetValues(typeof(T)).Length - 1);
        }

        /// <summary>
        /// Gets the <see cref="Enum"/> value based of the <see cref="string"/> representation of the type.
        /// </summary>
        /// <typeparam name="T">The <see cref="Enum"/> type.</typeparam>
        /// <param name="value">The type representation.</param>
        /// <returns>The value for the type representation.</returns>
        public static T GetByString<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value.ToString(), true);
        }
    }
}