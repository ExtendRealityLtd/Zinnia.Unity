namespace Zinnia.Utility
{
    using UnityEngine;

    /// <summary>
    /// A container for an <see cref="Object"/> that implements an interface that can be utilized within a Unity Inspector.
    /// </summary>
    public abstract class InterfaceContainer
    {
        /// <summary>
        /// The contained object.
        /// </summary>
        [Tooltip("The contained object.")]
        [SerializeField]
        protected Object field;
        /// <summary>
        /// The contained object.
        /// </summary>
        protected Object Field
        {
            get
            {
                return field;
            }
            set
            {
                field = value;
                if (Application.isPlaying)
                {
                    OnAfterFieldChange();
                }
            }
        }

        /// <summary>
        /// Called after <see cref="Field"/> has been changed.
        /// </summary>
        protected abstract void OnAfterFieldChange();
    }

    /// <summary>
    /// A container for a given interface type.
    /// </summary>
    /// <typeparam name="TInterface">The interface type to contain.</typeparam>
    public abstract class InterfaceContainer<TInterface> : InterfaceContainer, ISerializationCallbackReceiver
    {
        /// <summary>
        /// The contained interface.
        /// </summary>
        public TInterface Interface
        {
            get
            {
                return _interface;
            }
            set
            {
                Object @object = value as Object;
                if (@object == null)
                {
                    _interface = value;
                    Field = null;
                }
                else
                {
                    _interface = default;
                    Field = @object;
                }
            }
        }
        private TInterface _interface;

        /// <inheritdoc />
        public void OnBeforeSerialize() { }

        /// <inheritdoc />
        public void OnAfterDeserialize()
        {
            if (Field is TInterface @interface)
            {
                _interface = @interface;
            }
            else
            {
                field = null;
            }
        }

        /// <summary>
        /// Called after <see cref="InterfaceContainer.Field"/> has been changed.
        /// </summary>
        protected override void OnAfterFieldChange()
        {
            if (Field is TInterface @interface)
            {
                _interface = @interface;
            }
            else
            {
                field = null;
            }
        }
    }
}