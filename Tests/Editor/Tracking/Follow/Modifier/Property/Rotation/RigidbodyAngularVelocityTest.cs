using Zinnia.Tracking.Follow.Modifier.Property.Rotation;

namespace Test.Zinnia.Tracking.Follow.Modifier.Property.Rotation
{
    using NUnit.Framework;
    using System.Collections;
    using Test.Zinnia.Utility;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using UnityEngine.TestTools;
    using UnityEngine.TestTools.Utils;

    public class RigidbodyAngularVelocityTest
    {
        private GameObject containingObject;
        private RigidbodyAngularVelocity subject;
        private Rigidbody subjectRigidbody;
        private TimeSettingOverride timeOverride;

        [SetUp]
        public void SetUp()
        {
            timeOverride = new TimeSettingOverride(0.02f, 0.3333333f, 1f, 0.03f);

            containingObject = new GameObject("RigidbodyAngularVelocityTest");
            subject = containingObject.AddComponent<RigidbodyAngularVelocity>();
            subjectRigidbody = containingObject.AddComponent<Rigidbody>();
            subjectRigidbody.useGravity = false;
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
            timeOverride.ResetTime();
        }

        [Test]
        public void Modify()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject source = new GameObject("RigidbodyAngularVelocityTest");
            GameObject target = subject.gameObject;

            source.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
            target.transform.rotation = Quaternion.identity;

            Vector3 expectedVelocity = Vector3.zero;
            Vector3 expectedAngularVelocity = Vector3.right * 10f;

            Assert.That(subjectRigidbody.velocity, Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.That(subjectRigidbody.angularVelocity, Is.EqualTo(Vector3.zero).Using(comparer));

            subject.Modify(source, target);

            Assert.That(subjectRigidbody.velocity, Is.EqualTo(expectedVelocity).Using(comparer));
            Assert.That(subjectRigidbody.angularVelocity, Is.EqualTo(expectedAngularVelocity).Using(comparer));

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
        }

        [UnityTest]
        public IEnumerator ModifyAndDiverge()
        {
            subject.TrackDivergence = true;
            GameObject source = new GameObject("RigidbodyAngularVelocityTest");
            GameObject target = subject.gameObject;

            UnityEventListenerMock convergedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock divergedListenerMock = new UnityEventListenerMock();
            subject.Converged.AddListener(convergedListenerMock.Listen);
            subject.Diverged.AddListener(divergedListenerMock.Listen);

            source.transform.rotation = Quaternion.identity;
            target.transform.rotation = Quaternion.identity;

            Assert.IsFalse(convergedListenerMock.Received);
            Assert.IsFalse(divergedListenerMock.Received);
            convergedListenerMock.Reset();
            divergedListenerMock.Reset();

            Assert.IsFalse(subject.AreDiverged(source, target));

            source.transform.rotation = Quaternion.Euler(90f, 0f, 0f);

            subject.Modify(source, target);

            Assert.IsFalse(convergedListenerMock.Received);
            Assert.IsTrue(divergedListenerMock.Received);
            convergedListenerMock.Reset();
            divergedListenerMock.Reset();

            Assert.IsTrue(subject.AreDiverged(source, target));

            while (subject.AreDiverged(source, target))
            {
                subject.Modify(source, target);
                yield return null;
            }

            Assert.IsTrue(convergedListenerMock.Received);
            Assert.IsFalse(divergedListenerMock.Received);
            Assert.IsFalse(subject.AreDiverged(source, target));

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
        }

        [Test]
        public void ModifyWithOffset()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject source = new GameObject("RigidbodyAngularVelocityTest");
            GameObject target = subject.gameObject;
            GameObject offset = new GameObject("RigidbodyAngularVelocityTest");

            offset.transform.SetParent(target.transform);

            source.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
            target.transform.rotation = Quaternion.identity;
            offset.transform.rotation = Quaternion.Euler(45f, 0f, 0f);

            Vector3 expectedVelocity = Vector3.zero;
            Vector3 expectedAngularVelocity = Vector3.right * 10f;

            Assert.That(subjectRigidbody.velocity, Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.That(subjectRigidbody.angularVelocity, Is.EqualTo(Vector3.zero).Using(comparer));

            subject.Modify(source, target, offset);

            Assert.That(subjectRigidbody.velocity, Is.EqualTo(expectedVelocity).Using(comparer));
            Assert.That(subjectRigidbody.angularVelocity, Is.EqualTo(expectedAngularVelocity).Using(comparer));

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
            Object.DestroyImmediate(offset);
        }

        [UnityTest]
        public IEnumerator ModifyWithOffsetAndDiverge()
        {
            subject.TrackDivergence = true;
            GameObject source = new GameObject("RigidbodyAngularVelocityTest");
            GameObject target = subject.gameObject;
            GameObject offset = new GameObject("RigidbodyAngularVelocityTest");

            UnityEventListenerMock convergedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock divergedListenerMock = new UnityEventListenerMock();
            subject.Converged.AddListener(convergedListenerMock.Listen);
            subject.Diverged.AddListener(divergedListenerMock.Listen);

            offset.transform.SetParent(target.transform);

            source.transform.rotation = Quaternion.identity;
            target.transform.rotation = Quaternion.identity;
            offset.transform.rotation = Quaternion.Euler(45f, 0f, 0f);

            Assert.IsFalse(convergedListenerMock.Received);
            Assert.IsFalse(divergedListenerMock.Received);
            convergedListenerMock.Reset();
            divergedListenerMock.Reset();

            Assert.IsFalse(subject.AreDiverged(source, target));

            source.transform.rotation = Quaternion.Euler(90f, 0f, 0f);

            subject.Modify(source, target, offset);

            Assert.IsFalse(convergedListenerMock.Received);
            Assert.IsTrue(divergedListenerMock.Received);
            convergedListenerMock.Reset();
            divergedListenerMock.Reset();

            Assert.IsTrue(subject.AreDiverged(source, target));

            while (subject.AreDiverged(source, target))
            {
                subject.Modify(source, target);
                yield return null;
            }

            Assert.IsTrue(convergedListenerMock.Received);
            Assert.IsFalse(divergedListenerMock.Received);
            Assert.IsFalse(subject.AreDiverged(source, target));

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
        }

        [Test]
        public void ModifyWithOffsetIgnored()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject source = new GameObject("RigidbodyAngularVelocityTest");
            GameObject target = subject.gameObject;
            GameObject offset = new GameObject("RigidbodyAngularVelocityTest");

            source.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
            target.transform.rotation = Quaternion.identity;
            offset.transform.position = Vector3.one * 2f;

            Vector3 expectedVelocity = Vector3.zero;
            Vector3 expectedAngularVelocity = Vector3.right * 10f;

            Assert.That(subjectRigidbody.velocity, Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.That(subjectRigidbody.angularVelocity, Is.EqualTo(Vector3.zero).Using(comparer));

            subject.ApplyOffset = false;
            subject.Modify(source, target, offset);

            Assert.That(subjectRigidbody.velocity, Is.EqualTo(expectedVelocity).Using(comparer));
            Assert.That(subjectRigidbody.angularVelocity, Is.EqualTo(expectedAngularVelocity).Using(comparer));

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
            Object.DestroyImmediate(offset);
        }

        [Test]
        public void ModifyInactiveGameObject()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject source = new GameObject("RigidbodyAngularVelocityTest");
            GameObject target = subject.gameObject;

            source.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
            target.transform.rotation = Quaternion.identity;

            Assert.That(subjectRigidbody.velocity, Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.That(subjectRigidbody.angularVelocity, Is.EqualTo(Vector3.zero).Using(comparer));

            subject.gameObject.SetActive(false);
            subject.Modify(source, target);

            Assert.That(subjectRigidbody.velocity, Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.That(subjectRigidbody.angularVelocity, Is.EqualTo(Vector3.zero).Using(comparer));

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
        }

        [Test]
        public void ModifyInactiveComponent()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject source = new GameObject("RigidbodyAngularVelocityTest");
            GameObject target = subject.gameObject;

            source.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
            target.transform.rotation = Quaternion.identity;

            Assert.That(subjectRigidbody.velocity, Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.That(subjectRigidbody.angularVelocity, Is.EqualTo(Vector3.zero).Using(comparer));

            subject.enabled = false;
            subject.Modify(source, target);

            Assert.That(subjectRigidbody.velocity, Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.That(subjectRigidbody.angularVelocity, Is.EqualTo(Vector3.zero).Using(comparer));

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
        }
    }
}