namespace Zinnia.Association
{
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;
    using UnityEngine;
    using Zinnia.Data.Collection.List;

    /// <summary>
    /// Holds <see cref="GameObject"/>s to (de)activate.
    /// </summary>
    public abstract class GameObjectsAssociation : MonoBehaviour
    {
        /// <summary>
        /// The <see cref="GameObject"/>s to (de)activate.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public GameObjectObservableList GameObjects { get; set; }

        /// <summary>
        /// Whether the <see cref="GameObjects"/> should be activated.
        /// </summary>
        /// <returns></returns>
        public abstract bool ShouldBeActive();
    }
}