namespace VRTK.Core.Prefabs.Pointers
{
    using UnityEngine;

    /// <summary>
    /// Provides user specific setup information for the pointer prefabs.
    /// </summary>
    public class PointerUserSetup : MonoBehaviour
    {
        /// <summary>
        /// The target for the pointer to follow around.
        /// </summary>
        [Tooltip("The target for the pointer to follow around.")]
        public GameObject followTarget;
    }
}