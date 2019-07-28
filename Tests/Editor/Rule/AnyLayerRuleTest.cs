using Zinnia.Extension;
using Zinnia.Rule;

namespace Test.Zinnia.Rule
{
    using UnityEngine;
    using NUnit.Framework;
    using Assert = UnityEngine.Assertions.Assert;

    public class AnyLayerRuleTest
    {
        private GameObject containingObject;
        private RuleContainer container;
        private AnyLayerRule subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            container = new RuleContainer();
            subject = containingObject.AddComponent<AnyLayerRule>();
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
            containingObject.layer = LayerMask.NameToLayer("UI");
            subject.LayerMask = LayerMask.GetMask("UI");
            Assert.IsTrue(container.Accepts(containingObject));
        }

        [Test]
        public void AcceptsMatchMultipleLayers()
        {
            containingObject.layer = LayerMask.NameToLayer("UI") | LayerMask.NameToLayer("Water");
            subject.LayerMask = LayerMask.GetMask("UI");
            Assert.IsTrue(container.Accepts(containingObject));
        }

        [Test]
        public void AcceptsMatchMultipleLayerMask()
        {
            containingObject.layer = LayerMask.NameToLayer("UI");
            subject.LayerMask = LayerMask.GetMask("UI") | LayerMask.GetMask("Water");
            Assert.IsTrue(container.Accepts(containingObject));
        }

        [Test]
        public void RefusesEmpty()
        {
            containingObject.layer = LayerMask.NameToLayer("UI");
            Assert.IsFalse(container.Accepts(containingObject));
        }

        [Test]
        public void RefusesDifferent()
        {
            containingObject.layer = LayerMask.NameToLayer("UI");
            subject.LayerMask = LayerMask.GetMask("Ignore Raycast");
            Assert.IsFalse(container.Accepts(containingObject));
        }

        [Test]
        public void RefusesDifferentMultipleLayers()
        {
            containingObject.layer = LayerMask.NameToLayer("UI") | LayerMask.NameToLayer("Water");
            subject.LayerMask = LayerMask.GetMask("Ignore Raycast");
            Assert.IsFalse(container.Accepts(containingObject));
        }

        [Test]
        public void RefusesDifferentMultipleLayerMask()
        {
            containingObject.layer = LayerMask.NameToLayer("UI");
            subject.LayerMask = LayerMask.GetMask("Ignore Raycast") | LayerMask.GetMask("Water");
            Assert.IsFalse(container.Accepts(containingObject));
        }

        [Test]
        public void RefusesInactiveGameObject()
        {
            containingObject.layer = LayerMask.NameToLayer("UI");
            subject.LayerMask = LayerMask.GetMask("UI");
            subject.gameObject.SetActive(false);
            Assert.IsFalse(container.Accepts(containingObject));
        }

        [Test]
        public void RefusesInactiveComponent()
        {
            containingObject.layer = LayerMask.NameToLayer("UI");
            subject.LayerMask = LayerMask.GetMask("UI");
            subject.enabled = false;
            Assert.IsFalse(container.Accepts(containingObject));
        }
    }
}