using Zinnia.Tracking.Velocity;

namespace Test.Zinnia.Tracking.Velocity
{
    using NUnit.Framework;
    using UnityEngine;
    using UnityEngine.TestTools.Utils;

    public class RigidbodyVelocityTrackerTest
    {
        private GameObject containingObject;
        private RigidbodyVelocityTracker subject;
        private GameObject anotherObject;
        private Rigidbody source;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject("RigidbodyVelocityTrackerTest");
            subject = containingObject.AddComponent<RigidbodyVelocityTracker>();
            anotherObject = new GameObject("RigidbodyVelocityTrackerTest");
            source = anotherObject.AddComponent<Rigidbody>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
            Object.DestroyImmediate(anotherObject);
        }

        [Test]
        public void IsActiveSourceActive()
        {
            subject.Source = source;

            bool actualResult = subject.IsActive();
            Assert.IsTrue(actualResult);
        }

        [Test]
        public void IsActiveSourceInActive()
        {
            source.gameObject.SetActive(false);
            subject.Source = source;

            bool actualResult = subject.IsActive();
            Assert.IsFalse(actualResult);
        }

        [Test]
        public void IsActiveNoSource()
        {
            bool actualResult = subject.IsActive();
            Assert.IsFalse(actualResult);
        }

        [Test]
        public void GetVelocity()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            Vector3 expectedResult = new Vector3(1.2f, 1.3f, 1.4f);
            Vector3 unexpectedResult = Vector3.zero;

            // Ensure the subject has a valid source to check
            subject.Source = source;
            source.velocity = expectedResult;

            Vector3 actualResult = subject.GetVelocity();
            Assert.That(actualResult, Is.EqualTo(expectedResult).Using(comparer));
            Assert.That(actualResult, Is.Not.EqualTo(unexpectedResult).Using(comparer));
        }

        [Test]
        public void GetAngularVelocity()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            Vector3 expectedResult = new Vector3(1.2f, 1.3f, 1.4f);
            Vector3 unexpectedResult = Vector3.zero;

            // Ensure the subject has a valid source to check
            subject.Source = source;
            source.angularVelocity = expectedResult;

            Vector3 actualResult = subject.GetAngularVelocity();
            Assert.That(actualResult, Is.EqualTo(expectedResult).Using(comparer));
            Assert.That(actualResult, Is.Not.EqualTo(unexpectedResult).Using(comparer));
        }

        [Test]
        public void ClearSource()
        {
            Assert.IsNull(subject.Source);
            subject.Source = source;
            Assert.AreEqual(source, subject.Source);
            subject.ClearSource();
            Assert.IsNull(subject.Source);
        }

        [Test]
        public void ClearSourceInactiveGameObject()
        {
            Assert.IsNull(subject.Source);
            subject.Source = source;
            Assert.AreEqual(source, subject.Source);
            subject.gameObject.SetActive(false);
            subject.ClearSource();
            Assert.AreEqual(source, subject.Source);
        }

        [Test]
        public void ClearSourceInactiveComponent()
        {
            Assert.IsNull(subject.Source);
            subject.Source = source;
            Assert.AreEqual(source, subject.Source);
            subject.enabled = false;
            subject.ClearSource();
            Assert.AreEqual(source, subject.Source);
        }
    }
}