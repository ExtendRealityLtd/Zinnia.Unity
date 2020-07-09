using Zinnia.Extension;

namespace Test.Zinnia.Extension
{
    using NUnit.Framework;
    using Assert = UnityEngine.Assertions.Assert;

    public class EnumExtensionsTest
    {
        protected enum Test
        {
            First,
            Second,
            Third
        }

        [Test]
        public void GetByIndex()
        {
            Assert.AreEqual(Test.First, EnumExtensions.GetByIndex<Test>(-1));
            Assert.AreEqual(Test.First, EnumExtensions.GetByIndex<Test>(0));
            Assert.AreEqual(Test.Second, EnumExtensions.GetByIndex<Test>(1));
            Assert.AreEqual(Test.Third, EnumExtensions.GetByIndex<Test>(2));
            Assert.AreEqual(Test.Third, EnumExtensions.GetByIndex<Test>(3));
        }

        [Test]
        public void GetByString()
        {
            Assert.AreEqual(Test.First, EnumExtensions.GetByString<Test>("first"));
            Assert.AreEqual(Test.First, EnumExtensions.GetByString<Test>("First"));
            Assert.AreEqual(Test.Second, EnumExtensions.GetByString<Test>("second"));
            Assert.AreEqual(Test.Second, EnumExtensions.GetByString<Test>("Second"));
            Assert.AreEqual(Test.Third, EnumExtensions.GetByString<Test>("third"));
            Assert.AreEqual(Test.Third, EnumExtensions.GetByString<Test>("Third"));
            NUnit.Framework.Assert.Throws<System.ArgumentException>(() => EnumExtensions.GetByString<Test>("Fourth"));
        }
    }
}