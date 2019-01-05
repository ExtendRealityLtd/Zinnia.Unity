namespace Zinnia.Data.Enum
{
    using System;

    /// <summary>
    /// Properties of a <see cref="UnityEngine.Transform"/>.
    /// </summary>
    [Flags]
    public enum TransformProperties
    {
        /// <summary>
        /// The Position of a <see cref="UnityEngine.Transform"/>.
        /// </summary>
        Position = 1 << 0,
        /// <summary>
        /// The Rotation of a <see cref="UnityEngine.Transform"/>.
        /// </summary>
        Rotation = 1 << 1,
        /// <summary>
        /// The Scale of a <see cref="UnityEngine.Transform"/>.
        /// </summary>
        Scale = 1 << 2
    }
}