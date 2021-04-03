using Zinnia.Data.Type;

namespace Test.Zinnia.Data.Type
{
    using NUnit.Framework;
    using UnityEngine;
    using Assert = UnityEngine.Assertions.Assert;

    public class IntRangeTest
    {
        [Test]
        public void DefaultConstructor()
        {
            IntRange range = new IntRange();
            Assert.AreEqual(0, range.minimum);
            Assert.AreEqual(0, range.maximum);
        }

        [Test]
        public void ConstructFromInts()
        {
            IntRange range = new IntRange(1, 2);
            Assert.AreEqual(1, range.minimum);
            Assert.AreEqual(2, range.maximum);
        }

        [Test]
        public void ConstructFromFloatRange()
        {
            IntRange range = new IntRange(new FloatRange(1.1f, 2.1f));
            Assert.AreEqual(1, range.minimum);
            Assert.AreEqual(2, range.maximum);
        }

        [Test]
        public void ConstructFromVector2()
        {
            IntRange range = new IntRange(new Vector2(1f, 2f));
            Assert.AreEqual(1, range.minimum);
            Assert.AreEqual(2, range.maximum);
        }

        [Test]
        public void ConstructFromStaticMinMax()
        {
            IntRange range = IntRange.MinMax;
            Assert.AreEqual(int.MinValue, range.minimum);
            Assert.AreEqual(int.MaxValue, range.maximum);
        }

        [Test]
        public void Contains()
        {
            IntRange range = new IntRange(3, 8);
            Assert.IsFalse(range.Contains(0));
            Assert.IsFalse(range.Contains(1));
            Assert.IsFalse(range.Contains(9));
            Assert.IsTrue(range.Contains(3));
            Assert.IsTrue(range.Contains(5));
            Assert.IsTrue(range.Contains(8));

            range.minimum = 4;
            range.maximum = 7;

            Assert.IsFalse(range.Contains(1));
            Assert.IsFalse(range.Contains(3));
            Assert.IsFalse(range.Contains(8));
            Assert.IsTrue(range.Contains(4));
            Assert.IsTrue(range.Contains(5));
            Assert.IsTrue(range.Contains(6));

            range.minimum = 7;
            range.maximum = 4;

            Assert.IsFalse(range.Contains(4));
            Assert.IsFalse(range.Contains(5));
            Assert.IsFalse(range.Contains(6));

            range.minimum = -7;
            range.maximum = -3;

            Assert.IsTrue(range.Contains(-4));
            Assert.IsTrue(range.Contains(-4));
            Assert.IsTrue(range.Contains(-5));
            Assert.IsFalse(range.Contains(-1));
            Assert.IsFalse(range.Contains(-8));
            Assert.IsFalse(range.Contains(1));
        }

        [Test]
        public void ToVector2()
        {
            IntRange range = new IntRange(1, 2);
            Assert.AreEqual(new Vector2(1f, 2f), range.ToVector2());
        }

        [Test]
        public void Comparison()
        {
            IntRange rangeA = new IntRange(1, 1);
            IntRange rangeB = new IntRange(1, 1);
            IntRange rangeC = new IntRange(2, 2);

            Assert.IsTrue(rangeA.Equals(rangeB));
            Assert.IsFalse(rangeA.Equals(rangeC));
            Assert.IsFalse(rangeB.Equals(rangeC));
            Assert.AreEqual(rangeA, rangeB);
            Assert.AreNotEqual(rangeA, rangeC);
            Assert.AreNotEqual(rangeB, rangeC);
        }

        [Test]
        public void ConvertToString()
        {
            IntRange rangeA = new IntRange(1, 1);
            Assert.AreEqual("{ minimum = 1 | maximum = 1 }", rangeA.ToString());
        }
    }
}