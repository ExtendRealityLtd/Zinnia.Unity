using Zinnia.Tracking.Velocity;

namespace Test.Zinnia.Tracking.Velocity
{
    using NUnit.Framework;
    using UnityEngine;

    public class XRNodeVelocityEstimatorTest
    {
        private GameObject containingObject;
        private XRNodeVelocityEstimator subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<XRNodeVelocityEstimator>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void ClearRelativeTo()
        {
            Assert.IsNull(subject.RelativeTo);
            subject.RelativeTo = containingObject;
            Assert.AreEqual(containingObject, subject.RelativeTo);
            subject.ClearRelativeTo();
            Assert.IsNull(subject.RelativeTo);
        }

        [Test]
        public void ClearRelativeToInactiveGameObject()
        {
            Assert.IsNull(subject.RelativeTo);
            subject.RelativeTo = containingObject;
            Assert.AreEqual(containingObject, subject.RelativeTo);
            subject.gameObject.SetActive(false);
            subject.ClearRelativeTo();
            Assert.AreEqual(containingObject, subject.RelativeTo);
        }

        [Test]
        public void ClearRelativeToInactiveComponent()
        {
            Assert.IsNull(subject.RelativeTo);
            subject.RelativeTo = containingObject;
            Assert.AreEqual(containingObject, subject.RelativeTo);
            subject.enabled = false;
            subject.ClearRelativeTo();
            Assert.AreEqual(containingObject, subject.RelativeTo);
        }
    }
}