namespace Zinnia.Utility
{
    using UnityEngine;
    using Zinnia.Extension;

    /// <summary>
    /// Sets a parameter value on a given <see cref="Animator"/>.
    /// </summary>
    public class AnimatorParameterMutator : MonoBehaviour
    {
        [Tooltip("The Animator with the parameter to set.")]
        [SerializeField]
        private Animator timeline;
        /// <summary>
        /// The <see cref="Animator"/> with the parameter to set.
        /// </summary>
        public Animator Timeline
        {
            get
            {
                return timeline;
            }
            set
            {
                timeline = value;
            }
        }
        [Tooltip("The name of the parameter to set.")]
        [SerializeField]
        private string parameterName;
        /// <summary>
        /// The name of the parameter to set.
        /// </summary>
        public string ParameterName
        {
            get
            {
                return parameterName;
            }
            set
            {
                parameterName = value;
            }
        }

        /// <summary>
        /// The value if the <see cref="ParameterName"/> is a float value.
        /// </summary>
        public virtual float FloatValue
        {
            get
            {
                return CanMutate() ? Timeline.GetFloat(ParameterName) : default;
            }
            set
            {
                if (CanMutate())
                {
                    Timeline.SetFloat(ParameterName, value);
                }
            }
        }

        /// <summary>
        /// The value if the <see cref="ParameterName"/> is a integer value.
        /// </summary>
        public virtual int IntegerValue
        {
            get
            {
                return CanMutate() ? Timeline.GetInteger(ParameterName) : default;
            }
            set
            {
                if (CanMutate())
                {
                    Timeline.SetInteger(ParameterName, value);
                }
            }
        }

        /// <summary>
        /// The value if the <see cref="ParameterName"/> is a bool value.
        /// </summary>
        public virtual bool BoolValue
        {
            get
            {
                return CanMutate() ? Timeline.GetBool(ParameterName) : default;
            }
            set
            {
                if (CanMutate())
                {
                    Timeline.SetBool(ParameterName, value);
                }
            }
        }

        /// <summary>
        /// Sets the <see cref="ParameterName"/> trigger.
        /// </summary>
        public virtual void SetTrigger()
        {
            if (!CanMutate())
            {
                return;
            }

            Timeline.SetTrigger(ParameterName);
        }

        /// <summary>
        /// Resets the <see cref="ParameterName"/> trigger.
        /// </summary>
        public virtual void ResetTrigger()
        {
            if (!CanMutate())
            {
                return;
            }

            Timeline.ResetTrigger(ParameterName);
        }

        /// <summary>
        /// Determines whether the component can be mutated.
        /// </summary>
        /// <returns>Whether the component can be mutated.</returns>
        protected virtual bool CanMutate()
        {
            return this.IsValidState() && Timeline != null && !string.IsNullOrEmpty(ParameterName);
        }
    }
}