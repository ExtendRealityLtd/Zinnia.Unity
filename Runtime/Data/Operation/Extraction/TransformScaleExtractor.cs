namespace Zinnia.Data.Operation.Extraction
{
    using UnityEngine;

    /// <summary>
    /// Extracts the scale of a <see cref="Transform"/>.
    /// </summary>
    public class TransformScaleExtractor : TransformVector3PropertyExtractor
    {
        /// <inheritdoc />
        protected override Vector3 ExtractValue()
        {
            return UseLocal ? Source.transform.localScale : Source.transform.lossyScale;
        }
    }
}