namespace VRTK.Core.Action
{
    using UnityEngine;
    using VRTK.Core.Data.Type;
    using VRTK.Core.Extension;

    /// <summary>
    /// The SurfaceChangeAction emits an event when the received SurfaceData current position and previous position exceed the specified distance.
    /// </summary>
    public class SurfaceChangeAction : BooleanAction
    {
        [Tooltip("The distance between the current surface and previous surface to consider a valid change.")]
        public float changeDistance = 0.5f;
        [Tooltip("The axes to check for distance differences on.")]
        public Vector3State checkAxis = new Vector3State(true, true, true);

        /// <summary>
        /// The Receive method digests SurfaceData and compares the current surface to the previous surface to determine if a change has occured.
        /// </summary>
        /// <param name="surfaceData">The SurfaceData to check on.</param>
        /// <param name="sender">The sender of the action.</param>
        public virtual void Receive(SurfaceData surfaceData, object sender = null)
        {
            if (ValidSurfaceData(surfaceData))
            {
                Vector3 generatedOrigin = GetCollisionPoint(surfaceData.PreviousCollisionData);
                Vector3 generatedTarget = GeneratePoint(surfaceData.Position);

                bool result = !generatedOrigin.Compare(generatedTarget, changeDistance);
                Receive(result, sender);
            }
        }

        /// <summary>
        /// The ValidSurfaceData method checks to see if the given SurfaceData is valid.
        /// </summary>
        /// <param name="surfaceData">The SurfaceData to check on.</param>
        /// <returns>Returns `true` if the SurfaceData given is valid.</returns>
        protected virtual bool ValidSurfaceData(SurfaceData surfaceData)
        {
            return (surfaceData != null && surfaceData.Valid);
        }

        /// <summary>
        /// The GetCollisionPoint method attempts to get the collision point for the given RaycastHit data.
        /// </summary>
        /// <param name="collisionData">The RaycastHit data to get the collision point from.</param>
        /// <returns>Returns a Vector3 of the collision point.</returns>
        protected virtual Vector3 GetCollisionPoint(RaycastHit collisionData)
        {
            return (collisionData.transform != null ? GeneratePoint(collisionData.point) : Vector3.zero);
        }

        /// <summary>
        /// The GeneratePoint method creates a Vector3 based on the given point for the valid axes.
        /// </summary>
        /// <param name="point">The Point to generate the Vector3 from.</param>
        /// <returns>A Vector3 of the point only within the valid axes.</returns>
        protected virtual Vector3 GeneratePoint(Vector3 point)
        {
            float resultX = (checkAxis.xState ? point.x : 0f);
            float resultY = (checkAxis.yState ? point.y : 0f);
            float resultZ = (checkAxis.zState ? point.z : 0f);
            return new Vector3(resultX, resultY, resultZ);
        }
    }
}