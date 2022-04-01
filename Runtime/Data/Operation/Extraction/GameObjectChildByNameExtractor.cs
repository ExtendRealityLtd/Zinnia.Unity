namespace Zinnia.Data.Operation.Extraction
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// Extracts a child <see cref="GameObject"/> by the given named path.
    /// </summary>
    public class GameObjectChildByNameExtractor : GameObjectExtractor<GameObject, ComponentGameObjectExtractor.UnityEvent>
    {
        [Tooltip("The path name to the child GameObject.")]
        [SerializeField]
        private string childNamePath;
        /// <summary>
        /// The path name to the child <see cref="GameObject"/>.
        /// </summary>
        public string ChildNamePath
        {
            get
            {
                return childNamePath;
            }
            set
            {
                childNamePath = value;
            }
        }

        /// <summary>
        /// Defines the event with the specified <see cref="GameObject"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<GameObject> { }

        /// <inheritdoc />
        protected override GameObject ExtractValue()
        {
            if (Source == null)
            {
                return null;
            }

            Transform found = Source.transform.Find(ChildNamePath);
            return found != null ? found.gameObject : null;
        }
    }
}