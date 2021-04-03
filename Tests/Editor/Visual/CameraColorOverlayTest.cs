using Zinnia.Visual;

namespace Test.Zinnia.Visual
{
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using Assert = UnityEngine.Assertions.Assert;

    public class CameraColorOverlayTest
    {
        private GameObject containingObject;
        private CameraColorOverlayMock subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<CameraColorOverlayMock>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void AddColorOverlay()
        {
            UnityEventListenerMock colorOverlayAddedMock = new UnityEventListenerMock();
            subject.Added.AddListener(colorOverlayAddedMock.Listen);

            subject.AddColorOverlay();

            Assert.IsTrue(colorOverlayAddedMock.Received);
            colorOverlayAddedMock.Reset();

            subject.AddColorOverlay();

            //Shouldn't be true if it's called with the same parameters and the color matches the existing target color
            Assert.IsFalse(colorOverlayAddedMock.Received);
            colorOverlayAddedMock.Reset();

            subject.OverlayColor = Color.red;
            subject.AddColorOverlay();

            Assert.IsTrue(colorOverlayAddedMock.Received);
            Assert.AreEqual("{ Color = RGBA(1.000, 0.000, 0.000, 1.000) }", subject.GetEventData().ToString());
        }

        [Test]
        public void RemoveColorOverlay()
        {
            UnityEventListenerMock colorOverlayAddedMock = new UnityEventListenerMock();
            subject.Added.AddListener(colorOverlayAddedMock.Listen);
            UnityEventListenerMock colorOverlayRemovedMock = new UnityEventListenerMock();
            subject.Removed.AddListener(colorOverlayRemovedMock.Listen);

            subject.RemoveColorOverlay();

            Assert.IsTrue(colorOverlayRemovedMock.Received);
            Assert.IsFalse(colorOverlayAddedMock.Received);
        }

        [Test]
        public void EventsNotEmittedOnInactiveGameObject()
        {
            UnityEventListenerMock colorOverlayAddedMock = new UnityEventListenerMock();
            subject.Added.AddListener(colorOverlayAddedMock.Listen);
            subject.gameObject.SetActive(false);
            subject.AddColorOverlay();

            Assert.IsFalse(colorOverlayAddedMock.Received);
        }

        [Test]
        public void EventsNotEmittedOnDisabledComponent()
        {
            UnityEventListenerMock colorOverlayAddedMock = new UnityEventListenerMock();
            subject.Added.AddListener(colorOverlayAddedMock.Listen);
            subject.enabled = false;
            subject.AddColorOverlay();

            Assert.IsFalse(colorOverlayAddedMock.Received);
        }

        private class CameraColorOverlayMock : CameraColorOverlay
        {
            public EventData GetEventData()
            {
                return eventData;
            }
        }
    }
}