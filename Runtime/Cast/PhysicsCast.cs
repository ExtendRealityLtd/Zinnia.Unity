namespace Zinnia.Cast
{
    using UnityEngine;
    using System;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;

    /// <summary>
    /// Allows customizing of Unity Physics casting within other scripts by applying settings at edit time.
    /// </summary>
    public class PhysicsCast : MonoBehaviour
    {
        /// <summary>
        /// A reusable array of <see cref="RaycastHit"/>s to use with non-allocating <see cref="Physics"/> API.
        /// </summary>
        protected static readonly RaycastHit[] Hits = new RaycastHit[1000];
        /// <summary>
        /// A reusable array of <see cref="Collider"/>s to use with non-allocating <see cref="Physics"/> API.
        /// </summary>
        protected static readonly Collider[] Colliders = new Collider[1000];

        /// <summary>
        /// The layers to ignore when casting.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public LayerMask LayersToIgnore { get; set; } = Physics.IgnoreRaycastLayer;
        /// <summary>
        /// Determines whether the cast will interact with trigger colliders.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public QueryTriggerInteraction TriggerInteraction { get; set; } = QueryTriggerInteraction.UseGlobal;

        /// <summary>
        /// Generates a Raycast either from the given <see cref="PhysicsCast"/> object or a default <see cref="Physics.Raycast(Ray,out RaycastHit,float,int,QueryTriggerInteraction)"/>.
        /// </summary>
        /// <param name="customCast">The optional <see cref="PhysicsCast"/> with customized cast parameters.</param>
        /// <param name="ray">The <see cref="Ray"/> to cast with.</param>
        /// <param name="hitData">The <see cref="RaycastHit"/> data.</param>
        /// <param name="length">The maximum length of the <see cref="Ray"/>.</param>
        /// <param name="ignoreLayers">A <see cref="LayerMask"/> of layers to ignore from the <see cref="Ray"/>.</param>
        /// <param name="affectTriggers">Determines the trigger interaction level of the <see cref="Ray"/>.</param>
        /// <returns>Whether the <see cref="Ray"/> successfully collides with a valid <see cref="GameObject"/>.</returns>
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
        /// Generates a RaycastAll either from the given <see cref="PhysicsCast"/> object or a default <see cref="Physics.RaycastAll(Ray,float,int,QueryTriggerInteraction)"/>.
        /// </summary>
        /// <param name="customCast">The optional <see cref="PhysicsCast"/> with customized cast parameters.</param>
        /// <param name="ray">The <see cref="Ray"/> to cast with.</param>
        /// <param name="length">The maximum length of the <see cref="Ray"/>.</param>
        /// <param name="ignoreLayers">A <see cref="LayerMask"/> of layers to ignore from the <see cref="Ray"/>.</param>
        /// <param name="affectTriggers">Determines the trigger interaction level of the <see cref="Ray"/>.</param>
        /// <returns>A collection of collisions determined by the cast.</returns>
        public static ArraySegment<RaycastHit> RaycastAll(PhysicsCast customCast, Ray ray, float length, LayerMask ignoreLayers, QueryTriggerInteraction affectTriggers = QueryTriggerInteraction.UseGlobal)
        {
            if (customCast != null)
            {
                return customCast.CustomRaycastAll(ray, length);
            }
            else
            {
                int count = Physics.RaycastNonAlloc(ray, Hits, length, ~ignoreLayers, affectTriggers);
                return new ArraySegment<RaycastHit>(Hits, 0, count);
            }
        }

        /// <summary>
        /// Generates a Linecast either from the given <see cref="PhysicsCast"/> object or a default <see cref="Physics.Linecast(Vector3,Vector3,out RaycastHit,int,QueryTriggerInteraction)"/>.
        /// </summary>
        /// <param name="customCast">The optional <see cref="PhysicsCast"/> with customized cast parameters.</param>
        /// <param name="startPosition">The world position to start the Linecast from.</param>
        /// <param name="endPosition">The world position to end the Linecast at.</param>
        /// <param name="hitData">The <see cref="RaycastHit"/> data.</param>
        /// <param name="ignoreLayers">A <see cref="LayerMask"/> of layers to ignore from the Linecast.</param>
        /// <param name="affectTriggers">Determines the trigger interaction level of the cast.</param>
        /// <returns>Whether the Linecast successfully collides with a valid <see cref="GameObject"/>.</returns>
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
        /// Generates a SphereCast either from the given <see cref="PhysicsCast"/> object or a default <see cref="Physics.SphereCast(Vector3,float,Vector3,out RaycastHit,float,int,QueryTriggerInteraction)"/>.
        /// </summary>
        /// <param name="customCast">The optional <see cref="PhysicsCast"/> with customized cast parameters.</param>
        /// <param name="origin">The origin point of the sphere to cast.</param>
        /// <param name="radius">The radius of the sphere.</param>
        /// <param name="direction">The direction into which to sweep the sphere.</param>
        /// <param name="hitData">The <see cref="RaycastHit"/> data.</param>
        /// <param name="maxDistance">The max length of the sweep.</param>
        /// <param name="ignoreLayers">A <see cref="LayerMask"/> of layers to ignore from the SphereCast.</param>
        /// <param name="affectTriggers">Determines the trigger interaction level of the cast.</param>
        /// <returns>Whether the SphereCast successfully collides with a valid <see cref="GameObject"/>.</returns>
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
        /// Generates a SphereCastAll either from the given <see cref="PhysicsCast"/> object or a default <see cref="Physics.SphereCastAll(Vector3,float,Vector3,float,int,QueryTriggerInteraction)"/>.
        /// </summary>
        /// <param name="customCast">The optional <see cref="PhysicsCast"/> with customized cast parameters.</param>
        /// <param name="origin">The origin point of the sphere to cast.</param>
        /// <param name="radius">The radius of the sphere.</param>
        /// <param name="direction">The direction into which to sweep the sphere.</param>
        /// <param name="maxDistance">The max length of the sweep.</param>
        /// <param name="ignoreLayers">A <see cref="LayerMask"/> of layers to ignore from the SphereCast.</param>
        /// <param name="affectTriggers">Determines the trigger interaction level of the cast.</param>
        /// <returns>A collection of collisions determined by the cast.</returns>
        public static ArraySegment<RaycastHit> SphereCastAll(PhysicsCast customCast, Vector3 origin, float radius, Vector3 direction, float maxDistance, LayerMask ignoreLayers, QueryTriggerInteraction affectTriggers = QueryTriggerInteraction.UseGlobal)
        {
            if (customCast != null)
            {
                return customCast.CustomSphereCastAll(origin, radius, direction, maxDistance);
            }
            else
            {
                int count = Physics.SphereCastNonAlloc(origin, radius, direction, Hits, maxDistance, ~ignoreLayers, affectTriggers);
                return new ArraySegment<RaycastHit>(Hits, 0, count);
            }
        }

        /// <summary>
        /// Returns an array segment with all colliders touching or inside a sphere with an optional from the given <see cref="PhysicsCast"/> object to override some parameters.
        /// </summary>
        /// <param name="customCast">The optional <see cref="PhysicsCast"/> with customized cast parameters.</param>
        /// <param name="center">The center of the sphere.</param>
        /// <param name="radius">The radius of the sphere.</param>
        /// <param name="ignoreLayers">The layers to ignore..</param>
        /// <param name="affectTriggers">The trigger interaction to use.</param>
        /// <returns>The colliders touching or inside the sphere.</returns>
        public static ArraySegment<Collider> OverlapSphereAll(PhysicsCast customCast, Vector3 center, float radius, LayerMask ignoreLayers, QueryTriggerInteraction affectTriggers = QueryTriggerInteraction.UseGlobal)
        {
            if (customCast != null)
            {
                return customCast.CustomOverlapSphereAll(center, radius);
            }
            else
            {
                int count = Physics.OverlapSphereNonAlloc(center, radius, Colliders, ~ignoreLayers, affectTriggers);
                return new ArraySegment<Collider>(Colliders, 0, count);
            }
        }

        /// <summary>
        /// Generates a CapsuleCast either from the given <see cref="PhysicsCast"/> object or a default <see cref="Physics.CapsuleCast(Vector3,Vector3,float,Vector3,out RaycastHit,float,int,QueryTriggerInteraction)"/>.
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
        /// <returns>Whether the CapsuleCast successfully collides with a valid <see cref="GameObject"/>.</returns>
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
        /// Generates a CapsuleCastAll either from the given <see cref="PhysicsCast"/> object or a default <see cref="Physics.CapsuleCastAll(Vector3,Vector3,float,Vector3,float,int,QueryTriggerInteraction)"/>.
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
        public static ArraySegment<RaycastHit> CapsuleCastAll(PhysicsCast customCast, Vector3 point1, Vector3 point2, float radius, Vector3 direction, float maxDistance, LayerMask ignoreLayers, QueryTriggerInteraction affectTriggers = QueryTriggerInteraction.UseGlobal)
        {
            if (customCast != null)
            {
                return customCast.CustomCapsuleCastAll(point1, point2, radius, direction, maxDistance);
            }
            else
            {
                int count = Physics.CapsuleCastNonAlloc(point1, point2, radius, direction, Hits, maxDistance, ~ignoreLayers, affectTriggers);
                return new ArraySegment<RaycastHit>(Hits, 0, count);
            }
        }

        /// <summary>
        /// Generates a BoxCast either from the given <see cref="PhysicsCast"/> object or a default <see cref="Physics.BoxCast(Vector3,Vector3,Vector3,out RaycastHit,Quaternion,float,int,QueryTriggerInteraction)"/>.
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
        /// <returns>Whether the BoxCast successfully collides with a valid <see cref="GameObject"/>.</returns>
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
        /// Generates a BoxCastAll either from the given <see cref="PhysicsCast"/> object or a default <see cref="Physics.BoxCastAll(Vector3,Vector3,Vector3,Quaternion,float,int,QueryTriggerInteraction)"/>.
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
        public static ArraySegment<RaycastHit> BoxCastAll(PhysicsCast customCast, Vector3 center, Vector3 halfExtents, Vector3 direction, Quaternion orientation, float maxDistance, LayerMask ignoreLayers, QueryTriggerInteraction affectTriggers = QueryTriggerInteraction.UseGlobal)
        {
            if (customCast != null)
            {
                return customCast.CustomBoxCastAll(center, halfExtents, direction, orientation, maxDistance);
            }
            else
            {
                int count = Physics.BoxCastNonAlloc(center, halfExtents, direction, Hits, orientation, maxDistance, ~ignoreLayers, affectTriggers);
                return new ArraySegment<RaycastHit>(Hits, 0, count);
            }
        }

        /// <summary>
        /// Generates a <see cref="Physics.Raycast(Ray,out RaycastHit,float,int,QueryTriggerInteraction)"/> based on the options defined in the <see cref="PhysicsCast"/> object.
        /// </summary>
        /// <param name="ray">The <see cref="Ray"/> to cast with.</param>
        /// <param name="hitData">The <see cref="RaycastHit"/> data.</param>
        /// <param name="length">The maximum length of the <see cref="Ray"/>.</param>
        /// <returns>Whether the raycast successfully collides with a valid <see cref="GameObject"/>.</returns>
        public virtual bool CustomRaycast(Ray ray, out RaycastHit hitData, float length)
        {
            return Physics.Raycast(ray, out hitData, length, ~LayersToIgnore, TriggerInteraction);
        }

        /// <summary>
        /// Generates a <see cref="Physics.RaycastAll(Ray,float,int,QueryTriggerInteraction)"/> based on the options defined in the <see cref="PhysicsCast"/> object.
        /// </summary>
        /// <param name="ray">The <see cref="Ray"/> to cast with.</param>
        /// <param name="length">The maximum length of the <see cref="Ray"/>.</param>
        /// <returns>A collection of collisions determined by the cast.</returns>
        public virtual ArraySegment<RaycastHit> CustomRaycastAll(Ray ray, float length)
        {
            int count = Physics.RaycastNonAlloc(ray, Hits, length, ~LayersToIgnore, TriggerInteraction);
            return new ArraySegment<RaycastHit>(Hits, 0, count);
        }

        /// <summary>
        /// Generates a <see cref="Physics.Linecast(Vector3,Vector3,out RaycastHit,int,QueryTriggerInteraction)"/> based on the options defined in the <see cref="PhysicsCast"/> object.
        /// </summary>
        /// <param name="startPosition">The world position to start the linecast from.</param>
        /// <param name="endPosition">The world position to end the linecast at.</param>
        /// <param name="hitData">The <see cref="RaycastHit"/> data.</param>
        /// <returns>Whether the linecast successfully collides with a valid <see cref="GameObject"/>.</returns>
        public virtual bool CustomLinecast(Vector3 startPosition, Vector3 endPosition, out RaycastHit hitData)
        {
            return Physics.Linecast(startPosition, endPosition, out hitData, ~LayersToIgnore, TriggerInteraction);
        }

        /// <summary>
        /// Generates a <see cref="Physics.SphereCast(Vector3,float,Vector3,out RaycastHit,float,int,QueryTriggerInteraction)"/> based on the options defined in the <see cref="PhysicsCast"/> object.
        /// </summary>
        /// <param name="origin">The origin point of the spherecast.</param>
        /// <param name="radius">The radius of the spherecast.</param>
        /// <param name="direction">The direction into which to sweep the spherecast.</param>
        /// <param name="hitData">The <see cref="RaycastHit"/> data.</param>
        /// <param name="maxDistance">The max length of the sweep.</param>
        /// <returns>Whether the spherecast successfully collides with a valid <see cref="GameObject"/>.</returns>
        public virtual bool CustomSphereCast(Vector3 origin, float radius, Vector3 direction, out RaycastHit hitData, float maxDistance)
        {
            return Physics.SphereCast(origin, radius, direction, out hitData, maxDistance, ~LayersToIgnore, TriggerInteraction);
        }

        /// <summary>
        /// Generates a <see cref="Physics.SphereCastAll(Vector3,float,Vector3,float,int,QueryTriggerInteraction)"/> based on the options defined in the <see cref="PhysicsCast"/> object.
        /// </summary>
        /// <param name="origin">The origin point of the spherecast.</param>
        /// <param name="radius">The radius of the spherecast.</param>
        /// <param name="direction">The direction into which to sweep the spherecast.</param>
        /// <param name="maxDistance">The max length of the sweep.</param>
        /// <returns>A collection of collisions determined by the cast.</returns>
        public virtual ArraySegment<RaycastHit> CustomSphereCastAll(Vector3 origin, float radius, Vector3 direction, float maxDistance)
        {
            int count = Physics.SphereCastNonAlloc(origin, radius, direction, Hits, maxDistance, ~LayersToIgnore, TriggerInteraction);
            return new ArraySegment<RaycastHit>(Hits, 0, count);
        }

        /// <summary>
        /// Returns an array segment with all colliders touching or inside a sphere based on the options defined in the <see cref="PhysicsCast"/> object.
        /// </summary>
        /// <param name="center">The center of the sphere.</param>
        /// <param name="radius">The radius of the sphere.</param>
        /// <returns>The colliders touching or inside the sphere.</returns>
        public virtual ArraySegment<Collider> CustomOverlapSphereAll(Vector3 center, float radius)
        {
            int count = Physics.OverlapSphereNonAlloc(center, radius, Colliders, ~LayersToIgnore, TriggerInteraction);
            return new ArraySegment<Collider>(Colliders, 0, count);
        }

        /// <summary>
        /// Generates a <see cref="Physics.CapsuleCast(Vector3,Vector3,float,Vector3,out RaycastHit,float,int,QueryTriggerInteraction)"/> based on the options defined in the <see cref="PhysicsCast"/> object.
        /// </summary>
        /// <param name="point1">The center of the sphere at the start of the capsulecast.</param>
        /// <param name="point2">The center of the sphere at the end of the capsulecast.</param>
        /// <param name="radius">The radius of the capsulecast.</param>
        /// <param name="direction">The direction into which to sweep the capsulecast.</param>
        /// <param name="hitData">The <see cref="RaycastHit"/> data.</param>
        /// <param name="maxDistance">The max length of the sweep.</param>
        /// <returns>Whether the capsulecast successfully collides with a valid <see cref="GameObject"/>.</returns>
        public virtual bool CustomCapsuleCast(Vector3 point1, Vector3 point2, float radius, Vector3 direction, out RaycastHit hitData, float maxDistance)
        {
            return Physics.CapsuleCast(point1, point2, radius, direction, out hitData, maxDistance, ~LayersToIgnore, TriggerInteraction);
        }

        /// <summary>
        /// Generates a <see cref="Physics.CapsuleCastAll(Vector3,Vector3,float,Vector3,float,int,QueryTriggerInteraction)"/> based on the options defined in the <see cref="PhysicsCast"/> object.
        /// </summary>
        /// <param name="point1">The center of the sphere at the start of the capsulecast.</param>
        /// <param name="point2">The center of the sphere at the end of the capsulecast.</param>
        /// <param name="radius">The radius of the capsulecast.</param>
        /// <param name="direction">The direction into which to sweep the capsulecast.</param>
        /// <param name="maxDistance">The max length of the sweep.</param>
        /// <returns>A collection of collisions determined by the cast.</returns>
        public virtual ArraySegment<RaycastHit> CustomCapsuleCastAll(Vector3 point1, Vector3 point2, float radius, Vector3 direction, float maxDistance)
        {
            int count = Physics.CapsuleCastNonAlloc(point1, point2, radius, direction, Hits, maxDistance, ~LayersToIgnore, TriggerInteraction);
            return new ArraySegment<RaycastHit>(Hits, 0, count);
        }

        /// <summary>
        /// Generates a <see cref="Physics.BoxCast(Vector3,Vector3,Vector3,out RaycastHit,Quaternion,float,int,QueryTriggerInteraction)"/> based on the options defined in the <see cref="PhysicsCast"/> object.
        /// </summary>
        /// <param name="center">The center of the boxcast.</param>
        /// <param name="halfExtents">Half the size of the boxcast in each dimension.</param>
        /// <param name="direction">The direction in which to cast the boxcast.</param>
        /// <param name="hitData">The <see cref="RaycastHit"/> data.</param>
        /// <param name="orientation">The rotation of the boxcast.</param>
        /// <param name="maxDistance">The max length of the boxcast.</param>
        /// <returns>Whether the boxcast successfully collides with a valid <see cref="GameObject"/>.</returns>
        public virtual bool CustomBoxCast(Vector3 center, Vector3 halfExtents, Vector3 direction, out RaycastHit hitData, Quaternion orientation, float maxDistance)
        {
            return Physics.BoxCast(center, halfExtents, direction, out hitData, orientation, maxDistance, ~LayersToIgnore, TriggerInteraction);
        }

        /// <summary>
        /// Generates a <see cref="Physics.BoxCastAll(Vector3,Vector3,Vector3,Quaternion,float,int,QueryTriggerInteraction)"/> based on the options defined in the <see cref="PhysicsCast"/> object.
        /// </summary>
        /// <param name="center">The center of the boxcast.</param>
        /// <param name="halfExtents">Half the size of the boxcast in each dimension.</param>
        /// <param name="direction">The direction in which to cast the boxcast.</param>
        /// <param name="orientation">The rotation of the boxcast.</param>
        /// <param name="maxDistance">The max length of the boxcast.</param>
        /// <returns>A collection of collisions determined by the cast.</returns>
        public virtual ArraySegment<RaycastHit> CustomBoxCastAll(Vector3 center, Vector3 halfExtents, Vector3 direction, Quaternion orientation, float maxDistance)
        {
            int count = Physics.BoxCastNonAlloc(center, halfExtents, direction, Hits, orientation, maxDistance, ~LayersToIgnore, TriggerInteraction);
            return new ArraySegment<RaycastHit>(Hits, 0, count);
        }
    }
}