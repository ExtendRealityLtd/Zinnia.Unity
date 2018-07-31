namespace VRTK.Core.Tracking.Modification
{
    using UnityEngine;
    using VRTK.Core.Cast;

    /// <summary>
    /// Rotates the <see cref="GameObject"/> to look into a normal direction.
    /// </summary>
    public class PointNormalRotator : MonoBehaviour
    {
        /// <summary>
        /// The target to apply the rotations to.
        /// </summary>
        [Tooltip("The target to apply the rotations to.")]
        public GameObject target;

        /// <summary>
        /// Handles the provided data to rotate the <see cref="GameObject"/>.
        /// </summary>
        /// <param name="data">The data to take the rotation info from.</param>
        public virtual void HandleData(PointsCast.EventData data)
        {
            if (!isActiveAndEnabled || target == null || data.targetHit == null)
            {
                return;
            }

            target.transform.rotation = Quaternion.FromToRotation(Vector3.up, data.targetHit.Value.normal);
        }
    }
}