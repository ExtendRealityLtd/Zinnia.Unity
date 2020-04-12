namespace Zinnia.Data.Operation.Extraction
{
    using Malimbe.MemberClearanceMethod;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;
    using UnityEngine;

    /// <summary>
    /// Extracts and emits the <see cref="Source"/> residing <see cref="GameObject"/>.
    /// </summary>
    public class ComponentGameObjectExtractor : GameObjectExtractor
    {
        /// <summary>
        /// The source to extract from.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public Component Source { get; set; }

        /// <inheritdoc />
        public override GameObject Extract()
        {
            if (!isActiveAndEnabled || Source == null)
            {
                Result = null;
                return null;
            }

            Result = Source.gameObject;
            return base.Extract();
        }
    }
}