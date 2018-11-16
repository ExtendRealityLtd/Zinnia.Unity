namespace VRTK.Core.Data.Operation
{
    using UnityEngine;

    /// <summary>
    /// Extracts the <see cref="Transform.position"/>.
    /// </summary>
    public class TransformPositionExtractor : TransformVector3PropertyExtractor
    {
        /// <inheritdoc />
        protected override Vector3 ExtractValue()
        {
            return (useLocal ? source.transform.localPosition : source.transform.position);
        }
    }
}