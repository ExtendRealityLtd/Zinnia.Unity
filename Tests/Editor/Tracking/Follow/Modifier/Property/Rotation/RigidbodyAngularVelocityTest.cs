using Zinnia.Tracking.Follow.Modifier.Property.Rotation;

namespace Test.Zinnia.Tracking.Follow.Modifier.Property.Rotation
{
    using NUnit.Framework;
    using System.Collections;
    using Test.Zinnia.Utility;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using UnityEngine.TestTools;
    using Assert = UnityEngine.Assertions.Assert;

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

            containingObject = new GameObject();
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
            GameObject source = new GameObject();
            GameObject target = subject.gameObject;

            source.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
            target.transform.rotation = Quaternion.identity;

            Vector3 expectedVelocity = Vector3.zero;
            Vector3 expectedAngularVelocity = Vector3.right * 10f;

            Assert.AreEqual(Vector3.zero, subjectRigidbody.velocity);
            Assert.AreEqual(Vector3.zero, subjectRigidbody.angularVelocity);

            subject.Modify(source, target);

            Assert.AreEqual(expectedVelocity.ToString(), subjectRigidbody.velocity.ToString());
            Assert.AreEqual(expectedAngularVelocity, subjectRigidbody.angularVelocity);

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
        }

        [UnityTest]
        public IEnumerator ModifyAndDiverge()
        {
            subject.TrackDivergence = true;
            GameObject source = new GameObject();
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
            GameObject source = new GameObject();
            GameObject target = subject.gameObject;
            GameObject offset = new GameObject();

            offset.transform.SetParent(target.transform);

            source.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
            target.transform.rotation = Quaternion.identity;
            offset.transform.rotation = Quaternion.Euler(45f, 0f, 0f);

            Vector3 expectedVelocity = Vector3.zero;
            Vector3 expectedAngularVelocity = Vector3.right * 10f;

            Assert.AreEqual(Vector3.zero, subjectRigidbody.velocity);
            Assert.AreEqual(Vector3.zero, subjectRigidbody.angularVelocity);

            subject.Modify(source, target, offset);

            Assert.AreEqual(expectedVelocity.ToString(), subjectRigidbody.velocity.ToString());
            Assert.AreEqual(expectedAngularVelocity, subjectRigidbody.angularVelocity);

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
            Object.DestroyImmediate(offset);
        }

        [UnityTest]
        public IEnumerator ModifyWithOffsetAndDiverge()
        {
            subject.TrackDivergence = true;
            GameObject source = new GameObject();
            GameObject target = subject.gameObject;
            GameObject offset = new GameObject();

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
            GameObject source = new GameObject();
            GameObject target = subject.gameObject;
            GameObject offset = new GameObject();

            source.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
            target.transform.rotation = Quaternion.identity;
            offset.transform.position = Vector3.one * 2f;

            Vector3 expectedVelocity = Vector3.zero;
            Vector3 expectedAngularVelocity = Vector3.right * 10f;

            Assert.AreEqual(Vector3.zero, subjectRigidbody.velocity);
            Assert.AreEqual(Vector3.zero, subjectRigidbody.angularVelocity);

            subject.ApplyOffset = false;
            subject.Modify(source, target, offset);

            Assert.AreEqual(expectedVelocity.ToString(), subjectRigidbody.velocity.ToString());
            Assert.AreEqual(expectedAngularVelocity, subjectRigidbody.angularVelocity);

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
            Object.DestroyImmediate(offset);
        }

        [Test]
        public void ModifyInactiveGameObject()
        {
            GameObject source = new GameObject();
            GameObject target = subject.gameObject;

            source.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
            target.transform.rotation = Quaternion.identity;

            Assert.AreEqual(Vector3.zero, subjectRigidbody.velocity);
            Assert.AreEqual(Vector3.zero, subjectRigidbody.angularVelocity);

            subject.gameObject.SetActive(false);
            subject.Modify(source, target);

            Assert.AreEqual(Vector3.zero, subjectRigidbody.velocity);
            Assert.AreEqual(Vector3.zero, subjectRigidbody.angularVelocity);

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
        }

        [Test]
        public void ModifyInactiveComponent()
        {
            GameObject source = new GameObject();
            GameObject target = subject.gameObject;

            source.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
            target.transform.rotation = Quaternion.identity;

            Assert.AreEqual(Vector3.zero, subjectRigidbody.velocity);
            Assert.AreEqual(Vector3.zero, subjectRigidbody.angularVelocity);

            subject.enabled = false;
            subject.Modify(source, target);

            Assert.AreEqual(Vector3.zero, subjectRigidbody.velocity);
            Assert.AreEqual(Vector3.zero, subjectRigidbody.angularVelocity);

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
        }
    }
}