namespace Zinnia.Data.Type
{
    using UnityEngine;
    using System;

    /// <summary>
    /// Specifies a serializable data type.
    /// </summary>
    [Serializable]
    public class SerializableType
    {
        /// <summary>
        /// The actual <see cref="Type"/> of the held serializable type.
        /// </summary>
        public Type ActualType
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(assemblyQualifiedTypeName) && !assemblyQualifiedTypeName.Equals(currentAssemblyQualifiedTypeName))
                {
                    ActualType = Type.GetType(assemblyQualifiedTypeName);
                    currentAssemblyQualifiedTypeName = assemblyQualifiedTypeName;
                }

                return actualType;
            }
            protected set
            {
                actualType = value;
                assemblyQualifiedTypeName = value.AssemblyQualifiedName;
            }
        }
        private Type actualType;

        /// <summary>
        /// The string equivalent of the type name in full assembly qualified format.
        /// </summary>
        [SerializeField]
        private string assemblyQualifiedTypeName;
        /// <summary>
        /// The current set string equivalent of the type name.
        /// </summary>
        private string currentAssemblyQualifiedTypeName;

        /// <summary>
        /// Allows conversion of <see cref="SerializableType"/> to <see cref="Type"/>.
        /// </summary>
        /// <param name="serializableType">The item to convert.</param>
        public static implicit operator Type(SerializableType serializableType)
        {
            return serializableType.ActualType;
        }

        /// <summary>
        /// Allows conversion of <see cref="Type"/> to <see cref="SerializableType"/>.
        /// </summary>
        /// <param name="type">The item to convert.</param>
        public static implicit operator SerializableType(Type type)
        {
            return new SerializableType
            {
                actualType = type,
                assemblyQualifiedTypeName = type.AssemblyQualifiedName
            };
        }
    }
}