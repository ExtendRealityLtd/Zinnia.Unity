namespace Zinnia.Rule
{
    using UnityEngine;
    using System.Collections.Generic;
    using Malimbe.XmlDocumentationAttribute;

    /// <summary>
    /// Determines whether a <see cref="GameObject"/>'s <see cref="GameObject.tag"/> is part of a list.
    /// </summary>
    public class AnyTagRule : GameObjectRule
    {
        /// <summary>
        /// The tags to check against.
        /// </summary>
        [DocumentedByXml]
        public List<string> tags = new List<string>();

        /// <inheritdoc />
        protected override bool Accepts(GameObject targetGameObject)
        {
            foreach (string testedTag in tags)
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