using VRTK.Core.Extension;
using VRTK.Core.Rule;

namespace Test.VRTK.Core.Rule
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
            subject.componentTypes.Add(typeof(TestScript));

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
            containingObject.AddComponent<Light>();
            subject.componentTypes.Add(typeof(TestScript));
            Assert.IsFalse(container.Accepts(containingObject));
        }

        private class TestScript : MonoBehaviour
        {
        }
    }
}