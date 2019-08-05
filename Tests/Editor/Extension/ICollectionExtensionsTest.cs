using Zinnia.Extension;

namespace Test.Zinnia.Extension
{
    using NUnit.Framework;
    using System.Collections.Generic;
    using Assert = UnityEngine.Assertions.Assert;

    public class ICollectionExtensionsTest
    {
        [Test]
        public void GetClampedIndex()
        {
            List<string> list = new List<string>() { "A", "B", "C", "D", "E" };

            Assert.AreEqual(1, list.ClampIndex(1));
            Assert.AreEqual(4, list.ClampIndex(-1));
            Assert.AreEqual(3, list.ClampIndex(-2));
            Assert.AreEqual(4, list.ClampIndex(7));
            Assert.AreEqual(0, list.ClampIndex(-7));
            Assert.AreEqual(0, list.ClampIndex(-2, false));
        }
    }
}