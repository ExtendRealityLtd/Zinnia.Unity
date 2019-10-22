using Zinnia.Rule;
using Zinnia.Extension;
using BaseRule = Zinnia.Rule.Rule;

namespace Test.Zinnia.Rule
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.Zinnia.Utility.Stub;
    using Assert = UnityEngine.Assertions.Assert;

    public class NegationRuleTest
    {
        private GameObject containingObject;
        private RuleContainer container;
        private NegationRule subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            container = new RuleContainer();
            subject = containingObject.AddComponent<NegationRule>();
            container.Interface = subject;
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void AcceptsFalse()
        {
            subject.Rule = new RuleContainer
            {
                Interface = new FalseRuleStub()
            };
            Assert.IsTrue(container.Accepts(containingObject));
        }

        [Test]
        public void RefusesEmpty()
        {
            Assert.IsFalse(container.Accepts(containingObject));
        }

        [Test]
        public void RefusesTrue()
        {
            subject.Rule = new RuleContainer
            {
                Interface = new TrueRuleStub()
            };
            Assert.IsFalse(container.Accepts(containingObject));
        }

        [Test]
        public void RefusesInactiveGameObject()
        {
            subject.Rule = new RuleContainer
            {
                Interface = new FalseRuleStub()
            };
            subject.gameObject.SetActive(false);
            Assert.IsFalse(container.Accepts(containingObject));
        }

        [Test]
        public void RefusesInactiveComponent()
        {
            subject.Rule = new RuleContainer
            {
                Interface = new FalseRuleStub()
            };
            subject.enabled = false;
            Assert.IsFalse(container.Accepts(containingObject));
        }

        [Test]
        public void AcceptInactiveGameObject()
        {
            subject.Rule = new RuleContainer
            {
                Interface = new FalseRuleStub()
            };

            subject.AutoRejectStates = BaseRule.RejectRuleStates.RuleComponentIsDisabled;
            subject.gameObject.SetActive(false);

            Assert.IsTrue(container.Accepts(containingObject));

            subject.enabled = false;

            Assert.IsFalse(container.Accepts(containingObject));
        }

        [Test]
        public void AcceptInactiveComponent()
        {
            subject.Rule = new RuleContainer
            {
                Interface = new FalseRuleStub()
            };

            subject.AutoRejectStates = BaseRule.RejectRuleStates.RuleGameObjectIsNotActiveInHierarchy;
            subject.enabled = false;

            Assert.IsTrue(container.Accepts(containingObject));

            subject.gameObject.SetActive(false);

            Assert.IsFalse(container.Accepts(containingObject));
        }
    }
}