namespace Zinnia.Data.Operation
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using Zinnia.Extension;

    /// <summary>
    /// Duplicates a <see cref="GameObject"/> by cloning it.
    /// </summary>
    public class GameObjectCloner : MonoBehaviour
    {
        /// <summary>
        /// Defines the event with the <see cref="GameObject"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<GameObject> { }

        [Tooltip("The object to clone.")]
        [SerializeField]
        private GameObject source;
        /// <summary>
        /// The object to clone.
        /// </summary>
        public GameObject Source
        {
            get
            {
                return source;
            }
            set
            {
                source = value;
            }
        }
        [Tooltip("An optional object to parent the cloned objects to.")]
        [SerializeField]
        private GameObject parent;
        /// <summary>
        /// An optional object to parent the cloned objects to.
        /// </summary>
        public GameObject Parent
        {
            get
            {
                return parent;
            }
            set
            {
                parent = value;
            }
        }

        /// <summary>
        /// Emitted when an object has been cloned.
        /// </summary>
        public UnityEvent Cloned = new UnityEvent();

        /// <summary>
        /// Clears <see cref="Source"/>.
        /// </summary>
        public virtual void ClearSource()
        {
            if (!this.IsValidState())
            {
                return;
            }

            Source = default;
        }

        /// <summary>
        /// Clears <see cref="Parent"/>.
        /// </summary>
        public virtual void ClearParent()
        {
            if (!this.IsValidState())
            {
                return;
            }

            Parent = default;
        }

        /// <summary>
        /// Duplicates <see cref="Source"/> by cloning it and optionally parents the cloned object to <see cref="Parent"/>.
        /// </summary>
        /// <returns>The cloned object, or <see langword="null"/> if no clone has been created.</returns>
        public virtual GameObject Clone()
        {
            if (!this.IsValidState())
            {
                return null;
            }

            return Clone(Source);
        }

        /// <summary>
        /// Duplicates a <see cref="GameObject"/> by cloning it and optionally parents the cloned object to <see cref="Parent"/>.
        /// </summary>
        /// <param name="source">The object to clone.</param>
        /// <returns>The cloned object, or <see langword="null"/> if no clone has been created.</returns>
        public virtual GameObject Clone(GameObject source)
        {
            if (!this.IsValidState() || source == null)
            {
                return null;
            }

            GameObject clone = Instantiate(source, Parent == null ? null : Parent.transform);
            Cloned?.Invoke(clone);
            return clone;
        }

        /// <summary>
        /// Duplicates <see cref="Source"/> by cloning it and parents the cloned object to <see cref="Parent"/>.
        /// </summary>
        public virtual void DoClone()
        {
            Clone();
        }

        /// <summary>
        /// Duplicates <see cref="Source"/> by cloning it and parents the cloned object to <see cref="Parent"/>.
        /// </summary>
        /// <param name="source">The object to clone.</param>
        public virtual void DoClone(GameObject source)
        {
            Clone(source);
        }
    }
}
