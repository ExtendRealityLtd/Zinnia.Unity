using Zinnia.Cast;
using Zinnia.Cast.Operation.Conversion;

namespace Test.Zinnia.Cast
{
    using NUnit.Framework;
    using System.Collections.Generic;
    using UnityEngine;

    public class PhysicsCastTest
    {
        private GameObject containingObject;
        private PhysicsCast subject;
        private GameObject validSurface;

        [SetUp]
        public void SetUp()
        {
            Physics.autoSimulation = false;
            containingObject = new GameObject();
            subject = containingObject.AddComponent<PhysicsCast>();
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
        public void CustomRaycast()
        {
            validSurface.transform.position = Vector3.forward * 2f;
            Physics.Simulate(Time.fixedDeltaTime);

            bool result = subject.CustomRaycast(new Ray(Vector3.zero, Vector3.forward), out RaycastHit hitData, 10f);

            Assert.IsTrue(result);
            Assert.AreEqual(validSurface, hitData.transform.gameObject);

            validSurface.transform.position += Vector3.up * 2f;
            Physics.Simulate(Time.fixedDeltaTime);

            result = subject.CustomRaycast(new Ray(Vector3.zero, Vector3.forward), out RaycastHit missData, 10f);

            Assert.IsFalse(result);
            Assert.IsNull(missData.transform);
        }

        [Test]
        public void CustomRaycastIgnoreLayers()
        {
            validSurface.transform.position = Vector3.forward * 2f;
            validSurface.layer = LayerMask.NameToLayer("Water");
            Physics.Simulate(Time.fixedDeltaTime);

            bool result = subject.CustomRaycast(new Ray(Vector3.zero, Vector3.forward), out RaycastHit hitData, 10f);

            Assert.IsTrue(result);
            Assert.AreEqual(validSurface, hitData.transform.gameObject);

            subject.LayersToIgnore = LayerMask.GetMask("Water");
            Physics.Simulate(Time.fixedDeltaTime);

            result = subject.CustomRaycast(new Ray(Vector3.zero, Vector3.forward), out RaycastHit ignoreData, 10f);

            Assert.IsFalse(result);
            Assert.IsNull(ignoreData.transform);
        }

        [Test]
        public void CustomRaycastIgnoreTrigger()
        {
            validSurface.transform.position = Vector3.forward * 2f;
            validSurface.GetComponent<Collider>().isTrigger = true;
            subject.TriggerInteraction = QueryTriggerInteraction.Collide;
            Physics.Simulate(Time.fixedDeltaTime);

            bool result = subject.CustomRaycast(new Ray(Vector3.zero, Vector3.forward), out RaycastHit hitData, 10f);

            Assert.IsTrue(result);
            Assert.AreEqual(validSurface, hitData.transform.gameObject);

            subject.TriggerInteraction = QueryTriggerInteraction.Ignore;

            Physics.Simulate(Time.fixedDeltaTime);

            result = subject.CustomRaycast(new Ray(Vector3.zero, Vector3.forward), out RaycastHit ignoreData, 10f);

            Assert.IsFalse(result);
            Assert.IsNull(ignoreData.transform);
        }

        [Test]
        public void CustomRaycastAll()
        {
            GameObject backSurface = GameObject.CreatePrimitive(PrimitiveType.Cube);
            backSurface.transform.position = Vector3.forward * 5f;

            validSurface.transform.position = Vector3.forward * 2f;
            Physics.Simulate(Time.fixedDeltaTime);

            IList<RaycastHit> result = subject.CustomRaycastAll(new Ray(Vector3.zero, Vector3.forward), 10f);

            Assert.AreEqual(result.Count, 2);
            Assert.AreEqual(validSurface, result[0].transform.gameObject);
            Assert.AreEqual(backSurface, result[1].transform.gameObject);

            validSurface.transform.position += Vector3.up * 2f;
            Physics.Simulate(Time.fixedDeltaTime);

            result = subject.CustomRaycastAll(new Ray(Vector3.zero, Vector3.forward), 10f);

            Assert.AreEqual(result.Count, 1);
            Assert.AreEqual(backSurface, result[0].transform.gameObject);

            Object.DestroyImmediate(backSurface);
        }

        [Test]
        public void CustomLinecast()
        {
            validSurface.transform.position = Vector3.forward * 2f;
            Physics.Simulate(Time.fixedDeltaTime);

            bool result = subject.CustomLinecast(Vector3.zero, Vector3.forward * 10f, out RaycastHit hitData);

            Assert.IsTrue(result);
            Assert.AreEqual(validSurface, hitData.transform.gameObject);

            validSurface.transform.position += Vector3.up * 2f;
            Physics.Simulate(Time.fixedDeltaTime);

            result = subject.CustomLinecast(Vector3.zero, Vector3.forward * 10f, out RaycastHit missData);

            Assert.IsFalse(result);
            Assert.IsNull(missData.transform);
        }

        [Test]
        public void CustomLinecastIgnoreLayers()
        {
            validSurface.transform.position = Vector3.forward * 2f;
            validSurface.layer = LayerMask.NameToLayer("Water");
            Physics.Simulate(Time.fixedDeltaTime);

            bool result = subject.CustomLinecast(Vector3.zero, Vector3.forward * 10f, out RaycastHit hitData);

            Assert.IsTrue(result);
            Assert.AreEqual(validSurface, hitData.transform.gameObject);

            subject.LayersToIgnore = LayerMask.GetMask("Water");
            Physics.Simulate(Time.fixedDeltaTime);

            result = subject.CustomLinecast(Vector3.zero, Vector3.forward * 10f, out RaycastHit ignoreData);

            Assert.IsFalse(result);
            Assert.IsNull(ignoreData.transform);
        }

        [Test]
        public void CustomLinecastIgnoreTrigger()
        {
            validSurface.transform.position = Vector3.forward * 2f;
            validSurface.GetComponent<Collider>().isTrigger = true;
            subject.TriggerInteraction = QueryTriggerInteraction.Collide;
            Physics.Simulate(Time.fixedDeltaTime);

            bool result = subject.CustomLinecast(Vector3.zero, Vector3.forward * 10f, out RaycastHit hitData);

            Assert.IsTrue(result);
            Assert.AreEqual(validSurface, hitData.transform.gameObject);

            subject.TriggerInteraction = QueryTriggerInteraction.Ignore;

            Physics.Simulate(Time.fixedDeltaTime);

            result = subject.CustomLinecast(Vector3.zero, Vector3.forward * 10f, out RaycastHit ignoreData);

            Assert.IsFalse(result);
            Assert.IsNull(ignoreData.transform);
        }

        [Test]
        public void CustomSphereCast()
        {
            validSurface.transform.position = Vector3.forward * 2f;
            Physics.Simulate(Time.fixedDeltaTime);

            bool result = subject.CustomSphereCast(Vector3.zero, 0.1f, Vector3.forward, out RaycastHit hitData, 10f);

            Assert.IsTrue(result);
            Assert.AreEqual(validSurface, hitData.transform.gameObject);

            validSurface.transform.position += Vector3.up * 2f;
            Physics.Simulate(Time.fixedDeltaTime);

            result = subject.CustomSphereCast(Vector3.zero, 0.1f, Vector3.forward, out RaycastHit missData, 10f);

            Assert.IsFalse(result);
            Assert.IsNull(missData.transform);
        }

        [Test]
        public void CustomSphereCastIgnoreLayers()
        {
            validSurface.transform.position = Vector3.forward * 2f;
            validSurface.layer = LayerMask.NameToLayer("Water");
            Physics.Simulate(Time.fixedDeltaTime);

            bool result = subject.CustomSphereCast(Vector3.zero, 0.1f, Vector3.forward, out RaycastHit hitData, 10f);

            Assert.IsTrue(result);
            Assert.AreEqual(validSurface, hitData.transform.gameObject);

            subject.LayersToIgnore = LayerMask.GetMask("Water");
            Physics.Simulate(Time.fixedDeltaTime);

            result = subject.CustomSphereCast(Vector3.zero, 0.1f, Vector3.forward, out RaycastHit ignoreData, 10f);

            Assert.IsFalse(result);
            Assert.IsNull(ignoreData.transform);
        }

        [Test]
        public void CustomSphereCastIgnoreTrigger()
        {
            validSurface.transform.position = Vector3.forward * 2f;
            validSurface.GetComponent<Collider>().isTrigger = true;
            subject.TriggerInteraction = QueryTriggerInteraction.Collide;
            Physics.Simulate(Time.fixedDeltaTime);

            bool result = subject.CustomSphereCast(Vector3.zero, 0.1f, Vector3.forward, out RaycastHit hitData, 10f);

            Assert.IsTrue(result);
            Assert.AreEqual(validSurface, hitData.transform.gameObject);

            subject.TriggerInteraction = QueryTriggerInteraction.Ignore;

            Physics.Simulate(Time.fixedDeltaTime);

            result = subject.CustomSphereCast(Vector3.zero, 0.1f, Vector3.forward, out RaycastHit ignoreData, 10f);

            Assert.IsFalse(result);
            Assert.IsNull(ignoreData.transform);
        }

        [Test]
        public void CustomSphereCastAll()
        {
            GameObject backSurface = GameObject.CreatePrimitive(PrimitiveType.Cube);
            backSurface.transform.position = Vector3.forward * 5f;

            validSurface.transform.position = Vector3.forward * 2f;
            Physics.Simulate(Time.fixedDeltaTime);

            IList<RaycastHit> result = subject.CustomSphereCastAll(Vector3.zero, 0.1f, Vector3.forward, 10f);

            Assert.AreEqual(result.Count, 2);
            Assert.AreEqual(validSurface, result[0].transform.gameObject);
            Assert.AreEqual(backSurface, result[1].transform.gameObject);

            validSurface.transform.position += Vector3.up * 2f;
            Physics.Simulate(Time.fixedDeltaTime);

            result = subject.CustomSphereCastAll(Vector3.zero, 0.1f, Vector3.forward, 10f);

            Assert.AreEqual(result.Count, 1);
            Assert.AreEqual(backSurface, result[0].transform.gameObject);

            Object.DestroyImmediate(backSurface);
        }

        [Test]
        public void CustomCapsuleCast()
        {
            validSurface.transform.position = Vector3.forward * 2f;
            Physics.Simulate(Time.fixedDeltaTime);

            bool result = subject.CustomCapsuleCast(Vector3.up * 0.1f, Vector3.up * -0.1f, 0.1f, Vector3.forward, out RaycastHit hitData, 10f);

            Assert.IsTrue(result);
            Assert.AreEqual(validSurface, hitData.transform.gameObject);

            validSurface.transform.position += Vector3.up * 2f;
            Physics.Simulate(Time.fixedDeltaTime);

            result = subject.CustomCapsuleCast(Vector3.up * 0.1f, Vector3.up * -0.1f, 0.1f, Vector3.forward, out RaycastHit missData, 10f);

            Assert.IsFalse(result);
            Assert.IsNull(missData.transform);
        }

        [Test]
        public void CustomCapsuleCastIgnoreLayers()
        {
            validSurface.transform.position = Vector3.forward * 2f;
            validSurface.layer = LayerMask.NameToLayer("Water");
            Physics.Simulate(Time.fixedDeltaTime);

            bool result = subject.CustomCapsuleCast(Vector3.up * 0.1f, Vector3.up * -0.1f, 0.1f, Vector3.forward, out RaycastHit hitData, 10f);

            Assert.IsTrue(result);
            Assert.AreEqual(validSurface, hitData.transform.gameObject);

            subject.LayersToIgnore = LayerMask.GetMask("Water");
            Physics.Simulate(Time.fixedDeltaTime);

            result = subject.CustomCapsuleCast(Vector3.up * 0.1f, Vector3.up * -0.1f, 0.1f, Vector3.forward, out RaycastHit ignoreData, 10f);

            Assert.IsFalse(result);
            Assert.IsNull(ignoreData.transform);
        }

        [Test]
        public void CustomCapsuleCastIgnoreTrigger()
        {
            validSurface.transform.position = Vector3.forward * 2f;
            validSurface.GetComponent<Collider>().isTrigger = true;
            subject.TriggerInteraction = QueryTriggerInteraction.Collide;
            Physics.Simulate(Time.fixedDeltaTime);

            bool result = subject.CustomCapsuleCast(Vector3.up * 0.1f, Vector3.up * -0.1f, 0.1f, Vector3.forward, out RaycastHit hitData, 10f);

            Assert.IsTrue(result);
            Assert.AreEqual(validSurface, hitData.transform.gameObject);

            subject.TriggerInteraction = QueryTriggerInteraction.Ignore;

            Physics.Simulate(Time.fixedDeltaTime);

            result = subject.CustomCapsuleCast(Vector3.up * 0.1f, Vector3.up * -0.1f, 0.1f, Vector3.forward, out RaycastHit ignoreData, 10f);

            Assert.IsFalse(result);
            Assert.IsNull(ignoreData.transform);
        }

        [Test]
        public void CustomCapsuleCastAll()
        {
            GameObject backSurface = GameObject.CreatePrimitive(PrimitiveType.Cube);
            backSurface.transform.position = Vector3.forward * 5f;

            validSurface.transform.position = Vector3.forward * 2f;
            Physics.Simulate(Time.fixedDeltaTime);

            IList<RaycastHit> result = subject.CustomCapsuleCastAll(Vector3.up * 0.1f, Vector3.up * -0.1f, 0.1f, Vector3.forward, 10f);

            Assert.AreEqual(result.Count, 2);
            Assert.AreEqual(validSurface, result[0].transform.gameObject);
            Assert.AreEqual(backSurface, result[1].transform.gameObject);

            validSurface.transform.position += Vector3.up * 2f;
            Physics.Simulate(Time.fixedDeltaTime);

            result = subject.CustomCapsuleCastAll(Vector3.up * 0.1f, Vector3.up * -0.1f, 0.1f, Vector3.forward, 10f);

            Assert.AreEqual(result.Count, 1);
            Assert.AreEqual(backSurface, result[0].transform.gameObject);

            Object.DestroyImmediate(backSurface);
        }

        [Test]
        public void CustomBoxCast()
        {
            validSurface.transform.position = Vector3.forward * 2f;
            Physics.Simulate(Time.fixedDeltaTime);

            bool result = subject.CustomBoxCast(Vector3.zero, Vector3.one * 0.1f, Vector3.forward, out RaycastHit hitData, Quaternion.identity, 10f);

            Assert.IsTrue(result);
            Assert.AreEqual(validSurface, hitData.transform.gameObject);

            validSurface.transform.position += Vector3.up * 2f;
            Physics.Simulate(Time.fixedDeltaTime);

            result = subject.CustomBoxCast(Vector3.zero, Vector3.one * 0.1f, Vector3.forward, out RaycastHit missData, Quaternion.identity, 10f);

            Assert.IsFalse(result);
            Assert.IsNull(missData.transform);
        }

        [Test]
        public void CustomBoxCastIgnoreLayers()
        {
            validSurface.transform.position = Vector3.forward * 2f;
            validSurface.layer = LayerMask.NameToLayer("Water");
            Physics.Simulate(Time.fixedDeltaTime);

            bool result = subject.CustomBoxCast(Vector3.zero, Vector3.one * 0.1f, Vector3.forward, out RaycastHit hitData, Quaternion.identity, 10f);

            Assert.IsTrue(result);
            Assert.AreEqual(validSurface, hitData.transform.gameObject);

            subject.LayersToIgnore = LayerMask.GetMask("Water");
            Physics.Simulate(Time.fixedDeltaTime);

            result = subject.CustomBoxCast(Vector3.zero, Vector3.one * 0.1f, Vector3.forward, out RaycastHit ignoreData, Quaternion.identity, 10f);

            Assert.IsFalse(result);
            Assert.IsNull(ignoreData.transform);
        }

        [Test]
        public void CustomBoxCastIgnoreTrigger()
        {
            validSurface.transform.position = Vector3.forward * 2f;
            validSurface.GetComponent<Collider>().isTrigger = true;
            subject.TriggerInteraction = QueryTriggerInteraction.Collide;
            Physics.Simulate(Time.fixedDeltaTime);

            bool result = subject.CustomBoxCast(Vector3.zero, Vector3.one * 0.1f, Vector3.forward, out RaycastHit hitData, Quaternion.identity, 10f);

            Assert.IsTrue(result);
            Assert.AreEqual(validSurface, hitData.transform.gameObject);

            subject.TriggerInteraction = QueryTriggerInteraction.Ignore;

            Physics.Simulate(Time.fixedDeltaTime);

            result = subject.CustomBoxCast(Vector3.zero, Vector3.one * 0.1f, Vector3.forward, out RaycastHit ignoreData, Quaternion.identity, 10f);

            Assert.IsFalse(result);
            Assert.IsNull(ignoreData.transform);
        }

        [Test]
        public void CustomBoxCastAll()
        {
            GameObject backSurface = GameObject.CreatePrimitive(PrimitiveType.Cube);
            backSurface.transform.position = Vector3.forward * 5f;

            validSurface.transform.position = Vector3.forward * 2f;
            Physics.Simulate(Time.fixedDeltaTime);

            IList<RaycastHit> result = subject.CustomBoxCastAll(Vector3.zero, Vector3.one * 0.1f, Vector3.forward, Quaternion.identity, 10f);

            Assert.AreEqual(result.Count, 2);
            Assert.AreEqual(validSurface, result[0].transform.gameObject);
            Assert.AreEqual(backSurface, result[1].transform.gameObject);

            validSurface.transform.position += Vector3.up * 2f;
            Physics.Simulate(Time.fixedDeltaTime);

            result = subject.CustomBoxCastAll(Vector3.zero, Vector3.one * 0.1f, Vector3.forward, Quaternion.identity, 10f);

            Assert.AreEqual(result.Count, 1);
            Assert.AreEqual(backSurface, result[0].transform.gameObject);

            Object.DestroyImmediate(backSurface);
        }

        [Test]
        public void ClearConvertTo()
        {
            ToBoxCastConverter converter = containingObject.AddComponent<ToBoxCastConverter>();
            subject.ConvertTo = converter;

            Assert.AreEqual(converter, subject.ConvertTo);
            subject.ClearConvertTo();
            Assert.IsNull(subject.ConvertTo);
        }

        [Test]
        public void ClearConvertToInactiveGameObject()
        {
            ToBoxCastConverter converter = containingObject.AddComponent<ToBoxCastConverter>();
            subject.ConvertTo = converter;

            Assert.AreEqual(converter, subject.ConvertTo);
            subject.gameObject.SetActive(false);
            subject.ClearConvertTo();
            Assert.AreEqual(converter, subject.ConvertTo);
        }

        [Test]
        public void ClearConvertToInactiveComponent()
        {
            ToBoxCastConverter converter = containingObject.AddComponent<ToBoxCastConverter>();
            subject.ConvertTo = converter;

            Assert.AreEqual(converter, subject.ConvertTo);
            subject.enabled = false;
            subject.ClearConvertTo();
            Assert.AreEqual(converter, subject.ConvertTo);
        }

        [Test]
        public void SetLayersToIgnoreByInt()
        {
            Assert.IsTrue(subject.LayersToIgnore == (subject.LayersToIgnore | (1 << 2)));
            Assert.IsFalse(subject.LayersToIgnore == (subject.LayersToIgnore | (1 << 4)));

            subject.SetLayersToIgnore(4);

            Assert.IsFalse(subject.LayersToIgnore == (subject.LayersToIgnore | (1 << 2)));
            Assert.IsTrue(subject.LayersToIgnore == (subject.LayersToIgnore | (1 << 4)));
        }

        [Test]
        public void SetLayersToIgnoreByString()
        {
            Assert.IsTrue(subject.LayersToIgnore == (subject.LayersToIgnore | (1 << 2)));
            Assert.IsFalse(subject.LayersToIgnore == (subject.LayersToIgnore | (1 << 4)));

            subject.SetLayersToIgnore("Water");

            Assert.IsFalse(subject.LayersToIgnore == (subject.LayersToIgnore | (1 << 2)));
            Assert.IsTrue(subject.LayersToIgnore == (subject.LayersToIgnore | (1 << 4)));
        }


        [Test]
        public void SetTriggerInteraction()
        {
            Assert.AreEqual(QueryTriggerInteraction.UseGlobal, subject.TriggerInteraction);

            subject.SetTriggerInteraction(1);

            Assert.AreEqual(QueryTriggerInteraction.Ignore, subject.TriggerInteraction);

            subject.SetTriggerInteraction(2);

            Assert.AreEqual(QueryTriggerInteraction.Collide, subject.TriggerInteraction);

            subject.SetTriggerInteraction(0);

            Assert.AreEqual(QueryTriggerInteraction.UseGlobal, subject.TriggerInteraction);
        }
    }
}