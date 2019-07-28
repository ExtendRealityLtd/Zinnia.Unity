using Zinnia.Extension;

namespace Test.Zinnia.Extension
{
    using NUnit.Framework;
    using Assert = UnityEngine.Assertions.Assert;

    public class ArraySortExtensionsTest
    {
        [Test]
        public void HeapAllocationFreeSortIsAvailable()
        {
            Assert.IsTrue(ArraySortExtensions<object>.IsHeapAllocationFreeSortAvailable);
        }
    }
}