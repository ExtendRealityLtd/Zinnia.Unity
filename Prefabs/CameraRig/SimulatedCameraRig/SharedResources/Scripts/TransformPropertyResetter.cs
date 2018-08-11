namespace VRTK.Core.Prefabs.CameraRig.SimulatedCameraRig
{
    using UnityEngine;

    /// <summary>
    /// Resets the saved properties of a given transform.
    /// </summary>
    public class TransformPropertyResetter : MonoBehaviour
    {
        /// <summary>
        /// The source to cache and reset.
        /// </summary>
        [Tooltip("The source to cache and reset.")]
        public Transform source;

        protected Vector3 initialLocalPosition;
        protected Quaternion initialLocalRotation;
        protected Vector3 initialLocalScale;
        protected bool initialSet;

        /// <summary>
        /// Resets to the cached properties.
        /// </summary>
        public virtual void ResetProperties()
        {
            if (initialSet && source != null)
            {
                source.localPosition = initialLocalPosition;
                source.localRotation = initialLocalRotation;
                source.localScale = initialLocalScale;
            }
        }

        protected virtual void Awake()
        {
            if (source != null)
            {
                initialLocalPosition = source.localPosition;
                initialLocalRotation = source.localRotation;
                initialLocalScale = source.localScale;
                initialSet = true;
            }
        }
    }
}