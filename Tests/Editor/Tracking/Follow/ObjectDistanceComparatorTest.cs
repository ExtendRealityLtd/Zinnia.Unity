using Zinnia.Tracking.Follow;

namespace Test.Zinnia.Tracking.Follow
{
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using UnityEngine.TestTools.Utils;

    public class ObjectDistanceComparatorTest
    {
        private GameObject containingObject;
        private ObjectDistanceComparator subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject("ObjectDistanceComparatorTest");
            subject = containingObject.AddComponent<ObjectDistanceComparator>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void Process()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            UnityEventListenerMock thresholdExceededMock = new UnityEventListenerMock();
            UnityEventListenerMock thresholdResumedMock = new UnityEventListenerMock();
            subject.ThresholdExceeded.AddListener(thresholdExceededMock.Listen);
            subject.ThresholdResumed.AddListener(thresholdResumedMock.Listen);

            GameObject source = new GameObject("ObjectDistanceComparatorTest");
            GameObject target = new GameObject("ObjectDistanceComparatorTest");

            subject.Source = source;
            subject.Target = target;
            subject.DistanceThreshold = 0.5f;

            source.transform.position = Vector3.zero;
            target.transform.position = Vector3.zero;

            subject.Process();

            Assert.IsFalse(thresholdExceededMock.Received);
            Assert.IsFalse(thresholdResumedMock.Received);
            Assert.IsFalse(subject.Exceeding);
            Assert.That(subject.Difference, Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.AreEqual(0f, subject.Distance);

            target.transform.position = Vector3.forward;

            subject.Process();

            Assert.IsTrue(thresholdExceededMock.Received);
            Assert.IsFalse(thresholdResumedMock.Received);
            Assert.IsTrue(subject.Exceeding);
            Assert.That(subject.Difference, Is.EqualTo(Vector3.forward).Using(comparer));
            Assert.AreEqual(1f, subject.Distance);

            thresholdExceededMock.Reset();
            thresholdResumedMock.Reset();

            target.transform.position = Vector3.zero;

            subject.Process();

            Assert.IsFalse(thresholdExceededMock.Received);
            Assert.IsTrue(thresholdResumedMock.Received);
            Assert.IsFalse(subject.Exceeding);
            Assert.That(subject.Difference, Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.AreEqual(0f, subject.Distance);

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
        }

        [Test]
        public void ProcessSourceTargetEquals()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            UnityEventListenerMock thresholdExceededMock = new UnityEventListenerMock();
            UnityEventListenerMock thresholdResumedMock = new UnityEventListenerMock();
            subject.ThresholdExceeded.AddListener(thresholdExceededMock.Listen);
            subject.ThresholdResumed.AddListener(thresholdResumedMock.Listen);

            GameObject target = new GameObject("ObjectDistanceComparatorTest");

            subject.Source = target;
            subject.Target = target;
            subject.DistanceThreshold = 0.5f;

            target.transform.position = Vector3.zero;

            subject.Process();

            Assert.IsFalse(thresholdExceededMock.Received);
            Assert.IsFalse(thresholdResumedMock.Received);
            Assert.IsFalse(subject.Exceeding);
            Assert.That(subject.Difference, Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.AreEqual(0f, subject.Distance);

            target.transform.position = Vector3.forward;

            subject.Process();

            Assert.IsTrue(thresholdExceededMock.Received);
            Assert.IsFalse(thresholdResumedMock.Received);
            Assert.IsTrue(subject.Exceeding);
            Assert.That(subject.Difference, Is.EqualTo(Vector3.forward).Using(comparer));
            Assert.AreEqual(1f, subject.Distance);

            thresholdExceededMock.Reset();
            thresholdResumedMock.Reset();
            subject.SavePosition();

            target.transform.position = Vector3.zero;

            subject.Process();

            Assert.IsTrue(thresholdExceededMock.Received);
            Assert.IsFalse(thresholdResumedMock.Received);
            Assert.IsTrue(subject.Exceeding);
            Assert.That(subject.Difference, Is.EqualTo(Vector3.forward * -1f).Using(comparer));
            Assert.AreEqual(1f, subject.Distance);

            Object.DestroyImmediate(target);
        }

        [Test]
        public void ProcessNoSource()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            UnityEventListenerMock thresholdExceededMock = new UnityEventListenerMock();
            UnityEventListenerMock thresholdResumedMock = new UnityEventListenerMock();
            subject.ThresholdExceeded.AddListener(thresholdExceededMock.Listen);
            subject.ThresholdResumed.AddListener(thresholdResumedMock.Listen);

            GameObject target = new GameObject("ObjectDistanceComparatorTest");

            subject.Target = target;
            subject.DistanceThreshold = 0.5f;

            target.transform.position = Vector3.forward;

            subject.Process();

            Assert.IsFalse(thresholdExceededMock.Received);
            Assert.IsFalse(thresholdResumedMock.Received);
            Assert.IsFalse(subject.Exceeding);
            Assert.That(subject.Difference, Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.AreEqual(0f, subject.Distance);

            Object.DestroyImmediate(target);
        }

        [Test]
        public void ProcessNoTarget()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            UnityEventListenerMock thresholdExceededMock = new UnityEventListenerMock();
            UnityEventListenerMock thresholdResumedMock = new UnityEventListenerMock();
            subject.ThresholdExceeded.AddListener(thresholdExceededMock.Listen);
            subject.ThresholdResumed.AddListener(thresholdResumedMock.Listen);

            GameObject source = new GameObject("ObjectDistanceComparatorTest");

            subject.Source = source;
            subject.DistanceThreshold = 0.5f;

            subject.Process();

            Assert.IsFalse(thresholdExceededMock.Received);
            Assert.IsFalse(thresholdResumedMock.Received);
            Assert.IsFalse(subject.Exceeding);
            Assert.That(subject.Difference, Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.AreEqual(0f, subject.Distance);

            Object.DestroyImmediate(source);
        }

        [Test]
        public void ProcessInactiveGameObject()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            UnityEventListenerMock thresholdExceededMock = new UnityEventListenerMock();
            UnityEventListenerMock thresholdResumedMock = new UnityEventListenerMock();
            subject.ThresholdExceeded.AddListener(thresholdExceededMock.Listen);
            subject.ThresholdResumed.AddListener(thresholdResumedMock.Listen);

            GameObject source = new GameObject("ObjectDistanceComparatorTest");
            GameObject target = new GameObject("ObjectDistanceComparatorTest");

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
            Assert.That(subject.Difference, Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.AreEqual(0f, subject.Distance);

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
        }

        [Test]
        public void ProcessInactiveComponent()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            UnityEventListenerMock thresholdExceededMock = new UnityEventListenerMock();
            UnityEventListenerMock thresholdResumedMock = new UnityEventListenerMock();
            subject.ThresholdExceeded.AddListener(thresholdExceededMock.Listen);
            subject.ThresholdResumed.AddListener(thresholdResumedMock.Listen);

            GameObject source = new GameObject("ObjectDistanceComparatorTest");
            GameObject target = new GameObject("ObjectDistanceComparatorTest");

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
            Assert.That(subject.Difference, Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.AreEqual(0f, subject.Distance);

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
        }

        [Test]
        public void ClearSource()
        {
            Assert.IsNull(subject.Source);
            subject.Source = containingObject;
            Assert.AreEqual(containingObject, subject.Source);
            subject.ClearSource();
            Assert.IsNull(subject.Source);
        }

        [Test]
        public void ClearSourceInactiveGameObject()
        {
            Assert.IsNull(subject.Source);
            subject.Source = containingObject;
            Assert.AreEqual(containingObject, subject.Source);
            subject.gameObject.SetActive(false);
            subject.ClearSource();
            Assert.AreEqual(containingObject, subject.Source);
        }

        [Test]
        public void ClearSourceInactiveComponent()
        {
            Assert.IsNull(subject.Source);
            subject.Source = containingObject;
            Assert.AreEqual(containingObject, subject.Source);
            subject.enabled = false;
            subject.ClearSource();
            Assert.AreEqual(containingObject, subject.Source);
        }

        [Test]
        public void ClearTarget()
        {
            Assert.IsNull(subject.Target);
            subject.Target = containingObject;
            Assert.AreEqual(containingObject, subject.Target);
            subject.ClearTarget();
            Assert.IsNull(subject.Target);
        }

        [Test]
        public void ClearTargetInactiveGameObject()
        {
            Assert.IsNull(subject.Target);
            subject.Target = containingObject;
            Assert.AreEqual(containingObject, subject.Target);
            subject.gameObject.SetActive(false);
            subject.ClearTarget();
            Assert.AreEqual(containingObject, subject.Target);
        }

        [Test]
        public void ClearTargetInactiveComponent()
        {
            Assert.IsNull(subject.Target);
            subject.Target = containingObject;
            Assert.AreEqual(containingObject, subject.Target);
            subject.enabled = false;
            subject.ClearTarget();
            Assert.AreEqual(containingObject, subject.Target);
        }

        [Test]
        public void SavePosition()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            ObjectDistanceComparatorMock subjectMock = containingObject.AddComponent<ObjectDistanceComparatorMock>();
            GameObject source = new GameObject("Source");

            subjectMock.Source = source;
            subjectMock.Target = source;

            Assert.That(subjectMock.GetTheSourcePosition(), Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.IsFalse(subjectMock.GetPreviousState());

            source.transform.position = Vector3.one;
            subjectMock.SetPreviousState(true);

            subjectMock.SavePosition();

            Assert.That(subjectMock.GetTheSourcePosition(), Is.EqualTo(Vector3.one).Using(comparer));
            Assert.IsFalse(subjectMock.GetPreviousState());

            Object.DestroyImmediate(source);
        }

        [Test]
        public void SavePositionNullSource()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            ObjectDistanceComparatorMock subjectMock = containingObject.AddComponent<ObjectDistanceComparatorMock>();
            GameObject source = new GameObject("Source");

            subjectMock.Target = source;

            Assert.That(subjectMock.GetTheSourcePosition(), Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.IsFalse(subjectMock.GetPreviousState());

            source.transform.position = Vector3.one;
            subjectMock.SetPreviousState(true);

            subjectMock.SavePosition();

            Assert.That(subjectMock.GetTheSourcePosition(), Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.IsTrue(subjectMock.GetPreviousState());

            Object.DestroyImmediate(source);
        }

        [Test]
        public void SavePositionSourceNotEqualTarget()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            ObjectDistanceComparatorMock subjectMock = containingObject.AddComponent<ObjectDistanceComparatorMock>();
            GameObject source = new GameObject("Source");
            GameObject target = new GameObject("Target");

            subjectMock.Source = source;
            subjectMock.Target = target;

            Assert.That(subjectMock.GetTheSourcePosition(), Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.IsFalse(subjectMock.GetPreviousState());

            source.transform.position = Vector3.one;
            subjectMock.SetPreviousState(true);

            subjectMock.SavePosition();

            Assert.That(subjectMock.GetTheSourcePosition(), Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.IsTrue(subjectMock.GetPreviousState());

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
        }

        protected class ObjectDistanceComparatorMock : ObjectDistanceComparator
        {
            public Vector3 GetTheSourcePosition()
            {
                return sourcePosition;
            }

            public bool GetPreviousState()
            {
                return previousState;
            }

            public void SetPreviousState(bool state)
            {
                previousState = state;
            }
        }
    }
}