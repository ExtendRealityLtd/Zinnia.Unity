namespace Zinnia.Data.Operation.Extraction
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
            return UseLocal ? Source.transform.localPosition : Source.transform.position;
        }
    }
}