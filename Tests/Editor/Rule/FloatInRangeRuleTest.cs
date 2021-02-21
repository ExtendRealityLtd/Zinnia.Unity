using Zinnia.Data.Type;
using Zinnia.Extension;
using Zinnia.Rule;
using BaseRule = Zinnia.Rule.Rule;

namespace Test.Zinnia.Rule
{
    using NUnit.Framework;
    using UnityEngine;
    using Assert = UnityEngine.Assertions.Assert;

    public class FloatInRangeRuleTest
    {
        private GameObject containingObject;
        private RuleContainer container;
        private FloatInRangeRule subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            container = new RuleContainer();
            subject = containingObject.AddComponent<FloatInRangeRule>();
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
            float toFind = 3f;
            subject.Range = new FloatRange(1f, 4f);
            Assert.IsTrue(container.Accepts(toFind));
        }

        [Test]
        public void RefuseOutOfRange()
        {
            float toFind = 5f;
            subject.Range = new FloatRange(1f, 4f);
            Assert.IsFalse(container.Accepts(toFind));
        }

        [Test]
        public void RefuseInactiveGameObject()
        {
            float toFind = 3f;
            subject.Range = new FloatRange(1f, 4f);
            subject.gameObject.SetActive(false);

            Assert.IsFalse(container.Accepts(toFind));
        }

        [Test]
        public void RefuseInactiveComponent()
        {
            float toFind = 3f;
            subject.Range = new FloatRange(1f, 4f);
            subject.enabled = false;

            Assert.IsFalse(container.Accepts(toFind));
        }

        [Test]
        public void AcceptsInactiveGameObject()
        {
            float toFind = 3f;
            subject.Range = new FloatRange(1f, 4f);
            subject.AutoRejectStates = BaseRule.RejectRuleStates.RuleComponentIsDisabled;

            subject.gameObject.SetActive(false);
            Assert.IsTrue(container.Accepts(toFind));
        }

        [Test]
        public void AcceptsInactiveComponent()
        {
            float toFind = 3f;
            subject.Range = new FloatRange(1f, 4f);
            subject.AutoRejectStates = BaseRule.RejectRuleStates.RuleGameObjectIsNotActiveInHierarchy;

            subject.enabled = false;
            Assert.IsTrue(container.Accepts(toFind));
        }
    }
}