using Zinnia.Tracking.Follow.Modifier.Property.Rotation;

namespace Test.Zinnia.Tracking.Follow.Modifier.Property.Rotation
{
    using NUnit.Framework;
    using UnityEngine;
    using Assert = UnityEngine.Assertions.Assert;

    public class RigidbodyForceAtPositionTest
    {
        private GameObject containingObject;
        private RigidbodyForceAtPosition subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<RigidbodyForceAtPosition>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void ClearAttachmentPoint()
        {
            Assert.IsNull(subject.AttachmentPoint);
            subject.AttachmentPoint = containingObject;
            Assert.AreEqual(containingObject, subject.AttachmentPoint);
            subject.ClearAttachmentPoint();
            Assert.IsNull(subject.AttachmentPoint);
        }

        [Test]
        public void ClearAttachmentPointInactiveGameObject()
        {
            Assert.IsNull(subject.AttachmentPoint);
            subject.AttachmentPoint = containingObject;
            Assert.AreEqual(containingObject, subject.AttachmentPoint);
            subject.gameObject.SetActive(false);
            subject.ClearAttachmentPoint();
            Assert.AreEqual(containingObject, subject.AttachmentPoint);
        }

        [Test]
        public void ClearAttachmentPointInactiveComponent()
        {
            Assert.IsNull(subject.AttachmentPoint);
            subject.AttachmentPoint = containingObject;
            Assert.AreEqual(containingObject, subject.AttachmentPoint);
            subject.enabled = false;
            subject.ClearAttachmentPoint();
            Assert.AreEqual(containingObject, subject.AttachmentPoint);
        }
    }
}