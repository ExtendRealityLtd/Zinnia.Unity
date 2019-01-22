using Zinnia.Extension;
using Zinnia.Rule;

namespace Test.Zinnia.Rule
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.Zinnia.Utility.Stub;

    public class AnyRuleTest
    {
        private GameObject containingObject;
        private RuleContainer container;
        private AnyRule subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            container = new RuleContainer();
            subject = containingObject.AddComponent<AnyRule>();
            container.Interface = subject;
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void AcceptsMatch()
        {
            subject.rules.Add(
                new RuleContainer
                {
                    Interface = new TrueRuleStub()
                });
            subject.rules.Add(
                new RuleContainer
                {
                    Interface = new FalseRuleStub()
                });
            Assert.IsTrue(container.Accepts(containingObject));
        }

        [Test]
        public void RefusesEmpty()
        {
            Assert.IsFalse(container.Accepts(containingObject));
        }

        [Test]
        public void RefusesNonEmpty()
        {
            subject.rules.Add(
                new RuleContainer
                {
                    Interface = new FalseRuleStub()
                });
            subject.rules.Add(
                new RuleContainer
                {
                    Interface = new FalseRuleStub()
                });
            Assert.IsFalse(container.Accepts(containingObject));
        }
    }
}