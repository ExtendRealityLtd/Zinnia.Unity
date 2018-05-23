namespace VRTK.Core.Extension
{
    using UnityEngine;
    using NUnit.Framework;

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
    }
}