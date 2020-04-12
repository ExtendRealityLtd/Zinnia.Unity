namespace Zinnia.Data.Operation.Extraction
{
    using Malimbe.MemberClearanceMethod;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;
    using UnityEngine;
    using Zinnia.Data.Type;

    /// <summary>
    /// Extracts and emits the <see cref="Source"/> residing <see cref="GameObject"/>.
    /// </summary>
    public class TransformDataGameObjectExtractor : GameObjectExtractor
    {
        /// <summary>
        /// The source to extract from.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public TransformData Source { get; set; }

        /// <inheritdoc />
        public override GameObject Extract()
        {
            if (Source == null || Source.Transform == null)
            {
                Result = null;
                return null;
            }

            Result = Source.Transform.gameObject;
            return base.Extract();
        }
    }
}