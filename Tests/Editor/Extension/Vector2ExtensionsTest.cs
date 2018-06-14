namespace VRTK.Core.Extension
{
    using UnityEngine;
    using NUnit.Framework;

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
    }
}