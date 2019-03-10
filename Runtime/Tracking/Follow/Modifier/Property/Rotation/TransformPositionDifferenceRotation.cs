namespace Zinnia.Tracking.Follow.Modifier.Property.Rotation
{
    using UnityEngine;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;
    using Zinnia.Extension;
    using Zinnia.Data.Type;

    /// <summary>
    /// Updates the transform rotation of the target to match the difference in position of the source.
    /// </summary>
    public class TransformPositionDifferenceRotation : PropertyModifier
    {
        /// <summary>
        /// The drag applied to the rotation to slow it down.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public float AngularDrag { get; set; } = 1f;

        /// <summary>
        /// Determines which axes to rotate.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public Vector3State FollowOnAxis { get; set; } = Vector3State.True;

        /// <summary>
        /// The current angular velocity the rotation is applying to the target.
        /// </summary>
        public Vector3 AngularVelocity { get; protected set; }

        /// <summary>
        /// The previous source world position.
        /// </summary>
        protected Vector3? previousSourcePosition;

        /// <summary>
        /// Resets the state of the source previous position.
        /// </summary>
        public virtual void ResetPreviousState()
        {
            previousSourcePosition = null;
        }

        /// <summary>
        /// Rotates the target based on the position difference of the source.
        /// </summary>
        /// <param name="source">The source to utilize in the modification.</param>
        /// <param name="target">The target to modify.</param>
        /// <param name="offset">The offset of the target against the source when modifying.</param>
        protected override void DoModify(GameObject source, GameObject target, GameObject offset = null)
        {
            AngularVelocity = CalculateAngularVelocity(source, target);
            target.transform.localRotation *= Quaternion.Euler(AngularVelocity);
        }

        protected virtual void OnDisable()
        {
            ResetPreviousState();
        }

        /// <summary>
        /// Calculates the angular velocity based on the differing source position in relation to the target position.
        /// </summary>
        /// <param name="source">The source to utilize in the modification.</param>
        /// <param name="target">The target to modify.</param>
        /// <returns>The angular velocity to project onto the target.</returns>
        protected virtual Vector3 CalculateAngularVelocity(GameObject source, GameObject target)
        {
            Vector3 sourcePosition = source.transform.position;
            Vector3 targetPosition = target.transform.position;

            if (previousSourcePosition == null)
            {
                previousSourcePosition = sourcePosition;
            }

            float xDegree = FollowOnAxis.xState ? CalculateAngle(target.transform.right, targetPosition, (Vector3)previousSourcePosition, sourcePosition) : 0f;
            float yDegree = FollowOnAxis.yState ? CalculateAngle(target.transform.up, targetPosition, (Vector3)previousSourcePosition, sourcePosition) : 0f;
            float zDegree = FollowOnAxis.zState ? CalculateAngle(target.transform.forward, targetPosition, (Vector3)previousSourcePosition, sourcePosition) : 0f;

            previousSourcePosition = sourcePosition;

            return ApplyDrag(new Vector3(xDegree, yDegree, zDegree));
        }

        /// <summary>
        /// Calculates the rotational angle for an axis based on the difference between two points around the origin.
        /// </summary>
        /// <param name="originDirection">The direction representing the axis.</param>
        /// <param name="originPoint">The angle centre.</param>
        /// <param name="pointA">The first point to calculate the angle from.</param>
        /// <param name="pointB">The second point to calculate the angle to.</param>
        /// <returns>The angle in degrees between the two points.</returns>
        protected virtual float CalculateAngle(Vector3 originDirection, Vector3 originPoint, Vector3 pointA, Vector3 pointB)
        {
            Vector3 heading = pointB - originPoint;
            float headingMagnitude = heading.magnitude;
            Vector3 sideA = pointA - originPoint;
            if (headingMagnitude.ApproxEquals(0f))
            {
                return 0f;
            }

            Vector3 sideB = heading * (1f / headingMagnitude);
            return Mathf.Atan2(Vector3.Dot(originDirection, Vector3.Cross(sideA, sideB)), Vector3.Dot(sideA, sideB)) * Mathf.Rad2Deg;
        }

        /// <summary>
        /// Applies an opposing drag force to the current rotational velocity.
        /// </summary>
        /// <param name="angularVelocity">The current rotational velocity being applied.</param>
        /// <returns>The rotational velocity with the opposing drag applied to slow it down.</returns>
        protected virtual Vector3 ApplyDrag(Vector3 angularVelocity)
        {
            return angularVelocity * (1f / AngularDrag);
        }
    }
}