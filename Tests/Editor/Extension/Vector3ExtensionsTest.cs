using Zinnia.Extension;

namespace Test.Zinnia.Extension
{
    using NUnit.Framework;
    using UnityEngine;
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

        [Test]
        public void WithinDistance()
        {
            Vector3 a = new Vector3(0f, 0f, 0f);
            Vector3 b = new Vector3(0f, 0f, 0f);
            Vector3 tolerance = new Vector3(0.2f, 0.3f, 0.4f);

            Assert.IsTrue(a.WithinDistance(b, tolerance));

            b = new Vector3(0.1f, 0.1f, 0.1f);
            Assert.IsTrue(a.WithinDistance(b, tolerance));

            b = new Vector3(0.2f, 0.2f, 0.2f);
            Assert.IsTrue(a.WithinDistance(b, tolerance));

            b = new Vector3(0.2f, 0.3f, 0.3f);
            Assert.IsTrue(a.WithinDistance(b, tolerance));

            b = new Vector3(0.3f, 0.3f, 0.3f);
            Assert.IsFalse(a.WithinDistance(b, tolerance));

            b = new Vector3(0.3f, 0.4f, 0.4f);
            Assert.IsFalse(a.WithinDistance(b, tolerance));

            b = new Vector3(0.3f, 0.4f, 0.5f);
            Assert.IsFalse(a.WithinDistance(b, tolerance));
        }

        [Test]
        public void UnsignedEulerToSignedEuler()
        {
            Assert.AreEqual(new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f).UnsignedEulerToSignedEuler());
            Assert.AreEqual(new Vector3(90f, 90f, 90f), new Vector3(90f, 90f, 90f).UnsignedEulerToSignedEuler());
            Assert.AreEqual(new Vector3(179f, 179f, 179f), new Vector3(179f, 179f, 179f).UnsignedEulerToSignedEuler());
            Assert.AreEqual(new Vector3(180f, 180f, 180f), new Vector3(180f, 180f, 180f).UnsignedEulerToSignedEuler());
            Assert.AreEqual(new Vector3(-179f, -179f, -179f), new Vector3(181f, 181f, 181f).UnsignedEulerToSignedEuler());
            Assert.AreEqual(new Vector3(-90f, -90f, -90f), new Vector3(270f, 270f, 270f).UnsignedEulerToSignedEuler());
            Assert.AreEqual(new Vector3(0f, 0f, 0f), new Vector3(360f, 360f, 360f).UnsignedEulerToSignedEuler());
        }
    }
}