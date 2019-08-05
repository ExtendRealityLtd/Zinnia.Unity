using Zinnia.Tracking.Follow;

namespace Test.Zinnia.Tracking.Follow
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using Assert = UnityEngine.Assertions.Assert;

    public class ObjectDistanceComparatorTest
    {
        private GameObject containingObject;
        private ObjectDistanceComparator subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<ObjectDistanceComparator>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void Process()
        {
            UnityEventListenerMock thresholdExceededMock = new UnityEventListenerMock();
            UnityEventListenerMock thresholdResumedMock = new UnityEventListenerMock();
            subject.ThresholdExceeded.AddListener(thresholdExceededMock.Listen);
            subject.ThresholdResumed.AddListener(thresholdResumedMock.Listen);

            GameObject source = new GameObject();
            GameObject target = new GameObject();

            subject.Source = source;
            subject.Target = target;
            subject.DistanceThreshold = 0.5f;

            source.transform.position = Vector3.zero;
            target.transform.position = Vector3.zero;

            subject.Process();

            Assert.IsFalse(thresholdExceededMock.Received);
            Assert.IsFalse(thresholdResumedMock.Received);
            Assert.IsFalse(subject.Exceeding);
            Assert.AreEqual(Vector3.zero, subject.Difference);
            Assert.AreEqual(0f, subject.Distance);

            target.transform.position = Vector3.forward * 1f;

            subject.Process();

            Assert.IsTrue(thresholdExceededMock.Received);
            Assert.IsFalse(thresholdResumedMock.Received);
            Assert.IsTrue(subject.Exceeding);
            Assert.AreEqual(Vector3.forward * 1f, subject.Difference);
            Assert.AreEqual(1f, subject.Distance);

            thresholdExceededMock.Reset();
            thresholdResumedMock.Reset();

            target.transform.position = Vector3.zero;

            subject.Process();

            Assert.IsFalse(thresholdExceededMock.Received);
            Assert.IsTrue(thresholdResumedMock.Received);
            Assert.IsFalse(subject.Exceeding);
            Assert.AreEqual(Vector3.zero, subject.Difference);
            Assert.AreEqual(0f, subject.Distance);

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
        }

        [Test]
        public void ProcessSourceTargetEquals()
        {
            UnityEventListenerMock thresholdExceededMock = new UnityEventListenerMock();
            UnityEventListenerMock thresholdResumedMock = new UnityEventListenerMock();
            subject.ThresholdExceeded.AddListener(thresholdExceededMock.Listen);
            subject.ThresholdResumed.AddListener(thresholdResumedMock.Listen);

            GameObject target = new GameObject();

            subject.Source = target;
            subject.Target = target;
            subject.DistanceThreshold = 0.5f;

            target.transform.position = Vector3.zero;

            subject.Process();

            Assert.IsFalse(thresholdExceededMock.Received);
            Assert.IsFalse(thresholdResumedMock.Received);
            Assert.IsFalse(subject.Exceeding);
            Assert.AreEqual(Vector3.zero, subject.Difference);
            Assert.AreEqual(0f, subject.Distance);

            target.transform.position = Vector3.forward * 1f;

            subject.Process();

            Assert.IsTrue(thresholdExceededMock.Received);
            Assert.IsFalse(thresholdResumedMock.Received);
            Assert.IsTrue(subject.Exceeding);
            Assert.AreEqual(Vector3.forward * 1f, subject.Difference);
            Assert.AreEqual(1f, subject.Distance);

            thresholdExceededMock.Reset();
            thresholdResumedMock.Reset();
            subject.SavePosition();

            target.transform.position = Vector3.zero;

            subject.Process();

            Assert.IsTrue(thresholdExceededMock.Received);
            Assert.IsFalse(thresholdResumedMock.Received);
            Assert.IsTrue(subject.Exceeding);
            Assert.AreEqual(Vector3.forward * -1f, subject.Difference);
            Assert.AreEqual(1f, subject.Distance);

            Object.DestroyImmediate(target);
        }

        [Test]
        public void ProcessNoSource()
        {
            UnityEventListenerMock thresholdExceededMock = new UnityEventListenerMock();
            UnityEventListenerMock thresholdResumedMock = new UnityEventListenerMock();
            subject.ThresholdExceeded.AddListener(thresholdExceededMock.Listen);
            subject.ThresholdResumed.AddListener(thresholdResumedMock.Listen);

            GameObject target = new GameObject();

            subject.Target = target;
            subject.DistanceThreshold = 0.5f;

            target.transform.position = Vector3.forward * 1f;

            subject.Process();

            Assert.IsFalse(thresholdExceededMock.Received);
            Assert.IsFalse(thresholdResumedMock.Received);
            Assert.IsFalse(subject.Exceeding);
            Assert.AreEqual(Vector3.zero, subject.Difference);
            Assert.AreEqual(0f, subject.Distance);

            Object.DestroyImmediate(target);
        }

        [Test]
        public void ProcessNoTarget()
        {
            UnityEventListenerMock thresholdExceededMock = new UnityEventListenerMock();
            UnityEventListenerMock thresholdResumedMock = new UnityEventListenerMock();
            subject.ThresholdExceeded.AddListener(thresholdExceededMock.Listen);
            subject.ThresholdResumed.AddListener(thresholdResumedMock.Listen);

            GameObject source = new GameObject();

            subject.Source = source;
            subject.DistanceThreshold = 0.5f;

            subject.Process();

            Assert.IsFalse(thresholdExceededMock.Received);
            Assert.IsFalse(thresholdResumedMock.Received);
            Assert.IsFalse(subject.Exceeding);
            Assert.AreEqual(Vector3.zero, subject.Difference);
            Assert.AreEqual(0f, subject.Distance);

            Object.DestroyImmediate(source);
        }

        [Test]
        public void ProcessInactiveGameObject()
        {
            UnityEventListenerMock thresholdExceededMock = new UnityEventListenerMock();
            UnityEventListenerMock thresholdResumedMock = new UnityEventListenerMock();
            subject.ThresholdExceeded.AddListener(thresholdExceededMock.Listen);
            subject.ThresholdResumed.AddListener(thresholdResumedMock.Listen);

            GameObject source = new GameObject();
            GameObject target = new GameObject();

            subject.Source = source;
            subject.Target = target;
            subject.DistanceThreshold = 0.5f;

            source.transform.position = Vector3.zero;
            target.transform.position = Vector3.forward * 1f;

            subject.gameObject.SetActive(false);

            subject.Process();

            Assert.IsFalse(thresholdExceededMock.Received);
            Assert.IsFalse(thresholdResumedMock.Received);
            Assert.IsFalse(subject.Exceeding);
            Assert.AreEqual(Vector3.zero, subject.Difference);
            Assert.AreEqual(0f, subject.Distance);

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
        }

        [Test]
        public void ProcessInactiveComponent()
        {
            UnityEventListenerMock thresholdExceededMock = new UnityEventListenerMock();
            UnityEventListenerMock thresholdResumedMock = new UnityEventListenerMock();
            subject.ThresholdExceeded.AddListener(thresholdExceededMock.Listen);
            subject.ThresholdResumed.AddListener(thresholdResumedMock.Listen);

            GameObject source = new GameObject();
            GameObject target = new GameObject();

            subject.Source = source;
            subject.Target = target;
            subject.DistanceThreshold = 0.5f;

            source.transform.position = Vector3.zero;
            target.transform.position = Vector3.forward * 1f;

            subject.enabled = false;

            subject.Process();

            Assert.IsFalse(thresholdExceededMock.Received);
            Assert.IsFalse(thresholdResumedMock.Received);
            Assert.IsFalse(subject.Exceeding);
            Assert.AreEqual(Vector3.zero, subject.Difference);
            Assert.AreEqual(0f, subject.Distance);

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
        }
    }
}