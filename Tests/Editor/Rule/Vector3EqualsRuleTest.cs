using Zinnia.Extension;
using Zinnia.Rule;
using BaseRule = Zinnia.Rule.Rule;

namespace Test.Zinnia.Rule
{
    using NUnit.Framework;
    using UnityEngine;
    using Assert = UnityEngine.Assertions.Assert;

    public class Vector3EqualsRuleTest
    {
        private GameObject containingObject;
        private RuleContainer container;
        private Vector3EqualsRule subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            container = new RuleContainer();
            subject = containingObject.AddComponent<Vector3EqualsRule>();
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
            Vector3 toFind = new Vector3(1f, 2f, 1f);
            subject.Target = new Vector3(1f, 2f, 1f);
            Assert.IsTrue(container.Accepts(toFind));
        }

        [Test]
        public void RefuseNoMatch()
        {
            Vector3 toFind = new Vector3(1f, 2f, 1f);
            subject.Target = new Vector3(2f, 2f, 1f);
            Assert.IsFalse(container.Accepts(toFind));
        }

        [Test]
        public void AcceptsNearMatch()
        {
            Vector3 toFind = new Vector3(1f, 2f, 1f);
            subject.Target = new Vector3(2f, 2f, 1f);
            subject.Tolerance = 1f;
            Assert.IsTrue(container.Accepts(toFind));
        }

        [Test]
        public void RefuseTooFar()
        {
            Vector3 toFind = new Vector3(1f, 2f, 1f);
            subject.Target = new Vector3(3f, 3f, 1f);
            subject.Tolerance = 1f;
            Assert.IsFalse(container.Accepts(toFind));
        }

        [Test]
        public void RefuseInactiveGameObject()
        {
            Vector3 toFind = new Vector3(1f, 2f, 1f);
            subject.Target = new Vector3(1f, 2f, 1f);
            subject.gameObject.SetActive(false);

            Assert.IsFalse(container.Accepts(toFind));
        }

        [Test]
        public void RefuseInactiveComponent()
        {
            Vector3 toFind = new Vector3(1f, 2f, 1f);
            subject.Target = new Vector3(1f, 2f, 1f);
            subject.enabled = false;

            Assert.IsFalse(container.Accepts(toFind));
        }

        [Test]
        public void AcceptsInactiveGameObject()
        {
            Vector3 toFind = new Vector3(1f, 2f, 1f);
            subject.Target = new Vector3(1f, 2f, 1f);
            subject.AutoRejectStates = BaseRule.RejectRuleStates.RuleComponentIsDisabled;

            subject.gameObject.SetActive(false);
            Assert.IsTrue(container.Accepts(toFind));
        }

        [Test]
        public void AcceptsInactiveComponent()
        {
            Vector3 toFind = new Vector3(1f, 2f, 1f);
            subject.Target = new Vector3(1f, 2f, 1f);
            subject.AutoRejectStates = BaseRule.RejectRuleStates.RuleGameObjectIsNotActiveInHierarchy;

            subject.enabled = false;
            Assert.IsTrue(container.Accepts(toFind));
        }
    }
}