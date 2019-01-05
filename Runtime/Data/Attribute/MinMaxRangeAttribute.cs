namespace Zinnia.Data.Attribute
{
    using UnityEngine;

    /// <summary>
    /// Defines the <c>[MinMaxRange]</c> attribute.
    /// </summary>
    public class MinMaxRangeAttribute : PropertyAttribute
    {
        public readonly float max;
        public readonly float min;

        public MinMaxRangeAttribute(float min, float max)
        {
            this.min = min;
            this.max = max;
        }
    }
}