namespace Zinnia.Rule
{
    using UnityEngine;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;
    using Zinnia.Data.Collection.List;

    /// <summary>
    /// Determines whether a <see cref="GameObject"/>'s <see cref="GameObject.tag"/> is part of a list.
    /// </summary>
    public class AnyTagRule : GameObjectRule
    {
        /// <summary>
        /// The tags to check against.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public StringObservableList Tags { get; set; }

        /// <inheritdoc />
        protected override bool Accepts(GameObject targetGameObject)
        {
            if (Tags == null)
            {
                return false;
            }

            foreach (string testedTag in Tags.NonSubscribableElements)
            {
                if (targetGameObject.CompareTag(testedTag))
                {
                    return true;
                }
            }

            return false;
        }
    }
}