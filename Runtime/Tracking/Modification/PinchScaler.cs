namespace Zinnia.Tracking.Modification
{
    using UnityEngine;
    using Malimbe.MemberChangeMethod;
    using Malimbe.MemberClearanceMethod;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.BehaviourStateRequirementMethod;
    using Zinnia.Process;
    using Zinnia.Extension;

    /// <summary>
    /// Scales a given target based on the distance between two points.
    /// </summary>
    public class PinchScaler : MonoBehaviour, IProcessable
    {
        /// <summary>
        /// The target to scale.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public GameObject Target { get; set; }
        /// <summary>
        /// The point to determine distance from.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public GameObject PrimaryPoint { get; set; }
        /// <summary>
        /// The point to determine distance to.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public GameObject SecondaryPoint { get; set; }
        /// <summary>
        /// A scale factor multiplier.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public float Multiplier { get; set; } = 1f;
        /// <summary>
        /// Determines whether to use local or global scale.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public bool UseLocalScale { get; set; } = true;

        /// <summary>
        /// The previous distance between <see cref="PrimaryPoint"/> and <see cref="SecondaryPoint"/>.
        /// </summary>
        protected float? previousDistance;
        /// <summary>
        /// The original scale of <see cref="Target"/>.
        /// </summary>
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
            if (UseLocalScale)
            {
                Target.transform.localScale = originalScale;
            }
            else
            {
                Target.transform.SetGlobalScale(originalScale);
            }
        }

        protected virtual void OnDisable()
        {
            previousDistance = null;
        }

        /// <summary>
        /// Attempts to scale the target.
        /// </summary>
        protected virtual void Scale()
        {
            previousDistance = previousDistance == null ? GetDistance() : previousDistance;

            float distanceDelta = GetDistance() - (float)previousDistance;
            Vector3 newScale = Vector3.one * distanceDelta * Multiplier;
            if (UseLocalScale)
            {
                Target.transform.localScale += newScale;
            }
            else
            {
                Target.transform.SetGlobalScale(Target.transform.lossyScale + newScale);
            }

            previousDistance = GetDistance();
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
            return UseLocalScale ? Target.transform.localScale : Target.transform.lossyScale;
        }

        /// <summary>
        /// Called after <see cref="PrimaryPoint"/> has been changed.
        /// </summary>
        [CalledAfterChangeOf(nameof(PrimaryPoint))]
        protected virtual void OnAfterPrimaryPointChange()
        {
            previousDistance = null;
        }

        /// <summary>
        /// Called after <see cref="SecondaryPoint"/> has been changed.
        /// </summary>
        [CalledAfterChangeOf(nameof(SecondaryPoint))]
        protected virtual void OnAfterSecondaryPointChange()
        {
            previousDistance = null;
        }
    }
}