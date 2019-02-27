using Zinnia.Extension;
using Zinnia.Rule;

namespace Test.Zinnia.Rule
{
    using UnityEngine;
    using NUnit.Framework;

    public class ListContainsRuleTest
    {
        private GameObject containingObject;
        private RuleContainer container;
        private ListContainsRule subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            container = new RuleContainer();
            subject = containingObject.AddComponent<ListContainsRule>();
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
            subject.objects.Add(containingObject);
            Assert.IsTrue(container.Accepts(containingObject));
        }

        [Test]
        public void RefusesEmpty()
        {
            Assert.IsFalse(container.Accepts(containingObject));
        }

        [Test]
        public void RefusesDifferent()
        {
            GameObject wrongGameObject = new GameObject();
            subject.objects.Add(wrongGameObject);
            Assert.IsFalse(container.Accepts(containingObject));

            Object.DestroyImmediate(wrongGameObject);
        }

        [Test]
        public void RefusesInactiveGameObject()
        {
            subject.objects.Add(containingObject);
            subject.gameObject.SetActive(false);
            Assert.IsFalse(container.Accepts(containingObject));
        }

        [Test]
        public void RefusesInactiveComponent()
        {
            subject.objects.Add(containingObject);
            subject.enabled = false;
            Assert.IsFalse(container.Accepts(containingObject));
        }
    }
}