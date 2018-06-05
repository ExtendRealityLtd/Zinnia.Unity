namespace VRTK.Core.Tracking
{
    using UnityEngine;
    using NUnit.Framework;
    using System.Collections.Generic;
    using VRTK.Core.Utility;
    using VRTK.Core.Utility.Mock;
    using VRTK.Core.Utility.Stub;

    public class SurfaceLocatorTest
    {
        private GameObject containingObject;
        private SurfaceLocator subject;
        private GameObject validSurface;
        private GameObject searchOrigin;

        [SetUp]
        public void SetUp()
        {
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
        }

        [Test]
        public void ValidSurface()
        {
            UnityEventListenerMock surfaceLocatedMock = new UnityEventListenerMock();
            subject.SurfaceLocated.AddListener(surfaceLocatedMock.Listen);

            validSurface.transform.position = Vector3.forward * 5f;

            subject.searchOrigin = searchOrigin.transform;
            subject.searchDirection = Vector3.forward;

            //Process just calls Locate() so may as well just test the first point
            subject.Process();

            Assert.IsTrue(surfaceLocatedMock.Received);
            Assert.AreEqual(validSurface.transform, subject.SurfaceData.transform);
        }

        [Test]
        public void InvalidSurface()
        {
            UnityEventListenerMock surfaceLocatedMock = new UnityEventListenerMock();
            subject.SurfaceLocated.AddListener(surfaceLocatedMock.Listen);

            validSurface.transform.position = Vector3.forward * 5f;

            subject.searchOrigin = searchOrigin.transform;
            subject.searchDirection = Vector3.down;

            subject.Locate();
            Assert.IsFalse(surfaceLocatedMock.Received);
        }

        [Test]
        public void InvalidSurfaceDueToPolicy()
        {
            UnityEventListenerMock surfaceLocatedMock = new UnityEventListenerMock();
            subject.SurfaceLocated.AddListener(surfaceLocatedMock.Listen);

            validSurface.transform.position = Vector3.forward * 5f;
            validSurface.AddComponent<ExclusionRuleStub>();
            ExclusionRule exclusions = validSurface.AddComponent<ExclusionRule>();
            exclusions.checkType = ExclusionRule.CheckTypes.Script;
            exclusions.identifiers = new List<string>() { "ExclusionRuleStub" };

            subject.searchOrigin = searchOrigin.transform;
            subject.searchDirection = Vector3.forward;
            subject.targetValidity = exclusions;

            subject.Locate();
            Assert.IsFalse(surfaceLocatedMock.Received);
        }

        [Test]
        public void EventsNotEmittedOnInactiveGameObject()
        {
            UnityEventListenerMock surfaceLocatedMock = new UnityEventListenerMock();
            subject.SurfaceLocated.AddListener(surfaceLocatedMock.Listen);

            validSurface.transform.position = Vector3.forward * 5f;

            subject.searchOrigin = searchOrigin.transform;
            subject.searchDirection = Vector3.forward;
            subject.gameObject.SetActive(false);
            subject.Process();

            Assert.IsFalse(surfaceLocatedMock.Received);
        }

        [Test]
        public void EventsNotEmittedOnDisabledComponent()
        {
            UnityEventListenerMock surfaceLocatedMock = new UnityEventListenerMock();
            subject.SurfaceLocated.AddListener(surfaceLocatedMock.Listen);

            validSurface.transform.position = Vector3.forward * 5f;

            subject.searchOrigin = searchOrigin.transform;
            subject.searchDirection = Vector3.forward;
            subject.enabled = false;
            subject.Process();

            Assert.IsFalse(surfaceLocatedMock.Received);
        }
    }
}