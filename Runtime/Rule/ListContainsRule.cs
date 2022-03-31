namespace Zinnia.Rule
{
    using UnityEngine;
    using Zinnia.Data.Collection.List;

    /// <summary>
    /// Determines whether an object is part of a list.
    /// </summary>
    public class ListContainsRule : Rule
    {
        /// <summary>
        /// The objects to check against.
        /// </summary>
        [Tooltip("The objects to check against.")]
        [SerializeField]
        private UnityObjectObservableList _objects;
        public UnityObjectObservableList Objects
        {
            get
            {
                return _objects;
            }
            set
            {
                _objects = value;
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