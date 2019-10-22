using Zinnia.Rule;
using Zinnia.Rule.Collection;
using Zinnia.Extension;
using BaseRule = Zinnia.Rule.Rule;

namespace Test.Zinnia.Rule
{
    using UnityEngine;
    using UnityEngine.TestTools;
    using System.Collections;
    using NUnit.Framework;
    using Test.Zinnia.Utility.Stub;
    using Assert = UnityEngine.Assertions.Assert;

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

        [UnityTest]
        public IEnumerator AcceptsMatch()
        {
            RuleContainerObservableList rules = containingObject.AddComponent<RuleContainerObservableList>();
            yield return null;
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

        [UnityTest]
        public IEnumerator RefusesEmpty()
        {
            RuleContainerObservableList rules = containingObject.AddComponent<RuleContainerObservableList>();
            yield return null;
            subject.Rules = rules;

            Assert.IsFalse(container.Accepts(containingObject));
        }

        [Test]
        public void RefusesNullRules()
        {
            Assert.IsFalse(container.Accepts(containingObject));
        }

        [UnityTest]
        public IEnumerator RefusesNonEmpty()
        {
            RuleContainerObservableList rules = containingObject.AddComponent<RuleContainerObservableList>();
            yield return null;
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

        [UnityTest]
        public IEnumerator RefusesInactiveGameObject()
        {
            RuleContainerObservableList rules = containingObject.AddComponent<RuleContainerObservableList>();
            yield return null;
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

        [UnityTest]
        public IEnumerator RefusesInactiveComponent()
        {
            RuleContainerObservableList rules = containingObject.AddComponent<RuleContainerObservableList>();
            yield return null;
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

        [UnityTest]
        public IEnumerator AcceptInactiveGameObject()
        {
            RuleContainerObservableList rules = containingObject.AddComponent<RuleContainerObservableList>();
            yield return null;
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

            subject.AutoRejectStates = BaseRule.RejectRuleStates.RuleComponentIsDisabled;
            subject.gameObject.SetActive(false);

            Assert.IsTrue(container.Accepts(containingObject));

            subject.enabled = false;

            Assert.IsFalse(container.Accepts(containingObject));
        }

        [UnityTest]
        public IEnumerator AcceptInactiveComponent()
        {
            RuleContainerObservableList rules = containingObject.AddComponent<RuleContainerObservableList>();
            yield return null;
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

            subject.AutoRejectStates = BaseRule.RejectRuleStates.RuleGameObjectIsNotActiveInHierarchy;
            subject.enabled = false;

            Assert.IsTrue(container.Accepts(containingObject));

            subject.gameObject.SetActive(false);

            Assert.IsFalse(container.Accepts(containingObject));
        }
    }
}