namespace Zinnia.Data.Attribute
{
    using UnityEngine;
    using System;

    /// <summary>
    /// Draws the property in a specified restricted way.
    /// </summary>
    public class RestrictedAttribute : PropertyAttribute
    {
        /// <summary>
        /// The restriction options that can be applied.
        /// </summary>
        [Flags]
        public enum Restrictions
        {
            /// <summary>
            /// The property is always read-only.
            /// </summary>
            ReadOnlyAlways = 1 << 0,
            /// <summary>
            /// The property is read-only when the application is playing and the component is enabled.
            /// </summary>
            ReadOnlyWhenPlayingAndEnabled = 1 << 1,
            /// <summary>
            /// The property is de-emphasized.
            /// </summary>
            Muted = 1 << 2
        }

        /// <summary>
        /// The restriction options to apply.
        /// </summary>
        public readonly Restrictions restrictions;

        /// <summary>
        /// Draws the property in a specified restricted way.
        /// </summary>
        /// <param name="restrictions">The restriction options to apply.</param>
        public RestrictedAttribute(Restrictions restrictions = Restrictions.ReadOnlyWhenPlayingAndEnabled | Restrictions.Muted)
        {
            this.restrictions = restrictions;
        }
    }
}