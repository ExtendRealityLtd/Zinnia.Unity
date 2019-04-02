namespace Zinnia.Extension
{
    using UnityEngine;
    using Zinnia.Data.Type;

    /// <summary>
    /// Extended methods for the <see cref="TransformData"/> Type.
    /// </summary>
    public static class TransformDataExtensions
    {
        /// <summary>
        /// Attempts to retrieve the <see cref="GameObject"/> from a given <see cref="TransformData"/>.
        /// </summary>
        /// <param name="transformData">The <see cref="TransformData"/> to retrieve the <see cref="GameObject"/> from.</param>
        /// <returns>The <see cref="GameObject"/> if one exists on the given <see cref="TransformData"/>.</returns>
        public static GameObject TryGetGameObject(this TransformData transformData)
        {
            return transformData?.Transform == null ? null : transformData.Transform.gameObject;
        }
    }
}