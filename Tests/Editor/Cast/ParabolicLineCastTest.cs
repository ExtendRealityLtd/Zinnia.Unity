using VRTK.Core.Cast;
using VRTK.Core.Rule;

namespace Test.VRTK.Core.Cast
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.VRTK.Core.Utility.Mock;
    using Test.VRTK.Core.Utility.Stub;

    public class ParabolicLineCastTest
    {
        private GameObject containingObject;
        private ParabolicLineCast subject;
        private GameObject validSurface;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<ParabolicLineCast>();
            validSurface = GameObject.CreatePrimitive(PrimitiveType.Cube);
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
            Object.DestroyImmediate(validSurface);
        }

        [Test]
        public void CastPointsValidTarget()
        {
            UnityEventListenerMock castResultsChangedMock = new UnityEventListenerMock();
            subject.ResultsChanged.AddListener(castResultsChangedMock.Listen);

            validSurface.transform.position = Vector3.forward * 5f + Vector3.down * 4f;

            subject.maximumLength = new Vector2(5f, 5f);
            subject.segmentCount = 5;

            subject.Process();

            Vector3[] expectedPoints = new Vector3[]
            {
                Vector3.zero,
                new Vector3(0f, -0.1f, 2.9f),
                new Vector3(0f, -1.4f, 4.4f),
                new Vector3(0f, -2.8f, 4.9f),
                new Vector3(0f, validSurface.transform.position.y + (validSurface.transform.localScale.y / 2f), validSurface.transform.position.z)
            };

            for (int i = 0; i < subject.Points.Count; i++)
            {
                Assert.AreEqual(expectedPoints[i].ToString(), subject.Points[i].ToString(), "Index " + i);
            }

            Assert.AreEqual(validSurface.transform, subject.TargetHit.Value.transform);
            Assert.IsTrue(castResultsChangedMock.Received);
        }

        [Test]
        public void CastPointsInsufficientForwardBeamLength()
        {
            UnityEventListenerMock castResultsChangedMock = new UnityEventListenerMock();
            subject.ResultsChanged.AddListener(castResultsChangedMock.Listen);

            validSurface.transform.position = Vector3.forward * 5f + Vector3.down * 4f;

            subject.maximumLength = new Vector2(2f, 5f);
            subject.segmentCount = 5;

            subject.Process();

            Vector3[] expectedPoints = new Vector3[]
            {
                Vector3.zero,
                new Vector3(0f, 0.4f, 1.2f),
                new Vector3(0f, 0.4f, 1.7f),
                new Vector3(0f, 0.1f, 2f),
                new Vector3(0f, 0f, 2f)
            };

            for (int i = 0; i < subject.Points.Count; i++)
            {
                Assert.AreEqual(expectedPoints[i].ToString(), subject.Points[i].ToString(), "Index " + i);
            }

            Assert.IsNull(subject.TargetHit);
            Assert.IsTrue(castResultsChangedMock.Received);
        }

        [Test]
        public void CastPointsInsufficientDownwardBeamLength()
        {
            UnityEventListenerMock castResultsChangedMock = new UnityEventListenerMock();
            subject.ResultsChanged.AddListener(castResultsChangedMock.Listen);

            validSurface.transform.position = Vector3.forward * 5f + Vector3.down * 4f;

            subject.maximumLength = new Vector2(5f, 2f);
            subject.segmentCount = 5;

            subject.Process();

            Vector3[] expectedPoints = new Vector3[]
            {
                Vector3.zero,
                new Vector3(0f, 0.4f, 2.9f),
                new Vector3(0f, 0.4f, 4.4f),
                new Vector3(0f, 0.1f, 4.9f),
                new Vector3(0f, 0f, 5f)
            };

            for (int i = 0; i < subject.Points.Count; i++)
            {
                Assert.AreEqual(expectedPoints[i].ToString(), subject.Points[i].ToString(), "Index " + i);
            }

            Assert.IsNull(subject.TargetHit);
            Assert.IsTrue(castResultsChangedMock.Received);
        }

        [Test]
        public void CastPointsInvalidTarget()
        {
            UnityEventListenerMock castResultsChangedMock = new UnityEventListenerMock();
            subject.ResultsChanged.AddListener(castResultsChangedMock.Listen);

            validSurface.transform.position = Vector3.forward * 5f + Vector3.down * 4f;
            validSurface.AddComponent<RuleStub>();
            NegationRule negationRule = validSurface.AddComponent<NegationRule>();
            AnyComponentTypeRule anyComponentTypeRule = validSurface.AddComponent<AnyComponentTypeRule>();
            anyComponentTypeRule.componentTypes.Add(typeof(RuleStub));
            negationRule.rule = new RuleContainer
            {
                Interface = anyComponentTypeRule
            };
            subject.targetValidity = new RuleContainer
            {
                Interface = negationRule
            };

            subject.maximumLength = new Vector2(5f, 5f);
            subject.segmentCount = 5;

            subject.Process();

            Vector3[] expectedPoints = new Vector3[]
            {
                Vector3.zero,
                new Vector3(0f, -0.1f, 2.9f),
                new Vector3(0f, -1.4f, 4.4f),
                new Vector3(0f, -2.8f, 4.9f),
                new Vector3(0f, validSurface.transform.position.y + (validSurface.transform.localScale.y / 2f), validSurface.transform.position.z)
            };

            for (int i = 0; i < subject.Points.Count; i++)
            {
                Assert.AreEqual(expectedPoints[i].ToString(), subject.Points[i].ToString(), "Index " + i);
            }

            Assert.IsNull(subject.TargetHit);
            Assert.IsTrue(castResultsChangedMock.Received);
        }

        [Test]
        public void EventsNotEmittedOnInactiveGameObject()
        {
            UnityEventListenerMock castResultsChangedMock = new UnityEventListenerMock();
            subject.ResultsChanged.AddListener(castResultsChangedMock.Listen);

            validSurface.transform.position = Vector3.forward * 5f + Vector3.down * 4f;

            subject.maximumLength = new Vector2(5f, 5f);
            subject.segmentCount = 5;
            subject.gameObject.SetActive(false);

            subject.Process();

            Assert.AreEqual(0, subject.Points.Count);
            Assert.IsNull(subject.TargetHit);
            Assert.IsFalse(castResultsChangedMock.Received);
        }

        [Test]
        public void EventsNotEmittedOnDisabledComponent()
        {
            UnityEventListenerMock castResultsChangedMock = new UnityEventListenerMock();
            subject.ResultsChanged.AddListener(castResultsChangedMock.Listen);

            validSurface.transform.position = Vector3.forward * 5f + Vector3.down * 4f;

            subject.maximumLength = new Vector2(5f, 5f);
            subject.segmentCount = 5;
            subject.enabled = false;

            subject.Process();

            Assert.AreEqual(0, subject.Points.Count);
            Assert.IsNull(subject.TargetHit);
            Assert.IsFalse(castResultsChangedMock.Received);
        }
    }
}