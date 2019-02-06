namespace Zinnia.Rule
{
    using UnityEngine;

    /// <summary>
    /// Determines whether a <see cref="GameObject"/> is active in the scene hierarchy.
    /// </summary>
    public class ActiveInHierarchyRule : GameObjectRule
    {
        /// <inheritdoc />
        protected override bool Accepts(GameObject targetGameObject)
        {
            return targetGameObject.activeInHierarchy;
        }
    }
}