namespace Zinnia.Association
{
    using UnityEngine;
    using Zinnia.Data.Collection.List;

    /// <summary>
    /// Holds <see cref="GameObject"/>s to (de)activate.
    /// </summary>
    public abstract class GameObjectsAssociation : MonoBehaviour
    {
        [Tooltip("The GameObjects to (de)activate.")]
        [SerializeField]
        private GameObjectObservableList gameObjects;
        /// <summary>
        /// The <see cref="GameObject"/>s to (de)activate.
        /// </summary>
        public GameObjectObservableList GameObjects
        {
            get
            {
                return gameObjects;
            }
            set
            {
                gameObjects = value;
            }
        }

        /// <summary>
        /// Whether the <see cref="GameObjects"/> should be activated.
        /// </summary>
        /// <returns>Whether the association should be active.</returns>
        public abstract bool ShouldBeActive();
    }
}