namespace Zinnia.Data.Type
{
    using UnityEngine;
    using System;
    using Malimbe.MemberChangeMethod;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;

    /// <summary>
    /// Holds information about the located surface.
    /// </summary>
    [Serializable]
    public class SurfaceData : TransformData
    {
        /// <summary>
        /// The origin in which the surface search came from.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public Vector3 Origin { get; set; }
        /// <summary>
        /// The direction in which the surface search came from.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public Vector3 Direction { get; set; }

        /// <summary>
        /// <see cref="RaycastHit"/> data about the current collision.
        /// </summary>
        public RaycastHit CollisionData { get; set; }

        /// <summary>
        /// <see cref="RaycastHit"/> data about the previous collision.
        /// </summary>
        public RaycastHit PreviousCollisionData { get; protected set; }

        /// <summary>
        /// Creates a new <see cref="SurfaceData"/> with a default <see cref="Origin"/> and <see cref="Direction"/> for the collision search.
        /// </summary>
        public SurfaceData() { }

        /// <summary>
        /// Creates a new <see cref="SurfaceData"/> with a given <see cref="Origin"/> and <see cref="Direction"/> for the collision search.
        /// </summary>
        /// <param name="origin">The given origin to instantiate the <see cref="SurfaceData"/> with.</param>
        /// <param name="direction">The given direction to instantiate the <see cref="SurfaceData"/> with.</param>
        public SurfaceData(Vector3 origin, Vector3 direction)
        {
            Origin = origin;
            Direction = direction;
        }

        /// <inheritdoc />
        public override void Clear()
        {
            base.Clear();
            Origin = new Vector3();
            Direction = new Vector3();
            CollisionData = new RaycastHit();
            PreviousCollisionData = new RaycastHit();
        }

        /// <summary>
        /// Called before <see cref="CollisionData"/> has been changed.
        /// </summary>
        [CalledBeforeChangeOf(nameof(CollisionData))]
        protected virtual void OnBeforeCollisionDataChange()
        {
            PreviousCollisionData = CollisionData;
        }
    }
}