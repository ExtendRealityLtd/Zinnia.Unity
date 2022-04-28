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
        [Tooltip("The distance between the current surface and previous surface to consider a valid change.")]
        [SerializeField]
        private float changeDistance = 0.5f;
        /// <summary>
        /// The distance between the current surface and previous surface to consider a valid change.
        /// </summary>
        public float ChangeDistance
        {
            get
            {
                return changeDistance;
            }
            set
            {
                changeDistance = value;
                if (this.IsMemberChangeAllowed())
                {
                    OnAfterChangeDistanceChange();
                }
            }
        }
        [Tooltip("The axes to check for distance differences on.")]
        [SerializeField]
        private Vector3State checkAxis = Vector3State.True;
        /// <summary>
        /// The axes to check for distance differences on.
        /// </summary>
        public Vector3State CheckAxis
        {
            get
            {
                return checkAxis;
            }
            set
            {
                checkAxis = value;
                if (this.IsMemberChangeAllowed())
                {
                    OnAfterCheckAxisChange();
                }
            }
        }

        protected SurfaceData previousData;

        /// <summary>
        /// Sets the <see cref="CheckAxis"/> x value.
        /// </summary>
        /// <param name="value">The value to set to.</param>
        public virtual void SetCheckAxisX(bool value)
        {
            CheckAxis = new Vector3State(value, CheckAxis.yState, CheckAxis.zState);
        }

        /// <summary>
        /// Sets the <see cref="CheckAxis"/> y value.
        /// </summary>
        /// <param name="value">The value to set to.</param>
        public virtual void SetCheckAxisY(bool value)
        {
            CheckAxis = new Vector3State(CheckAxis.xState, value, CheckAxis.zState);
        }

        /// <summary>
        /// Sets the <see cref="CheckAxis"/> z value.
        /// </summary>
        /// <param name="value">The value to set to.</param>
        public virtual void SetCheckAxisZ(bool value)
        {
            CheckAxis = new Vector3State(CheckAxis.xState, CheckAxis.yState, value);
        }

        /// <summary>
        /// Digests <see cref="SurfaceData"/> and compares the current surface to the previous surface to determine if a change has occurred.
        /// </summary>
        /// <param name="surfaceData">The <see cref="SurfaceData"/> to check on.</param>
        public virtual void Receive(SurfaceData surfaceData)
        {
            if (!this.IsValidState() || !ValidSurfaceData(surfaceData))
            {
                return;
            }

            Vector3 generatedOrigin = GetCollisionPoint(surfaceData.PreviousCollisionData, surfaceData.PositionalOffset);
            Vector3 generatedTarget = GeneratePoint(surfaceData.Position, Vector3.zero);

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
        /// <param name="offset">The positional offset to apply.</param>
        /// <returns>The collision point.</returns>
        protected virtual Vector3 GetCollisionPoint(RaycastHit collisionData, Vector3 offset)
        {
            return collisionData.transform != null ? GeneratePoint(collisionData.point, offset) : Vector3.zero;
        }

        /// <summary>
        /// Creates a <see cref="Vector3"/> based on the given point for the valid axes.
        /// </summary>
        /// <param name="point">The Point to generate the <see cref="Vector3"/> from.</param>
        /// <param name="offset">The positional offset to apply.</param>
        /// <returns>The point only within the valid axes.</returns>
        protected virtual Vector3 GeneratePoint(Vector3 point, Vector3 offset)
        {
            float resultX = CheckAxis.xState ? point.x + offset.x : 0f;
            float resultY = CheckAxis.yState ? point.y + offset.y : 0f;
            float resultZ = CheckAxis.zState ? point.z + offset.z : 0f;
            return new Vector3(resultX, resultY, resultZ);
        }

        /// <summary>
        /// Called after <see cref="ChangeDistance"/> has been changed.
        /// </summary>
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
        protected virtual void OnAfterCheckAxisChange()
        {
            if (previousData != null)
            {
                Receive(previousData);
            }
        }
    }
}