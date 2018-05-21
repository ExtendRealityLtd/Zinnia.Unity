﻿namespace VRTK.Core.Tracking
{
    using UnityEngine;
    using VRTK.Core.Cast;

    /// <summary>
    /// Rotates the <see cref="GameObject"/> to look into a normal direction.
    /// </summary>
    public class PointNormalRotator : MonoBehaviour
    {
        /// <summary>
        /// Handles the provided data to rotate the <see cref="GameObject"/>.
        /// </summary>
        /// <param name="data">The data to take the rotation info from.</param>
        /// <param name="initiator">The initiator of this method.</param>
        public virtual void HandleData(PointsCastData data, object initiator = null)
        {
            if (data.targetHit != null)
            {
                transform.rotation = Quaternion.FromToRotation(Vector3.up, data.targetHit.Value.normal);
            }
        }
    }
}