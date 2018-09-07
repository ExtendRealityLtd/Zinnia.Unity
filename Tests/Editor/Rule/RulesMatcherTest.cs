using VRTK.Core.Rule;

namespace Test.VRTK.Core.Rule
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.VRTK.Core.Utility.Mock;

    public class RulesMatcherTest
    {
        private GameObject containingObject;
        private RulesMatcher subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<RulesMatcher>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void Match()
        {
            GameObject objectOne = new GameObject();
            GameObject objectTwo = new GameObject();
            UnityEventListenerMock ruleOneMatched = new UnityEventListenerMock();
            UnityEventListenerMock ruleTwoMatched = new UnityEventListenerMock();

            RulesMatcher.Element elementOne = new RulesMatcher.Element() { rule = CreateRule(objectOne) };
            RulesMatcher.Element elementTwo = new RulesMatcher.Element() { rule = CreateRule(objectTwo) };

            elementOne.Matched.AddListener(ruleOneMatched.Listen);
            elementTwo.Matched.AddListener(ruleTwoMatched.Listen);

            subject.elements.Add(elementOne);
            subject.elements.Add(elementTwo);

            Assert.IsFalse(ruleOneMatched.Received);
            Assert.IsFalse(ruleTwoMatched.Received);

            subject.Match(objectOne);

            Assert.IsTrue(ruleOneMatched.Received);
            Assert.IsFalse(ruleTwoMatched.Received);

            ruleOneMatched.Reset();
            ruleTwoMatched.Reset();

            subject.Match(objectTwo);

            Assert.IsFalse(ruleOneMatched.Received);
            Assert.IsTrue(ruleTwoMatched.Received);

            Object.DestroyImmediate(objectOne);
            Object.DestroyImmediate(objectTwo);
        }

        [Test]
        public void MatchMultiple()
        {
            GameObject objectOne = new GameObject();
            GameObject objectTwo = new GameObject();
            UnityEventListenerMock ruleOneMatched = new UnityEventListenerMock();
            UnityEventListenerMock ruleTwoMatched = new UnityEventListenerMock();
            UnityEventListenerMock ruleThreeMatched = new UnityEventListenerMock();

            RulesMatcher.Element elementOne = new RulesMatcher.Element() { rule = CreateRule(objectOne) };
            RulesMatcher.Element elementTwo = new RulesMatcher.Element() { rule = CreateRule(objectTwo) };
            RulesMatcher.Element elementThree = new RulesMatcher.Element() { rule = CreateRule(objectOne) };

            elementOne.Matched.AddListener(ruleOneMatched.Listen);
            elementTwo.Matched.AddListener(ruleTwoMatched.Listen);
            elementThree.Matched.AddListener(ruleThreeMatched.Listen);

            subject.elements.Add(elementOne);
            subject.elements.Add(elementTwo);
            subject.elements.Add(elementThree);

            Assert.IsFalse(ruleOneMatched.Received);
            Assert.IsFalse(ruleTwoMatched.Received);
            Assert.IsFalse(ruleThreeMatched.Received);

            subject.Match(objectOne);

            Assert.IsTrue(ruleOneMatched.Received);
            Assert.IsFalse(ruleTwoMatched.Received);
            Assert.IsTrue(ruleThreeMatched.Received);

            Object.DestroyImmediate(objectOne);
            Object.DestroyImmediate(objectTwo);
        }

        [Test]
        public void MatchInactiveGameObject()
        {
            GameObject objectOne = new GameObject();
            GameObject objectTwo = new GameObject();
            UnityEventListenerMock ruleOneMatched = new UnityEventListenerMock();
            UnityEventListenerMock ruleTwoMatched = new UnityEventListenerMock();

            RulesMatcher.Element elementOne = new RulesMatcher.Element() { rule = CreateRule(objectOne) };
            RulesMatcher.Element elementTwo = new RulesMatcher.Element() { rule = CreateRule(objectTwo) };

            elementOne.Matched.AddListener(ruleOneMatched.Listen);
            elementTwo.Matched.AddListener(ruleTwoMatched.Listen);

            subject.elements.Add(elementOne);
            subject.elements.Add(elementTwo);

            subject.gameObject.SetActive(false);

            Assert.IsFalse(ruleOneMatched.Received);
            Assert.IsFalse(ruleTwoMatched.Received);

            subject.Match(objectOne);

            Assert.IsFalse(ruleOneMatched.Received);
            Assert.IsFalse(ruleTwoMatched.Received);

            Object.DestroyImmediate(objectOne);
            Object.DestroyImmediate(objectTwo);
        }

        [Test]
        public void MatchInactiveComponent()
        {
            GameObject objectOne = new GameObject();
            GameObject objectTwo = new GameObject();
            UnityEventListenerMock ruleOneMatched = new UnityEventListenerMock();
            UnityEventListenerMock ruleTwoMatched = new UnityEventListenerMock();

            RulesMatcher.Element elementOne = new RulesMatcher.Element() { rule = CreateRule(objectOne) };
            RulesMatcher.Element elementTwo = new RulesMatcher.Element() { rule = CreateRule(objectTwo) };

            elementOne.Matched.AddListener(ruleOneMatched.Listen);
            elementTwo.Matched.AddListener(ruleTwoMatched.Listen);

            subject.elements.Add(elementOne);
            subject.elements.Add(elementTwo);

            subject.enabled = false;

            Assert.IsFalse(ruleOneMatched.Received);
            Assert.IsFalse(ruleTwoMatched.Received);

            subject.Match(objectOne);

            Assert.IsFalse(ruleOneMatched.Received);
            Assert.IsFalse(ruleTwoMatched.Received);

            Object.DestroyImmediate(objectOne);
            Object.DestroyImmediate(objectTwo);
        }

        protected virtual RuleContainer CreateRule(GameObject element)
        {
            RuleContainer container = new RuleContainer();
            ListContainsRule rule = containingObject.AddComponent<ListContainsRule>();
            rule.objects.Add(element);
            container.Interface = rule;
            return container;
        }
    }
}