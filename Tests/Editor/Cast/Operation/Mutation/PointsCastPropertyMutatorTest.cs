using Zinnia.Cast;
using Zinnia.Cast.Operation.Mutation;
using Zinnia.Rule;

namespace Test.Zinnia.Cast.Operation.Mutation
{
    using NUnit.Framework;
    using UnityEngine;
    using UnityEngine.TestTools.Utils;

    public class PointsCastPropertyMutatorTest
    {
        private GameObject containingObject;
        private PointsCastPropertyMutator subject;

        [SetUp]
        public void SetUp()
        {
#if UNITY_2022_2_OR_NEWER
            Physics.simulationMode = SimulationMode.Script;
#else
            Physics.autoSimulation = false;
#endif
            containingObject = new GameObject("PointsCastPropertyMutatorTest");
            subject = containingObject.AddComponent<PointsCastPropertyMutator>();

        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
#if UNITY_2022_2_OR_NEWER
            Physics.simulationMode = SimulationMode.FixedUpdate;
#else
            Physics.autoSimulation = true;
#endif
        }

        [Test]
        public void ClearTarget()
        {
            Assert.IsNull(subject.Target);
            StraightLineCast cast = containingObject.AddComponent<StraightLineCast>();
            subject.Target = cast;
            Assert.AreEqual(cast, subject.Target);
            subject.ClearTarget();
            Assert.IsNull(subject.Target);
        }

        [Test]
        public void ClearTargetInactiveGameObject()
        {
            Assert.IsNull(subject.Target);
            StraightLineCast cast = containingObject.AddComponent<StraightLineCast>();
            subject.Target = cast;
            Assert.AreEqual(cast, subject.Target);
            subject.gameObject.SetActive(false);
            subject.ClearTarget();
            Assert.AreEqual(cast, subject.Target);
        }

        [Test]
        public void ClearTargetInactiveComponent()
        {
            Assert.IsNull(subject.Target);
            StraightLineCast cast = containingObject.AddComponent<StraightLineCast>();
            subject.Target = cast;
            Assert.AreEqual(cast, subject.Target);
            subject.enabled = false;
            subject.ClearTarget();
            Assert.AreEqual(cast, subject.Target);
        }

        [Test]
        public void ClearOrigin()
        {
            Assert.IsNull(subject.Origin);
            subject.Origin = containingObject;
            Assert.AreEqual(containingObject, subject.Origin);
            subject.ClearOrigin();
            Assert.IsNull(subject.Origin);
        }

        [Test]
        public void ClearOriginInactiveGameObject()
        {
            Assert.IsNull(subject.Origin);
            subject.Origin = containingObject;
            Assert.AreEqual(containingObject, subject.Origin);
            subject.gameObject.SetActive(false);
            subject.ClearOrigin();
            Assert.AreEqual(containingObject, subject.Origin);
        }

        [Test]
        public void ClearOriginInactiveComponent()
        {
            Assert.IsNull(subject.Origin);
            subject.Origin = containingObject;
            Assert.AreEqual(containingObject, subject.Origin);
            subject.enabled = false;
            subject.ClearOrigin();
            Assert.AreEqual(containingObject, subject.Origin);
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
        public void SetTarget()
        {
            StraightLineCast cast = containingObject.AddComponent<StraightLineCast>();

            Assert.IsNull(subject.Target);
            subject.SetTarget(containingObject);
            Assert.AreEqual(cast, subject.Target);
        }

        [Test]
        public void SetTargetInChild()
        {
            GameObject child = new GameObject("PointsCastPropertyMutatorTest");
            child.transform.SetParent(containingObject.transform);

            StraightLineCast cast = child.AddComponent<StraightLineCast>();

            Assert.IsNull(subject.Target);
            subject.SetTarget(containingObject);
            Assert.AreEqual(cast, subject.Target);
        }

        [Test]
        public void SetTargetInParent()
        {
            GameObject parent = new GameObject("PointsCastPropertyMutatorTest");
            containingObject.transform.SetParent(parent.transform);

            StraightLineCast cast = parent.AddComponent<StraightLineCast>();
            Assert.IsNull(subject.Target);
            subject.SetTarget(containingObject);
            Assert.AreEqual(cast, subject.Target);

            Object.DestroyImmediate(parent);
        }

        [Test]
        public void SetTargetInactiveGameObject()
        {
            containingObject.AddComponent<StraightLineCast>();

            Assert.IsNull(subject.Target);
            subject.gameObject.SetActive(false);
            subject.SetTarget(containingObject);
            Assert.IsNull(subject.Target);
        }

        [Test]
        public void SetTargetInactiveComponent()
        {
            containingObject.AddComponent<StraightLineCast>();

            Assert.IsNull(subject.Target);
            subject.enabled = false;
            subject.SetTarget(containingObject);
            Assert.IsNull(subject.Target);
        }

        [Test]
        public void SetTargetNullParameter()
        {
            StraightLineCast cast = containingObject.AddComponent<StraightLineCast>();
            subject.Target = cast;

            Assert.AreEqual(cast, subject.Target);
            subject.SetTarget(null);
            Assert.AreEqual(cast, subject.Target);
        }

        [Test]
        public void SetDestinationPointOverride()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);

            Assert.IsNull(subject.DestinationPointOverride);
            subject.SetDestinationPointOverride(Vector3.one);
            Assert.That(subject.DestinationPointOverride, Is.EqualTo(Vector3.one).Using(comparer));
        }

        [Test]
        public void ClearDestinationPointOverride()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            StraightLineCast cast = containingObject.AddComponent<StraightLineCast>();
            subject.Target = cast;
            subject.SetDestinationPointOverride(Vector3.one);

            Assert.That(subject.DestinationPointOverride, Is.EqualTo(Vector3.one).Using(comparer));
            Assert.That(subject.Target.DestinationPointOverride, Is.EqualTo(Vector3.one).Using(comparer));

            subject.ClearDestinationPointOverride();

            Assert.IsNull(subject.DestinationPointOverride);
            Assert.IsNull(subject.Target.DestinationPointOverride);
        }

        [Test]
        public void ClearDestinationPointOverrideInactiveGameObject()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject castObject = new GameObject("PointsCastPropertyMutatorTest");
            StraightLineCast cast = castObject.AddComponent<StraightLineCast>();
            subject.Target = cast;
            subject.SetDestinationPointOverride(Vector3.one);

            Assert.That(subject.DestinationPointOverride, Is.EqualTo(Vector3.one).Using(comparer));
            Assert.That(subject.Target.DestinationPointOverride, Is.EqualTo(Vector3.one).Using(comparer));

            subject.gameObject.SetActive(false);
            subject.ClearDestinationPointOverride();

            Assert.That(subject.DestinationPointOverride, Is.EqualTo(Vector3.one).Using(comparer));
            Assert.That(subject.Target.DestinationPointOverride, Is.EqualTo(Vector3.one).Using(comparer));

            Object.DestroyImmediate(castObject);
        }

        [Test]
        public void ClearDestinationPointOverrideInactiveComponent()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            StraightLineCast cast = containingObject.AddComponent<StraightLineCast>();
            subject.Target = cast;
            subject.SetDestinationPointOverride(Vector3.one);

            Assert.That(subject.DestinationPointOverride, Is.EqualTo(Vector3.one).Using(comparer));
            Assert.That(subject.Target.DestinationPointOverride, Is.EqualTo(Vector3.one).Using(comparer));

            subject.enabled = false;
            subject.ClearDestinationPointOverride();

            Assert.That(subject.DestinationPointOverride, Is.EqualTo(Vector3.one).Using(comparer));
            Assert.That(subject.Target.DestinationPointOverride, Is.EqualTo(Vector3.one).Using(comparer));
        }

        [Test]
        public void ClearDestinationPointOverrideNullTarget()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            StraightLineCast cast = containingObject.AddComponent<StraightLineCast>();
            subject.Target = cast;
            subject.SetDestinationPointOverride(Vector3.one);

            Assert.That(subject.DestinationPointOverride, Is.EqualTo(Vector3.one).Using(comparer));
            Assert.That(subject.Target.DestinationPointOverride, Is.EqualTo(Vector3.one).Using(comparer));

            subject.Target = null;
            subject.ClearDestinationPointOverride();

            Assert.That(subject.DestinationPointOverride, Is.EqualTo(Vector3.one).Using(comparer));
        }
    }
}