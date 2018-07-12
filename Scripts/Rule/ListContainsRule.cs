namespace VRTK.Core.Rule
{
    using UnityEngine;
    using System.Collections.Generic;

    /// <summary>
    /// Determines whether an object is part of a list.
    /// </summary>
    public class ListContainsRule : MonoBehaviour, IRule
    {
        /// <summary>
        /// The objects to check against.
        /// </summary>
        [Tooltip("The objects to check against.")]
        public List<Object> objects = new List<Object>();

        /// <inheritdoc />
        public bool Accepts(object target)
        {
            Object targetObject = target as Object;
            return targetObject != null && objects.Contains(targetObject);
        }
    }
}