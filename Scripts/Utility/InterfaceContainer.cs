namespace VRTK.Core.Utility
{
    using UnityEngine;

    /// <summary>
    /// The InterfaceContainer acts as a container for an object that implements an interface that can be utilized within a Unity Inspector.
    /// </summary>
    public abstract class InterfaceContainer
    {
        [SerializeField]
        protected Object field;
    }

    /// <summary>
    /// The InterfaceContainer acts as a container for a given interface type.
    /// </summary>
    /// <typeparam name="TInterface">The type of container for the interface.</typeparam>
    public abstract class InterfaceContainer<TInterface> : InterfaceContainer
    {
        public TInterface Interface
        {
            get
            {
                return field == null ? _interface : (TInterface)(object)field;
            }
            set
            {
                Object @object = value as Object;
                if (@object == null)
                {
                    _interface = value;
                    field = null;
                }
                else
                {
                    field = @object;
                    _interface = default(TInterface);
                }
            }
        }
        private TInterface _interface;
    }
}