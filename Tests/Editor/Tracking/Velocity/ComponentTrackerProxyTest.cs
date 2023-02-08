using Zinnia.Tracking.Velocity;

namespace Test.Zinnia.Tracking.Velocity
{
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using UnityEngine.TestTools.Utils;

    public class ComponentTrackerProxyTest
    {
        private GameObject containingObject;
        private ComponentTrackerProxy subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject("ComponentTrackerProxyTest");
            subject = containingObject.AddComponent<ComponentTrackerProxy>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void GetVelocity()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject sourceObject;
            VelocityTrackerMock.Generate(true, Vector3.one, Vector3.one, out sourceObject);

            subject.ProxySource = sourceObject;
            Assert.That(subject.GetVelocity(), Is.EqualTo(Vector3.one).Using(comparer));

            Object.DestroyImmediate(sourceObject);
        }

        [Test]
        public void GetAngularVelocity()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject sourceObject;
            VelocityTrackerMock.Generate(true, Vector3.one, Vector3.one, out sourceObject);

            subject.ProxySource = sourceObject;
            Assert.That(subject.GetAngularVelocity(), Is.EqualTo(Vector3.one).Using(comparer));

            Object.DestroyImmediate(sourceObject);
        }

        [Test]
        public void NullProxySourceProperty()
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
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            Assert.That(subject.GetVelocity(), Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.That(subject.GetAngularVelocity(), Is.EqualTo(Vector3.zero).Using(comparer));
        }

        [Test]
        public void SourceInactiveGameObject()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject sourceObject;
            VelocityTrackerMock.Generate(true, Vector3.one, Vector3.one, out sourceObject);
            sourceObject.SetActive(false);

            subject.ProxySource = sourceObject;
            Assert.That(subject.GetVelocity(), Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.That(subject.GetAngularVelocity(), Is.EqualTo(Vector3.zero).Using(comparer));

            Object.DestroyImmediate(sourceObject);
        }

        [Test]
        public void SourceInactiveComponent()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject sourceObject;
            VelocityTrackerMock tracker = VelocityTrackerMock.Generate(true, Vector3.one, Vector3.one, out sourceObject);
            tracker.enabled = false;

            subject.ProxySource = sourceObject;
            Assert.That(subject.GetVelocity(), Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.That(subject.GetAngularVelocity(), Is.EqualTo(Vector3.zero).Using(comparer));

            Object.DestroyImmediate(sourceObject);
        }

        [Test]
        public void InactiveGameObject()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject sourceObject;
            VelocityTrackerMock.Generate(true, Vector3.one, Vector3.one, out sourceObject);
            subject.gameObject.SetActive(false);

            subject.ProxySource = sourceObject;
            Assert.That(subject.GetVelocity(), Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.That(subject.GetAngularVelocity(), Is.EqualTo(Vector3.zero).Using(comparer));

            Object.DestroyImmediate(sourceObject);
        }

        [Test]
        public void InactiveComponent()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject sourceObject;
            VelocityTrackerMock.Generate(true, Vector3.one, Vector3.one, out sourceObject);
            subject.enabled = false;

            subject.ProxySource = sourceObject;
            Assert.That(subject.GetVelocity(), Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.That(subject.GetAngularVelocity(), Is.EqualTo(Vector3.zero).Using(comparer));

            Object.DestroyImmediate(sourceObject);
        }

        [Test]
        public void ClearProxySource()
        {
            Assert.IsNull(subject.ProxySource);
            subject.ProxySource = containingObject;
            Assert.AreEqual(containingObject, subject.ProxySource);
            subject.ClearProxySource();
            Assert.IsNull(subject.ProxySource);
        }

        [Test]
        public void ClearProxySourceInactiveGameObject()
        {
            Assert.IsNull(subject.ProxySource);
            subject.ProxySource = containingObject;
            Assert.AreEqual(containingObject, subject.ProxySource);
            subject.gameObject.SetActive(false);
            subject.ClearProxySource();
            Assert.AreEqual(containingObject, subject.ProxySource);
        }

        [Test]
        public void ClearProxySourceInactiveComponent()
        {
            Assert.IsNull(subject.ProxySource);
            subject.ProxySource = containingObject;
            Assert.AreEqual(containingObject, subject.ProxySource);
            subject.enabled = false;
            subject.ClearProxySource();
            Assert.AreEqual(containingObject, subject.ProxySource);
        }
    }
}
