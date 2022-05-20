using Zinnia.Extension;

namespace Test.Zinnia.Extension
{
    using NUnit.Framework;
    using UnityEngine;
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

        [Test]
        public void WithinDistance()
        {
            Vector2 a = new Vector2(0f, 0f);
            Vector2 b = new Vector2(0f, 0f);
            Vector2 tolerance = new Vector2(0.2f, 0.3f);

            Assert.IsTrue(a.WithinDistance(b, tolerance));

            b = new Vector2(0.1f, 0.1f);
            Assert.IsTrue(a.WithinDistance(b, tolerance));

            b = new Vector2(0.2f, 0.2f);
            Assert.IsTrue(a.WithinDistance(b, tolerance));

            b = new Vector2(0.2f, 0.3f);
            Assert.IsTrue(a.WithinDistance(b, tolerance));

            b = new Vector2(0.3f, 0.3f);
            Assert.IsFalse(a.WithinDistance(b, tolerance));

            b = new Vector2(0.3f, 0.4f);
            Assert.IsFalse(a.WithinDistance(b, tolerance));
        }

        [Test]
        public void UnsignedEulerToSignedEuler()
        {
            Assert.AreEqual(new Vector2(0f, 0f), new Vector2(0f, 0f).UnsignedEulerToSignedEuler());
            Assert.AreEqual(new Vector2(90f, 90f), new Vector2(90f, 90f).UnsignedEulerToSignedEuler());
            Assert.AreEqual(new Vector2(179f, 179f), new Vector2(179f, 179f).UnsignedEulerToSignedEuler());
            Assert.AreEqual(new Vector2(180f, 180f), new Vector2(180f, 180f).UnsignedEulerToSignedEuler());
            Assert.AreEqual(new Vector2(-179f, -179f), new Vector2(181f, 181f).UnsignedEulerToSignedEuler());
            Assert.AreEqual(new Vector2(-90f, -90f), new Vector2(270f, 270f).UnsignedEulerToSignedEuler());
            Assert.AreEqual(new Vector2(0f, 0f), new Vector2(360f, 360f).UnsignedEulerToSignedEuler());
        }
    }
}