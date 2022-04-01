namespace Zinnia.Rule
{
    using System.Text.RegularExpressions;
    using UnityEngine;
    using Zinnia.Data.Collection.List;
    using Zinnia.Extension;

    /// <summary>
    /// Determines whether a <see cref="GameObject"/> with a <see cref="StringObservableList"/> component contains a string that matches a specified pattern.
    /// </summary>
    public class StringInListRule : GameObjectRule
    {
        [Tooltip("The regular expression pattern to match against a string contained in the StringObservableList.")]
        [SerializeField]
        private string inListPattern;
        /// <summary>
        /// The regular expression pattern to match against a string contained in the <see cref="StringObservableList"/>.
        /// </summary>
        public string InListPattern
        {
            get
            {
                return inListPattern;
            }
            set
            {
                inListPattern = value;
            }
        }

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