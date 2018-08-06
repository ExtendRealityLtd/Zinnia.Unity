using VRTK.Core.Extension;
using VRTK.Core.Rule;

namespace Test.VRTK.Core.Rule
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
    }
}