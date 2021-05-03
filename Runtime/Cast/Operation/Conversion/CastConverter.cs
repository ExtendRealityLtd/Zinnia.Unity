namespace Zinnia.Cast.Operation.Conversion
{
    using System;
    using UnityEngine;

    /// <summary>
    /// Converts between different Physics cast types.
    /// </summary>
    public abstract class CastConverter : MonoBehaviour
    {
        /// <summary>
        /// Converts from <see cref="Physics.BoxCast"/>.
        /// </summary>
        /// <param name="customCast">The optional <see cref="PhysicsCast"/> with customized cast parameters.</param>
        /// <param name="hitData">The <see cref="RaycastHit"/> data.</param>
        /// <param name="center">The center of the box.</param>
        /// <param name="halfExtents">Half the size of the box in each dimension.</param>
        /// <param name="direction">The direction in which to cast the box.</param>
        /// <param name="orientation">The rotation of the box.</param>
        /// <param name="maxDistance">The max length of the cast.</param>
        /// <returns>Whether the BoxCast successfully collides with a valid <see cref="GameObject"/>.</returns>
        public abstract bool ConvertFromBoxCast(PhysicsCast customCast, out RaycastHit hitData, Vector3 center, Vector3 halfExtents, Vector3 direction, Quaternion orientation, float maxDistance);
        /// <summary>
        /// Converts from <see cref="Physics.BoxCastAll"/>.
        /// </summary>
        /// <param name="customCast">The optional <see cref="PhysicsCast"/> with customized cast parameters.</param>
        /// <param name="center">The center of the box.</param>
        /// <param name="halfExtents">Half the size of the box in each dimension.</param>
        /// <param name="direction">The direction in which to cast the box.</param>
        /// <param name="orientation">The rotation of the box.</param>
        /// <param name="maxDistance">The max length of the cast.</param>
        /// <returns>A collection of collisions determined by the cast.</returns>
        public abstract ArraySegment<RaycastHit> ConvertFromBoxCastAll(PhysicsCast customCast, Vector3 center, Vector3 halfExtents, Vector3 direction, Quaternion orientation, float maxDistance);
        /// <summary>
        /// Converts from <see cref="Physics.CapsuleCast"/>.
        /// </summary>
        /// <param name="customCast">The optional <see cref="PhysicsCast"/> with customized cast parameters.</param>
        /// <param name="hitData">The <see cref="RaycastHit"/> data.</param>
        /// <param name="point1">The center of the sphere at the start of the capsule.</param>
        /// <param name="point2">The center of the sphere at the end of the capsule.</param>
        /// <param name="radius">The radius of the capsule.</param>
        /// <param name="direction">The direction into which to sweep the capsule.</param>
        /// <param name="maxDistance">The max length of the sweep.</param>
        /// <returns>Whether the CapsuleCast successfully collides with a valid <see cref="GameObject"/>.</returns>
        public abstract bool ConvertFromCapsuleCast(PhysicsCast customCast, out RaycastHit hitData, Vector3 point1, Vector3 point2, float radius, Vector3 direction, float maxDistance);
        /// <summary>
        /// Converts from <see cref="Physics.CapsuleCastAll"/>.
        /// </summary>
        /// <param name="customCast">The optional <see cref="PhysicsCast"/> with customized cast parameters.</param>
        /// <param name="point1">The center of the sphere at the start of the capsule.</param>
        /// <param name="point2">The center of the sphere at the end of the capsule.</param>
        /// <param name="radius">The radius of the capsule.</param>
        /// <param name="direction">The direction into which to sweep the capsule.</param>
        /// <param name="maxDistance">The max length of the sweep.</param>
        /// <returns>A collection of collisions determined by the cast.</returns>
        public abstract ArraySegment<RaycastHit> ConvertFromCapsuleCastAll(PhysicsCast customCast, Vector3 point1, Vector3 point2, float radius, Vector3 direction, float maxDistance);
        /// <summary>
        /// Converts from <see cref="Physics.Linecast"/>.
        /// </summary>
        /// <param name="customCast">The optional <see cref="PhysicsCast"/> with customized cast parameters.</param>
        /// <param name="hitData">The <see cref="RaycastHit"/> data.</param>
        /// <param name="startPosition">The world position to start the Linecast from.</param>
        /// <param name="endPosition">The world position to end the Linecast at.</param>
        /// <returns>Whether the Linecast successfully collides with a valid <see cref="GameObject"/>.</returns>
        public abstract bool ConvertFromLinecast(PhysicsCast customCast, out RaycastHit hitData, Vector3 startPosition, Vector3 endPosition);
        /// <summary>
        /// Converts from <see cref="Physics.Raycast"/>.
        /// </summary>
        /// <param name="customCast">The optional <see cref="PhysicsCast"/> with customized cast parameters.</param>
        /// <param name="hitData">The <see cref="RaycastHit"/> data.</param>
        /// <param name="ray">The <see cref="Ray"/> to cast with.</param>
        /// <param name="maxDistance">The maximum length of the <see cref="Ray"/>.</param>
        /// <returns>Whether the <see cref="Ray"/> successfully collides with a valid <see cref="GameObject"/>.</returns>
        public abstract bool ConvertFromRaycast(PhysicsCast customCast, out RaycastHit hitData, Ray ray, float maxDistance);
        /// <summary>
        /// Converts from <see cref="Physics.RaycastAll"/>.
        /// </summary>
        /// <param name="customCast">The optional <see cref="PhysicsCast"/> with customized cast parameters.</param>
        /// <param name="ray">The <see cref="Ray"/> to cast with.</param>
        /// <param name="maxDistance">The maximum length of the <see cref="Ray"/>.</param>
        /// <returns>A collection of collisions determined by the cast.</returns>
        public abstract ArraySegment<RaycastHit> ConvertFromRaycastAll(PhysicsCast customCast, Ray ray, float maxDistance);
        /// <summary>
        /// Converts from <see cref="Physics.SphereCast"/>.
        /// </summary>
        /// <param name="customCast">The optional <see cref="PhysicsCast"/> with customized cast parameters.</param>
        /// <param name="hitData">The <see cref="RaycastHit"/> data.</param>
        /// <param name="origin">The origin point of the sphere to cast.</param>
        /// <param name="radius">The radius of the sphere.</param>
        /// <param name="direction">The direction into which to sweep the sphere.</param>
        /// <param name="maxDistance">The max length of the sweep.</param>
        /// <returns>Whether the SphereCast successfully collides with a valid <see cref="GameObject"/>.</returns>
        public abstract bool ConvertFromSphereCast(PhysicsCast customCast, out RaycastHit hitData, Vector3 origin, float radius, Vector3 direction, float maxDistance);
        /// <summary>
        /// Converts from <see cref="Physics.SphereCastAll"/>.
        /// </summary>
        /// <param name="customCast">The optional <see cref="PhysicsCast"/> with customized cast parameters.</param>
        /// <param name="origin">The origin point of the sphere to cast.</param>
        /// <param name="radius">The radius of the sphere.</param>
        /// <param name="direction">The direction into which to sweep the sphere.</param>
        /// <param name="maxDistance">The max length of the sweep.</param>
        /// <returns>A collection of collisions determined by the cast.</returns>
        public abstract ArraySegment<RaycastHit> ConvertFromSphereCastAll(PhysicsCast customCast, Vector3 origin, float radius, Vector3 direction, float maxDistance);

        /// <summary>
        /// Gets the direction between two points and also gets the distance.
        /// </summary>
        /// <param name="startPoint">The starting point.</param>
        /// <param name="endPoint">The end point.</param>
        /// <param name="distance">The distance between the two points.</param>
        /// <returns>The direction vector between the start and the end point.</returns>
        protected virtual Vector3 GetDirectionVector(Vector3 startPoint, Vector3 endPoint, out float distance)
        {
            Vector3 heading = endPoint - startPoint;
            distance = heading.magnitude;
            return heading / distance;
        }

        /// <summary>
        /// Gets the center point between two vectors.
        /// </summary>
        /// <param name="point1">The first point.</param>
        /// <param name="point2">The second point.</param>
        /// <returns>The central point between the first and second point.</returns>
        protected virtual Vector3 GetCenterVector(Vector3 point1, Vector3 point2)
        {
            return (point1 + point2) * 0.5f;
        }

        /// <summary>
        /// Gets a point projected from an origin in a given direction at a given distance.
        /// </summary>
        /// <param name="origin">The point to start the projection from.</param>
        /// <param name="direction">The direction to project.</param>
        /// <param name="distance">The distance to project the point at.</param>
        /// <returns>The projected point away from the origin in the given direction at the given distance.</returns>
        protected virtual Vector3 GetProjectedPoint(Vector3 origin, Vector3 direction, float distance)
        {
            return origin + (direction * distance);
        }
    }
}