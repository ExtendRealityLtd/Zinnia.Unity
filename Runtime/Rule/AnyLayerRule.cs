namespace Zinnia.Rule
{
    using UnityEngine;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;

    /// <summary>
    /// Determines whether a <see cref="GameObject"/>'s <see cref="GameObject.layer"/> is part of a list.
    /// </summary>
    public class AnyLayerRule : GameObjectRule
    {
        /// <summary>
        /// The layers to check against.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public LayerMask LayerMask { get; set; }

        /// <inheritdoc />
        protected override bool Accepts(GameObject targetGameObject)
        {
            return (LayerMask & (1 << targetGameObject.layer)) != 0;
        }
    }
}