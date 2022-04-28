namespace Zinnia.Rule
{
    using UnityEngine;
    using UnityEngine.AI;

    /// <summary>
    /// Determines whether a given <see cref="Vector3"/> point is within the <see cref="NavMesh"/>.
    /// </summary>
    public class NavMeshRule : Vector3Rule
    {
        [Tooltip("The relative vertical displacement of the NavMesh to the nearest surface.")]
        [SerializeField]
        private float baseOffset = 0f;
        /// <summary>
        /// The relative vertical displacement of the <see cref="NavMesh"/> to the nearest surface.
        /// </summary>
        public float BaseOffset
        {
            get
            {
                return baseOffset;
            }
            set
            {
                baseOffset = value;
            }
        }
        [Tooltip("The max distance given point can be outside the NavMesh to be considered valid.")]
        [SerializeField]
        private float distanceLimit = 0.1f;
        /// <summary>
        /// The max distance given point can be outside the <see cref="NavMesh"/> to be considered valid.
        /// </summary>
        public float DistanceLimit
        {
            get
            {
                return distanceLimit;
            }
            set
            {
                distanceLimit = value;
            }
        }
        [Tooltip("The parts of the NavMesh that are considered valid.")]
        [SerializeField]
        private int validAreas = -1;
        /// <summary>
        /// The parts of the <see cref="NavMesh"/> that are considered valid.
        /// </summary>
        public int ValidAreas
        {
            get
            {
                return validAreas;
            }
            set
            {
                validAreas = value;
            }
        }

        /// <inheritdoc />
        protected override bool Accepts(Vector3 targetVector3)
        {
            return NavMesh.SamplePosition(targetVector3 + (Vector3.up * BaseOffset), out NavMeshHit hit, DistanceLimit, ValidAreas);
        }
    }
}