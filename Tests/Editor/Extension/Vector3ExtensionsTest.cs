using Zinnia.Extension;

namespace Test.Zinnia.Extension
{
    using NUnit.Framework;
    using UnityEngine;
    using UnityEngine.TestTools.Utils;

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
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            Vector3 a = Vector3.one * 10f;
            Assert.That(Vector3Extensions.Divide(2f, a), Is.EqualTo(Vector3.one * 0.2f).Using(comparer));
            Assert.That(Vector3Extensions.Divide(5f, a), Is.EqualTo(Vector3.one * 0.5f).Using(comparer));
            Assert.That(Vector3Extensions.Divide(10f, a), Is.EqualTo(Vector3.one).Using(comparer));
            Assert.That(Vector3Extensions.Divide(0f, a), Is.EqualTo(Vector3.zero).Using(comparer));
        }

        [Test]
        public void DivideByVector()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            Vector3 a = Vector3.one * 10f;
            Vector3 b = Vector3.one * 2f;
            Assert.That(a.Divide(b), Is.EqualTo(Vector3.one * 5f).Using(comparer));
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
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);

            Assert.That(new Vector3(0f, 0f, 0f).UnsignedEulerToSignedEuler(), Is.EqualTo(new Vector3(0f, 0f, 0f)).Using(comparer));
            Assert.That(new Vector3(90f, 90f, 90f).UnsignedEulerToSignedEuler(), Is.EqualTo(new Vector3(90f, 90f, 90f)).Using(comparer));
            Assert.That(new Vector3(179f, 179f, 179f).UnsignedEulerToSignedEuler(), Is.EqualTo(new Vector3(179f, 179f, 179f)).Using(comparer));
            Assert.That(new Vector3(180f, 180f, 180f).UnsignedEulerToSignedEuler(), Is.EqualTo(new Vector3(180f, 180f, 180f)).Using(comparer));
            Assert.That(new Vector3(181f, 181f, 181f).UnsignedEulerToSignedEuler(), Is.EqualTo(new Vector3(-179f, -179f, -179f)).Using(comparer));
            Assert.That(new Vector3(270f, 270f, 270f).UnsignedEulerToSignedEuler(), Is.EqualTo(new Vector3(-90f, -90f, -90f)).Using(comparer));
            Assert.That(new Vector3(360f, 360f, 360f).UnsignedEulerToSignedEuler(), Is.EqualTo(new Vector3(0f, 0f, 0f)).Using(comparer));
        }

        [Test]
        public void Direction()
        {
            Vector3 source = Vector3.zero;
            Vector3 target = new Vector3(1.234f, 3.23f, 2.1234f);
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            Assert.That(Vector3Extensions.Direction(source, target), Is.EqualTo(target).Using(comparer));
            Assert.That(Vector3Extensions.Direction(source, target, true), Is.EqualTo(target.normalized).Using(comparer));
        }
    }
}