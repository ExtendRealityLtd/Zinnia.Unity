﻿namespace Zinnia.Data.Operation
{
    using UnityEngine;

    /// <summary>
    /// Extracts the euler angles of a <see cref="Transform"/>.
    /// </summary>
    public class TransformEulerRotationExtractor : TransformVector3PropertyExtractor
    {
        /// <inheritdoc />
        protected override Vector3 ExtractValue()
        {
            return useLocal ? source.transform.localEulerAngles : source.transform.eulerAngles;
        }
    }
}