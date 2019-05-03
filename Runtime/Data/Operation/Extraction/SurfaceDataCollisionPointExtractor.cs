namespace Zinnia.Data.Operation.Extraction
{
    using UnityEngine;
    using Malimbe.MemberClearanceMethod;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;
    using Zinnia.Data.Type;

    /// <summary>
    /// Extracts and emits the point of collision from <see cref="SurfaceData"/>.
    /// </summary>
    public class SurfaceDataCollisionPointExtractor : Vector3Extractor
    {
        /// <summary>
        /// The source to extract from.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public SurfaceData Source { get; set; }

        /// <inheritdoc />
        public override Vector3? Extract()
        {
            if (Source == null || Source.CollisionData.transform == null)
            {
                Result = null;
                return null;
            }

            Result = Source.CollisionData.point;
            return base.Extract();
        }

        /// <summary>
        /// Extracts the <see cref="Vector3"/> from the given <see cref="SurfaceData"/>.
        /// </summary>
        /// <param name="data">The data to extract from.</param>
        /// <returns>The extracted collision point.</returns>
        public virtual Vector3? Extract(SurfaceData data)
        {
            Source = data;
            return Extract();
        }

        /// <summary>
        /// Extracts the <see cref="Vector3"/> from the given <see cref="SurfaceData"/>.
        /// </summary>
        /// <param name="data">The data to extract from.</param>
        public virtual void DoExtract(SurfaceData data)
        {
            Extract(data);
        }
    }
}