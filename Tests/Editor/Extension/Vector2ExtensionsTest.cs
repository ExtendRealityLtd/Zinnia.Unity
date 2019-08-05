using Zinnia.Extension;

namespace Test.Zinnia.Extension
{
    using UnityEngine;
    using NUnit.Framework;
    using Assert = UnityEngine.Assertions.Assert;

    public class Vector2ExtensionsTest
    {
        [Test]
        public void ApproxEqualsTrue()
        {
            Vector2 a = Vector2.zero;
            Vector2 b = Vector2.zero;
            float tolerance = 0f;
            Assert.IsTrue(a.ApproxEquals(b, tolerance));
            tolerance = 1f;
            Assert.IsTrue(a.ApproxEquals(b, tolerance));
            b = Vector2.one * 0.5f;
            Assert.IsTrue(a.ApproxEquals(b, tolerance));
        }

        [Test]
        public void ApproxEqualsFalse()
        {
            Vector2 a = Vector2.zero;
            Vector2 b = Vector2.one;
            float tolerance = 0f;
            Assert.IsFalse(a.ApproxEquals(b, tolerance));
            tolerance = 1f;
            Assert.IsFalse(a.ApproxEquals(b, tolerance));
        }

        [Test]
        public void DivideWithFloat()
        {
            Vector2 a = Vector2.one * 10f;
            Assert.AreEqual(Vector2.one * 0.2f, Vector2Extensions.Divide(2f, a));
            Assert.AreEqual(Vector2.one * 0.5f, Vector2Extensions.Divide(5f, a));
            Assert.AreEqual(Vector2.one, Vector2Extensions.Divide(10f, a));
            Assert.AreEqual(Vector2.zero, Vector2Extensions.Divide(0f, a));
        }

        [Test]
        public void DivideByVector()
        {
            Vector2 a = Vector2.one * 10f;
            Vector2 b = Vector2.one * 2f;
            Assert.AreEqual(Vector2.one * 5f, a.Divide(b));
        }
    }
}