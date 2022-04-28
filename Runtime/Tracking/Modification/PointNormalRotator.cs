namespace Zinnia.Tracking.Modification
{
    using UnityEngine;
    using Zinnia.Cast;
    using Zinnia.Extension;

    /// <summary>
    /// Rotates the <see cref="GameObject"/> to look into a normal direction.
    /// </summary>
    public class PointNormalRotator : MonoBehaviour
    {
        [Tooltip("The target to apply the rotations to.")]
        [SerializeField]
        private GameObject target;
        /// <summary>
        /// The target to apply the rotations to.
        /// </summary>
        public GameObject Target
        {
            get
            {
                return target;
            }
            set
            {
                target = value;
            }
        }

        /// <summary>
        /// Clears <see cref="Target"/>.
        /// </summary>
        public virtual void ClearTarget()
        {
            if (!this.IsValidState())
            {
                return;
            }

            Target = default;
        }

        /// <summary>
        /// Handles the provided data to rotate the <see cref="GameObject"/>.
        /// </summary>
        /// <param name="data">The data to take the rotation info from.</param>
        public virtual void HandleData(PointsCast.EventData data)
        {
            if (!this.IsValidState() || Target == null || data.HitData == null)
            {
                return;
            }

            Target.transform.rotation = Quaternion.FromToRotation(Vector3.up, data.HitData.Value.normal);
        }
    }
}