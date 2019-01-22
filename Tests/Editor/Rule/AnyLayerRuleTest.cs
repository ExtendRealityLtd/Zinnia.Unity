using Zinnia.Extension;
using Zinnia.Rule;

namespace Test.Zinnia.Rule
{
    using UnityEngine;
    using NUnit.Framework;

    public class AnyLayerRuleTest
    {
        private GameObject containingObject;
        private RuleContainer container;
        private AnyLayerRule subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject
            {
                layer = LayerMask.NameToLayer("UI")
            };
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
            subject.layerMask = LayerMask.NameToLayer("UI");
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
            subject.layerMask = LayerMask.NameToLayer("Ignore Raycast");
            Assert.IsFalse(container.Accepts(containingObject));
        }
    }
}