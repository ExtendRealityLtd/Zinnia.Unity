using Zinnia.Rule;
using Zinnia.Rule.Collection;
using Zinnia.Extension;

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
            RuleContainerObservableList rules = containingObject.AddComponent<RuleContainerObservableList>();
            subject.Rules = rules;

            rules.Add(
                new RuleContainer
                {
                    Interface = new TrueRuleStub()
                });
            rules.Add(
                new RuleContainer
                {
                    Interface = new FalseRuleStub()
                });

            Assert.IsTrue(container.Accepts(containingObject));
        }

        [Test]
        public void RefusesEmpty()
        {
            RuleContainerObservableList rules = containingObject.AddComponent<RuleContainerObservableList>();
            subject.Rules = rules;

            Assert.IsFalse(container.Accepts(containingObject));
        }

        [Test]
        public void RefusesNullRules()
        {
            Assert.IsFalse(container.Accepts(containingObject));
        }

        [Test]
        public void RefusesNonEmpty()
        {
            RuleContainerObservableList rules = containingObject.AddComponent<RuleContainerObservableList>();
            subject.Rules = rules;

            rules.Add(
                new RuleContainer
                {
                    Interface = new FalseRuleStub()
                });
            rules.Add(
                new RuleContainer
                {
                    Interface = new FalseRuleStub()
                });

            Assert.IsFalse(container.Accepts(containingObject));
        }

        [Test]
        public void RefusesInactiveGameObject()
        {
            RuleContainerObservableList rules = containingObject.AddComponent<RuleContainerObservableList>();
            subject.Rules = rules;

            rules.Add(
                new RuleContainer
                {
                    Interface = new TrueRuleStub()
                });
            rules.Add(
                new RuleContainer
                {
                    Interface = new FalseRuleStub()
                });

            subject.gameObject.SetActive(false);

            Assert.IsFalse(container.Accepts(containingObject));
        }

        [Test]
        public void RefusesInactiveComponent()
        {
            RuleContainerObservableList rules = containingObject.AddComponent<RuleContainerObservableList>();
            subject.Rules = rules;

            rules.Add(
                new RuleContainer
                {
                    Interface = new TrueRuleStub()
                });
            rules.Add(
                new RuleContainer
                {
                    Interface = new FalseRuleStub()
                });

            subject.enabled = false;

            Assert.IsFalse(container.Accepts(containingObject));
        }
    }
}