namespace Zinnia.Tracking.Modification
{
    using Malimbe.BehaviourStateRequirementMethod;
    using Malimbe.MemberClearanceMethod;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.PropertyValidationMethod;
    using Malimbe.XmlDocumentationAttribute;
    using UnityEngine;
    using Zinnia.Extension;
    using Zinnia.Process;

    /// <summary>
    /// Scales a given target based on the distance between two points.
    /// </summary>
    public class PinchScaler : MonoBehaviour, IProcessable
    {
        /// <summary>
        /// The target to scale.
        /// </summary>
        [Serialized, Validated, Cleared]
        [field: DocumentedByXml]
        public GameObject Target { get; set; }
        /// <summary>
        /// The point to determine distance from.
        /// </summary>
        [Serialized, Validated, Cleared]
        [field: DocumentedByXml]
        public GameObject PrimaryPoint { get; set; }
        /// <summary>
        /// The point to determine distance to.
        /// </summary>
        [Serialized, Validated, Cleared]
        [field: DocumentedByXml]
        public GameObject SecondaryPoint { get; set; }
        /// <summary>
        /// A scale factor multiplier.
        /// </summary>
        [DocumentedByXml]
        public float multiplier = 1f;
        /// <summary>
        /// Determines whether to use local or global scale.
        /// </summary>
        [DocumentedByXml]
        public bool useLocalScale = true;

        protected bool initialized;
        protected float previousDistance;
        protected Vector3 originalScale;

        /// <summary>
        /// Processes the current scale factor onto the target.
        /// </summary>
        [RequiresBehaviourState]
        public virtual void Process()
        {
            if (Target == null || PrimaryPoint == null || SecondaryPoint == null)
            {
                return;
            }

            Scale();
        }

        /// <summary>
        /// Saves the existing target scale.
        /// </summary>
        public virtual void SaveCurrentScale()
        {
            originalScale = GetTargetScale();
        }

        /// <summary>
        /// Restores the saved target scale.
        /// </summary>
        public virtual void RestoreSavedScale()
        {
            if (useLocalScale)
            {
                Target.transform.localScale = originalScale;
            }
            else
            {
                Target.transform.SetGlobalScale(originalScale);
            }
        }

        protected virtual void OnEnable()
        {
            initialized = false;
        }

        /// <summary>
        /// Attempts to scale the target.
        /// </summary>
        protected virtual void Scale()
        {
            if (initialized)
            {
                float distanceDelta = GetDistance() - previousDistance;
                Vector3 newScale = (Vector3.one * distanceDelta) * multiplier;
                if (useLocalScale)
                {
                    Target.transform.localScale += newScale;
                }
                else
                {
                    Target.transform.SetGlobalScale(Target.transform.lossyScale + newScale);
                }
            }
            previousDistance = GetDistance();
            initialized = true;
        }

        /// <summary>
        /// Gets the distance between the primary point and secondary point;
        /// </summary>
        /// <returns>The distance between the points.</returns>
        protected virtual float GetDistance()
        {
            return Vector3.Distance(PrimaryPoint.transform.position, SecondaryPoint.transform.position);
        }

        /// <summary>
        /// Gets the scale of the target in either local or global scale.
        /// </summary>
        /// <returns>The scale of the target.</returns>
        protected virtual Vector3 GetTargetScale()
        {
            return (useLocalScale ? Target.transform.localScale : Target.transform.lossyScale);
        }
    }
}