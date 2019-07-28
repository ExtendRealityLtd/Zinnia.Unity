using Zinnia.Extension;
using Zinnia.Rule;

namespace Test.Zinnia.Extension
{
    using NUnit.Framework;
    using Assert = UnityEngine.Assertions.Assert;

    public class RuleContainerExtensionsTest
    {
        private RuleContainer subject;
        private readonly object argument = new object();

        [SetUp]
        public void SetUp()
        {
            subject = new RuleContainer();
        }

        [Test]
        public void AcceptsOnNullContainer()
        {
            subject = null;
            Assert.IsTrue(subject.Accepts(argument));
        }

        [Test]
        public void AcceptsOnNull()
        {
            subject.Interface = null;
            Assert.IsTrue(subject.Accepts(argument));
        }

        [Test]
        public void AcceptsValid()
        {
            subject.Interface = new RuleMock(true);
            Assert.IsTrue(subject.Accepts(argument));
        }

        [Test]
        public void RefusesInvalid()
        {
            subject.Interface = new RuleMock(false);
            Assert.IsFalse(subject.Accepts(argument));
        }

        private class RuleMock : IRule
        {
            public readonly bool accepts;

            public RuleMock(bool accepts)
            {
                this.accepts = accepts;
            }

            public bool Accepts(object target)
            {
                return accepts;
            }
        }
    }
}