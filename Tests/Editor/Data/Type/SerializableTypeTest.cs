using Zinnia.Data.Type;

namespace Test.Zinnia.Data.Type
{
    using UnityEngine;
    using System;
    using NUnit.Framework;

    public class SerializableTypeTest
    {
        [Test]
        public void ConstructFromType()
        {
            Component componentType = new Component();
            SerializableType subject = componentType.GetType();
            Assert.AreEqual(componentType.GetType(), subject.ActualType);
        }

        [Test]
        public void ConstructFromString()
        {
            Component componentType = new Component();
            SerializableType subject = Type.GetType("UnityEngine.Component, UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null");
            Assert.AreEqual(componentType.GetType(), subject.ActualType);
        }

        [Test]
        public void ChangeAssemblyQualifiedTypeName()
        {
            Component componentType = new Component();
            FloatRange altType = new FloatRange();

            SerializableType subject = componentType.GetType();

            Assert.AreEqual(componentType.GetType(), subject.ActualType);

            var prop = subject.GetType().GetField("assemblyQualifiedTypeName", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            prop.SetValue(subject, altType.GetType().AssemblyQualifiedName);

            Assert.AreEqual(altType.GetType(), subject.ActualType);
        }

        [Test]
        public void ConvertToType()
        {
            Component componentType = new Component();
            SerializableType subject = componentType.GetType();

            Type convertedType = subject;

            Assert.AreEqual(componentType.GetType(), convertedType);
        }
    }
}