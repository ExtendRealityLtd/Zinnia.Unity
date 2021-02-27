using Zinnia.Extension;
using Zinnia.Rule;
using BaseRule = Zinnia.Rule.Rule;

namespace Test.Zinnia.Rule
{
    using NUnit.Framework;
    using UnityEngine;
    using Assert = UnityEngine.Assertions.Assert;

    public class StringMatchesRuleTest
    {
        private GameObject containingObject;
        private RuleContainer container;
        private StringMatchesRule subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            container = new RuleContainer();
            subject = containingObject.AddComponent<StringMatchesRule>();
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
            string toFind = "test";
            subject.TargetPattern = "test";
            Assert.IsTrue(container.Accepts(toFind));
        }

        [Test]
        public void RefuseNoMatch()
        {
            string toFind = "test";
            subject.TargetPattern = "nottest";
            Assert.IsFalse(container.Accepts(toFind));
        }

        [Test]
        public void AcceptsFuzzyMatch()
        {
            string toFind = "one two three";
            subject.TargetPattern = "^one.*$";
            Assert.IsTrue(container.Accepts(toFind));
        }

        [Test]
        public void RefuseNotFoundFuzzy()
        {
            string toFind = "one two three";
            subject.TargetPattern = "^.*one$";
            Assert.IsFalse(container.Accepts(toFind));
        }

        [Test]
        public void RefuseInactiveGameObject()
        {
            string toFind = "test";
            subject.TargetPattern = "test";
            subject.gameObject.SetActive(false);

            Assert.IsFalse(container.Accepts(toFind));
        }

        [Test]
        public void RefuseInactiveComponent()
        {
            string toFind = "test";
            subject.TargetPattern = "test";
            subject.enabled = false;

            Assert.IsFalse(container.Accepts(toFind));
        }

        [Test]
        public void AcceptsInactiveGameObject()
        {
            string toFind = "test";
            subject.TargetPattern = "test";
            subject.AutoRejectStates = BaseRule.RejectRuleStates.RuleComponentIsDisabled;

            subject.gameObject.SetActive(false);
            Assert.IsTrue(container.Accepts(toFind));
        }

        [Test]
        public void AcceptsInactiveComponent()
        {
            string toFind = "test";
            subject.TargetPattern = "test";
            subject.AutoRejectStates = BaseRule.RejectRuleStates.RuleGameObjectIsNotActiveInHierarchy;

            subject.enabled = false;
            Assert.IsTrue(container.Accepts(toFind));
        }
    }
}