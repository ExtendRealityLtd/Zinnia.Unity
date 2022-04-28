namespace Zinnia.Rule
{
    using UnityEngine;
    using Zinnia.Data.Collection.List;

    /// <summary>
    /// Determines whether an object is part of a list.
    /// </summary>
    public class ListContainsRule : Rule
    {
        [Tooltip("The objects to check against.")]
        [SerializeField]
        private UnityObjectObservableList objects;
        /// <summary>
        /// The objects to check against.
        /// </summary>
        public UnityObjectObservableList Objects
        {
            get
            {
                return objects;
            }
            set
            {
                objects = value;
            }
        }

        /// <inheritdoc />
        public override bool Accepts(object target)
        {
            if (ShouldAutoRejectDueToState() || Objects == null)
            {
                return false;
            }

            Object targetObject = target as Object;
            return targetObject != null && Objects.Contains(targetObject);
        }
    }
}