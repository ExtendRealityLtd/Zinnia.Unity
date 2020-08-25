namespace Zinnia.Data.Attribute
{
    using System;
    using UnityEngine;

    /// <summary>
    /// Defines the <c>[TypePicker]</c> attribute.
    /// </summary>
    /// <remarks>This attribute is only valid on fields that use <see cref="Data.Type.SerializableType"/>.</remarks>
    public class TypePickerAttribute : PropertyAttribute
    {
        public readonly Type baseType;

        public TypePickerAttribute(Type baseType)
        {
            this.baseType = baseType;
        }
    }
}