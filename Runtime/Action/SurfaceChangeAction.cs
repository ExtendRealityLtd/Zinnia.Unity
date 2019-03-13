namespace Zinnia.Action
{
    using Malimbe.BehaviourStateRequirementMethod;
    using Malimbe.MemberChangeMethod;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;
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
        [Serialized]
        [field: DocumentedByXml]
        public float ChangeDistance { get; set; } = 0.5f;
        /// <summary>
        /// The axes to check for distance differences on.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public Vector3State CheckAxis { get; set; } = Vector3State.True;

        protected SurfaceData previousData;

        /// <summary>
        /// Digests <see cref="SurfaceData"/> and compares the current surface to the previous surface to determine if a change has occured.
        /// </summary>
        /// <param name="surfaceData">The <see cref="SurfaceData"/> to check on.</param>
        [RequiresBehaviourState]
        public virtual void Receive(SurfaceData surfaceData)
        {
            if (!ValidSurfaceData(surfaceData))
            {
                return;
            }

            Vector3 generatedOrigin = GetCollisionPoint(surfaceData.PreviousCollisionData);
            Vector3 generatedTarget = GeneratePoint(surfaceData.Position);

            bool result = !generatedOrigin.ApproxEquals(generatedTarget, ChangeDistance);
            Receive(result);
            previousData = surfaceData;
        }

        /// <summary>
        /// Checks to see if the given <see cref="SurfaceData"/> is valid.
        /// </summary>
        /// <param name="surfaceData">The <see cref="SurfaceData"/> to check on.</param>
        /// <returns><see langword="true"/> if the <see cref="SurfaceData"/> given is valid.</returns>
        protected virtual bool ValidSurfaceData(SurfaceData surfaceData)
        {
            return surfaceData != null && surfaceData.IsValid;
        }

        /// <summary>
        /// Attempts to get the collision point for the given <see cref="RaycastHit"/> data.
        /// </summary>
        /// <param name="collisionData">The <see cref="RaycastHit"/> data to get the collision point from.</param>
        /// <returns>The collision point.</returns>
        protected virtual Vector3 GetCollisionPoint(RaycastHit collisionData)
        {
            return collisionData.transform != null ? GeneratePoint(collisionData.point) : Vector3.zero;
        }

        /// <summary>
        /// Creates a <see cref="Vector3"/> based on the given point for the valid axes.
        /// </summary>
        /// <param name="point">The Point to generate the <see cref="Vector3"/> from.</param>
        /// <returns>The point only within the valid axes.</returns>
        protected virtual Vector3 GeneratePoint(Vector3 point)
        {
            float resultX = CheckAxis.xState ? point.x : 0f;
            float resultY = CheckAxis.yState ? point.y : 0f;
            float resultZ = CheckAxis.zState ? point.z : 0f;
            return new Vector3(resultX, resultY, resultZ);
        }

        /// <summary>
        /// Called after <see cref="ChangeDistance"/> has been changed.
        /// </summary>
        [CalledAfterChangeOf(nameof(ChangeDistance))]
        protected virtual void OnAfterChangeDistanceChange()
        {
            if (previousData != null)
            {
                Receive(previousData);
            }
        }

        /// <summary>
        /// Called after <see cref="CheckAxis"/> has been changed.
        /// </summary>
        [CalledAfterChangeOf(nameof(CheckAxis))]
        protected virtual void OnAfterCheckAxisChange()
        {
            if (previousData != null)
            {
                Receive(previousData);
            }
        }
    }
}