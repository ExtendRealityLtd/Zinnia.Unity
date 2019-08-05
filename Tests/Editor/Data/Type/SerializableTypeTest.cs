using Zinnia.Data.Type;

namespace Test.Zinnia.Data.Type
{
    using UnityEngine;
    using System;
    using NUnit.Framework;
    using Assert = UnityEngine.Assertions.Assert;

    public class SerializableTypeTest
    {
        [Test]
        public void ConvertFromType()
        {
            Component componentType = new Component();
            SerializableType subject = componentType.GetType();
            Assert.AreEqual(componentType.GetType(), subject.ActualType);
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