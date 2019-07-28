using Zinnia.Extension;

namespace Test.Zinnia.Extension
{
    using NUnit.Framework;
    using Assert = UnityEngine.Assertions.Assert;

    public class FloatExtensionsTest
    {
        [Test]
        public void ApproxEqualsTrue()
        {
            float a = 0f;
            float b = 0f;
            float tolerance = 0f;
            Assert.IsTrue(a.ApproxEquals(b, tolerance));
            tolerance = 1f;
            Assert.IsTrue(a.ApproxEquals(b, tolerance));
            b = 0.5f;
            Assert.IsTrue(a.ApproxEquals(b, tolerance));

            a = .33333f;
            b = (float)1 / 3;
            tolerance = .0001f;

            Assert.IsTrue(a.ApproxEquals(b, tolerance));
        }

        [Test]
        public void ApproxEqualsFalse()
        {
            const float a = 0f;
            const float b = 1f;
            float tolerance = 0f;
            Assert.IsFalse(a.ApproxEquals(b, tolerance));
            tolerance = 0.5f;
            Assert.IsFalse(a.ApproxEquals(b, tolerance));
        }
    }
}