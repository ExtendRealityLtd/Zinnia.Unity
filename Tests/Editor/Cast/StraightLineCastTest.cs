using Zinnia.Cast;
using Zinnia.Data.Collection.List;
using Zinnia.Rule;

namespace Test.Zinnia.Cast
{
    using NUnit.Framework;
    using System.Collections;
    using Test.Zinnia.Utility.Mock;
    using Test.Zinnia.Utility.Stub;
    using UnityEngine;
    using UnityEngine.TestTools;
    using UnityEngine.TestTools.Utils;

    public class StraightLineCastTest
    {
        private GameObject containingObject;
        private StraightLineCastMock subject;
        private GameObject validSurface;

        [SetUp]
        public void SetUp()
        {
#if UNITY_2022_2_OR_NEWER
            Physics.simulationMode = SimulationMode.Script;
#else
            Physics.autoSimulation = false;
#endif
            containingObject = new GameObject("StraightLineCastTest");
            subject = containingObject.AddComponent<StraightLineCastMock>();
            validSurface = GameObject.CreatePrimitive(PrimitiveType.Cube);
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
            Object.DestroyImmediate(validSurface);
#if UNITY_2022_2_OR_NEWER
            Physics.simulationMode = SimulationMode.FixedUpdate;
#else
            Physics.autoSimulation = true;
#endif
        }

        [Test]
        public void CastPointsValidTarget()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            UnityEventListenerMock castResultsChangedMock = new UnityEventListenerMock();
            subject.ResultsChanged.AddListener(castResultsChangedMock.Listen);
            subject.Origin = subject.gameObject;

            validSurface.transform.position = Vector3.forward * 5f;

            subject.ManualOnEnable();
            Physics.Simulate(Time.fixedDeltaTime);
            subject.Process();

            Vector3 expectedStart = Vector3.zero;
            Vector3 expectedEnd = validSurface.transform.position - (Vector3.forward * (validSurface.transform.localScale.z / 2f));

            Assert.That(subject.Points[0], Is.EqualTo(expectedStart).Using(comparer));
            Assert.That(subject.Points[1], Is.EqualTo(expectedEnd).Using(comparer));
            Assert.AreEqual(validSurface.transform, subject.TargetHit.Value.transform);
            Assert.IsTrue(subject.IsTargetHitValid);
            Assert.IsTrue(castResultsChangedMock.Received);
        }

        [Test]
        public void CastPointsInsufficientBeamLength()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            UnityEventListenerMock castResultsChangedMock = new UnityEventListenerMock();
            subject.ResultsChanged.AddListener(castResultsChangedMock.Listen);
            subject.Origin = subject.gameObject;

            validSurface.transform.position = Vector3.forward * 5f;
            subject.MaximumLength = validSurface.transform.position.z / 2f;

            subject.ManualOnEnable();
            Physics.Simulate(Time.fixedDeltaTime);
            subject.Process();

            Vector3 expectedStart = Vector3.zero;
            Vector3 expectedEnd = Vector3.forward * subject.MaximumLength;

            Assert.That(subject.Points[0], Is.EqualTo(expectedStart).Using(comparer));
            Assert.That(subject.Points[1], Is.EqualTo(expectedEnd).Using(comparer));
            Assert.IsFalse(subject.TargetHit.HasValue);
            Assert.IsTrue(castResultsChangedMock.Received);
        }

        [UnityTest]
        public IEnumerator CastPointsInvalidTarget()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            UnityEventListenerMock castResultsChangedMock = new UnityEventListenerMock();
            subject.ResultsChanged.AddListener(castResultsChangedMock.Listen);
            subject.Origin = subject.gameObject;

            validSurface.transform.position = Vector3.forward * 5f;
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

            subject.ManualOnEnable();
            Physics.Simulate(Time.fixedDeltaTime);
            subject.Process();

            Vector3 expectedStart = Vector3.zero;
            Vector3 expectedEnd = validSurface.transform.position - (Vector3.forward * (validSurface.transform.localScale.z / 2f));

            Assert.That(subject.Points[0], Is.EqualTo(expectedStart).Using(comparer));
            Assert.That(subject.Points[1], Is.EqualTo(expectedEnd).Using(comparer));
            Assert.AreEqual(validSurface.transform, subject.TargetHit.Value.transform);
            Assert.IsFalse(subject.IsTargetHitValid);
            Assert.IsTrue(castResultsChangedMock.Received);
        }

        [UnityTest]
        public IEnumerator CastPointsInvalidTargetPoint()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            UnityEventListenerMock castResultsChangedMock = new UnityEventListenerMock();
            subject.ResultsChanged.AddListener(castResultsChangedMock.Listen);
            subject.Origin = subject.gameObject;

            validSurface.transform.position = Vector3.forward * 5f;
            validSurface.AddComponent<RuleStub>();
            NegationRule negationRule = validSurface.AddComponent<NegationRule>();
            Vector3RuleStub pointRule = validSurface.AddComponent<Vector3RuleStub>();

            yield return null;

            negationRule.Rule = new RuleContainer
            {
                Interface = pointRule
            };
            subject.TargetPointValidity = new RuleContainer
            {
                Interface = negationRule
            };

            Vector3 expectedStart = Vector3.zero;
            Vector3 expectedEnd = validSurface.transform.position - (Vector3.forward * (validSurface.transform.localScale.z / 2f));
            pointRule.toMatch = expectedEnd;

            subject.ManualOnEnable();
            Physics.Simulate(Time.fixedDeltaTime);
            subject.Process();

            Assert.That(subject.Points[0], Is.EqualTo(expectedStart).Using(comparer));
            Assert.That(subject.Points[1], Is.EqualTo(expectedEnd).Using(comparer));
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

            validSurface.transform.position = Vector3.forward * 5f;

            subject.ManualOnEnable();
            subject.gameObject.SetActive(false);
            subject.ManualOnDisable();
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

            validSurface.transform.position = Vector3.forward * 5f;

            subject.ManualOnEnable();
            subject.enabled = false;
            subject.ManualOnDisable();
            Physics.Simulate(Time.fixedDeltaTime);
            subject.Process();

            Assert.AreEqual(0, subject.Points.Count);
            Assert.IsFalse(subject.TargetHit.HasValue);
            Assert.IsFalse(castResultsChangedMock.Received);
        }

        [Test]
        public void EventsNotEmittedOnNoOrigin()
        {
            UnityEventListenerMock castResultsChangedMock = new UnityEventListenerMock();
            subject.ResultsChanged.AddListener(castResultsChangedMock.Listen);

            validSurface.transform.position = Vector3.forward * 5f;

            subject.ManualOnEnable();
            Physics.Simulate(Time.fixedDeltaTime);
            subject.Process();

            Assert.AreEqual(4, subject.Points.Count);
            Assert.IsFalse(subject.TargetHit.HasValue);
            Assert.IsFalse(castResultsChangedMock.Received);
        }

        [Test]
        public void ClearOrigin()
        {
            Assert.IsNull(subject.Origin);
            subject.Origin = subject.gameObject;
            Assert.AreEqual(subject.gameObject, subject.Origin);
            subject.ClearOrigin();
            Assert.IsNull(subject.Origin);
        }

        [Test]
        public void ClearOriginInactiveGameObject()
        {
            Assert.IsNull(subject.Origin);
            subject.Origin = subject.gameObject;
            Assert.AreEqual(subject.gameObject, subject.Origin);
            subject.gameObject.SetActive(false);
            subject.ClearOrigin();
            Assert.AreEqual(subject.gameObject, subject.Origin);
        }

        [Test]
        public void ClearOriginInactiveComponent()
        {
            Assert.IsNull(subject.Origin);
            subject.Origin = subject.gameObject;
            Assert.AreEqual(subject.gameObject, subject.Origin);
            subject.enabled = false;
            subject.ClearOrigin();
            Assert.AreEqual(subject.gameObject, subject.Origin);
        }

        [Test]
        public void ClearPhysicsCast()
        {
            Assert.IsNull(subject.PhysicsCast);
            PhysicsCast cast = containingObject.AddComponent<PhysicsCast>();
            subject.PhysicsCast = cast;
            Assert.AreEqual(cast, subject.PhysicsCast);
            subject.ClearPhysicsCast();
            Assert.IsNull(subject.PhysicsCast);
        }

        [Test]
        public void ClearPhysicsCastInactiveGameObject()
        {
            Assert.IsNull(subject.PhysicsCast);
            PhysicsCast cast = containingObject.AddComponent<PhysicsCast>();
            subject.PhysicsCast = cast;
            Assert.AreEqual(cast, subject.PhysicsCast);
            subject.gameObject.SetActive(false);
            subject.ClearPhysicsCast();
            Assert.AreEqual(cast, subject.PhysicsCast);
        }

        [Test]
        public void ClearPhysicsCastInactiveComponent()
        {
            Assert.IsNull(subject.PhysicsCast);
            PhysicsCast cast = containingObject.AddComponent<PhysicsCast>();
            subject.PhysicsCast = cast;
            Assert.AreEqual(cast, subject.PhysicsCast);
            subject.enabled = false;
            subject.ClearPhysicsCast();
            Assert.AreEqual(cast, subject.PhysicsCast);
        }

        [Test]
        public void ClearTargetValidity()
        {
            Assert.IsNull(subject.TargetValidity);
            RuleContainer rule = new RuleContainer();
            subject.TargetValidity = rule;
            Assert.AreEqual(rule, subject.TargetValidity);
            subject.ClearTargetValidity();
            Assert.IsNull(subject.TargetValidity);
        }

        [Test]
        public void ClearTargetValidityInactiveGameObject()
        {
            Assert.IsNull(subject.TargetValidity);
            RuleContainer rule = new RuleContainer();
            subject.TargetValidity = rule;
            Assert.AreEqual(rule, subject.TargetValidity);
            subject.gameObject.SetActive(false);
            subject.ClearTargetValidity();
            Assert.AreEqual(rule, subject.TargetValidity);
        }

        [Test]
        public void ClearTargetValidityInactiveComponent()
        {
            Assert.IsNull(subject.TargetValidity);
            RuleContainer rule = new RuleContainer();
            subject.TargetValidity = rule;
            Assert.AreEqual(rule, subject.TargetValidity);
            subject.enabled = false;
            subject.ClearTargetValidity();
            Assert.AreEqual(rule, subject.TargetValidity);
        }

        [Test]
        public void ClearTargetPointValidity()
        {
            Assert.IsNull(subject.TargetPointValidity);
            RuleContainer rule = new RuleContainer();
            subject.TargetPointValidity = rule;
            Assert.AreEqual(rule, subject.TargetPointValidity);
            subject.ClearTargetPointValidity();
            Assert.IsNull(subject.TargetPointValidity);
        }

        [Test]
        public void ClearTargetPointValidityInactiveGameObject()
        {
            Assert.IsNull(subject.TargetPointValidity);
            RuleContainer rule = new RuleContainer();
            subject.TargetPointValidity = rule;
            Assert.AreEqual(rule, subject.TargetPointValidity);
            subject.gameObject.SetActive(false);
            subject.ClearTargetPointValidity();
            Assert.AreEqual(rule, subject.TargetPointValidity);
        }

        [Test]
        public void ClearTargetPointValidityInactiveComponent()
        {
            Assert.IsNull(subject.TargetPointValidity);
            RuleContainer rule = new RuleContainer();
            subject.TargetPointValidity = rule;
            Assert.AreEqual(rule, subject.TargetPointValidity);
            subject.enabled = false;
            subject.ClearTargetPointValidity();
            Assert.AreEqual(rule, subject.TargetPointValidity);
        }

        [Test]
        public void ClearDestinationPointOverride()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            subject.DestinationPointOverride = Vector3.one;

            Assert.That(subject.DestinationPointOverride, Is.EqualTo(Vector3.one).Using(comparer));

            subject.ClearDestinationPointOverride();

            Assert.AreEqual(null, subject.DestinationPointOverride);
        }
    }

    public class StraightLineCastMock : StraightLineCast
    {
        public EventData GetEventData()
        {
            return eventData;
        }

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