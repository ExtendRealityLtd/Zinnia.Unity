using VRTK.Core.Extension;

namespace Test.VRTK.Core.Extension
{
    using NUnit.Framework;
    using System.Collections.Generic;

    public class ICollectionExtensionsTest
    {
        [Test]
        public void GetWrappedAndClampedIndex()
        {
            List<string> list = new List<string>() { "A", "B", "C", "D", "E" };

            Assert.AreEqual(1, list.GetWrappedAndClampedIndex(1));
            Assert.AreEqual(4, list.GetWrappedAndClampedIndex(-1));
            Assert.AreEqual(3, list.GetWrappedAndClampedIndex(-2));
            Assert.AreEqual(4, list.GetWrappedAndClampedIndex(7));
            Assert.AreEqual(0, list.GetWrappedAndClampedIndex(-7));
        }
    }
}