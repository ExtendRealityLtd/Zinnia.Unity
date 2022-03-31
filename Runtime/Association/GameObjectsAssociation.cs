namespace Zinnia.Association
{
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
        [Tooltip("The GameObjects to (de)activate.")]
        [SerializeField]
        private GameObjectObservableList _gameObjects;
        public GameObjectObservableList GameObjects
        {
            get
            {
                return _gameObjects;
            }
            set
            {
                _gameObjects = value;
            }
        }

        /// <summary>
        /// Whether the <see cref="GameObjects"/> should be activated.
        /// </summary>
        /// <returns>Whether the association should be active.</returns>
        public abstract bool ShouldBeActive();
    }
}