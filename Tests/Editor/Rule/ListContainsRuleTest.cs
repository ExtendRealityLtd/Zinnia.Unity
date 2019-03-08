using Zinnia.Rule;
using Zinnia.Extension;
using Zinnia.Data.Collection;

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
            UnityObjectObservableList objects = containingObject.AddComponent<UnityObjectObservableList>();
            subject.Objects = objects;
            objects.Add(containingObject);

            Assert.IsTrue(container.Accepts(containingObject));
        }

        [Test]
        public void RefusesEmpty()
        {
            UnityObjectObservableList objects = containingObject.AddComponent<UnityObjectObservableList>();
            subject.Objects = objects;

            Assert.IsFalse(container.Accepts(containingObject));
        }

        [Test]
        public void RefusesNullObjects()
        {
            Assert.IsFalse(container.Accepts(containingObject));
        }

        [Test]
        public void RefusesDifferent()
        {
            GameObject wrongGameObject = new GameObject();
            UnityObjectObservableList objects = containingObject.AddComponent<UnityObjectObservableList>();
            subject.Objects = objects;
            objects.Add(wrongGameObject);

            Assert.IsFalse(container.Accepts(containingObject));

            Object.DestroyImmediate(wrongGameObject);
        }

        [Test]
        public void RefusesInactiveGameObject()
        {
            UnityObjectObservableList objects = containingObject.AddComponent<UnityObjectObservableList>();
            subject.Objects = objects;
            objects.Add(containingObject);

            subject.gameObject.SetActive(false);
            Assert.IsFalse(container.Accepts(containingObject));
        }

        [Test]
        public void RefusesInactiveComponent()
        {
            UnityObjectObservableList objects = containingObject.AddComponent<UnityObjectObservableList>();
            subject.Objects = objects;
            objects.Add(containingObject);

            subject.enabled = false;
            Assert.IsFalse(container.Accepts(containingObject));
        }
    }
}