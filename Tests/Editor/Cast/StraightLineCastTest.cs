using VRTK.Core.Cast;
using VRTK.Core.Rule;

namespace Test.VRTK.Core.Cast
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.VRTK.Core.Utility.Mock;
    using Test.VRTK.Core.Utility.Stub;

    public class StraightLineCastTest
    {
        private GameObject containingObject;
        private StraightLineCastMock subject;
        private GameObject validSurface;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<StraightLineCastMock>();
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

            validSurface.transform.position = Vector3.forward * 5f;

            subject.ManualOnEnable();
            subject.Process();

            Vector3 expectedStart = Vector3.zero;
            Vector3 expectedEnd = validSurface.transform.position - (Vector3.forward * (validSurface.transform.localScale.z / 2f));

            Assert.AreEqual(expectedStart, subject.Points[0]);
            Assert.AreEqual(expectedEnd, subject.Points[1]);
            Assert.AreEqual(validSurface.transform, subject.TargetHit.Value.transform);
            Assert.IsTrue(castResultsChangedMock.Received);
        }

        [Test]
        public void CastPointsInsufficientBeamLength()
        {
            UnityEventListenerMock castResultsChangedMock = new UnityEventListenerMock();
            subject.ResultsChanged.AddListener(castResultsChangedMock.Listen);

            validSurface.transform.position = Vector3.forward * 5f;
            subject.maximumLength = validSurface.transform.position.z / 2f;

            subject.ManualOnEnable();
            subject.Process();

            Vector3 expectedStart = Vector3.zero;
            Vector3 expectedEnd = Vector3.forward * subject.maximumLength;

            Assert.AreEqual(expectedStart, subject.Points[0]);
            Assert.AreEqual(expectedEnd, subject.Points[1]);
            Assert.IsNull(subject.TargetHit);
            Assert.IsTrue(castResultsChangedMock.Received);
        }

        [Test]
        public void CastPointsInvalidTarget()
        {
            UnityEventListenerMock castResultsChangedMock = new UnityEventListenerMock();
            subject.ResultsChanged.AddListener(castResultsChangedMock.Listen);

            validSurface.transform.position = Vector3.forward * 5f;
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

            subject.ManualOnEnable();
            subject.Process();

            Vector3 expectedStart = Vector3.zero;
            Vector3 expectedEnd = validSurface.transform.position - (Vector3.forward * (validSurface.transform.localScale.z / 2f));

            Assert.AreEqual(expectedStart, subject.Points[0]);
            Assert.AreEqual(expectedEnd, subject.Points[1]);
            Assert.IsNull(subject.TargetHit);
            Assert.IsTrue(castResultsChangedMock.Received);
        }

        [Test]
        public void EventsNotEmittedOnInactiveGameObject()
        {
            UnityEventListenerMock castResultsChangedMock = new UnityEventListenerMock();
            subject.ResultsChanged.AddListener(castResultsChangedMock.Listen);

            validSurface.transform.position = Vector3.forward * 5f;

            subject.ManualOnEnable();
            subject.gameObject.SetActive(false);
            subject.ManualOnDisable();
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

            validSurface.transform.position = Vector3.forward * 5f;

            subject.ManualOnEnable();
            subject.enabled = false;
            subject.ManualOnDisable();
            subject.Process();

            Assert.AreEqual(0, subject.Points.Count);
            Assert.IsNull(subject.TargetHit);
            Assert.IsFalse(castResultsChangedMock.Received);
        }
    }

    public class StraightLineCastMock : StraightLineCast
    {
        public void ManualOnEnable()
        {
            OnEnable();
        }

        public void ManualOnDisable()
        {
            OnDisable();
        }
    }
}