using Zinnia.Extension;

namespace Test.Zinnia.Extension
{
    using UnityEngine;
    using NUnit.Framework;
    using Assert = UnityEngine.Assertions.Assert;

    public class Vector3ExtensionsTest
    {
        [Test]
        public void ApproxEqualsTrue()
        {
            Vector3 a = Vector3.zero;
            Vector3 b = Vector3.zero;
            float tolerance = 0f;
            Assert.IsTrue(a.ApproxEquals(b, tolerance));
            tolerance = 1f;
            Assert.IsTrue(a.ApproxEquals(b, tolerance));
            b = Vector3.one * 0.5f;
            Assert.IsTrue(a.ApproxEquals(b, tolerance));
        }

        [Test]
        public void ApproxEqualsFalse()
        {
            Vector3 a = Vector3.zero;
            Vector3 b = Vector3.one;
            float tolerance = 0f;
            Assert.IsFalse(a.ApproxEquals(b, tolerance));
            tolerance = 1f;
            Assert.IsFalse(a.ApproxEquals(b, tolerance));
        }

        [Test]
        public void DivideWithFloat()
        {
            Vector3 a = Vector3.one * 10f;
            Assert.AreEqual(Vector3.one * 0.2f, Vector3Extensions.Divide(2f, a));
            Assert.AreEqual(Vector3.one * 0.5f, Vector3Extensions.Divide(5f, a));
            Assert.AreEqual(Vector3.one, Vector3Extensions.Divide(10f, a));
            Assert.AreEqual(Vector3.zero, Vector3Extensions.Divide(0f, a));
        }

        [Test]
        public void DivideByVector()
        {
            Vector3 a = Vector3.one * 10f;
            Vector3 b = Vector3.one * 2f;
            Assert.AreEqual(Vector3.one * 5f, a.Divide(b));
        }
    }
}