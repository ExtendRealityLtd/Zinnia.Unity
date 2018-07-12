namespace VRTK.Core.Rule
{
    using UnityEngine;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Determines whether a <see cref="GameObject"/>'s <see cref="GameObject.tag"/> is part of a list.
    /// </summary>
    public class AnyTagRule : BaseGameObjectRule
    {
        /// <summary>
        /// The tags to check against.
        /// </summary>
        [Tooltip("The tags to check against.")]
        public List<string> tags = new List<string>();

        /// <inheritdoc />
        protected override bool Accepts(GameObject targetGameObject)
        {
            return tags.Any(targetGameObject.CompareTag);
        }
    }
}