namespace VRTK.Core.Tracking
{
    using UnityEngine;
    using VRTK.Core.Cast;

    /// <summary>
    /// Rotates the <see cref="GameObject"/> to look into a normal direction.
    /// </summary>
    public class PointNormalRotator : MonoBehaviour
    {
        /// <summary>
        /// The source to apply the rotations to.
        /// </summary>
        [Tooltip("The source to apply the rotations to.")]
        public Transform source;

        /// <summary>
        /// Handles the provided data to rotate the <see cref="GameObject"/>.
        /// </summary>
        /// <param name="data">The data to take the rotation info from.</param>
        public virtual void HandleData(PointsCast.EventData data)
        {
            if (!isActiveAndEnabled || source == null || data.targetHit == null)
            {
                return;
            }

            source.rotation = Quaternion.FromToRotation(Vector3.up, data.targetHit.Value.normal);
        }
    }
}