namespace Zinnia.Rule
{
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;
    using UnityEngine;
    using UnityEngine.AI;

    /// <summary>
    /// Determines whether a given <see cref="Vector3"/> point is within the <see cref="NavMesh"/>.
    /// </summary>
    public class NavMeshRule : Vector3Rule
    {
        /// <summary>
        /// The relative vertical displacement of the <see cref="NavMesh"/> to the nearest surface.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public float BaseOffset { get; set; } = 0f;
        /// <summary>
        /// The max distance given point can be outside the <see cref="NavMesh"/> to be considered valid.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public float DistanceLimit { get; set; } = 0.1f;
        /// <summary>
        /// The parts of the <see cref="NavMesh"/> that are considered valid.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public int ValidAreas { get; set; } = -1;

        /// <inheritdoc />
        protected override bool Accepts(Vector3 targetVector3)
        {
            return NavMesh.SamplePosition(targetVector3 + (Vector3.up * BaseOffset), out NavMeshHit hit, DistanceLimit, ValidAreas);
        }
    }
}