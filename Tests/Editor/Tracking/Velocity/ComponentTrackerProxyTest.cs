using Zinnia.Tracking.Velocity;

namespace Test.Zinnia.Tracking.Velocity
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using Assert = UnityEngine.Assertions.Assert;

    public class ComponentTrackerProxyTest
    {
        private GameObject containingObject;
        private ComponentTrackerProxy subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<ComponentTrackerProxy>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void GetVelocity()
        {
            GameObject sourceObject;
            VelocityTrackerMock.Generate(true, Vector3.one, Vector3.one, out sourceObject);

            subject.ProxySource = sourceObject;
            Assert.AreEqual(Vector3.one, subject.GetVelocity());

            Object.DestroyImmediate(sourceObject);
        }

        [Test]
        public void GetAngularVelocity()
        {
            GameObject sourceObject;
            VelocityTrackerMock.Generate(true, Vector3.one, Vector3.one, out sourceObject);

            subject.ProxySource = sourceObject;
            Assert.AreEqual(Vector3.one, subject.GetAngularVelocity());

            Object.DestroyImmediate(sourceObject);
        }

        [Test]
        public void ClearProxySource()
        {
            GameObject sourceObject;
            VelocityTrackerMock.Generate(true, Vector3.one, Vector3.one, out sourceObject);

            subject.ProxySource = sourceObject;
            Assert.AreEqual(sourceObject, subject.ProxySource);
            subject.ProxySource = null;
            Assert.IsNull(subject.ProxySource);
            Object.DestroyImmediate(sourceObject);
        }

        [Test]
        public void SourceInvalid()
        {
            Assert.AreEqual(Vector3.zero, subject.GetVelocity());
            Assert.AreEqual(Vector3.zero, subject.GetAngularVelocity());
        }

        [Test]
        public void SourceInactiveGameObject()
        {
            GameObject sourceObject;
            VelocityTrackerMock.Generate(true, Vector3.one, Vector3.one, out sourceObject);
            sourceObject.SetActive(false);

            subject.ProxySource = sourceObject;
            Assert.AreEqual(Vector3.zero, subject.GetVelocity());
            Assert.AreEqual(Vector3.zero, subject.GetAngularVelocity());

            Object.DestroyImmediate(sourceObject);
        }

        [Test]
        public void SourceInactiveComponent()
        {
            GameObject sourceObject;
            VelocityTrackerMock tracker = VelocityTrackerMock.Generate(true, Vector3.one, Vector3.one, out sourceObject);
            tracker.enabled = false;

            subject.ProxySource = sourceObject;
            Assert.AreEqual(Vector3.zero, subject.GetVelocity());
            Assert.AreEqual(Vector3.zero, subject.GetAngularVelocity());

            Object.DestroyImmediate(sourceObject);
        }

        [Test]
        public void InactiveGameObject()
        {
            GameObject sourceObject;
            VelocityTrackerMock.Generate(true, Vector3.one, Vector3.one, out sourceObject);
            subject.gameObject.SetActive(false);

            subject.ProxySource = sourceObject;
            Assert.AreEqual(Vector3.zero, subject.GetVelocity());
            Assert.AreEqual(Vector3.zero, subject.GetAngularVelocity());

            Object.DestroyImmediate(sourceObject);
        }

        [Test]
        public void InactiveComponent()
        {
            GameObject sourceObject;
            VelocityTrackerMock.Generate(true, Vector3.one, Vector3.one, out sourceObject);
            subject.enabled = false;

            subject.ProxySource = sourceObject;
            Assert.AreEqual(Vector3.zero, subject.GetVelocity());
            Assert.AreEqual(Vector3.zero, subject.GetAngularVelocity());

            Object.DestroyImmediate(sourceObject);
        }
    }
}
