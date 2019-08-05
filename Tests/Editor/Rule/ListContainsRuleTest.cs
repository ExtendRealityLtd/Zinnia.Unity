using Zinnia.Rule;
using Zinnia.Extension;
using Zinnia.Data.Collection.List;

namespace Test.Zinnia.Rule
{
    using UnityEngine;
    using UnityEngine.TestTools;
    using System.Collections;
    using NUnit.Framework;
    using Assert = UnityEngine.Assertions.Assert;

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

        [UnityTest]
        public IEnumerator AcceptsMatch()
        {
            UnityObjectObservableList objects = containingObject.AddComponent<UnityObjectObservableList>();
            yield return null;

            subject.Objects = objects;
            objects.Add(containingObject);

            Assert.IsTrue(container.Accepts(containingObject));
        }

        [UnityTest]
        public IEnumerator RefusesEmpty()
        {
            UnityObjectObservableList objects = containingObject.AddComponent<UnityObjectObservableList>();
            yield return null;

            subject.Objects = objects;

            Assert.IsFalse(container.Accepts(containingObject));
        }

        [Test]
        public void RefusesNullObjects()
        {
            Assert.IsFalse(container.Accepts(containingObject));
        }

        [UnityTest]
        public IEnumerator RefusesDifferent()
        {
            GameObject wrongGameObject = new GameObject();
            UnityObjectObservableList objects = containingObject.AddComponent<UnityObjectObservableList>();
            yield return null;

            subject.Objects = objects;
            objects.Add(wrongGameObject);

            Assert.IsFalse(container.Accepts(containingObject));

            Object.DestroyImmediate(wrongGameObject);
        }

        [UnityTest]
        public IEnumerator RefusesInactiveGameObject()
        {
            UnityObjectObservableList objects = containingObject.AddComponent<UnityObjectObservableList>();
            yield return null;

            subject.Objects = objects;
            objects.Add(containingObject);

            subject.gameObject.SetActive(false);
            Assert.IsFalse(container.Accepts(containingObject));
        }

        [UnityTest]
        public IEnumerator RefusesInactiveComponent()
        {
            UnityObjectObservableList objects = containingObject.AddComponent<UnityObjectObservableList>();
            yield return null;

            subject.Objects = objects;
            objects.Add(containingObject);

            subject.enabled = false;
            Assert.IsFalse(container.Accepts(containingObject));
        }
    }
}