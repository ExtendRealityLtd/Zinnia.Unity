namespace Zinnia.Rule
{
    using UnityEngine;
    using System.Collections.Generic;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.BehaviourStateRequirementMethod;

    /// <summary>
    /// Determines whether an object is part of a list.
    /// </summary>
    public class ListContainsRule : MonoBehaviour, IRule
    {
        /// <summary>
        /// The objects to check against.
        /// </summary>
        [DocumentedByXml]
        public List<Object> objects = new List<Object>();

        /// <inheritdoc />
        [RequiresBehaviourState]
        public virtual bool Accepts(object target)
        {
            Object targetObject = target as Object;
            return targetObject != null && objects.Contains(targetObject);
        }
    }
}