namespace Zinnia.Rule
{
    using UnityEngine;
    using Malimbe.XmlDocumentationAttribute;

    /// <summary>
    /// Determines whether a <see cref="GameObject"/>'s <see cref="GameObject.layer"/> is part of a list.
    /// </summary>
    public class AnyLayerRule : GameObjectRule
    {
        /// <summary>
        /// The layers to check against.
        /// </summary>
        [DocumentedByXml]
        public LayerMask layerMask;

        /// <inheritdoc />
        protected override bool Accepts(GameObject targetGameObject)
        {
            return (targetGameObject.layer & layerMask.value) != 0;
        }
    }
}