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
    using Assert = UnityEngine.Assertions.Assert;

    public class StraightLineCastTest
    {
        private GameObject containingObject;
        private StraightLineCastMock subject;
        private GameObject validSurface;

        [SetUp]
        public void SetUp()
        {
            Physics.autoSimulation = false;
            containingObject = new GameObject();
            subject = containingObject.AddComponent<StraightLineCastMock>();
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

            validSurface.transform.position = Vector3.forward * 5f;

            subject.ManualOnEnable();
            Physics.Simulate(Time.fixedDeltaTime);
            subject.Process();

            Vector3 expectedStart = Vector3.zero;
            Vector3 expectedEnd = validSurface.transform.position - (Vector3.forward * (validSurface.transform.localScale.z / 2f));

            Assert.AreEqual(expectedStart, subject.Points[0]);
            Assert.AreEqual(expectedEnd, subject.Points[1]);
            Assert.AreEqual(validSurface.transform, subject.TargetHit.Value.transform);
            Assert.IsTrue(subject.IsTargetHitValid);
            Assert.IsTrue(castResultsChangedMock.Received);
            Assert.AreEqual("{ HitData = { barycentricCoordinate = (1.0, 0.0, 0.0) | Collider = Cube (UnityEngine.BoxCollider) | Distance = 4.5 | Lightmap Coord = (0.0, 0.0) | Normal = (0.0, 0.0, -1.0) | Point = (0.0, 0.0, 4.5) | Rigidbody = [null] | Texture Coord = (0.0, 0.0) | Texture Coord2 = (0.0, 0.0) | Transform = Cube (UnityEngine.Transform) | Triangle Index = -1 } | IsValid = True }", subject.GetEventData().ToString());
        }

        [Test]
        public void CastPointsInsufficientBeamLength()
        {
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

            Assert.AreEqual(expectedStart, subject.Points[0]);
            Assert.AreEqual(expectedEnd, subject.Points[1]);
            Assert.IsFalse(subject.TargetHit.HasValue);
            Assert.IsTrue(castResultsChangedMock.Received);
        }

        [UnityTest]
        public IEnumerator CastPointsInvalidTarget()
        {
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

            Assert.AreEqual(expectedStart, subject.Points[0]);
            Assert.AreEqual(expectedEnd, subject.Points[1]);
            Assert.AreEqual(validSurface.transform, subject.TargetHit.Value.transform);
            Assert.IsFalse(subject.IsTargetHitValid);
            Assert.IsTrue(castResultsChangedMock.Received);
        }

        [UnityTest]
        public IEnumerator CastPointsInvalidTargetPoint()
        {
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

            Assert.AreEqual(expectedStart, subject.Points[0]);
            Assert.AreEqual(expectedEnd, subject.Points[1]);
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
            subject.DestinationPointOverride = Vector3.one;

            Assert.AreEqual(Vector3.one, subject.DestinationPointOverride);

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