namespace VRTK.Core.Data.Type
{
    using System;
    using UnityEngine;

    [Serializable]
    public class SerializableType
    {
        public Type ActualType
        {
            get
            {
                if (actualType == null && !string.IsNullOrWhiteSpace(assemblyQualifiedTypeName))
                {
                    actualType = Type.GetType(assemblyQualifiedTypeName);
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

        [SerializeField]
        private string assemblyQualifiedTypeName;

        public static implicit operator Type(SerializableType serializableType)
        {
            return serializableType.ActualType;
        }

        public static implicit operator SerializableType(Type type)
        {
            return new SerializableType
            {
                assemblyQualifiedTypeName = type.AssemblyQualifiedName
            };
        }
    }
}