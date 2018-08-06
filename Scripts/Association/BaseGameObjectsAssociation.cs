namespace VRTK.Core.Association
{
    using UnityEngine;
    using System.Collections.Generic;

    /// <summary>
    /// Holds <see cref="GameObject"/>s to (de)activate.
    /// </summary>
    public abstract class BaseGameObjectsAssociation : MonoBehaviour
    {
        /// <summary>
        /// The <see cref="GameObject"/>s to (de)activate.
        /// </summary>
        [Tooltip("The GameObjects to (de)activate.")]
        public List<GameObject> gameObjects = new List<GameObject>();

        /// <summary>
        /// Whether the <see cref="gameObjects"/> should be activated.
        /// </summary>
        /// <returns></returns>
        public abstract bool ShouldBeActive();
    }
}