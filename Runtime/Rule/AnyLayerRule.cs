namespace Zinnia.Rule
{
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;
    using UnityEngine;

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