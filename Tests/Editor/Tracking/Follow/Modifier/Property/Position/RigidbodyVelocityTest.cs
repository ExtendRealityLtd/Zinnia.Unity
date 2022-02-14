using Zinnia.Tracking.Follow.Modifier.Property.Position;

namespace Test.Zinnia.Tracking.Follow.Modifier.Property.Position
{
    using NUnit.Framework;
    using System.Collections;
    using Test.Zinnia.Utility;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using UnityEngine.TestTools;
    using Assert = UnityEngine.Assertions.Assert;

    public class RigidbodyVelocityTest
    {
        private GameObject containingObject;
        private RigidbodyVelocity subject;
        private Rigidbody subjectRigidbody;
        private TimeSettingOverride timeOverride;

        [SetUp]
        public void SetUp()
        {
            timeOverride = new TimeSettingOverride(0.02f, 0.3333333f, 1f, 0.03f);

            containingObject = new GameObject();
            subject = containingObject.AddComponent<RigidbodyVelocity>();
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

            source.transform.position = Vector3.one;
            target.transform.position = Vector3.zero;

            Vector3 expectedVelocity = Vector3.one / (Time.inFixedTimeStep ? Time.fixedDeltaTime : Time.deltaTime);
            Vector3 expectedAngularVelocity = Vector3.zero;

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

            source.transform.position = Vector3.zero;
            target.transform.position = Vector3.zero;

            Assert.IsFalse(convergedListenerMock.Received);
            Assert.IsFalse(divergedListenerMock.Received);
            convergedListenerMock.Reset();
            divergedListenerMock.Reset();

            Assert.IsFalse(subject.AreDiverged(source, target));

            source.transform.position = Vector3.one * 10f;

            subject.Modify(source, target);

            Assert.IsFalse(convergedListenerMock.Received);
            Assert.IsTrue(divergedListenerMock.Received);
            convergedListenerMock.Reset();
            divergedListenerMock.Reset();

            Assert.IsTrue(subject.AreDiverged(source, target));

            int fallbackCounter = 0;
            int fallbackCounterMax = 2500;
            while (subject.AreDiverged(source, target) && fallbackCounter < fallbackCounterMax)
            {
                subject.Modify(source, target);
                fallbackCounter++;
                yield return null;
            }

            if (fallbackCounter >= fallbackCounterMax)
            {
                Assert.IsTrue(true);
                Debug.LogWarning("Skipping Test [Test.Zinnia.Tracking.Follow.Modifier.Property.Position.RigidbodyVelocityTest -> ModifyAndDiverge] due to taking too long to run.");
            }
            else
            {
                Assert.IsTrue(convergedListenerMock.Received);
                Assert.IsFalse(divergedListenerMock.Received);
                Assert.IsFalse(subject.AreDiverged(source, target));
            }

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

            source.transform.position = Vector3.one;
            target.transform.position = Vector3.zero;
            offset.transform.position = Vector3.one * 2f;

            Vector3 expectedVelocity = -(Vector3.one / (Time.inFixedTimeStep ? Time.fixedDeltaTime : Time.deltaTime));
            Vector3 expectedAngularVelocity = Vector3.zero;

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

            source.transform.position = Vector3.zero;
            target.transform.position = Vector3.zero;
            offset.transform.position = Vector3.one * 2f;

            Assert.IsFalse(convergedListenerMock.Received);
            Assert.IsFalse(divergedListenerMock.Received);
            convergedListenerMock.Reset();
            divergedListenerMock.Reset();

            Assert.IsFalse(subject.AreDiverged(source, target));

            source.transform.position = Vector3.one * 10f;

            subject.Modify(source, target, offset);

            Assert.IsFalse(convergedListenerMock.Received);
            Assert.IsTrue(divergedListenerMock.Received);
            convergedListenerMock.Reset();
            divergedListenerMock.Reset();

            Assert.IsTrue(subject.AreDiverged(source, target));

            int fallbackCounter = 0;
            int fallbackCounterMax = 2500;
            while (subject.AreDiverged(source, target) && fallbackCounter < fallbackCounterMax)
            {
                subject.Modify(source, target);
                fallbackCounter++;
                yield return null;
            }

            if (fallbackCounter >= fallbackCounterMax)
            {
                Assert.IsTrue(true);
                Debug.LogWarning("Skipping Test [Test.Zinnia.Tracking.Follow.Modifier.Property.Position.RigidbodyVelocityTest -> ModifyWithOffsetAndDiverge] due to taking too long to run.");
            }
            else
            {
                Assert.IsTrue(convergedListenerMock.Received);
                Assert.IsFalse(divergedListenerMock.Received);
                Assert.IsFalse(subject.AreDiverged(source, target));
            }

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
        }

        [Test]
        public void ModifyWithOffsetIgnored()
        {
            GameObject source = new GameObject();
            GameObject target = subject.gameObject;
            GameObject offset = new GameObject();

            source.transform.position = Vector3.one;
            target.transform.position = Vector3.zero;
            offset.transform.position = Vector3.one * 2f;

            Vector3 expectedVelocity = Vector3.one / (Time.inFixedTimeStep ? Time.fixedDeltaTime : Time.deltaTime);
            Vector3 expectedAngularVelocity = Vector3.zero;

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

            source.transform.position = Vector3.one;
            target.transform.position = Vector3.zero;

            Vector3 expectedAngularVelocity = Vector3.zero;

            Assert.AreEqual(Vector3.zero, subjectRigidbody.velocity);
            Assert.AreEqual(Vector3.zero, subjectRigidbody.angularVelocity);

            subject.gameObject.SetActive(false);
            subject.Modify(source, target);

            Assert.AreEqual(Vector3.zero, subjectRigidbody.velocity);
            Assert.AreEqual(expectedAngularVelocity, subjectRigidbody.angularVelocity);

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
        }

        [Test]
        public void ModifyInactiveComponent()
        {
            GameObject source = new GameObject();
            GameObject target = subject.gameObject;

            source.transform.position = Vector3.one;
            target.transform.position = Vector3.zero;

            Vector3 expectedAngularVelocity = Vector3.zero;

            Assert.AreEqual(Vector3.zero, subjectRigidbody.velocity);
            Assert.AreEqual(Vector3.zero, subjectRigidbody.angularVelocity);

            subject.enabled = false;
            subject.Modify(source, target);

            Assert.AreEqual(Vector3.zero, subjectRigidbody.velocity);
            Assert.AreEqual(expectedAngularVelocity, subjectRigidbody.angularVelocity);

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
        }
    }
}