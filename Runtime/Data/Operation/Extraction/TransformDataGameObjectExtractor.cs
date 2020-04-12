namespace Zinnia.Data.Operation.Extraction
{
    using Malimbe.BehaviourStateRequirementMethod;
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
            if (!isActiveAndEnabled || Source == null || Source.Transform == null)
            {
                Result = null;
                return null;
            }

            Result = Source.Transform.gameObject;
            return base.Extract();
        }

        /// <summary>
        /// Extracts the <see cref="GameObject"/> from the given <see cref="TransformData"/>.
        /// </summary>
        /// <param name="data">The data to extract from.</param>
        /// <returns>The extracted <see cref="GameObject"/> from the given <see cref="TransformData"/>.</returns>
        [RequiresBehaviourState]
        public virtual GameObject Extract(TransformData data)
        {
            Source = data;
            return Extract();
        }

        /// <summary>
        /// Extracts the <see cref="GameObject"/> from the given <see cref="TransformData"/>.
        /// </summary>
        /// <param name="data">The data to extract from.</param>
        [RequiresBehaviourState]
        public virtual void DoExtract(TransformData data)
        {
            Extract(data);
        }
    }
}