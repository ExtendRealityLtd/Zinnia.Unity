namespace Zinnia.Rule
{
    using UnityEngine;
    using System.Text.RegularExpressions;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;
    using Zinnia.Extension;
    using Zinnia.Data.Collection.List;

    /// <summary>
    /// Determines whether a <see cref="GameObject"/> with a <see cref="StringObservableList"/> component contains a string that matches a specified pattern.
    /// </summary>
    public class StringInListRule : GameObjectRule
    {
        /// <summary>
        /// The regular expression pattern to match against a string contained in the <see cref="StringObservableList"/>.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public string InListPattern { get; set; }

        /// <inheritdoc/>
        protected override bool Accepts(GameObject targetGameObject)
        {
            StringObservableList list = targetGameObject.TryGetComponent<StringObservableList>();
            if (list != null)
            {
                foreach (string element in list.NonSubscribableElements)
                {
                    if (Regex.IsMatch(element, InListPattern))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}