using Zinnia.Cast;
using Zinnia.Rule;
using Zinnia.Data.Collection.List;

namespace Test.Zinnia.Cast
{
    using UnityEngine;
    using UnityEngine.TestTools;
    using System.Collections;
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using Test.Zinnia.Utility.Stub;
    using Assert = UnityEngine.Assertions.Assert;

    public class ParabolicLineCastTest
    {
        private GameObject containingObject;
        private ParabolicLineCast subject;
        private GameObject validSurface;

        [SetUp]
        public void SetUp()
        {
            Physics.autoSimulation = false;
            containingObject = new GameObject();
            subject = containingObject.AddComponent<ParabolicLineCast>();
            validSurface = GameObject.CreatePrimitive(PrimitiveType.Cube);
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
            Object.DestroyImmediate(validSurface);
            Physics.autoSimulation = true;
        }

        [Test]
        public void CastPointsValidTarget()
        {
            UnityEventListenerMock castResultsChangedMock = new UnityEventListenerMock();
            subject.ResultsChanged.AddListener(castResultsChangedMock.Listen);
            subject.Origin = subject.gameObject;

            validSurface.transform.position = Vector3.forward * 5f + Vector3.down * 4f;

            subject.MaximumLength = new Vector2(5f, 5f);
            subject.SegmentCount = 5;

            Physics.Simulate(Time.fixedDeltaTime);
            subject.Process();

            Vector3[] expectedPoints = new Vector3[]
            {
                Vector3.zero,
                new Vector3(0f, -0.1f, 2.9f),
                new Vector3(0f, -1.4f, 4.4f),
                new Vector3(0f, -2.8f, 4.9f),
                new Vector3(0f, validSurface.transform.position.y + (validSurface.transform.localScale.y / 2f), validSurface.transform.position.z)
            };

            for (int index = 0; index < subject.Points.Count; index++)
            {
                Assert.AreEqual(expectedPoints[index].ToString(), subject.Points[index].ToString(), "Index " + index);
            }

            Assert.AreEqual(validSurface.transform, subject.TargetHit.Value.transform);
            Assert.IsTrue(subject.IsTargetHitValid);
            Assert.IsTrue(castResultsChangedMock.Received);
        }

        [Test]
        public void CastPointsInsufficientForwardBeamLength()
        {
            UnityEventListenerMock castResultsChangedMock = new UnityEventListenerMock();
            subject.ResultsChanged.AddListener(castResultsChangedMock.Listen);
            subject.Origin = subject.gameObject;

            validSurface.transform.position = Vector3.forward * 5f + Vector3.down * 4f;

            subject.MaximumLength = new Vector2(2f, 5f);
            subject.SegmentCount = 5;

            Physics.Simulate(Time.fixedDeltaTime);
            subject.Process();

            Vector3[] expectedPoints = new Vector3[]
            {
                Vector3.zero,
                new Vector3(0f, 0.4f, 1.2f),
                new Vector3(0f, 0.4f, 1.7f),
                new Vector3(0f, 0.1f, 2f),
                new Vector3(0f, 0f, 2f)
            };

            for (int index = 0; index < subject.Points.Count; index++)
            {
                Assert.AreEqual(expectedPoints[index].ToString(), subject.Points[index].ToString(), "Index " + index);
            }

            Assert.IsFalse(subject.TargetHit.HasValue);
            Assert.IsTrue(castResultsChangedMock.Received);
        }

        [Test]
        public void CastPointsInsufficientDownwardBeamLength()
        {
            UnityEventListenerMock castResultsChangedMock = new UnityEventListenerMock();
            subject.ResultsChanged.AddListener(castResultsChangedMock.Listen);
            subject.Origin = subject.gameObject;

            validSurface.transform.position = Vector3.forward * 5f + Vector3.down * 4f;

            subject.MaximumLength = new Vector2(5f, 2f);
            subject.SegmentCount = 5;

            Physics.Simulate(Time.fixedDeltaTime);
            subject.Process();

            Vector3[] expectedPoints = new Vector3[]
            {
                Vector3.zero,
                new Vector3(0f, 0.4f, 2.9f),
                new Vector3(0f, 0.4f, 4.4f),
                new Vector3(0f, 0.1f, 4.9f),
                new Vector3(0f, 0f, 5f)
            };

            for (int index = 0; index < subject.Points.Count; index++)
            {
                Assert.AreEqual(expectedPoints[index].ToString(), subject.Points[index].ToString(), "Index " + index);
            }

            Assert.IsFalse(subject.TargetHit.HasValue);
            Assert.IsTrue(castResultsChangedMock.Received);
        }

        [UnityTest]
        public IEnumerator CastPointsInvalidTarget()
        {
            UnityEventListenerMock castResultsChangedMock = new UnityEventListenerMock();
            subject.ResultsChanged.AddListener(castResultsChangedMock.Listen);
            subject.Origin = subject.gameObject;

            validSurface.transform.position = Vector3.forward * 5f + Vector3.down * 4f;
            validSurface.AddComponent<RuleStub>();
            NegationRule negationRule = validSurface.AddComponent<NegationRule>();
            AnyComponentTypeRule anyComponentTypeRule = validSurface.AddComponent<AnyComponentTypeRule>();
            SerializableTypeComponentObservableList rules = containingObject.AddComponent<SerializableTypeComponentObservableList>();
            anyComponentTypeRule.ComponentTypes = rules;
            rules.Add(typeof(RuleStub));
            yield return null;

            negationRule.Rule = new RuleContainer
            {
                Interface = anyComponentTypeRule
            };
            subject.TargetValidity = new RuleContainer
            {
                Interface = negationRule
            };

            subject.MaximumLength = new Vector2(5f, 5f);
            subject.SegmentCount = 5;

            Physics.Simulate(Time.fixedDeltaTime);
            subject.Process();

            Vector3[] expectedPoints = new Vector3[]
            {
                Vector3.zero,
                new Vector3(0f, -0.1f, 2.9f),
                new Vector3(0f, -1.4f, 4.4f),
                new Vector3(0f, -2.8f, 4.9f),
                new Vector3(0f, validSurface.transform.position.y + (validSurface.transform.localScale.y / 2f), validSurface.transform.position.z)
            };

            for (int index = 0; index < subject.Points.Count; index++)
            {
                Assert.AreEqual(expectedPoints[index].ToString(), subject.Points[index].ToString(), "Index " + index);
            }

            Assert.AreEqual(validSurface.transform, subject.TargetHit.Value.transform);
            Assert.IsFalse(subject.IsTargetHitValid);
            Assert.IsTrue(castResultsChangedMock.Received);
        }

        [Test]
        public void EventsNotEmittedOnInactiveGameObject()
        {
            UnityEventListenerMock castResultsChangedMock = new UnityEventListenerMock();
            subject.ResultsChanged.AddListener(castResultsChangedMock.Listen);
            subject.Origin = subject.gameObject;

            validSurface.transform.position = Vector3.forward * 5f + Vector3.down * 4f;

            subject.MaximumLength = new Vector2(5f, 5f);
            subject.SegmentCount = 5;
            subject.gameObject.SetActive(false);

            Physics.Simulate(Time.fixedDeltaTime);
            subject.Process();

            Assert.AreEqual(0, subject.Points.Count);
            Assert.IsFalse(subject.TargetHit.HasValue);
            Assert.IsFalse(castResultsChangedMock.Received);
        }

        [Test]
        public void EventsNotEmittedOnDisabledComponent()
        {
            UnityEventListenerMock castResultsChangedMock = new UnityEventListenerMock();
            subject.ResultsChanged.AddListener(castResultsChangedMock.Listen);
            subject.Origin = subject.gameObject;

            validSurface.transform.position = Vector3.forward * 5f + Vector3.down * 4f;

            subject.MaximumLength = new Vector2(5f, 5f);
            subject.SegmentCount = 5;
            subject.enabled = false;

            Physics.Simulate(Time.fixedDeltaTime);
            subject.Process();

            Assert.AreEqual(0, subject.Points.Count);
            Assert.IsFalse(subject.TargetHit.HasValue);
            Assert.IsFalse(castResultsChangedMock.Received);
        }
    }
}