namespace VRTK.Core.Visual
{
    using UnityEngine;
    using NUnit.Framework;
    using VRTK.Core.Utility.Mock;

    public class CameraColorOverlayTest
    {
        private GameObject containingObject;
        private CameraColorOverlay subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<CameraColorOverlay>();
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
            subject.ColorOverlayAdded.AddListener(colorOverlayAddedMock.Listen);

            subject.AddColorOverlay();

            Assert.IsTrue(colorOverlayAddedMock.Received);
            colorOverlayAddedMock.Reset();

            subject.AddColorOverlay();

            //Shouldn't be true if it's called with the same parameters and the colour matches the existing target color
            Assert.IsFalse(colorOverlayAddedMock.Received);
            colorOverlayAddedMock.Reset();

            subject.overlayColor = Color.red;
            subject.AddColorOverlay();

            Assert.IsTrue(colorOverlayAddedMock.Received);
        }

        [Test]
        public void RemoveColorOverlay()
        {
            UnityEventListenerMock colorOverlayAddedMock = new UnityEventListenerMock();
            subject.ColorOverlayAdded.AddListener(colorOverlayAddedMock.Listen);
            UnityEventListenerMock colorOverlayRemovedMock = new UnityEventListenerMock();
            subject.ColorOverlayRemoved.AddListener(colorOverlayRemovedMock.Listen);

            subject.RemoveColorOverlay();

            Assert.IsTrue(colorOverlayRemovedMock.Received);
            Assert.IsFalse(colorOverlayAddedMock.Received);
        }
    }
}