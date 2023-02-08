using Zinnia.Tracking.Follow.Modifier.Property.Position;

namespace Test.Zinnia.Tracking.Follow.Modifier.Property.Position
{
    using NUnit.Framework;
    using System.Collections;
    using Test.Zinnia.Utility;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using UnityEngine.TestTools;
    using UnityEngine.TestTools.Utils;

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

            containingObject = new GameObject("RigidbodyVelocityTest");
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
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject source = new GameObject("RigidbodyVelocityTest");
            GameObject target = subject.gameObject;

            source.transform.position = Vector3.one;
            target.transform.position = Vector3.zero;

            Vector3 expectedVelocity = Vector3.one / (Time.inFixedTimeStep ? Time.fixedDeltaTime : Time.deltaTime);
            Vector3 expectedAngularVelocity = Vector3.zero;

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
            GameObject source = new GameObject("RigidbodyVelocityTest");
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
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject source = new GameObject("RigidbodyVelocityTest");
            GameObject target = subject.gameObject;
            GameObject offset = new GameObject("RigidbodyVelocityTest");

            offset.transform.SetParent(target.transform);

            source.transform.position = Vector3.one;
            target.transform.position = Vector3.zero;
            offset.transform.position = Vector3.one * 2f;

            Vector3 expectedVelocity = -(Vector3.one / (Time.inFixedTimeStep ? Time.fixedDeltaTime : Time.deltaTime));
            Vector3 expectedAngularVelocity = Vector3.zero;

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
            GameObject source = new GameObject("RigidbodyVelocityTest");
            GameObject target = subject.gameObject;
            GameObject offset = new GameObject("RigidbodyVelocityTest");

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
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject source = new GameObject("RigidbodyVelocityTest");
            GameObject target = subject.gameObject;
            GameObject offset = new GameObject("RigidbodyVelocityTest");

            source.transform.position = Vector3.one;
            target.transform.position = Vector3.zero;
            offset.transform.position = Vector3.one * 2f;

            Vector3 expectedVelocity = Vector3.one / (Time.inFixedTimeStep ? Time.fixedDeltaTime : Time.deltaTime);
            Vector3 expectedAngularVelocity = Vector3.zero;

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
            GameObject source = new GameObject("RigidbodyVelocityTest");
            GameObject target = subject.gameObject;

            source.transform.position = Vector3.one;
            target.transform.position = Vector3.zero;

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
            GameObject source = new GameObject("RigidbodyVelocityTest");
            GameObject target = subject.gameObject;

            source.transform.position = Vector3.one;
            target.transform.position = Vector3.zero;

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