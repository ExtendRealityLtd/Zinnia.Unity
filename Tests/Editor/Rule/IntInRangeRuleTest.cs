using Zinnia.Data.Type;
using Zinnia.Extension;
using Zinnia.Rule;
using BaseRule = Zinnia.Rule.Rule;

namespace Test.Zinnia.Rule
{
    using NUnit.Framework;
    using UnityEngine;
    using Assert = UnityEngine.Assertions.Assert;

    public class IntInRangeRuleTest
    {
        private GameObject containingObject;
        private RuleContainer container;
        private IntInRangeRule subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            container = new RuleContainer();
            subject = containingObject.AddComponent<IntInRangeRule>();
            container.Interface = subject;
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void AcceptsInRange()
        {
            int toFind = 3;
            subject.Range = new IntRange(1, 4);
            Assert.IsTrue(container.Accepts(toFind));
        }

        [Test]
        public void RefuseOutOfRange()
        {
            int toFind = 5;
            subject.Range = new IntRange(1, 4);
            Assert.IsFalse(container.Accepts(toFind));
        }

        [Test]
        public void RefuseInactiveGameObject()
        {
            int toFind = 3;
            subject.Range = new IntRange(1, 4);
            subject.gameObject.SetActive(false);

            Assert.IsFalse(container.Accepts(toFind));
        }

        [Test]
        public void RefuseInactiveComponent()
        {
            int toFind = 3;
            subject.Range = new IntRange(1, 4);
            subject.enabled = false;

            Assert.IsFalse(container.Accepts(toFind));
        }

        [Test]
        public void AcceptsInactiveGameObject()
        {
            int toFind = 3;
            subject.Range = new IntRange(1, 4);
            subject.AutoRejectStates = BaseRule.RejectRuleStates.RuleComponentIsDisabled;

            subject.gameObject.SetActive(false);
            Assert.IsTrue(container.Accepts(toFind));
        }

        [Test]
        public void AcceptsInactiveComponent()
        {
            int toFind = 3;
            subject.Range = new IntRange(1, 4);
            subject.AutoRejectStates = BaseRule.RejectRuleStates.RuleGameObjectIsNotActiveInHierarchy;

            subject.enabled = false;
            Assert.IsTrue(container.Accepts(toFind));
        }
    }
}