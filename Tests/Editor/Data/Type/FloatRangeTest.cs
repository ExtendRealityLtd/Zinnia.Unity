using Zinnia.Data.Type;

namespace Test.Zinnia.Data.Type
{
    using UnityEngine;
    using NUnit.Framework;
    using Assert = UnityEngine.Assertions.Assert;

    public class FloatRangeTest
    {
        [Test]
        public void DefaultConstructor()
        {
            FloatRange range = new FloatRange();
            Assert.AreEqual(0f, range.minimum);
            Assert.AreEqual(0f, range.maximum);
        }

        [Test]
        public void ConstructFromFloats()
        {
            FloatRange range = new FloatRange(1f, 2f);
            Assert.AreEqual(1f, range.minimum);
            Assert.AreEqual(2f, range.maximum);
        }

        [Test]
        public void ConstructFromVector2()
        {
            FloatRange range = new FloatRange(new Vector2(1f, 2f));
            Assert.AreEqual(1f, range.minimum);
            Assert.AreEqual(2f, range.maximum);
        }

        [Test]
        public void ConstructFromStaticMinMax()
        {
            FloatRange range = FloatRange.MinMax;
            Assert.AreEqual(float.MinValue, range.minimum);
            Assert.AreEqual(float.MaxValue, range.maximum);
        }

        [Test]
        public void Contains()
        {
            FloatRange range = new FloatRange(0.3f, 0.8f);
            Assert.IsFalse(range.Contains(0f));
            Assert.IsFalse(range.Contains(1f));
            Assert.IsTrue(range.Contains(0.3f));
            Assert.IsTrue(range.Contains(0.5f));
            Assert.IsTrue(range.Contains(0.8f));

            range.minimum = 1f;
            range.maximum = 2f;

            Assert.IsFalse(range.Contains(0.3f));
            Assert.IsFalse(range.Contains(0.5f));
            Assert.IsFalse(range.Contains(0.8f));
            Assert.IsTrue(range.Contains(1f));
            Assert.IsTrue(range.Contains(1.5f));
            Assert.IsTrue(range.Contains(2f));

            range.minimum = 2f;
            range.maximum = 1f;

            Assert.IsFalse(range.Contains(1f));
            Assert.IsFalse(range.Contains(1.5f));
            Assert.IsFalse(range.Contains(2f));

            range.minimum = -2f;
            range.maximum = -1f;

            Assert.IsTrue(range.Contains(-1f));
            Assert.IsTrue(range.Contains(-1.5f));
            Assert.IsTrue(range.Contains(-2f));
        }

        [Test]
        public void ToVector2()
        {
            FloatRange range = new FloatRange(1f, 2f);
            Assert.AreEqual(new Vector2(1f, 2f), range.ToVector2());
        }
    }
}