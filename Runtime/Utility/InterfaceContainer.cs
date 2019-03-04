namespace Zinnia.Utility
{
    using Malimbe.PropertySerializationAttribute;
    /*using Malimbe.PropertySetterMethod;*/
    /*using Malimbe.PropertyValidationMethod;*/
    using Malimbe.XmlDocumentationAttribute;
    using UnityEngine;

    /// <summary>
    /// A container for an <see cref="Object"/> that implements an interface that can be utilized within a Unity Inspector.
    /// </summary>
    public abstract class InterfaceContainer
    {
        /// <summary>
        /// The contained object.
        /// </summary>
        [Serialized, /*Validated*/]
        [field: DocumentedByXml]
        protected Object Field { get; set; }

        /// <summary>
        /// Handles changes to <see cref="Field"/>.
        /// </summary>
        /// <param name="previousValue">The previous value.</param>
        /// <param name="newValue">The new value.</param>
        /*[CalledBySetter(nameof(Field))]*/
        protected virtual void OnFieldChange(Object previousValue, ref Object newValue)
        {
        }
    }

    /// <summary>
    /// A container for a given interface type.
    /// </summary>
    /// <typeparam name="TInterface">The interface type to contain.</typeparam>
    public abstract class InterfaceContainer<TInterface> : InterfaceContainer, ISerializationCallbackReceiver
    {
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
        protected override void OnFieldChange(Object previousValue, ref Object newValue)
        {
            if (newValue is TInterface @interface)
            {
                _interface = @interface;
            }
            else
            {
                newValue = null;
            }
        }

        /// <inheritdoc />
        public void OnBeforeSerialize()
        {
        }

        /// <inheritdoc />
        public void OnAfterDeserialize()
        {
            Field = Field;
        }
    }
}