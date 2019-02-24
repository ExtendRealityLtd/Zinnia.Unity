using Zinnia.Rule;
using Zinnia.Tracking;

namespace Test.Zinnia.Tracking
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using Test.Zinnia.Utility.Stub;

    public class SurfaceLocatorTest
    {
        private GameObject containingObject;
        private SurfaceLocator subject;
        private GameObject validSurface;
        private GameObject searchOrigin;

        [SetUp]
        public void SetUp()
        {
            Physics.autoSimulation = false;
            containingObject = new GameObject();
            subject = containingObject.AddComponent<SurfaceLocator>();
            validSurface = GameObject.CreatePrimitive(PrimitiveType.Cube);
            searchOrigin = new GameObject();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);

            Object.DestroyImmediate(validSurface);
            Object.DestroyImmediate(searchOrigin);
            Physics.autoSimulation = true;
        }

        [Test]
        public void ValidSurface()
        {
            UnityEventListenerMock surfaceLocatedMock = new UnityEventListenerMock();
            subject.SurfaceLocated.AddListener(surfaceLocatedMock.Listen);

            validSurface.transform.position = Vector3.forward * 5f;

            subject.SearchOrigin = searchOrigin;
            subject.searchDirection = Vector3.forward;

            //Process just calls Locate() so may as well just test the first point
            Physics.Simulate(Time.fixedDeltaTime);
            subject.Process();

            Assert.IsTrue(surfaceLocatedMock.Received);
            Assert.AreEqual(validSurface.transform, subject.surfaceData.transform);
        }

        [Test]
        public void InvalidSurface()
        {
            UnityEventListenerMock surfaceLocatedMock = new UnityEventListenerMock();
            subject.SurfaceLocated.AddListener(surfaceLocatedMock.Listen);

            validSurface.transform.position = Vector3.forward * 5f;

            subject.SearchOrigin = searchOrigin;
            subject.searchDirection = Vector3.down;

            Physics.Simulate(Time.fixedDeltaTime);
            subject.Locate();
            Assert.IsFalse(surfaceLocatedMock.Received);
        }

        [Test]
        public void InvalidSurfaceDueToPolicy()
        {
            UnityEventListenerMock surfaceLocatedMock = new UnityEventListenerMock();
            subject.SurfaceLocated.AddListener(surfaceLocatedMock.Listen);

            validSurface.transform.position = Vector3.forward * 5f;
            validSurface.AddComponent<RuleStub>();
            NegationRule negationRule = validSurface.AddComponent<NegationRule>();
            AnyComponentTypeRule anyComponentTypeRule = validSurface.AddComponent<AnyComponentTypeRule>();
            anyComponentTypeRule.componentTypes.Add(typeof(RuleStub));
            negationRule.rule = new RuleContainer
            {
                Interface = anyComponentTypeRule
            };
            subject.targetValidity = new RuleContainer
            {
                Interface = negationRule
            };

            subject.SearchOrigin = searchOrigin;
            subject.searchDirection = Vector3.forward;

            Physics.Simulate(Time.fixedDeltaTime);
            subject.Locate();
            Assert.IsFalse(surfaceLocatedMock.Received);
        }

        [Test]
        public void EventsNotEmittedOnInactiveGameObject()
        {
            UnityEventListenerMock surfaceLocatedMock = new UnityEventListenerMock();
            subject.SurfaceLocated.AddListener(surfaceLocatedMock.Listen);

            validSurface.transform.position = Vector3.forward * 5f;

            subject.SearchOrigin = searchOrigin;
            subject.searchDirection = Vector3.forward;
            subject.gameObject.SetActive(false);
            Physics.Simulate(Time.fixedDeltaTime);
            subject.Process();

            Assert.IsFalse(surfaceLocatedMock.Received);
        }

        [Test]
        public void EventsNotEmittedOnDisabledComponent()
        {
            UnityEventListenerMock surfaceLocatedMock = new UnityEventListenerMock();
            subject.SurfaceLocated.AddListener(surfaceLocatedMock.Listen);

            validSurface.transform.position = Vector3.forward * 5f;

            subject.SearchOrigin = searchOrigin;
            subject.searchDirection = Vector3.forward;
            subject.enabled = false;
            Physics.Simulate(Time.fixedDeltaTime);
            subject.Process();

            Assert.IsFalse(surfaceLocatedMock.Received);
        }
    }
}