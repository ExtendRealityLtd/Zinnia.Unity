namespace Zinnia.Data.Operation.Extraction
{
    using UnityEngine;

    /// <summary>
    /// Extracts the euler angles of a <see cref="Transform"/>.
    /// </summary>
    public class TransformEulerRotationExtractor : TransformVector3PropertyExtractor
    {
        /// <inheritdoc />
        protected override Vector3? ExtractValue()
        {
            if (Source == null)
            {
                return null;
            }

            return UseLocal ? Source.transform.localEulerAngles : Source.transform.eulerAngles;
        }
    }
}