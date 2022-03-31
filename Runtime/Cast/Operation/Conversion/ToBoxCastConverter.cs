namespace Zinnia.Cast.Operation.Conversion
{
    using System;
    using UnityEngine;

    /// <summary>
    /// Converts any supported <see cref="Physics"/> caster to a <see cref="Physics.BoxCast"/>.
    /// </summary>
    public class ToBoxCastConverter : CastConverter
    {
        /// <summary>
        /// The half extends for the <see cref="Physics.BoxCast"/>.
        /// </summary>
        [Tooltip("The half extends for the Physics.BoxCast.")]
        [SerializeField]
        private Vector3 _halfExtentsOverride;
        public Vector3 HalfExtentsOverride
        {
            get
            {
                return _halfExtentsOverride;
            }
            set
            {
                _halfExtentsOverride = value;
            }
        }
        /// <summary>
        /// Whether to use the <see cref="HalfExtentsOverride"/> value if the source caster already supports a half extends. If the source caster does not support a half extends then the <see cref="HalfExtentsOverride"/> will always be used.
        /// </summary>
        [Tooltip("Whether to use the HalfExtentsOverride value if the source caster already supports a half extends. If the source caster does not support a half extends then the HalfExtentsOverride will always be used.")]
        [SerializeField]
        private bool _useHalfExtentsOverride = true;
        public bool UseHalfExtentsOverride
        {
            get
            {
                return _useHalfExtentsOverride;
            }
            set
            {
                _useHalfExtentsOverride = value;
            }
        }
        /// <summary>
        /// The orientation for the <see cref="Physics.BoxCast"/>.
        /// </summary>
        [Tooltip("The orientation for the Physics.BoxCast.")]
        [SerializeField]
        private Vector3 _orientationOverride;
        public Vector3 OrientationOverride
        {
            get
            {
                return _orientationOverride;
            }
            set
            {
                _orientationOverride = value;
            }
        }
        /// <summary>
        /// Whether to use the <see cref="OrientationOverride"/> value if the source caster already supports a half extends. If the source caster does not support a half extends then the <see cref="OrientationOverride"/> will always be used.
        /// </summary>
        [Tooltip("Whether to use the OrientationOverride value if the source caster already supports a half extends. If the source caster does not support a half extends then the OrientationOverride will always be used.")]
        [SerializeField]
        private bool _useOrientationOverride = true;
        public bool UseOrientationOverride
        {
            get
            {
                return _useOrientationOverride;
            }
            set
            {
                _useOrientationOverride = value;
            }
        }

        /// <inheritdoc />
        public override bool ConvertFromBoxCast(PhysicsCast customCast, out RaycastHit hitData, Vector3 center, Vector3 halfExtents, Vector3 direction, Quaternion orientation, float maxDistance)
        {
            return customCast.CustomBoxCast(center, UseHalfExtentsOverride ? HalfExtentsOverride : halfExtents, direction, out hitData, UseOrientationOverride ? Quaternion.Euler(OrientationOverride) : orientation, maxDistance, false);
        }

        /// <inheritdoc />
        public override ArraySegment<RaycastHit> ConvertFromBoxCastAll(PhysicsCast customCast, Vector3 center, Vector3 halfExtents, Vector3 direction, Quaternion orientation, float maxDistance)
        {
            return customCast.CustomBoxCastAll(center, UseHalfExtentsOverride ? HalfExtentsOverride : halfExtents, direction, UseOrientationOverride ? Quaternion.Euler(OrientationOverride) : orientation, maxDistance, false);
        }

        /// <inheritdoc />
        public override bool ConvertFromCapsuleCast(PhysicsCast customCast, out RaycastHit hitData, Vector3 point1, Vector3 point2, float radius, Vector3 direction, float maxDistance)
        {
            Vector3 origin = GetCenterVector(point1, point2);
            return customCast.CustomBoxCast(origin, HalfExtentsOverride, direction, out hitData, Quaternion.Euler(OrientationOverride), maxDistance, false);
        }

        /// <inheritdoc />
        public override ArraySegment<RaycastHit> ConvertFromCapsuleCastAll(PhysicsCast customCast, Vector3 point1, Vector3 point2, float radius, Vector3 direction, float maxDistance)
        {
            Vector3 origin = GetCenterVector(point1, point2);
            return customCast.CustomBoxCastAll(origin, HalfExtentsOverride, direction, Quaternion.Euler(OrientationOverride), maxDistance, false);
        }

        /// <inheritdoc />
        public override bool ConvertFromLinecast(PhysicsCast customCast, out RaycastHit hitData, Vector3 startPosition, Vector3 endPosition)
        {
            Vector3 direction = GetDirectionVector(startPosition, endPosition, out float distance);
            return customCast.CustomBoxCast(startPosition, HalfExtentsOverride, direction, out hitData, Quaternion.Euler(OrientationOverride), distance, false);
        }

        /// <inheritdoc />
        public override bool ConvertFromRaycast(PhysicsCast customCast, out RaycastHit hitData, Ray ray, float maxDistance)
        {
            return customCast.CustomBoxCast(ray.origin, HalfExtentsOverride, ray.direction, out hitData, Quaternion.Euler(OrientationOverride), maxDistance, false);
        }

        /// <inheritdoc />
        public override ArraySegment<RaycastHit> ConvertFromRaycastAll(PhysicsCast customCast, Ray ray, float maxDistance)
        {
            return customCast.CustomBoxCastAll(ray.origin, HalfExtentsOverride, ray.direction, Quaternion.Euler(OrientationOverride), maxDistance, false);
        }

        /// <inheritdoc />
        public override bool ConvertFromSphereCast(PhysicsCast customCast, out RaycastHit hitData, Vector3 origin, float radius, Vector3 direction, float maxDistance)
        {
            return customCast.CustomBoxCast(origin, HalfExtentsOverride, direction, out hitData, Quaternion.Euler(OrientationOverride), maxDistance, false);
        }

        /// <inheritdoc />
        public override ArraySegment<RaycastHit> ConvertFromSphereCastAll(PhysicsCast customCast, Vector3 origin, float radius, Vector3 direction, float maxDistance)
        {
            return customCast.CustomBoxCastAll(origin, HalfExtentsOverride, direction, Quaternion.Euler(OrientationOverride), maxDistance, false);
        }
    }
}
