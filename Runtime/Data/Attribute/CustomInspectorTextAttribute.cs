namespace Zinnia.Data.Attribute
{
    using UnityEngine;

    /// <summary>
    /// Defines the <c>[CustomInspectorText]</c> attribute.
    /// </summary>
    public class CustomInspectorTextAttribute : PropertyAttribute
    {
        public readonly string customText;

        public CustomInspectorTextAttribute(string customText)
        {
            this.customText = customText;
        }
    }
}