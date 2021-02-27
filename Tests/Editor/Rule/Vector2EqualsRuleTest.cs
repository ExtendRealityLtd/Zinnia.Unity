using Zinnia.Extension;
using Zinnia.Rule;
using BaseRule = Zinnia.Rule.Rule;

namespace Test.Zinnia.Rule
{
    using NUnit.Framework;
    using UnityEngine;
    using Assert = UnityEngine.Assertions.Assert;

    public class Vector2EqualsRuleTest
    {
        private GameObject containingObject;
        private RuleContainer container;
        private Vector2EqualsRule subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            container = new RuleContainer();
            subject = containingObject.AddComponent<Vector2EqualsRule>();
            container.Interface = subject;
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void AcceptsExactMatch()
        {
            Vector2 toFind = new Vector2(1f, 2f);
            subject.Target = new Vector2(1f, 2f);
            Assert.IsTrue(container.Accepts(toFind));
        }

        [Test]
        public void RefuseNoMatch()
        {
            Vector2 toFind = new Vector2(1f, 2f);
            subject.Target = new Vector2(2f, 2f);
            Assert.IsFalse(container.Accepts(toFind));
        }

        [Test]
        public void AcceptsNearMatch()
        {
            Vector2 toFind = new Vector2(1f, 2f);
            subject.Target = new Vector2(2f, 2f);
            subject.Tolerance = 1f;
            Assert.IsTrue(container.Accepts(toFind));
        }

        [Test]
        public void RefuseTooFar()
        {
            Vector2 toFind = new Vector2(1f, 2f);
            subject.Target = new Vector2(3f, 3f);
            subject.Tolerance = 1f;
            Assert.IsFalse(container.Accepts(toFind));
        }

        [Test]
        public void RefuseInactiveGameObject()
        {
            Vector2 toFind = new Vector2(1f, 2f);
            subject.Target = new Vector2(1f, 2f);
            subject.gameObject.SetActive(false);

            Assert.IsFalse(container.Accepts(toFind));
        }

        [Test]
        public void RefuseInactiveComponent()
        {
            Vector2 toFind = new Vector2(1f, 2f);
            subject.Target = new Vector2(1f, 2f);
            subject.enabled = false;

            Assert.IsFalse(container.Accepts(toFind));
        }

        [Test]
        public void AcceptsInactiveGameObject()
        {
            Vector2 toFind = new Vector2(1f, 2f);
            subject.Target = new Vector2(1f, 2f);
            subject.AutoRejectStates = BaseRule.RejectRuleStates.RuleComponentIsDisabled;

            subject.gameObject.SetActive(false);
            Assert.IsTrue(container.Accepts(toFind));
        }

        [Test]
        public void AcceptsInactiveComponent()
        {
            Vector2 toFind = new Vector2(1f, 2f);
            subject.Target = new Vector2(1f, 2f);
            subject.AutoRejectStates = BaseRule.RejectRuleStates.RuleGameObjectIsNotActiveInHierarchy;

            subject.enabled = false;
            Assert.IsTrue(container.Accepts(toFind));
        }
    }
}