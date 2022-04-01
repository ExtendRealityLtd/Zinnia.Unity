namespace Zinnia.Cast.Operation.Conversion
{
    using System;
    using UnityEngine;

    /// <summary>
    /// Converts any supported <see cref="Physics"/> caster to a <see cref="Physics.CapsuleCast"/>.
    /// </summary>
    public class ToCapsuleCastConverter : CastConverter
    {
        [Tooltip("Half of the capsule height.")]
        [SerializeField]
        private float halfHeight;
        /// <summary>
        /// Half of the capsule height.
        /// </summary>
        public float HalfHeight
        {
            get
            {
                return halfHeight;
            }
            set
            {
                halfHeight = value;
            }
        }
        [Tooltip("The radius for the Physics.CapsuleCast.")]
        [SerializeField]
        private float radiusOverride;
        /// <summary>
        /// The radius for the <see cref="Physics.CapsuleCast"/>.
        /// </summary>
        public float RadiusOverride
        {
            get
            {
                return radiusOverride;
            }
            set
            {
                radiusOverride = value;
            }
        }
        [Tooltip("Whether to use the RadiusOverride value if the source caster already supports a radius. If the source caster does not support a radius then the RadiusOverride will always be used.")]
        [SerializeField]
        private bool useRadiusOverride = true;
        /// <summary>
        /// Whether to use the <see cref="RadiusOverride"/> value if the source caster already supports a radius. If the source caster does not support a radius then the <see cref="RadiusOverride"/> will always be used.
        /// </summary>
        public bool UseRadiusOverride
        {
            get
            {
                return useRadiusOverride;
            }
            set
            {
                useRadiusOverride = value;
            }
        }

        /// <inheritdoc />
        public override bool ConvertFromBoxCast(PhysicsCast customCast, out RaycastHit hitData, Vector3 center, Vector3 halfExtents, Vector3 direction, Quaternion orientation, float maxDistance)
        {
            Vector3 halfHeight = Vector3.up * HalfHeight;
            return customCast.CustomCapsuleCast(center + halfHeight, center - halfHeight, RadiusOverride, direction, out hitData, maxDistance, false);
        }

        /// <inheritdoc />
        public override ArraySegment<RaycastHit> ConvertFromBoxCastAll(PhysicsCast customCast, Vector3 center, Vector3 halfExtents, Vector3 direction, Quaternion orientation, float maxDistance)
        {
            Vector3 halfHeight = Vector3.up * HalfHeight;
            return customCast.CustomCapsuleCastAll(center + halfHeight, center - halfHeight, RadiusOverride, direction, maxDistance, false);
        }

        /// <inheritdoc />
        public override bool ConvertFromCapsuleCast(PhysicsCast customCast, out RaycastHit hitData, Vector3 point1, Vector3 point2, float radius, Vector3 direction, float maxDistance)
        {
            return customCast.CustomCapsuleCast(point1, point2, UseRadiusOverride ? RadiusOverride : radius, direction, out hitData, maxDistance, false);
        }

        /// <inheritdoc />
        public override ArraySegment<RaycastHit> ConvertFromCapsuleCastAll(PhysicsCast customCast, Vector3 point1, Vector3 point2, float radius, Vector3 direction, float maxDistance)
        {
            return customCast.CustomCapsuleCastAll(point1, point2, UseRadiusOverride ? RadiusOverride : radius, direction, maxDistance, false);
        }

        /// <inheritdoc />
        public override bool ConvertFromLinecast(PhysicsCast customCast, out RaycastHit hitData, Vector3 startPosition, Vector3 endPosition)
        {
            Vector3 halfHeight = Vector3.up * HalfHeight;
            Vector3 direction = GetDirectionVector(startPosition, endPosition, out float distance);
            return customCast.CustomCapsuleCast(startPosition + halfHeight, startPosition - halfHeight, RadiusOverride, direction, out hitData, distance, false);
        }

        /// <inheritdoc />
        public override bool ConvertFromRaycast(PhysicsCast customCast, out RaycastHit hitData, Ray ray, float maxDistance)
        {
            Vector3 halfHeight = Vector3.up * HalfHeight;
            return customCast.CustomCapsuleCast(ray.origin + halfHeight, ray.origin - halfHeight, RadiusOverride, ray.direction, out hitData, maxDistance, false);
        }

        /// <inheritdoc />
        public override ArraySegment<RaycastHit> ConvertFromRaycastAll(PhysicsCast customCast, Ray ray, float maxDistance)
        {
            Vector3 halfHeight = Vector3.up * HalfHeight;
            return customCast.CustomCapsuleCastAll(ray.origin + halfHeight, ray.origin - halfHeight, RadiusOverride, ray.direction, maxDistance, false);
        }

        /// <inheritdoc />
        public override bool ConvertFromSphereCast(PhysicsCast customCast, out RaycastHit hitData, Vector3 origin, float radius, Vector3 direction, float maxDistance)
        {
            Vector3 halfHeight = Vector3.up * HalfHeight;
            return customCast.CustomCapsuleCast(origin + halfHeight, origin - halfHeight, UseRadiusOverride ? RadiusOverride : radius, direction, out hitData, maxDistance, false);
        }

        /// <inheritdoc />
        public override ArraySegment<RaycastHit> ConvertFromSphereCastAll(PhysicsCast customCast, Vector3 origin, float radius, Vector3 direction, float maxDistance)
        {
            Vector3 halfHeight = Vector3.up * HalfHeight;
            return customCast.CustomCapsuleCastAll(origin + halfHeight, origin - halfHeight, UseRadiusOverride ? RadiusOverride : radius, direction, maxDistance, false);
        }
    }
}