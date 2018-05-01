namespace VRTK.Core.Data.Enum
{
    using System;

    /// <summary>
    /// The TransformProperties is an enum collection of properties of a transform.
    /// </summary>
    [Flags]
    public enum TransformProperties
    {
        /// <summary>
        /// The Position of a transform.
        /// </summary>
        Position = 1 << 0,
        /// <summary>
        /// The Rotation of a transform.
        /// </summary>
        Rotation = 1 << 1,
        /// <summary>
        /// The Scale of a transform.
        /// </summary>
        Scale = 1 << 2
    }
}