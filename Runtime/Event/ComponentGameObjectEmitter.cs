namespace Zinnia.Event
{
    using Malimbe.MemberClearanceMethod;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.PropertyValidationMethod;
    using Malimbe.XmlDocumentationAttribute;
    using UnityEngine;

    /// <summary>
    /// Extracts the <see cref="GameObject"/> from the <see cref="Source"/> and emits an event containing the result.
    /// </summary>
    public class ComponentGameObjectEmitter : GameObjectEmitter
    {
        /// <summary>
        /// The source to extract from.
        /// </summary>
        [Serialized, Validated, Cleared]
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