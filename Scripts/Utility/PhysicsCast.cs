namespace VRTK.Core.Utility
{
    using UnityEngine;

    /// <summary>
    /// The PhysicsCast allows customising of Unity Physics casting within other scripts by applying settings at edit time.
    /// </summary>
    public class PhysicsCast : MonoBehaviour
    {
        [Tooltip("The layers to ignore when casting.")]
        public LayerMask layersToIgnore = Physics.IgnoreRaycastLayer;
        [Tooltip("Determines whether the cast will interact with trigger colliders.")]
        public QueryTriggerInteraction triggerInteraction = QueryTriggerInteraction.UseGlobal;

        /// <summary>
        /// The Raycast method is used to generate a Raycast either from the given PhysicsCast object or a default Physics.Raycast.
        /// </summary>
        /// <param name="customCast">The optional object with customised cast parameters.</param>
        /// <param name="ray">The Ray to cast with.</param>
        /// <param name="hitData">The Raycast hit data.</param>
        /// <param name="length">The maximum length of the Raycast.</param>
        /// <param name="ignoreLayers">A layermask of layers to ignore from the Raycast.</param>
        /// <param name="affectTriggers">Determines the trigger interaction level of the cast.</param>
        /// <returns>Returns `true` if the Raycast successfully collides with a valid object.</returns>
        public static bool Raycast(PhysicsCast customCast, Ray ray, out RaycastHit hitData, float length, LayerMask ignoreLayers, QueryTriggerInteraction affectTriggers = QueryTriggerInteraction.UseGlobal)
        {
            if (customCast != null)
            {
                return customCast.CustomRaycast(ray, out hitData, length);
            }
            else
            {
                return Physics.Raycast(ray, out hitData, length, ~ignoreLayers, affectTriggers);
            }
        }

        /// <summary>
        /// The Linecast method is used to generate a Linecast either from the given PhysicsCast object or a default Physics.Linecast.
        /// </summary>
        /// <param name="customCast">The optional object with customised cast parameters.</param>
        /// <param name="startPosition">The world position to start the Linecast from.</param>
        /// <param name="endPosition">The world position to end the Linecast at.</param>
        /// <param name="hitData">The Linecast hit data.</param>
        /// <param name="ignoreLayers">A layermask of layers to ignore from the Linecast.</param>
        /// <param name="affectTriggers">Determines the trigger interaction level of the cast.</param>
        /// <returns>Returns `true` if the Linecast successfully collides with a valid object.</returns>
        public static bool Linecast(PhysicsCast customCast, Vector3 startPosition, Vector3 endPosition, out RaycastHit hitData, LayerMask ignoreLayers, QueryTriggerInteraction affectTriggers = QueryTriggerInteraction.UseGlobal)
        {
            if (customCast != null)
            {
                return customCast.CustomLinecast(startPosition, endPosition, out hitData);
            }
            else
            {
                return Physics.Linecast(startPosition, endPosition, out hitData, ~ignoreLayers, affectTriggers);
            }
        }

        /// <summary>
        /// The SphereCast method is used to generate a SphereCast either from the given PhysicsCast object or a default Physics.SphereCast.
        /// </summary>
        /// <param name="customCast">The optional object with customised cast parameters.</param>
        /// <param name="origin">The origin point of the sphere to cast.</param>
        /// <param name="radius">The radius of the sphere.</param>
        /// <param name="direction">The direction into which to sweep the sphere.</param>
        /// <param name="hitData">The SphereCast hit data.</param>
        /// <param name="maxDistance">The max length of the sweep.</param>
        /// <param name="ignoreLayers">A layermask of layers to ignore from the SphereCast.</param>
        /// <param name="affectTriggers">Determines the trigger interaction level of the cast.</param>
        /// <returns>Returns `true` if the SphereCast successfully collides with a valid object.</returns>
        public static bool SphereCast(PhysicsCast customCast, Vector3 origin, float radius, Vector3 direction, out RaycastHit hitData, float maxDistance, LayerMask ignoreLayers, QueryTriggerInteraction affectTriggers = QueryTriggerInteraction.UseGlobal)
        {
            if (customCast != null)
            {
                return customCast.CustomSphereCast(origin, radius, direction, out hitData, maxDistance);
            }
            else
            {
                return Physics.SphereCast(origin, radius, direction, out hitData, maxDistance, ~ignoreLayers, affectTriggers);
            }
        }

        /// <summary>
        /// The CapsuleCast method is used to generate a CapsuleCast either from the given PhysicsCast object or a default Physics.CapsuleCast.
        /// </summary>
        /// <param name="customCast">The optional object with customised cast parameters.</param>
        /// <param name="point1">The center of the sphere at the start of the capsule.</param>
        /// <param name="point2">The center of the sphere at the end of the capsule.</param>
        /// <param name="radius">The radius of the capsule.</param>
        /// <param name="direction">The direction into which to sweep the capsule.</param>
        /// <param name="hitData">The CapsuleCast hit data.</param>
        /// <param name="maxDistance">The max length of the sweep.</param>
        /// <param name="ignoreLayers">A layermask of layers to ignore from the CapsuleCast.</param>
        /// <param name="affectTriggers">Determines the trigger interaction level of the cast.</param>
        /// <returns>Returns `true` if the CapsuleCast successfully collides with a valid object.</returns>
        public static bool CapsuleCast(PhysicsCast customCast, Vector3 point1, Vector3 point2, float radius, Vector3 direction, out RaycastHit hitData, float maxDistance, LayerMask ignoreLayers, QueryTriggerInteraction affectTriggers = QueryTriggerInteraction.UseGlobal)
        {
            if (customCast != null)
            {
                return customCast.CustomCapsuleCast(point1, point2, radius, direction, out hitData, maxDistance);
            }
            else
            {
                return Physics.CapsuleCast(point1, point2, radius, direction, out hitData, maxDistance, ~ignoreLayers, affectTriggers);
            }
        }

        /// <summary>
        /// The BoxCast method is used to generate a BoxCast either from the given PhysicsCast object or a default Physics.BoxCast.
        /// </summary>
        /// <param name="customCast">The optional object with customised cast parameters.</param>
        /// <param name="center">The center of the box.</param>
        /// <param name="halfExtents">Half the size of the box in each dimension.</param>
        /// <param name="direction">The direction in which to cast the box.</param>
        /// <param name="hitData">The BoxCast hit data.</param>
        /// <param name="orientation">The rotation of the box.</param>
        /// <param name="maxDistance">The max length of the cast.</param>
        /// <param name="ignoreLayers">A layermask of layers to ignore from the BoxCast.</param>
        /// <param name="affectTriggers">Determines the trigger interaction level of the cast.</param>
        /// <returns>Returns `true` if the BoxCast successfully collides with a valid object.</returns>
        public static bool BoxCast(PhysicsCast customCast, Vector3 center, Vector3 halfExtents, Vector3 direction, out RaycastHit hitData, Quaternion orientation, float maxDistance, LayerMask ignoreLayers, QueryTriggerInteraction affectTriggers = QueryTriggerInteraction.UseGlobal)
        {
            if (customCast != null)
            {
                return customCast.CustomBoxCast(center, halfExtents, direction, out hitData, orientation, maxDistance);
            }
            else
            {
                return Physics.BoxCast(center, halfExtents, direction, out hitData, orientation, maxDistance, ~ignoreLayers, affectTriggers);
            }
        }

        /// <summary>
        /// The CustomRaycast method is used to generate a Raycast based on the options defined in the PhysicsCast object.
        /// </summary>
        /// <param name="ray">The Ray to cast with.</param>
        /// <param name="hitData">The Raycast hit data.</param>
        /// <param name="length">The maximum length of the Raycast.</param>
        /// <returns>Returns `true` if the Raycast successfully collides with a valid object.</returns>
        public virtual bool CustomRaycast(Ray ray, out RaycastHit hitData, float length)
        {
            return Physics.Raycast(ray, out hitData, length, ~layersToIgnore, triggerInteraction);
        }

        /// <summary>
        /// The CustomLinecast method is used to generate a Linecast based on the options defined in the PhysicsCast object.
        /// </summary>
        /// <param name="startPosition">The world position to start the Linecast from.</param>
        /// <param name="endPosition">The world position to end the Linecast at.</param>
        /// <param name="hitData">The Linecast hit data.</param>
        /// <returns>Returns `true` if the Linecast successfully collides with a valid object.</returns>
        public virtual bool CustomLinecast(Vector3 startPosition, Vector3 endPosition, out RaycastHit hitData)
        {
            return Physics.Linecast(startPosition, endPosition, out hitData, ~layersToIgnore, triggerInteraction);
        }

        /// <summary>
        /// The CustomSphereCast method is used to generate a SphereCast based on the options defined in the PhysicsCast object.
        /// </summary>
        /// <param name="origin">The origin point of the sphere to cast.</param>
        /// <param name="radius">The radius of the sphere.</param>
        /// <param name="direction">The direction into which to sweep the sphere.</param>
        /// <param name="hitData">The SphereCast hit data.</param>
        /// <param name="maxDistance">The max length of the sweep.</param>
        /// <returns>Returns `true` if the SphereCast successfully collides with a valid object.</returns>
        public virtual bool CustomSphereCast(Vector3 origin, float radius, Vector3 direction, out RaycastHit hitData, float maxDistance)
        {
            return Physics.SphereCast(origin, radius, direction, out hitData, maxDistance, ~layersToIgnore, triggerInteraction);
        }

        /// <summary>
        /// The CustomCapsuleCast method is used to generate a CapsuleCast based on the options defined in the PhysicsCast object.
        /// </summary>
        /// <param name="point1">The center of the sphere at the start of the capsule.</param>
        /// <param name="point2">The center of the sphere at the end of the capsule.</param>
        /// <param name="radius">The radius of the capsule.</param>
        /// <param name="direction">The direction into which to sweep the capsule.</param>
        /// <param name="hitData">The CapsuleCast hit data.</param>
        /// <param name="maxDistance">The max length of the sweep.</param>
        /// <returns>Returns `true` if the CapsuleCast successfully collides with a valid object.</returns>
        public virtual bool CustomCapsuleCast(Vector3 point1, Vector3 point2, float radius, Vector3 direction, out RaycastHit hitData, float maxDistance)
        {
            return Physics.CapsuleCast(point1, point2, radius, direction, out hitData, maxDistance, ~layersToIgnore, triggerInteraction);
        }

        /// <summary>
        /// The CustomBoxCast method is used to generate a BoxCast based on the options defined in the PhysicsCast object.
        /// </summary>
        /// <param name="center">The center of the box.</param>
        /// <param name="halfExtents">Half the size of the box in each dimension.</param>
        /// <param name="direction">The direction in which to cast the box.</param>
        /// <param name="hitData">The BoxCast hit data.</param>
        /// <param name="orientation">The rotation of the box.</param>
        /// <param name="maxDistance">The max length of the cast.</param>
        /// <returns>Returns `true` if the box successfully collides with a valid object.</returns>
        public virtual bool CustomBoxCast(Vector3 center, Vector3 halfExtents, Vector3 direction, out RaycastHit hitData, Quaternion orientation, float maxDistance)
        {
            return Physics.BoxCast(center, halfExtents, direction, out hitData, orientation, maxDistance, ~layersToIgnore, triggerInteraction);
        }
    }
}