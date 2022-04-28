namespace Zinnia.Data.Collection.List
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;
    using Zinnia.Data.Attribute;
    using Zinnia.Data.Type;

    /// <summary>
    /// Allows observing changes to a <see cref="List{T}"/> of <see cref="SerializableType"/>s.
    /// </summary>
    public class SerializableTypeComponentObservableList : ObservableList<SerializableType, SerializableTypeComponentObservableList.UnityEvent>
    {
        /// <summary>
        /// The collection to observe changes of.
        /// </summary>
        [Tooltip("The collection to observe changes of.")]
        [SerializeField]
        [TypePicker(typeof(Component))]
        private List<SerializableType> elements = new List<SerializableType>();
        protected override List<SerializableType> Elements
        {
            get
            {
                return elements;
            }
            set
            {
                elements = value;
            }
        }

        /// <summary>
        /// Defines the event with the <see cref="SerializableType"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<SerializableType> { }
    }
}