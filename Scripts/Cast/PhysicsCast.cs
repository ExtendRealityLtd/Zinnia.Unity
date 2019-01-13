namespace Zinnia.Cast
{
    using UnityEngine;

    /// <summary>
    /// Allows customizing of Unity Physics casting within other scripts by applying settings at edit time.
    /// </summary>
    public class PhysicsCast : MonoBehaviour
    {
        /// <summary>
        /// The layers to ignore when casting.
        /// </summary>
        public LayerMask layersToIgnore = Physics.IgnoreRaycastLayer;
        /// <summary>
        /// Determines whether the cast will interact with trigger colliders.
        /// </summary>
        public QueryTriggerInteraction triggerInteraction = QueryTriggerInteraction.UseGlobal;

        /// <summary>
        /// Generates a Raycast either from the given <see cref="PhysicsCast"/> object or a default <see cref="Physics.Raycast"/>.
        /// </summary>
        /// <param name="customCast">The optional <see cref="PhysicsCast"/> with customized cast parameters.</param>
        /// <param name="ray">The <see cref="Ray"/> to cast with.</param>
        /// <param name="hitData">The <see cref="RaycastHit"/> data.</param>
        /// <param name="length">The maximum length of the <see cref="Ray"/>.</param>
        /// <param name="ignoreLayers">A <see cref="LayerMask"/> of layers to ignore from the <see cref="Ray"/>.</param>
        /// <param name="affectTriggers">Determines the trigger interaction level of the <see cref="Ray"/>.</param>
        /// <returns><see langword="true"/> if the <see cref="Ray"/> successfully collides with a valid <see cref="GameObject"/>.</returns>
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
        /// Generates a RaycastAll either from the given <see cref="PhysicsCast"/> object or a default <see cref="Physics.Raycast"/>.
        /// </summary>
        /// <param name="customCast">The optional <see cref="PhysicsCast"/> with customized cast parameters.</param>
        /// <param name="ray">The <see cref="Ray"/> to cast with.</param>
        /// <param name="length">The maximum length of the <see cref="Ray"/>.</param>
        /// <param name="ignoreLayers">A <see cref="LayerMask"/> of layers to ignore from the <see cref="Ray"/>.</param>
        /// <param name="affectTriggers">Determines the trigger interaction level of the <see cref="Ray"/>.</param>
        /// <returns>A collection of collisions determined by the cast.</returns>
        public static RaycastHit[] RaycastAll(PhysicsCast customCast, Ray ray, float length, LayerMask ignoreLayers, QueryTriggerInteraction affectTriggers = QueryTriggerInteraction.UseGlobal)
        {
            if (customCast != null)
            {
                return customCast.CustomRaycastAll(ray, length);
            }
            else
            {
                return Physics.RaycastAll(ray, length, ~ignoreLayers, affectTriggers);
            }
        }

        /// <summary>
        /// Generates a Linecast either from the given <see cref="PhysicsCast"/> object or a default <see cref="Physics.Linecast"/>.
        /// </summary>
        /// <param name="customCast">The optional <see cref="PhysicsCast"/> with customized cast parameters.</param>
        /// <param name="startPosition">The world position to start the Linecast from.</param>
        /// <param name="endPosition">The world position to end the Linecast at.</param>
        /// <param name="hitData">The <see cref="RaycastHit"/> data.</param>
        /// <param name="ignoreLayers">A <see cref="LayerMask"/> of layers to ignore from the Linecast.</param>
        /// <param name="affectTriggers">Determines the trigger interaction level of the cast.</param>
        /// <returns><see langword="true"/> if the Linecast successfully collides with a valid <see cref="GameObject"/>.</returns>
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
        /// Generates a SphereCast either from the given <see cref="PhysicsCast"/> object or a default <see cref="Physics.SphereCast"/>.
        /// </summary>
        /// <param name="customCast">The optional <see cref="PhysicsCast"/> with customized cast parameters.</param>
        /// <param name="origin">The origin point of the sphere to cast.</param>
        /// <param name="radius">The radius of the sphere.</param>
        /// <param name="direction">The direction into which to sweep the sphere.</param>
        /// <param name="hitData">The <see cref="RaycastHit"/> data.</param>
        /// <param name="maxDistance">The max length of the sweep.</param>
        /// <param name="ignoreLayers">A <see cref="LayerMask"/> of layers to ignore from the SphereCast.</param>
        /// <param name="affectTriggers">Determines the trigger interaction level of the cast.</param>
        /// <returns><see langword="true"/> if the SphereCast successfully collides with a valid <see cref="GameObject"/>.</returns>
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
        /// Generates a SphereCastAll either from the given <see cref="PhysicsCast"/> object or a default <see cref="Physics.SphereCast"/>.
        /// </summary>
        /// <param name="customCast">The optional <see cref="PhysicsCast"/> with customized cast parameters.</param>
        /// <param name="origin">The origin point of the sphere to cast.</param>
        /// <param name="radius">The radius of the sphere.</param>
        /// <param name="direction">The direction into which to sweep the sphere.</param>
        /// <param name="maxDistance">The max length of the sweep.</param>
        /// <param name="ignoreLayers">A <see cref="LayerMask"/> of layers to ignore from the SphereCast.</param>
        /// <param name="affectTriggers">Determines the trigger interaction level of the cast.</param>
        /// <returns>A collection of collisions determined by the cast.</returns>
        public static RaycastHit[] SphereCastAll(PhysicsCast customCast, Vector3 origin, float radius, Vector3 direction, float maxDistance, LayerMask ignoreLayers, QueryTriggerInteraction affectTriggers = QueryTriggerInteraction.UseGlobal)
        {
            if (customCast != null)
            {
                return customCast.CustomSphereCastAll(origin, radius, direction, maxDistance);
            }
            else
            {
                return Physics.SphereCastAll(origin, radius, direction, maxDistance, ~ignoreLayers, affectTriggers);
            }
        }

        /// <summary>
        /// Generates a CapsuleCast either from the given <see cref="PhysicsCast"/> object or a default <see cref="Physics.CapsuleCast"/>.
        /// </summary>
        /// <param name="customCast">The optional <see cref="PhysicsCast"/> with customized cast parameters.</param>
        /// <param name="point1">The center of the sphere at the start of the capsule.</param>
        /// <param name="point2">The center of the sphere at the end of the capsule.</param>
        /// <param name="radius">The radius of the capsule.</param>
        /// <param name="direction">The direction into which to sweep the capsule.</param>
        /// <param name="hitData">The <see cref="RaycastHit"/> data.</param>
        /// <param name="maxDistance">The max length of the sweep.</param>
        /// <param name="ignoreLayers">A <see cref="LayerMask"/> of layers to ignore from the CapsuleCast.</param>
        /// <param name="affectTriggers">Determines the trigger interaction level of the cast.</param>
        /// <returns><see langword="true"/> if the CapsuleCast successfully collides with a valid <see cref="GameObject"/>.</returns>
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
        /// Generates a CapsuleCastAll either from the given <see cref="PhysicsCast"/> object or a default <see cref="Physics.CapsuleCast"/>.
        /// </summary>
        /// <param name="customCast">The optional <see cref="PhysicsCast"/> with customized cast parameters.</param>
        /// <param name="point1">The center of the sphere at the start of the capsule.</param>
        /// <param name="point2">The center of the sphere at the end of the capsule.</param>
        /// <param name="radius">The radius of the capsule.</param>
        /// <param name="direction">The direction into which to sweep the capsule.</param>
        /// <param name="maxDistance">The max length of the sweep.</param>
        /// <param name="ignoreLayers">A <see cref="LayerMask"/> of layers to ignore from the CapsuleCast.</param>
        /// <param name="affectTriggers">Determines the trigger interaction level of the cast.</param>
        /// <returns>A collection of collisions determined by the cast.</returns>
        public static RaycastHit[] CapsuleCastAll(PhysicsCast customCast, Vector3 point1, Vector3 point2, float radius, Vector3 direction, float maxDistance, LayerMask ignoreLayers, QueryTriggerInteraction affectTriggers = QueryTriggerInteraction.UseGlobal)
        {
            if (customCast != null)
            {
                return customCast.CustomCapsuleCastAll(point1, point2, radius, direction, maxDistance);
            }
            else
            {
                return Physics.CapsuleCastAll(point1, point2, radius, direction, maxDistance, ~ignoreLayers, affectTriggers);
            }
        }

        /// <summary>
        /// Generates a BoxCast either from the given <see cref="PhysicsCast"/> object or a default <see cref="Physics.BoxCast"/>.
        /// </summary>
        /// <param name="customCast">The optional <see cref="PhysicsCast"/> with customized cast parameters.</param>
        /// <param name="center">The center of the box.</param>
        /// <param name="halfExtents">Half the size of the box in each dimension.</param>
        /// <param name="direction">The direction in which to cast the box.</param>
        /// <param name="hitData">The <see cref="RaycastHit"/> data.</param>
        /// <param name="orientation">The rotation of the box.</param>
        /// <param name="maxDistance">The max length of the cast.</param>
        /// <param name="ignoreLayers">A <see cref="LayerMask"/> of layers to ignore from the BoxCast.</param>
        /// <param name="affectTriggers">Determines the trigger interaction level of the cast.</param>
        /// <returns><see langword="true"/> if the BoxCast successfully collides with a valid <see cref="GameObject"/>.</returns>
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
        /// Generates a BoxCastAll either from the given <see cref="PhysicsCast"/> object or a default <see cref="Physics.BoxCast"/>.
        /// </summary>
        /// <param name="customCast">The optional <see cref="PhysicsCast"/> with customized cast parameters.</param>
        /// <param name="center">The center of the box.</param>
        /// <param name="halfExtents">Half the size of the box in each dimension.</param>
        /// <param name="direction">The direction in which to cast the box.</param>
        /// <param name="orientation">The rotation of the box.</param>
        /// <param name="maxDistance">The max length of the cast.</param>
        /// <param name="ignoreLayers">A <see cref="LayerMask"/> of layers to ignore from the BoxCast.</param>
        /// <param name="affectTriggers">Determines the trigger interaction level of the cast.</param>
        /// <returns>A collection of collisions determined by the cast.</returns>
        public static RaycastHit[] BoxCastAll(PhysicsCast customCast, Vector3 center, Vector3 halfExtents, Vector3 direction, Quaternion orientation, float maxDistance, LayerMask ignoreLayers, QueryTriggerInteraction affectTriggers = QueryTriggerInteraction.UseGlobal)
        {
            if (customCast != null)
            {
                return customCast.CustomBoxCastAll(center, halfExtents, direction, orientation, maxDistance);
            }
            else
            {
                return Physics.BoxCastAll(center, halfExtents, direction, orientation, maxDistance, ~ignoreLayers, affectTriggers);
            }
        }

        /// <summary>
        /// Generates a <see cref="Physics.Raycast"/> based on the options defined in the <see cref="PhysicsCast"/> object.
        /// </summary>
        /// <param name="ray">The <see cref="Ray"/> to cast with.</param>
        /// <param name="hitData">The <see cref="RaycastHit"/> data.</param>
        /// <param name="length">The maximum length of the <see cref="Ray"/>.</param>
        /// <returns><see langword="true"/> if the <see cref="Physics.Raycast"/> successfully collides with a valid <see cref="GameObject"/>.</returns>
        public virtual bool CustomRaycast(Ray ray, out RaycastHit hitData, float length)
        {
            return Physics.Raycast(ray, out hitData, length, ~layersToIgnore, triggerInteraction);
        }

        /// <summary>
        /// Generates a <see cref="Physics.RaycastAll"/> based on the options defined in the <see cref="PhysicsCast"/> object.
        /// </summary>
        /// <param name="ray">The <see cref="Ray"/> to cast with.</param>
        /// <param name="length">The maximum length of the <see cref="Ray"/>.</param>
        /// <returns>A collection of collisions determined by the cast.</returns>
        public virtual RaycastHit[] CustomRaycastAll(Ray ray, float length)
        {
            return Physics.RaycastAll(ray, length, ~layersToIgnore, triggerInteraction);
        }

        /// <summary>
        /// Generates a <see cref="Physics.Linecast"/> based on the options defined in the <see cref="PhysicsCast"/> object.
        /// </summary>
        /// <param name="startPosition">The world position to start the <see cref="Physics.Linecast"/> from.</param>
        /// <param name="endPosition">The world position to end the <see cref="Physics.Linecast"/> at.</param>
        /// <param name="hitData">The <see cref="RaycastHit"/> data.</param>
        /// <returns><see langword="true"/> if the <see cref="Physics.Linecast"/> successfully collides with a valid <see cref="GameObject"/>.</returns>
        public virtual bool CustomLinecast(Vector3 startPosition, Vector3 endPosition, out RaycastHit hitData)
        {
            return Physics.Linecast(startPosition, endPosition, out hitData, ~layersToIgnore, triggerInteraction);
        }

        /// <summary>
        /// Generates a <see cref="Physics.SphereCast"/> based on the options defined in the <see cref="PhysicsCast"/> object.
        /// </summary>
        /// <param name="origin">The origin point of the <see cref="Physics.SphereCast"/>.</param>
        /// <param name="radius">The radius of the <see cref="Physics.SphereCast"/>.</param>
        /// <param name="direction">The direction into which to sweep the <see cref="Physics.SphereCast"/>.</param>
        /// <param name="hitData">The <see cref="RaycastHit"/> data.</param>
        /// <param name="maxDistance">The max length of the sweep.</param>
        /// <returns><see langword="true"/> if the <see cref="Physics.SphereCast"/> successfully collides with a valid <see cref="GameObject"/>.</returns>
        public virtual bool CustomSphereCast(Vector3 origin, float radius, Vector3 direction, out RaycastHit hitData, float maxDistance)
        {
            return Physics.SphereCast(origin, radius, direction, out hitData, maxDistance, ~layersToIgnore, triggerInteraction);
        }

        /// <summary>
        /// Generates a <see cref="Physics.SphereCastAll"/> based on the options defined in the <see cref="PhysicsCast"/> object.
        /// </summary>
        /// <param name="origin">The origin point of the <see cref="Physics.SphereCast"/>.</param>
        /// <param name="radius">The radius of the <see cref="Physics.SphereCast"/>.</param>
        /// <param name="direction">The direction into which to sweep the <see cref="Physics.SphereCast"/>.</param>
        /// <param name="maxDistance">The max length of the sweep.</param>
        /// <returns>A collection of collisions determined by the cast.</returns>
        public virtual RaycastHit[] CustomSphereCastAll(Vector3 origin, float radius, Vector3 direction, float maxDistance)
        {
            return Physics.SphereCastAll(origin, radius, direction, maxDistance, ~layersToIgnore, triggerInteraction);
        }

        /// <summary>
        /// Generates a <see cref="Physics.CapsuleCast"/> based on the options defined in the <see cref="PhysicsCast"/> object.
        /// </summary>
        /// <param name="point1">The center of the sphere at the start of the <see cref="Physics.CapsuleCast"/>.</param>
        /// <param name="point2">The center of the sphere at the end of the <see cref="Physics.CapsuleCast"/>.</param>
        /// <param name="radius">The radius of the <see cref="Physics.CapsuleCast"/>.</param>
        /// <param name="direction">The direction into which to sweep the <see cref="Physics.CapsuleCast"/>.</param>
        /// <param name="hitData">The <see cref="RaycastHit"/> data.</param>
        /// <param name="maxDistance">The max length of the sweep.</param>
        /// <returns><see langword="true"/> if the <see cref="Physics.CapsuleCast"/> successfully collides with a valid <see cref="GameObject"/>.</returns>
        public virtual bool CustomCapsuleCast(Vector3 point1, Vector3 point2, float radius, Vector3 direction, out RaycastHit hitData, float maxDistance)
        {
            return Physics.CapsuleCast(point1, point2, radius, direction, out hitData, maxDistance, ~layersToIgnore, triggerInteraction);
        }

        /// <summary>
        /// Generates a <see cref="Physics.CapsuleCastAll"/> based on the options defined in the <see cref="PhysicsCast"/> object.
        /// </summary>
        /// <param name="point1">The center of the sphere at the start of the <see cref="Physics.CapsuleCast"/>.</param>
        /// <param name="point2">The center of the sphere at the end of the <see cref="Physics.CapsuleCast"/>.</param>
        /// <param name="radius">The radius of the <see cref="Physics.CapsuleCast"/>.</param>
        /// <param name="direction">The direction into which to sweep the <see cref="Physics.CapsuleCast"/>.</param>
        /// <param name="maxDistance">The max length of the sweep.</param>
        /// <returns>A collection of collisions determined by the cast.</returns>
        public virtual RaycastHit[] CustomCapsuleCastAll(Vector3 point1, Vector3 point2, float radius, Vector3 direction, float maxDistance)
        {
            return Physics.CapsuleCastAll(point1, point2, radius, direction, maxDistance, ~layersToIgnore, triggerInteraction);
        }

        /// <summary>
        /// Generates a <see cref="Physics.BoxCast"/> based on the options defined in the <see cref="PhysicsCast"/> object.
        /// </summary>
        /// <param name="center">The center of the <see cref="Physics.BoxCast"/>.</param>
        /// <param name="halfExtents">Half the size of the <see cref="Physics.BoxCast"/> in each dimension.</param>
        /// <param name="direction">The direction in which to cast the <see cref="Physics.BoxCast"/>.</param>
        /// <param name="hitData">The <see cref="RaycastHit"/> data.</param>
        /// <param name="orientation">The rotation of the <see cref="Physics.BoxCast"/>.</param>
        /// <param name="maxDistance">The max length of the <see cref="Physics.BoxCast"/>.</param>
        /// <returns><see langword="true"/> if the <see cref="Physics.BoxCast"/> successfully collides with a valid <see cref="GameObject"/>.</returns>
        public virtual bool CustomBoxCast(Vector3 center, Vector3 halfExtents, Vector3 direction, out RaycastHit hitData, Quaternion orientation, float maxDistance)
        {
            return Physics.BoxCast(center, halfExtents, direction, out hitData, orientation, maxDistance, ~layersToIgnore, triggerInteraction);
        }

        /// <summary>
        /// Generates a <see cref="Physics.BoxCastAll"/> based on the options defined in the <see cref="PhysicsCast"/> object.
        /// </summary>
        /// <param name="center">The center of the <see cref="Physics.BoxCast"/>.</param>
        /// <param name="halfExtents">Half the size of the <see cref="Physics.BoxCast"/> in each dimension.</param>
        /// <param name="direction">The direction in which to cast the <see cref="Physics.BoxCast"/>.</param>
        /// <param name="orientation">The rotation of the <see cref="Physics.BoxCast"/>.</param>
        /// <param name="maxDistance">The max length of the <see cref="Physics.BoxCast"/>.</param>
        /// <returns>A collection of collisions determined by the cast.</returns>
        public virtual RaycastHit[] CustomBoxCastAll(Vector3 center, Vector3 halfExtents, Vector3 direction, Quaternion orientation, float maxDistance)
        {
            return Physics.BoxCastAll(center, halfExtents, direction, orientation, maxDistance, ~layersToIgnore, triggerInteraction);
        }
    }
}