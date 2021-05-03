using Zinnia.Cast;
using Zinnia.Cast.Operation.Conversion;

namespace Test.Zinnia.Cast.Operation.Conversion
{
    using NUnit.Framework;
    using UnityEngine;

    public class ToRaycastConverterTest
    {
        private GameObject containingObject;
        private PhysicsCast caster;
        private ToRaycastConverter subject;
        private GameObject validSurface;

        [SetUp]
        public void SetUp()
        {
            Physics.autoSimulation = false;
            containingObject = new GameObject();
            caster = containingObject.AddComponent<PhysicsCast>();
            subject = containingObject.AddComponent<ToRaycastConverter>();
            validSurface = GameObject.CreatePrimitive(PrimitiveType.Cube);
            validSurface.transform.position = (Vector3.forward * 2f) + (Vector3.up * 0.55f);
            Physics.Simulate(Time.fixedDeltaTime);
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
            Object.DestroyImmediate(validSurface);
            Physics.autoSimulation = true;
        }

        [Test]
        public void ConvertFromBoxCast()
        {
            bool result = caster.CustomBoxCast(Vector3.zero, Vector3.one * 0.1f, Vector3.forward, out RaycastHit hitData, Quaternion.identity, 10f);
            Assert.IsTrue(result);
            Assert.AreEqual(validSurface, hitData.transform.gameObject);

            caster.ConvertTo = subject;
            result = caster.CustomBoxCast(Vector3.zero, Vector3.one * 0.1f, Vector3.forward, out RaycastHit missData, Quaternion.identity, 10f);
            Assert.IsFalse(result);
            Assert.IsNull(missData.transform);
        }

        [Test]
        public void ConvertFromCapsuleCast()
        {
            bool result = caster.CustomCapsuleCast(Vector3.up * 0.1f, Vector3.up * -0.1f, 0.1f, Vector3.forward, out RaycastHit hitData, 10f);
            Assert.IsTrue(result);
            Assert.AreEqual(validSurface, hitData.transform.gameObject);

            caster.ConvertTo = subject;
            result = caster.CustomCapsuleCast(Vector3.up * 0.1f, Vector3.up * -0.1f, 0.1f, Vector3.forward, out RaycastHit missData, 10f);
            Assert.IsFalse(result);
            Assert.IsNull(missData.transform);
        }

        [Test]
        public void ConvertFromLinecast()
        {
            caster.ConvertTo = subject;
            bool result = caster.CustomLinecast(Vector3.zero, Vector3.forward * 10f, out RaycastHit missData);
            Assert.IsFalse(result);
            Assert.IsNull(missData.transform);
        }

        [Test]
        public void ConvertFromRaycast()
        {
            caster.ConvertTo = subject;
            bool result = caster.CustomRaycast(new Ray(Vector3.zero, Vector3.forward), out RaycastHit missData, 10f);
            Assert.IsFalse(result);
            Assert.IsNull(missData.transform);
        }

        [Test]
        public void ConvertFromSphereCast()
        {
            bool result = caster.CustomSphereCast(Vector3.zero, 0.1f, Vector3.forward, out RaycastHit hitData, 10f);
            Assert.IsTrue(result);
            Assert.AreEqual(validSurface, hitData.transform.gameObject);

            caster.ConvertTo = subject;
            result = caster.CustomSphereCast(Vector3.zero, 0.1f, Vector3.forward, out RaycastHit missData, 10f);
            Assert.IsFalse(result);
            Assert.IsNull(missData.transform);
        }
    }
}