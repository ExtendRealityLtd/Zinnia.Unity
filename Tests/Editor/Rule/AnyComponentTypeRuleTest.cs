using Zinnia.Rule;
using Zinnia.Extension;
using Zinnia.Data.Collection;

namespace Test.Zinnia.Rule
{
    using UnityEngine;
    using NUnit.Framework;

    // TODO: Test all SearchOptions
    public class AnyComponentTypeRuleTest
    {
        private GameObject containingObject;
        private RuleContainer container;
        private AnyComponentTypeRule subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            container = new RuleContainer();
            subject = containingObject.AddComponent<AnyComponentTypeRule>();
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
            containingObject.AddComponent<TestScript>();
            SerializableTypeObservableList componentTypes = containingObject.AddComponent<SerializableTypeObservableList>();
            subject.ComponentTypes = componentTypes;
            componentTypes.Add(typeof(TestScript));

            Assert.IsTrue(container.Accepts(containingObject));
        }

        [Test]
        public void RefusesEmpty()
        {
            SerializableTypeObservableList componentTypes = containingObject.AddComponent<SerializableTypeObservableList>();
            subject.ComponentTypes = componentTypes;

            Assert.IsFalse(container.Accepts(containingObject));
        }

        [Test]
        public void RefusesNullComponentTypes()
        {
            Assert.IsFalse(container.Accepts(containingObject));
        }

        [Test]
        public void RefusesDifferent()
        {
            containingObject.AddComponent<Light>();
            SerializableTypeObservableList componentTypes = containingObject.AddComponent<SerializableTypeObservableList>();
            subject.ComponentTypes = componentTypes;
            componentTypes.Add(typeof(TestScript));

            Assert.IsFalse(container.Accepts(containingObject));
        }

        [Test]
        public void RefusesInactiveGameObject()
        {
            containingObject.AddComponent<TestScript>();
            SerializableTypeObservableList componentTypes = containingObject.AddComponent<SerializableTypeObservableList>();
            subject.ComponentTypes = componentTypes;
            componentTypes.Add(typeof(TestScript));

            subject.gameObject.SetActive(false);
            Assert.IsFalse(container.Accepts(containingObject));
        }

        [Test]
        public void RefusesInactiveComponent()
        {
            containingObject.AddComponent<TestScript>();
            SerializableTypeObservableList componentTypes = containingObject.AddComponent<SerializableTypeObservableList>();
            subject.ComponentTypes = componentTypes;
            componentTypes.Add(typeof(TestScript));

            subject.enabled = false;
            Assert.IsFalse(container.Accepts(containingObject));
        }

        private class TestScript : MonoBehaviour
        {
        }
    }
}