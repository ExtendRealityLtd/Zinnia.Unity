namespace Zinnia.Tracking.Follow.Modifier.Property
{
    using UnityEngine;
    using Zinnia.Data.Type;

    /// <summary>
    /// Modifies a specific property with the ability to limit the axes the property is applied to.
    /// </summary>
    public abstract class RestrictableTransformPropertyModifier : PropertyModifier
    {
        [Tooltip("Determines which axes to apply the modification on>.")]
        [SerializeField]
        private Vector3State applyModificationOnAxis = Vector3State.True;
        /// <summary>
        /// Determines which axes to apply the modification on>.
        /// </summary>
        public Vector3State ApplyModificationOnAxis
        {
            get
            {
                return applyModificationOnAxis;
            }
            set
            {
                applyModificationOnAxis = value;
            }
        }

        /// <summary>
        /// Whether the modifier has restrictable axes on.
        /// </summary>
        public virtual bool HasAxisRestrictions => !ApplyModificationOnAxis.ToVector3().Equals(Vector3.one);

        /// <summary>
        /// The original value of the property.
        /// </summary>
        protected Vector3 originalPropertyValue;

        /// <summary>
        /// Saves the original value for the property.
        /// </summary>
        /// <param name="valueToSave">The property value to save.</param>
        protected virtual void SaveOriginalPropertyValue(Vector3 valueToSave)
        {
            originalPropertyValue = valueToSave;
        }

        /// <summary>
        /// Applies the restrictions to the property value based on the axes to apply modification on.
        /// </summary>
        /// <param name="valueToRestrict">The property value to restrict.</param>
        /// <returns>The restricted property value.</returns>
        protected virtual Vector3 RestrictPropertyValue(Vector3 valueToRestrict)
        {
            valueToRestrict.x = ApplyModificationOnAxis.xState ? valueToRestrict.x : originalPropertyValue.x;
            valueToRestrict.y = ApplyModificationOnAxis.yState ? valueToRestrict.y : originalPropertyValue.y;
            valueToRestrict.z = ApplyModificationOnAxis.zState ? valueToRestrict.z : originalPropertyValue.z;
            return valueToRestrict;
        }
    }
}