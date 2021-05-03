namespace Zinnia.Cast.Operation.Conversion
{
    using System;
    using UnityEngine;

    /// <summary>
    /// Converts any supported <see cref="Physics"/> caster to a <see cref="Physics.Linecast"/>.
    /// </summary>
    public class ToLinecastConverter : CastConverter
    {
        /// <inheritdoc />
        public override bool ConvertFromBoxCast(PhysicsCast customCast, out RaycastHit hitData, Vector3 center, Vector3 halfExtents, Vector3 direction, Quaternion orientation, float maxDistance)
        {
            Vector3 endPosition = GetProjectedPoint(center, direction, maxDistance);
            return customCast.CustomLinecast(center, endPosition, out hitData, false);
        }

        /// <inheritdoc />
        public override ArraySegment<RaycastHit> ConvertFromBoxCastAll(PhysicsCast customCast, Vector3 center, Vector3 halfExtents, Vector3 direction, Quaternion orientation, float maxDistance)
        {
            throw new NotImplementedException("There is no implementation of `Physics.LinecastAll`");
        }

        /// <inheritdoc />
        public override bool ConvertFromCapsuleCast(PhysicsCast customCast, out RaycastHit hitData, Vector3 point1, Vector3 point2, float radius, Vector3 direction, float maxDistance)
        {
            Vector3 origin = GetCenterVector(point1, point2);
            Vector3 endPosition = GetProjectedPoint(origin, direction, maxDistance);
            return customCast.CustomLinecast(origin, endPosition, out hitData, false);
        }

        /// <inheritdoc />
        public override ArraySegment<RaycastHit> ConvertFromCapsuleCastAll(PhysicsCast customCast, Vector3 point1, Vector3 point2, float radius, Vector3 direction, float maxDistance)
        {
            throw new NotImplementedException("There is no implementation of `Physics.LinecastAll`");
        }

        /// <inheritdoc />
        public override bool ConvertFromLinecast(PhysicsCast customCast, out RaycastHit hitData, Vector3 startPosition, Vector3 endPosition)
        {
            return customCast.CustomLinecast(startPosition, endPosition, out hitData, false);
        }

        /// <inheritdoc />
        public override bool ConvertFromRaycast(PhysicsCast customCast, out RaycastHit hitData, Ray ray, float maxDistance)
        {
            Vector3 endPosition = GetProjectedPoint(ray.origin, ray.direction, maxDistance);
            return customCast.CustomLinecast(ray.origin, endPosition, out hitData, false);
        }

        /// <inheritdoc />
        public override ArraySegment<RaycastHit> ConvertFromRaycastAll(PhysicsCast customCast, Ray ray, float maxDistance)
        {
            throw new NotImplementedException("There is no implementation of `Physics.LinecastAll`");
        }

        /// <inheritdoc />
        public override bool ConvertFromSphereCast(PhysicsCast customCast, out RaycastHit hitData, Vector3 origin, float radius, Vector3 direction, float maxDistance)
        {
            Vector3 endPosition = GetProjectedPoint(origin, direction, maxDistance);
            return customCast.CustomLinecast(origin, endPosition, out hitData, false);
        }

        /// <inheritdoc />
        public override ArraySegment<RaycastHit> ConvertFromSphereCastAll(PhysicsCast customCast, Vector3 origin, float radius, Vector3 direction, float maxDistance)
        {
            throw new NotImplementedException("There is no implementation of `Physics.LinecastAll`");
        }
    }
}