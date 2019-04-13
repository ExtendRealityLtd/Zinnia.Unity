namespace Zinnia.Data.Operation.Extraction
{
    using UnityEngine;
    using Malimbe.MemberClearanceMethod;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;

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
            if (Source == null)
            {
                Result = null;
                return null;
            }

            Result = Source.gameObject;
            return base.Extract();
        }
    }
}