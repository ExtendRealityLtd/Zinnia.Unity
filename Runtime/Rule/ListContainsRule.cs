namespace Zinnia.Rule
{
    using UnityEngine;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;
    using Zinnia.Data.Collection.List;

    /// <summary>
    /// Determines whether an object is part of a list.
    /// </summary>
    public class ListContainsRule : Rule
    {
        /// <summary>
        /// The objects to check against.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public UnityObjectObservableList Objects { get; set; }

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