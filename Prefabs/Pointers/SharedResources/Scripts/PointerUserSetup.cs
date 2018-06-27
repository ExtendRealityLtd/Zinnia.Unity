namespace VRTK.Core.Prefabs.Pointers
{
    using UnityEngine;

    /// <summary>
    /// Provides user specific setup information for the pointer prefabs.
    /// </summary>
    public class PointerUserSetup : MonoBehaviour
    {
        /// <summary>
        /// The <see cref="Transform"/> target for the pointer to follow around.
        /// </summary>
        [Tooltip("The Transform target for the pointer to follow around.")]
        public Transform followTarget;
    }
}