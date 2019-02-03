namespace Zinnia.Association
{
    using UnityEngine;
    using System.Collections.Generic;
    using Malimbe.XmlDocumentationAttribute;

    /// <summary>
    /// Holds <see cref="GameObject"/>s to (de)activate.
    /// </summary>
    public abstract class GameObjectsAssociation : MonoBehaviour
    {
        /// <summary>
        /// The <see cref="GameObject"/>s to (de)activate.
        /// </summary>
        [DocumentedByXml]
        public List<GameObject> gameObjects = new List<GameObject>();

        /// <summary>
        /// Whether the <see cref="gameObjects"/> should be activated.
        /// </summary>
        /// <returns></returns>
        public abstract bool ShouldBeActive();
    }
}