namespace Zinnia.Data.Type
{
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;
    using System;
    using UnityEngine;
    using Zinnia.Extension;

    /// <summary>
    /// Holds <see cref="UnityEngine.Transform"/> information with the ability to override properties without affecting the scene <see cref="UnityEngine.Transform"/>.
    /// </summary>
    [Serializable]
    public class TransformData
    {
        /// <summary>
        /// A reference to the original <see cref="UnityEngine.Transform"/>.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public Transform Transform { get; set; }
        /// <summary>
        /// Determines whether to operate on the local or global values.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public bool UseLocalValues { get; set; }
        /// <summary>
        /// Position override of the <see cref="UnityEngine.Transform"/> object.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public Vector3? PositionOverride { get; set; }
        /// <summary>
        /// Rotation override of the <see cref="UnityEngine.Transform"/> object.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public Quaternion? RotationOverride { get; set; }
        /// <summary>
        /// Scale override of the <see cref="UnityEngine.Transform"/> object.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public Vector3? ScaleOverride { get; set; }

        /// <summary>
        /// The position of the <see cref="UnityEngine.Transform"/> or the <see cref="PositionOverride"/> if it is set.
        /// </summary>
        public Vector3 Position => PositionOverride ?? (UseLocalValues ? Transform.localPosition : Transform.position);

        /// <summary>
        /// The rotation of the <see cref="UnityEngine.Transform"/> or the <see cref="RotationOverride"/> if it is set.
        /// </summary>
        public Quaternion Rotation => RotationOverride ?? (UseLocalValues ? Transform.localRotation : Transform.rotation);

        /// <summary>
        /// The scale of the <see cref="UnityEngine.Transform"/> or the <see cref="ScaleOverride"/> if it is set.
        /// </summary>
        public Vector3 Scale => ScaleOverride ?? (UseLocalValues ? Transform.localScale : Transform.lossyScale);

        /// <summary>
        /// The state of whether the <see cref="TransformData"/> is valid.
        /// </summary>
        public bool IsValid => Transform != null;

        /// <summary>
        /// Creates a new <see cref="TransformData"/> for an empty <see cref="UnityEngine.Transform"/>.
        /// </summary>
        public TransformData() { }

        /// <summary>
        /// Creates a new <see cref="TransformData"/> from an existing <see cref="UnityEngine.Transform"/>.
        /// </summary>
        /// <param name="transform">The <see cref="UnityEngine.Transform"/> to create the <see cref="TransformData"/> from.</param>
        public TransformData(Transform transform)
        {
            Transform = transform;
        }

        /// <summary>
        /// Creates a new <see cref="TransformData"/> from an existing <see cref="GameObject"/>.
        /// </summary>
        /// <param name="gameObject">The <see cref="GameObject"/> to create the <see cref="TransformData"/> from.</param>
        public TransformData(GameObject gameObject) : this(gameObject != null ? gameObject.transform : null) { }

        /// <inheritdoc />
        public override bool Equals(object other)
        {
            if (other == null)
            {
                return false;
            }

            TransformData data = other as TransformData;
            return Equals(data);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            string[] titles = new string[]
{
                "Transform",
                "UseLocalValues",
                "PositionOverride",
                "RotationOverride",
                "ScaleOverride"
};

            object[] values = new object[]
            {
                Transform,
                UseLocalValues,
                PositionOverride,
                RotationOverride,
                ScaleOverride
            };

            return StringExtensions.FormatForToString(titles, values);
        }

        /// <summary>
        /// Checks to see if the given <see cref="TransformData"/> is equal to <see cref="this"/>.
        /// </summary>
        /// <param name="other">The instance to check equality with.</param>
        /// <returns>Whether the two instances are equal.</returns>
        public virtual bool Equals(TransformData other)
        {
            if (other == null || !GetType().Equals(other.GetType()))
            {
                return false;
            }

            return Transform == other.Transform &&
                UseLocalValues == other.UseLocalValues &&
                PositionOverride == other.PositionOverride &&
                RotationOverride == other.RotationOverride &&
                ScaleOverride == other.ScaleOverride;
        }

        /// <summary>
        /// Clears the state back to <see langword="null"/>.
        /// </summary>
        public virtual void Clear()
        {
            Transform = null;
            UseLocalValues = false;
            PositionOverride = null;
            RotationOverride = null;
            ScaleOverride = null;
        }
    }
}