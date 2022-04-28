using Zinnia.Cast;
using Zinnia.Data.Collection.List;
using Zinnia.Rule;
using Zinnia.Tracking;

namespace Test.Zinnia.Tracking
{
    using NUnit.Framework;
    using System.Collections;
    using Test.Zinnia.Utility.Mock;
    using Test.Zinnia.Utility.Stub;
    using UnityEngine;
    using UnityEngine.TestTools;
    using Assert = UnityEngine.Assertions.Assert;

    public class SurfaceLocatorTest
    {
        private GameObject containingObject;
        private SurfaceLocator subject;
        private WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

        [SetUp]
        public void SetUp()
        {
            Physics.autoSimulation = false;
            containingObject = new GameObject("ContainingObject");
            subject = containingObject.AddComponent<SurfaceLocator>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
            Physics.autoSimulation = true;
        }

        [Test]
        public void ValidSurface()
        {
            GameObject validSurface = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GameObject searchOrigin = new GameObject("SearchOrigin");

            UnityEventListenerMock surfaceLocatedMock = new UnityEventListenerMock();
            subject.SurfaceLocated.AddListener(surfaceLocatedMock.Listen);

            validSurface.transform.position = Vector3.forward * 5f;

            subject.SearchOrigin = searchOrigin;
            subject.SearchDirection = Vector3.forward;

            //Process just calls Locate() so may as well just test the first point
            Physics.Simulate(Time.fixedDeltaTime);
            subject.Process();

            Assert.IsTrue(surfaceLocatedMock.Received);
            Assert.AreEqual(validSurface.transform, subject.surfaceData.Transform);

            subject.gameObject.SetActive(false);
            subject.gameObject.SetActive(true);

            Assert.AreEqual(null, subject.surfaceData.Transform);

            Object.DestroyImmediate(validSurface);
            Object.DestroyImmediate(searchOrigin);
        }

        [Test]
        public void MissingSurface()
        {
            GameObject searchOrigin = new GameObject("SearchOrigin");

            UnityEventListenerMock surfaceLocatedMock = new UnityEventListenerMock();
            subject.SurfaceLocated.AddListener(surfaceLocatedMock.Listen);

            subject.SearchOrigin = searchOrigin;
            subject.SearchDirection = Vector3.down;

            Physics.Simulate(Time.fixedDeltaTime);
            subject.Locate();
            Assert.IsFalse(surfaceLocatedMock.Received);
            Assert.IsNull(subject.surfaceData.Transform);

            Object.DestroyImmediate(searchOrigin);
        }

        [Test]
        public void InvalidLocateSameSurface()
        {
            GameObject validSurface = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GameObject searchOrigin = new GameObject("SearchOrigin");

            UnityEventListenerMock surfaceLocatedMock = new UnityEventListenerMock();
            subject.SurfaceLocated.AddListener(surfaceLocatedMock.Listen);

            validSurface.transform.position = Vector3.forward * 5f;

            subject.SearchOrigin = searchOrigin;
            subject.SearchDirection = Vector3.forward;

            //Process just calls Locate() so may as well just test the first point
            Physics.Simulate(Time.fixedDeltaTime);
            subject.Process();

            Assert.IsTrue(surfaceLocatedMock.Received);
            Assert.AreEqual(validSurface.transform, subject.surfaceData.Transform);

            surfaceLocatedMock.Reset();

            subject.Process();

            Assert.IsFalse(surfaceLocatedMock.Received);
            Assert.AreEqual(validSurface.transform, subject.surfaceData.Transform);

            Object.DestroyImmediate(validSurface);
            Object.DestroyImmediate(searchOrigin);
        }

        [Test]
        public void ValidLocateSameSurfaceIgnoreEquality()
        {
            GameObject validSurface = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GameObject searchOrigin = new GameObject("SearchOrigin");

            UnityEventListenerMock surfaceLocatedMock = new UnityEventListenerMock();
            subject.SurfaceLocated.AddListener(surfaceLocatedMock.Listen);

            validSurface.transform.position = Vector3.forward * 5f;

            subject.MustChangePosition = false;
            subject.SearchOrigin = searchOrigin;
            subject.SearchDirection = Vector3.forward;

            //Process just calls Locate() so may as well just test the first point
            Physics.Simulate(Time.fixedDeltaTime);
            subject.Process();

            Assert.IsTrue(surfaceLocatedMock.Received);
            Assert.AreEqual(validSurface.transform, subject.surfaceData.Transform);

            surfaceLocatedMock.Reset();

            subject.Process();

            Assert.IsTrue(surfaceLocatedMock.Received);
            Assert.AreEqual(validSurface.transform, subject.surfaceData.Transform);

            Object.DestroyImmediate(validSurface);
            Object.DestroyImmediate(searchOrigin);
        }

        [UnityTest]
        public IEnumerator InvalidSurfaceDueToTargetValidity()
        {
            Physics.autoSimulation = true;

            GameObject invalidSurface = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GameObject searchOrigin = new GameObject("SearchOrigin");

            UnityEventListenerMock surfaceLocatedMock = new UnityEventListenerMock();
            subject.SurfaceLocated.AddListener(surfaceLocatedMock.Listen);

            invalidSurface.transform.position = Vector3.forward * 5f;
            invalidSurface.AddComponent<RuleStub>();
            NegationRule negationRule = invalidSurface.AddComponent<NegationRule>();
            AnyComponentTypeRule anyComponentTypeRule = invalidSurface.AddComponent<AnyComponentTypeRule>();
            SerializableTypeComponentObservableList rules = containingObject.AddComponent<SerializableTypeComponentObservableList>();
            yield return null;

            anyComponentTypeRule.ComponentTypes = rules;
            rules.Add(typeof(RuleStub));

            negationRule.Rule = new RuleContainer
            {
                Interface = anyComponentTypeRule
            };
            subject.TargetValidity = new RuleContainer
            {
                Interface = negationRule
            };

            subject.SearchOrigin = searchOrigin;
            subject.SearchDirection = Vector3.forward;

            yield return waitForFixedUpdate;
            subject.Locate();
            yield return waitForFixedUpdate;
            Assert.IsFalse(surfaceLocatedMock.Received);
            Assert.IsNull(subject.surfaceData.Transform);

            Object.DestroyImmediate(invalidSurface);
            Object.DestroyImmediate(searchOrigin);
        }

        [UnityTest]
        public IEnumerator ValidSurfaceDueToTargetValidity()
        {
            Physics.autoSimulation = true;

            GameObject validSurface = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GameObject searchOrigin = new GameObject("SearchOrigin");

            UnityEventListenerMock surfaceLocatedMock = new UnityEventListenerMock();
            subject.SurfaceLocated.AddListener(surfaceLocatedMock.Listen);

            validSurface.transform.position = Vector3.forward * 5f;
            validSurface.AddComponent<RuleStub>();
            AnyComponentTypeRule anyComponentTypeRule = validSurface.AddComponent<AnyComponentTypeRule>();
            SerializableTypeComponentObservableList rules = containingObject.AddComponent<SerializableTypeComponentObservableList>();
            yield return null;

            anyComponentTypeRule.ComponentTypes = rules;
            rules.Add(typeof(RuleStub));

            subject.TargetValidity = new RuleContainer
            {
                Interface = anyComponentTypeRule
            };

            subject.SearchOrigin = searchOrigin;
            subject.SearchDirection = Vector3.forward;

            yield return waitForFixedUpdate;
            subject.Locate();
            yield return waitForFixedUpdate;
            Assert.IsTrue(surfaceLocatedMock.Received);
            Assert.AreEqual(validSurface.transform, subject.surfaceData.Transform);

            Object.DestroyImmediate(validSurface);
            Object.DestroyImmediate(searchOrigin);
        }

        [UnityTest]
        public IEnumerator ValidSurfaceDueToEventualTargetValidity()
        {
            Physics.autoSimulation = true;

            GameObject invalidSurface = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GameObject validSurface = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GameObject searchOrigin = new GameObject("SearchOrigin");

            UnityEventListenerMock surfaceLocatedMock = new UnityEventListenerMock();
            subject.SurfaceLocated.AddListener(surfaceLocatedMock.Listen);

            validSurface.transform.position = Vector3.forward * 10f;
            invalidSurface.transform.position = Vector3.forward * 5f;
            invalidSurface.AddComponent<RuleStub>();
            NegationRule negationRule = invalidSurface.AddComponent<NegationRule>();
            AnyComponentTypeRule anyComponentTypeRule = invalidSurface.AddComponent<AnyComponentTypeRule>();
            SerializableTypeComponentObservableList rules = containingObject.AddComponent<SerializableTypeComponentObservableList>();
            yield return null;

            anyComponentTypeRule.ComponentTypes = rules;
            rules.Add(typeof(RuleStub));

            negationRule.Rule = new RuleContainer
            {
                Interface = anyComponentTypeRule
            };
            subject.TargetValidity = new RuleContainer
            {
                Interface = negationRule
            };

            subject.SearchOrigin = searchOrigin;
            subject.SearchDirection = Vector3.forward;

            yield return waitForFixedUpdate;
            subject.Locate();
            yield return waitForFixedUpdate;
            Assert.IsTrue(surfaceLocatedMock.Received);
            Assert.AreEqual(validSurface.transform, subject.surfaceData.Transform);

            Object.DestroyImmediate(invalidSurface);
            Object.DestroyImmediate(validSurface);
            Object.DestroyImmediate(searchOrigin);
        }

        [UnityTest]
        public IEnumerator InvalidSurfaceDueToTargetPointValidity()
        {
            Physics.autoSimulation = true;

            GameObject invalidSurface = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GameObject searchOrigin = new GameObject("SearchOrigin");

            UnityEventListenerMock surfaceLocatedMock = new UnityEventListenerMock();
            subject.SurfaceLocated.AddListener(surfaceLocatedMock.Listen);

            invalidSurface.transform.position = Vector3.forward * 5f;
            invalidSurface.AddComponent<RuleStub>();
            NegationRule negationRule = invalidSurface.AddComponent<NegationRule>();
            Vector3RuleStub vector3Point = invalidSurface.AddComponent<Vector3RuleStub>();
            vector3Point.toMatch = invalidSurface.transform.position - (Vector3.forward * invalidSurface.transform.localScale.z * 0.5f);
            yield return null;

            negationRule.Rule = new RuleContainer
            {
                Interface = vector3Point
            };
            subject.TargetPointValidity = new RuleContainer
            {
                Interface = negationRule
            };

            subject.SearchOrigin = searchOrigin;
            subject.SearchDirection = Vector3.forward;

            yield return waitForFixedUpdate;
            subject.Locate();

            yield return waitForFixedUpdate;
            Assert.IsFalse(surfaceLocatedMock.Received);
            Assert.IsNull(subject.surfaceData.Transform);

            Object.DestroyImmediate(invalidSurface);
            Object.DestroyImmediate(searchOrigin);
        }


        [UnityTest]
        public IEnumerator ValidSurfaceDueToTargetPointValidity()
        {
            Physics.autoSimulation = true;

            GameObject validSurface = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GameObject searchOrigin = new GameObject("SearchOrigin");

            UnityEventListenerMock surfaceLocatedMock = new UnityEventListenerMock();
            subject.SurfaceLocated.AddListener(surfaceLocatedMock.Listen);

            validSurface.transform.position = Vector3.forward * 5f;
            Vector3RuleStub vector3Point = validSurface.AddComponent<Vector3RuleStub>();
            vector3Point.toMatch = validSurface.transform.position - (Vector3.forward * validSurface.transform.localScale.z * 0.5f);
            yield return null;

            subject.TargetPointValidity = new RuleContainer
            {
                Interface = vector3Point
            };

            subject.SearchOrigin = searchOrigin;
            subject.SearchDirection = Vector3.forward;

            yield return waitForFixedUpdate;
            subject.Locate();
            yield return waitForFixedUpdate;
            Assert.IsTrue(surfaceLocatedMock.Received);
            Assert.AreEqual(validSurface.transform, subject.surfaceData.Transform);

            Object.DestroyImmediate(validSurface);
            Object.DestroyImmediate(searchOrigin);
        }

        [UnityTest]
        public IEnumerator MissingSurfaceDueToLocatorTermination()
        {
            Physics.autoSimulation = true;

            GameObject terminatingSurface = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GameObject searchOrigin = new GameObject("SearchOrigin");

            UnityEventListenerMock surfaceLocatedMock = new UnityEventListenerMock();
            subject.SurfaceLocated.AddListener(surfaceLocatedMock.Listen);

            terminatingSurface.transform.position = Vector3.forward * 5f;
            terminatingSurface.AddComponent<RuleStub>();

            AnyComponentTypeRule anyComponentTypeRule = terminatingSurface.AddComponent<AnyComponentTypeRule>();
            SerializableTypeComponentObservableList rules = containingObject.AddComponent<SerializableTypeComponentObservableList>();
            yield return null;

            anyComponentTypeRule.ComponentTypes = rules;
            rules.Add(typeof(RuleStub));

            subject.LocatorTermination = new RuleContainer
            {
                Interface = anyComponentTypeRule
            };

            subject.SearchOrigin = searchOrigin;
            subject.SearchDirection = Vector3.forward;

            yield return waitForFixedUpdate;
            subject.Locate();
            yield return waitForFixedUpdate;
            Assert.IsFalse(surfaceLocatedMock.Received);
            Assert.IsNull(subject.surfaceData.Transform);

            Object.DestroyImmediate(terminatingSurface);
            Object.DestroyImmediate(searchOrigin);
        }

        [UnityTest]
        public IEnumerator NoSurfaceDueToLocatorTermination()
        {
            Physics.autoSimulation = true;

            GameObject terminatingSurface = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GameObject validSurface = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GameObject searchOrigin = new GameObject("SearchOrigin");

            UnityEventListenerMock surfaceLocatedMock = new UnityEventListenerMock();
            subject.SurfaceLocated.AddListener(surfaceLocatedMock.Listen);

            validSurface.transform.position = Vector3.forward * 10f;
            terminatingSurface.transform.position = Vector3.forward * 5f;
            terminatingSurface.AddComponent<RuleStub>();

            AnyComponentTypeRule anyComponentTypeRule = terminatingSurface.AddComponent<AnyComponentTypeRule>();
            SerializableTypeComponentObservableList rules = containingObject.AddComponent<SerializableTypeComponentObservableList>();
            yield return null;

            anyComponentTypeRule.ComponentTypes = rules;
            rules.Add(typeof(RuleStub));

            subject.LocatorTermination = new RuleContainer
            {
                Interface = anyComponentTypeRule
            };

            subject.SearchOrigin = searchOrigin;
            subject.SearchDirection = Vector3.forward;

            yield return waitForFixedUpdate;
            subject.Locate();
            yield return waitForFixedUpdate;
            Assert.IsFalse(surfaceLocatedMock.Received);
            Assert.IsNull(subject.surfaceData.Transform);

            Object.DestroyImmediate(terminatingSurface);
            Object.DestroyImmediate(validSurface);
            Object.DestroyImmediate(searchOrigin);
        }

        [UnityTest]
        public IEnumerator NoSurfaceDueToLocatorTerminationWithMidInvalidTarget()
        {
            Physics.autoSimulation = true;

            GameObject invalidSurface = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GameObject terminatingSurface = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GameObject validSurface = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GameObject searchOrigin = new GameObject("SearchOrigin");

            UnityEventListenerMock surfaceLocatedMock = new UnityEventListenerMock();
            subject.SurfaceLocated.AddListener(surfaceLocatedMock.Listen);

            validSurface.transform.position = Vector3.forward * 15f;
            terminatingSurface.transform.position = Vector3.forward * 10f;
            terminatingSurface.AddComponent<RuleStub>();

            AnyComponentTypeRule anyComponentTypeLocatorTerminationRule = terminatingSurface.AddComponent<AnyComponentTypeRule>();
            SerializableTypeComponentObservableList locatorTerminationRules = containingObject.AddComponent<SerializableTypeComponentObservableList>();
            yield return null;

            anyComponentTypeLocatorTerminationRule.ComponentTypes = locatorTerminationRules;
            locatorTerminationRules.Add(typeof(RuleStub));

            subject.LocatorTermination = new RuleContainer
            {
                Interface = anyComponentTypeLocatorTerminationRule
            };

            yield return null;

            invalidSurface.transform.position = Vector3.forward * 5f;
            invalidSurface.AddComponent<AudioListener>();
            NegationRule negationRule = invalidSurface.AddComponent<NegationRule>();
            AnyComponentTypeRule anyComponentTypeTargetValidityRule = invalidSurface.AddComponent<AnyComponentTypeRule>();
            SerializableTypeComponentObservableList targetValidityRules = containingObject.AddComponent<SerializableTypeComponentObservableList>();
            yield return null;

            anyComponentTypeTargetValidityRule.ComponentTypes = targetValidityRules;
            targetValidityRules.Add(typeof(AudioListener));

            negationRule.Rule = new RuleContainer
            {
                Interface = anyComponentTypeTargetValidityRule
            };
            subject.TargetValidity = new RuleContainer
            {
                Interface = negationRule
            };

            subject.SearchOrigin = searchOrigin;
            subject.SearchDirection = Vector3.forward;

            yield return waitForFixedUpdate;
            subject.Locate();
            yield return waitForFixedUpdate;
            Assert.IsFalse(surfaceLocatedMock.Received);
            Assert.IsNull(subject.surfaceData.Transform);

            Object.DestroyImmediate(invalidSurface);
            Object.DestroyImmediate(terminatingSurface);
            Object.DestroyImmediate(validSurface);
            Object.DestroyImmediate(searchOrigin);
        }

        [Test]
        public void EventsNotEmittedOnInactiveGameObject()
        {
            GameObject validSurface = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GameObject searchOrigin = new GameObject("SearchOrigin");

            UnityEventListenerMock surfaceLocatedMock = new UnityEventListenerMock();
            subject.SurfaceLocated.AddListener(surfaceLocatedMock.Listen);

            validSurface.transform.position = Vector3.forward * 5f;

            subject.SearchOrigin = searchOrigin;
            subject.SearchDirection = Vector3.forward;
            subject.gameObject.SetActive(false);
            Physics.Simulate(Time.fixedDeltaTime);
            subject.Process();

            Assert.IsFalse(surfaceLocatedMock.Received);

            Object.DestroyImmediate(validSurface);
            Object.DestroyImmediate(searchOrigin);
        }

        [Test]
        public void EventsNotEmittedOnDisabledComponent()
        {
            GameObject validSurface = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GameObject searchOrigin = new GameObject("SearchOrigin");

            UnityEventListenerMock surfaceLocatedMock = new UnityEventListenerMock();
            subject.SurfaceLocated.AddListener(surfaceLocatedMock.Listen);

            validSurface.transform.position = Vector3.forward * 5f;

            subject.SearchOrigin = searchOrigin;
            subject.SearchDirection = Vector3.forward;
            subject.enabled = false;
            Physics.Simulate(Time.fixedDeltaTime);
            subject.Process();

            Assert.IsFalse(surfaceLocatedMock.Received);

            Object.DestroyImmediate(validSurface);
            Object.DestroyImmediate(searchOrigin);
        }

        [UnityTest]
        public IEnumerator NearestSurface()
        {
            Physics.autoSimulation = true;

            GameObject validSurface = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GameObject validSurface2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GameObject searchOrigin = new GameObject("SearchOrigin");
            validSurface.name = "validSurface";
            validSurface2.name = "validSurface2";

            UnityEventListenerMock surfaceLocatedMock = new UnityEventListenerMock();
            subject.SurfaceLocated.AddListener(surfaceLocatedMock.Listen);

            validSurface.AddComponent<RuleStub>();
            AnyComponentTypeRule anyComponentTypeRule = validSurface.AddComponent<AnyComponentTypeRule>();
            validSurface2.AddComponent<RuleStub>();

            SerializableTypeComponentObservableList rules = containingObject.AddComponent<SerializableTypeComponentObservableList>();
            yield return null;

            anyComponentTypeRule.ComponentTypes = rules;
            rules.Add(typeof(RuleStub));

            subject.TargetValidity = new RuleContainer
            {
                Interface = anyComponentTypeRule
            };

            subject.SearchOrigin = searchOrigin;
            subject.SearchDirection = Vector3.forward;

            validSurface.transform.position = Vector3.forward * 5f;
            validSurface2.transform.position = Vector3.forward * 4.9f;
            GameObject nearestSurface = validSurface2;

            yield return waitForFixedUpdate;
            subject.Locate();
            yield return waitForFixedUpdate;
            Assert.IsTrue(surfaceLocatedMock.Received);
            Assert.IsTrue(nearestSurface.transform == subject.surfaceData.Transform, "The returned surfaceData.Transform is not the nearest");

            Object.DestroyImmediate(validSurface);
            Object.DestroyImmediate(validSurface2);
            Object.DestroyImmediate(searchOrigin);
        }

        [Test]
        public void ClearSearchOrigin()
        {
            Assert.IsNull(subject.SearchOrigin);
            subject.SearchOrigin = containingObject;
            Assert.AreEqual(containingObject, subject.SearchOrigin);
            subject.ClearSearchOrigin();
            Assert.IsNull(subject.SearchOrigin);
        }

        [Test]
        public void ClearSearchOriginInactiveGameObject()
        {
            Assert.IsNull(subject.SearchOrigin);
            subject.SearchOrigin = containingObject;
            Assert.AreEqual(containingObject, subject.SearchOrigin);
            subject.gameObject.SetActive(false);
            subject.ClearSearchOrigin();
            Assert.AreEqual(containingObject, subject.SearchOrigin);
        }

        [Test]
        public void ClearSearchOriginInactiveComponent()
        {
            Assert.IsNull(subject.SearchOrigin);
            subject.SearchOrigin = containingObject;
            Assert.AreEqual(containingObject, subject.SearchOrigin);
            subject.enabled = false;
            subject.ClearSearchOrigin();
            Assert.AreEqual(containingObject, subject.SearchOrigin);
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
        public void ClearLocatorTermination()
        {
            Assert.IsNull(subject.LocatorTermination);
            RuleContainer rule = new RuleContainer();
            subject.LocatorTermination = rule;
            Assert.AreEqual(rule, subject.LocatorTermination);
            subject.ClearLocatorTermination();
            Assert.IsNull(subject.LocatorTermination);
        }

        [Test]
        public void ClearLocatorTerminationInactiveGameObject()
        {
            Assert.IsNull(subject.LocatorTermination);
            RuleContainer rule = new RuleContainer();
            subject.LocatorTermination = rule;
            Assert.AreEqual(rule, subject.LocatorTermination);
            subject.gameObject.SetActive(false);
            subject.ClearLocatorTermination();
            Assert.AreEqual(rule, subject.LocatorTermination);
        }

        [Test]
        public void ClearLocatorTerminationInactiveComponent()
        {
            Assert.IsNull(subject.LocatorTermination);
            RuleContainer rule = new RuleContainer();
            subject.LocatorTermination = rule;
            Assert.AreEqual(rule, subject.LocatorTermination);
            subject.enabled = false;
            subject.ClearLocatorTermination();
            Assert.AreEqual(rule, subject.LocatorTermination);
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
        public void SetSearchDirectionX()
        {
            Assert.AreEqual(Vector3.zero, subject.SearchDirection);
            subject.SetSearchDirectionX(1f);
            Assert.AreEqual(Vector3.right, subject.SearchDirection);
        }

        [Test]
        public void SetSearchDirectionY()
        {
            Assert.AreEqual(Vector3.zero, subject.SearchDirection);
            subject.SetSearchDirectionY(1f);
            Assert.AreEqual(Vector3.up, subject.SearchDirection);
        }

        [Test]
        public void SetSearchDirectionZ()
        {
            Assert.AreEqual(Vector3.zero, subject.SearchDirection);
            subject.SetSearchDirectionZ(1f);
            Assert.AreEqual(Vector3.forward, subject.SearchDirection);
        }

        [Test]
        public void SetDestinationOffsetX()
        {
            Assert.AreEqual(Vector3.zero, subject.DestinationOffset);
            subject.SetDestinationOffsetX(1f);
            Assert.AreEqual(Vector3.right, subject.DestinationOffset);
        }

        [Test]
        public void SetDestinationOffsetY()
        {
            Assert.AreEqual(Vector3.zero, subject.DestinationOffset);
            subject.SetDestinationOffsetY(1f);
            Assert.AreEqual(Vector3.up, subject.DestinationOffset);
        }

        [Test]
        public void SetDestinationOffsetZ()
        {
            Assert.AreEqual(Vector3.zero, subject.DestinationOffset);
            subject.SetDestinationOffsetZ(1f);
            Assert.AreEqual(Vector3.forward, subject.DestinationOffset);
        }
    }
}