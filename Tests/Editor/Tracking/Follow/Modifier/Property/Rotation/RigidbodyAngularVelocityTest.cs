using Zinnia.Tracking.Follow.Modifier.Property.Rotation;

namespace Test.Zinnia.Tracking.Follow.Modifier.Property.Rotation
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.Zinnia.Utility;
    using Assert = UnityEngine.Assertions.Assert;

    public class RigidbodyAngularVelocityTest
    {
        private GameObject containingObject;
        private RigidbodyAngularVelocity subject;
        private Rigidbody subjectRigidbody;
        private TimeSettingOverride timeOverride;

        [SetUp]
        public void SetUp()
        {
            timeOverride = new TimeSettingOverride(0.02f, 0.3333333f, 1f, 0.03f);

            containingObject = new GameObject();
            subject = containingObject.AddComponent<RigidbodyAngularVelocity>();
            subjectRigidbody = containingObject.AddComponent<Rigidbody>();
            subjectRigidbody.useGravity = false;
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
            timeOverride.ResetTime();
        }

        [Test]
        public void Modify()
        {
            GameObject source = new GameObject();
            GameObject target = subject.gameObject;

            source.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
            target.transform.rotation = Quaternion.identity;

            Vector3 expectedVelocity = Vector3.zero;
            Vector3 expectedAngularVelocity = Vector3.right * 10f;

            Assert.AreEqual(Vector3.zero, subjectRigidbody.velocity);
            Assert.AreEqual(Vector3.zero, subjectRigidbody.angularVelocity);

            subject.Modify(source, target);

            Assert.AreEqual(expectedVelocity.ToString(), subjectRigidbody.velocity.ToString());
            Assert.AreEqual(expectedAngularVelocity, subjectRigidbody.angularVelocity);

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
        }

        [Test]
        public void ModifyWithOffset()
        {
            GameObject source = new GameObject();
            GameObject target = subject.gameObject;
            GameObject offset = new GameObject();

            source.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
            target.transform.rotation = Quaternion.identity;
            offset.transform.position = Vector3.one * 2f;

            Vector3 expectedVelocity = Vector3.zero;
            Vector3 expectedAngularVelocity = Vector3.right * 10f;

            Assert.AreEqual(Vector3.zero, subjectRigidbody.velocity);
            Assert.AreEqual(Vector3.zero, subjectRigidbody.angularVelocity);

            subject.Modify(source, target, offset);

            Assert.AreEqual(expectedVelocity.ToString(), subjectRigidbody.velocity.ToString());
            Assert.AreEqual(expectedAngularVelocity, subjectRigidbody.angularVelocity);

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
            Object.DestroyImmediate(offset);
        }

        [Test]
        public void ModifyWithOffsetIgnored()
        {
            GameObject source = new GameObject();
            GameObject target = subject.gameObject;
            GameObject offset = new GameObject();

            source.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
            target.transform.rotation = Quaternion.identity;
            offset.transform.position = Vector3.one * 2f;

            Vector3 expectedVelocity = Vector3.zero;
            Vector3 expectedAngularVelocity = Vector3.right * 10f;

            Assert.AreEqual(Vector3.zero, subjectRigidbody.velocity);
            Assert.AreEqual(Vector3.zero, subjectRigidbody.angularVelocity);

            subject.ApplyOffset = false;
            subject.Modify(source, target, offset);

            Assert.AreEqual(expectedVelocity.ToString(), subjectRigidbody.velocity.ToString());
            Assert.AreEqual(expectedAngularVelocity, subjectRigidbody.angularVelocity);

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
            Object.DestroyImmediate(offset);
        }

        [Test]
        public void ModifyInactiveGameObject()
        {
            GameObject source = new GameObject();
            GameObject target = subject.gameObject;

            source.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
            target.transform.rotation = Quaternion.identity;

            Assert.AreEqual(Vector3.zero, subjectRigidbody.velocity);
            Assert.AreEqual(Vector3.zero, subjectRigidbody.angularVelocity);

            subject.gameObject.SetActive(false);
            subject.Modify(source, target);

            Assert.AreEqual(Vector3.zero, subjectRigidbody.velocity);
            Assert.AreEqual(Vector3.zero, subjectRigidbody.angularVelocity);

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
        }

        [Test]
        public void ModifyInactiveComponent()
        {
            GameObject source = new GameObject();
            GameObject target = subject.gameObject;

            source.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
            target.transform.rotation = Quaternion.identity;

            Assert.AreEqual(Vector3.zero, subjectRigidbody.velocity);
            Assert.AreEqual(Vector3.zero, subjectRigidbody.angularVelocity);

            subject.enabled = false;
            subject.Modify(source, target);

            Assert.AreEqual(Vector3.zero, subjectRigidbody.velocity);
            Assert.AreEqual(Vector3.zero, subjectRigidbody.angularVelocity);

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
        }
    }
}