namespace Zinnia.Tracking.Modification
{
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
        [Tooltip("The target to scale.")]
        public GameObject target;
        /// <summary>
        /// The point to determine distance from.
        /// </summary>
        [Tooltip("The point to determine distance from.")]
        public GameObject primaryPoint;
        /// <summary>
        /// The point to determine distance to.
        /// </summary>
        [Tooltip("The point to determine distance to.")]
        public GameObject secondaryPoint;
        /// <summary>
        /// A scale factor multiplier.
        /// </summary>
        [Tooltip("A scale factor multiplier.")]
        public float multiplier = 1f;
        /// <summary>
        /// Determines whether to use local or global scale.
        /// </summary>
        [Tooltip("Determines whether to use local or global scale.")]
        public bool useLocalScale = true;

        protected bool initialized;
        protected float previousDistance;
        protected Vector3 originalScale;

        /// <summary>
        /// Processes the current scale factor onto the target.
        /// </summary>
        public virtual void Process()
        {
            if (!isActiveAndEnabled || target == null || primaryPoint == null || secondaryPoint == null)
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
                target.transform.localScale = originalScale;
            }
            else
            {
                target.transform.SetGlobalScale(originalScale);
            }
        }

        /// <summary>
        /// Sets the target.
        /// </summary>
        /// <param name="target">The new target.</param>
        public virtual void SetTarget(GameObject target)
        {
            this.target = target;
        }

        /// <summary>
        /// Clears the existing target.
        /// </summary>
        public virtual void ClearTarget()
        {
            target = null;
        }

        /// <summary>
        /// Sets the primary point.
        /// </summary>
        /// <param name="primaryPoint">The new primary point.</param>
        public virtual void SetPrimaryPoint(GameObject primaryPoint)
        {
            this.primaryPoint = primaryPoint;
        }

        /// <summary>
        /// Clears the existing primary point.
        /// </summary>
        public virtual void ClearPrimaryPoint()
        {
            primaryPoint = null;
            initialized = false;
        }

        /// <summary>
        /// Sets the secondary point.
        /// </summary>
        /// <param name="secondaryPoint">The new secondary point.</param>
        public virtual void SetSecondaryPoint(GameObject secondaryPoint)
        {
            this.secondaryPoint = secondaryPoint;
        }

        /// <summary>
        /// Clears the existing secondary point.
        /// </summary>
        public virtual void ClearSecondaryPoint()
        {
            secondaryPoint = null;
            initialized = false;
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
                    target.transform.localScale += newScale;
                }
                else
                {
                    target.transform.SetGlobalScale(target.transform.lossyScale + newScale);
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
            return Vector3.Distance(primaryPoint.transform.position, secondaryPoint.transform.position);
        }

        /// <summary>
        /// Gets the scale of the target in either local or global scale.
        /// </summary>
        /// <returns>The scale of the target.</returns>
        protected virtual Vector3 GetTargetScale()
        {
            return (useLocalScale ? target.transform.localScale : target.transform.lossyScale);
        }
    }
}