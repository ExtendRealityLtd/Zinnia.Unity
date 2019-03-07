namespace Zinnia.Data.Type
{
    using UnityEngine;
    using System;

    /// <summary>
    /// Specifies a serializable data type.
    /// </summary>
    [Serializable]
    public class SerializableType : ISerializationCallbackReceiver
    {
        /// <summary>
        /// The actual <see cref="Type"/> of the held serializable type.
        /// </summary>
        public Type ActualType { get; set; }

        /// <summary>
        /// The name of the type in full assembly qualified format.
        /// </summary>
        [SerializeField]
        private string assemblyQualifiedTypeName;

        /// <summary>
        /// Converts from <see cref="SerializableType"/> to <see cref="Type"/>.
        /// </summary>
        /// <param name="serializableType">The item to convert.</param>
        public static implicit operator Type(SerializableType serializableType)
        {
            return serializableType.ActualType;
        }

        /// <summary>
        /// Converts from <see cref="Type"/> to <see cref="SerializableType"/>.
        /// </summary>
        /// <param name="type">The item to convert.</param>
        public static implicit operator SerializableType(Type type)
        {
            return new SerializableType
            {
                ActualType = type,
                assemblyQualifiedTypeName = type.AssemblyQualifiedName
            };
        }

        /// <inheritdoc />
        public void OnBeforeSerialize()
        {
            assemblyQualifiedTypeName = ActualType?.AssemblyQualifiedName;
        }

        /// <inheritdoc />
        public void OnAfterDeserialize()
        {
            try
            {
                ActualType = Type.GetType(assemblyQualifiedTypeName);
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
                ActualType = null;
            }
        }
    }
}