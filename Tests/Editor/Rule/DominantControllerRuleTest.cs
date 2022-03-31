using Zinnia.Extension;
using Zinnia.Rule;
using Zinnia.Tracking.CameraRig;

namespace Test.Zinnia.Rule
{
    using NUnit.Framework;
    using UnityEngine;
    using UnityEngine.XR;
    using Assert = UnityEngine.Assertions.Assert;

    public class DominantControllerRuleTest
    {
        private GameObject containingObject;
        private RuleContainer container;
        private DominantControllerRule subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            containingObject.SetActive(false);
            container = new RuleContainer();
            subject = containingObject.AddComponent<DominantControllerRule>();
            container.Interface = subject;
            containingObject.SetActive(true);
        }

        [TearDown]
        public void TearDown()
        {
            Object.Destroy(containingObject);
        }

        [Test]
        public void AcceptsTrueSingle()
        {
            MockDominantControllerObserver observer = containingObject.AddComponent<MockDominantControllerObserver>();
            observer.controllerType = XRNode.LeftHand;

            subject.Sources.Add(observer);
            subject.ToMatch = DominantControllerRule.Controller.LeftController;

            Assert.IsTrue(container.Accepts(null));

            observer.controllerType = XRNode.RightHand;
            subject.ToMatch = DominantControllerRule.Controller.RightController;

            Assert.IsTrue(container.Accepts(null));

            observer.controllerType = XRNode.Head;
            subject.ToMatch = DominantControllerRule.Controller.Head;

            Assert.IsTrue(container.Accepts(null));
        }

        [Test]
        public void AcceptsFalseSingle()
        {
            MockDominantControllerObserver observer = containingObject.AddComponent<MockDominantControllerObserver>();
            observer.controllerType = XRNode.LeftHand;

            subject.Sources.Add(observer);
            subject.ToMatch = DominantControllerRule.Controller.RightController;

            Assert.IsFalse(container.Accepts(null));

            subject.ToMatch = DominantControllerRule.Controller.Head;

            Assert.IsFalse(container.Accepts(null));

            observer.controllerType = XRNode.RightHand;
            subject.ToMatch = DominantControllerRule.Controller.LeftController;

            Assert.IsFalse(container.Accepts(null));

            subject.ToMatch = DominantControllerRule.Controller.Head;

            Assert.IsFalse(container.Accepts(null));
        }

        [Test]
        public void AcceptsMultiple()
        {
            MockDominantControllerObserver observerOne = containingObject.AddComponent<MockDominantControllerObserver>();
            observerOne.controllerType = XRNode.Head;

            MockDominantControllerObserver observerTwo = containingObject.AddComponent<MockDominantControllerObserver>();
            observerTwo.controllerType = XRNode.LeftHand;

            MockDominantControllerObserver observerThree = containingObject.AddComponent<MockDominantControllerObserver>();
            observerThree.controllerType = XRNode.RightHand;

            subject.Sources.Add(observerOne);
            subject.Sources.Add(observerTwo);
            subject.Sources.Add(observerThree);

            observerOne.enabled = false;
            observerTwo.enabled = true;
            observerThree.enabled = false;

            subject.ToMatch = DominantControllerRule.Controller.LeftController;

            Assert.IsTrue(container.Accepts(null));

            observerOne.enabled = true;
            observerTwo.enabled = true;
            observerThree.enabled = false;

            Assert.IsFalse(container.Accepts(null));

            subject.ToMatch = DominantControllerRule.Controller.RightController;

            observerOne.enabled = true;
            observerTwo.enabled = true;
            observerThree.enabled = true;

            Assert.IsFalse(container.Accepts(null));

            observerOne.enabled = false;
            observerTwo.enabled = false;
            observerThree.enabled = true;

            Assert.IsTrue(container.Accepts(null));
        }

        protected class MockDominantControllerObserver : DominantControllerObserver
        {
            public XRNode controllerType;

            public override XRNode DominantController => controllerType;
        }
    }
}