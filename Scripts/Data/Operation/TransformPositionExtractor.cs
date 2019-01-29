namespace Zinnia.Data.Operation
{
    using UnityEngine;

    /// <summary>
    /// Extracts the position of a <see cref="Transform"/>.
    /// </summary>
    public class TransformPositionExtractor : TransformVector3PropertyExtractor
    {
        /// <inheritdoc />
        protected override Vector3 ExtractValue()
        {
            return useLocal ? source.transform.localPosition : source.transform.position;
        }
    }
}