namespace Zinnia.Action
{
    using UnityEngine;
    using Zinnia.Data.Type;
    using Zinnia.Extension;

    /// <summary>
    /// Emits a <see cref="bool"/> value when the received <see cref="SurfaceData"/> current position and previous position exceed the specified distance.
    /// </summary>
    public class SurfaceChangeAction : BooleanAction
    {
        /// <summary>
        /// The distance between the current surface and previous surface to consider a valid change.
        /// </summary>
        public float changeDistance = 0.5f;
        /// <summary>
        /// The axes to check for distance differences on.
        /// </summary>
        public Vector3State checkAxis = Vector3State.True;

        /// <summary>
        /// Digests <see cref="SurfaceData"/> and compares the current surface to the previous surface to determine if a change has occured.
        /// </summary>
        /// <param name="surfaceData">The <see cref="SurfaceData"/> to check on.</param>
        public virtual void Receive(SurfaceData surfaceData)
        {
            if (ValidSurfaceData(surfaceData))
            {
                Vector3 generatedOrigin = GetCollisionPoint(surfaceData.PreviousCollisionData);
                Vector3 generatedTarget = GeneratePoint(surfaceData.Position);

                bool result = !generatedOrigin.ApproxEquals(generatedTarget, changeDistance);
                Receive(result);
            }
        }

        /// <summary>
        /// Checks to see if the given <see cref="SurfaceData"/> is valid.
        /// </summary>
        /// <param name="surfaceData">The <see cref="SurfaceData"/> to check on.</param>
        /// <returns><see langword="true"/> if the <see cref="SurfaceData"/> given is valid.</returns>
        protected virtual bool ValidSurfaceData(SurfaceData surfaceData)
        {
            return (surfaceData != null && surfaceData.Valid);
        }

        /// <summary>
        /// Attempts to get the collision point for the given <see cref="RaycastHit"/> data.
        /// </summary>
        /// <param name="collisionData">The <see cref="RaycastHit"/> data to get the collision point from.</param>
        /// <returns>The collision point.</returns>
        protected virtual Vector3 GetCollisionPoint(RaycastHit collisionData)
        {
            return (collisionData.transform != null ? GeneratePoint(collisionData.point) : Vector3.zero);
        }

        /// <summary>
        /// Creates a <see cref="Vector3"/> based on the given point for the valid axes.
        /// </summary>
        /// <param name="point">The Point to generate the <see cref="Vector3"/> from.</param>
        /// <returns>The point only within the valid axes.</returns>
        protected virtual Vector3 GeneratePoint(Vector3 point)
        {
            float resultX = (checkAxis.xState ? point.x : 0f);
            float resultY = (checkAxis.yState ? point.y : 0f);
            float resultZ = (checkAxis.zState ? point.z : 0f);
            return new Vector3(resultX, resultY, resultZ);
        }
    }
}