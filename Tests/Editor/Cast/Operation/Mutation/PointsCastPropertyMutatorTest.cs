using Zinnia.Cast;
using Zinnia.Cast.Operation.Mutation;
using Zinnia.Rule;

namespace Test.Zinnia.Cast.Operation.Mutation
{
    using NUnit.Framework;
    using UnityEngine;

    public class PointsCastPropertyMutatorTest
    {
        private GameObject containingObject;
        private PointsCastPropertyMutator subject;

        [SetUp]
        public void SetUp()
        {
            Physics.autoSimulation = false;
            containingObject = new GameObject();
            subject = containingObject.AddComponent<PointsCastPropertyMutator>();

        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
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
            GameObject child = new GameObject();
            child.transform.SetParent(containingObject.transform);

            StraightLineCast cast = child.AddComponent<StraightLineCast>();

            Assert.IsNull(subject.Target);
            subject.SetTarget(containingObject);
            Assert.AreEqual(cast, subject.Target);
        }

        [Test]
        public void SetTargetInParent()
        {
            GameObject parent = new GameObject();
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
            StraightLineCast cast = containingObject.AddComponent<StraightLineCast>();

            Assert.IsNull(subject.Target);
            subject.gameObject.SetActive(false);
            subject.SetTarget(containingObject);
            Assert.IsNull(subject.Target);
        }

        [Test]
        public void SetTargetInactiveComponent()
        {
            StraightLineCast cast = containingObject.AddComponent<StraightLineCast>();

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
            Assert.IsNull(subject.DestinationPointOverride);
            subject.SetDestinationPointOverride(Vector3.one);
            Assert.AreEqual(Vector3.one, subject.DestinationPointOverride);
        }

        [Test]
        public void ClearDestinationPointOverride()
        {
            StraightLineCast cast = containingObject.AddComponent<StraightLineCast>();
            subject.Target = cast;
            subject.SetDestinationPointOverride(Vector3.one);

            Assert.AreEqual(Vector3.one, subject.DestinationPointOverride);
            Assert.AreEqual(Vector3.one, subject.Target.DestinationPointOverride);

            subject.ClearDestinationPointOverride();

            Assert.IsNull(subject.DestinationPointOverride);
            Assert.IsNull(subject.Target.DestinationPointOverride);
        }

        [Test]
        public void ClearDestinationPointOverrideInactiveGameObject()
        {
            GameObject castObject = new GameObject();
            StraightLineCast cast = castObject.AddComponent<StraightLineCast>();
            subject.Target = cast;
            subject.SetDestinationPointOverride(Vector3.one);

            Assert.AreEqual(Vector3.one, subject.DestinationPointOverride);
            Assert.AreEqual(Vector3.one, subject.Target.DestinationPointOverride);

            subject.gameObject.SetActive(false);
            subject.ClearDestinationPointOverride();

            Assert.AreEqual(Vector3.one, subject.DestinationPointOverride);
            Assert.AreEqual(Vector3.one, subject.Target.DestinationPointOverride);

            Object.DestroyImmediate(castObject);
        }

        [Test]
        public void ClearDestinationPointOverrideInactiveComponent()
        {
            StraightLineCast cast = containingObject.AddComponent<StraightLineCast>();
            subject.Target = cast;
            subject.SetDestinationPointOverride(Vector3.one);

            Assert.AreEqual(Vector3.one, subject.DestinationPointOverride);
            Assert.AreEqual(Vector3.one, subject.Target.DestinationPointOverride);

            subject.enabled = false;
            subject.ClearDestinationPointOverride();

            Assert.AreEqual(Vector3.one, subject.DestinationPointOverride);
            Assert.AreEqual(Vector3.one, subject.Target.DestinationPointOverride);
        }

        [Test]
        public void ClearDestinationPointOverrideNullTarget()
        {
            StraightLineCast cast = containingObject.AddComponent<StraightLineCast>();
            subject.Target = cast;
            subject.SetDestinationPointOverride(Vector3.one);

            Assert.AreEqual(Vector3.one, subject.DestinationPointOverride);
            Assert.AreEqual(Vector3.one, subject.Target.DestinationPointOverride);

            subject.Target = null;
            subject.ClearDestinationPointOverride();

            Assert.AreEqual(Vector3.one, subject.DestinationPointOverride);
        }
    }
}