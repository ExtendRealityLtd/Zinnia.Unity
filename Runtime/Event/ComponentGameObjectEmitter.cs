namespace Zinnia.Event
{
    using UnityEngine;
    using Malimbe.MemberClearanceMethod;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;

    /// <summary>
    /// Extracts the <see cref="GameObject"/> from the <see cref="Source"/> and emits an event containing the result.
    /// </summary>
    public class ComponentGameObjectEmitter : GameObjectEmitter
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