namespace VRTK.Core.Extension
{
    using UnityEngine;
    using NUnit.Framework;

    public class Vector3ExtensionsTest
    {
        [Test]
        public void CompareTrue()
        {
            Vector3 a = Vector3.zero;
            Vector3 b = Vector3.zero;
            float distance = 0f;

            Assert.IsTrue(a.Compare(b, distance));

            distance = 1f;
            Assert.IsTrue(a.Compare(b, distance));

            b = Vector3.one * 0.5f;
            Assert.IsTrue(a.Compare(b, distance));
        }

        [Test]
        public void CompareFalse()
        {
            Vector3 a = Vector3.zero;
            Vector3 b = Vector3.one;
            float distance = 0f;

            Assert.IsFalse(a.Compare(b, distance));

            distance = 1f;
            Assert.IsFalse(a.Compare(b, distance));
        }
    }
}