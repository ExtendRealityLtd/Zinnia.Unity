using Zinnia.Tracking.Follow.Modifier.Property.Position;

namespace Test.Zinnia.Tracking.Follow.Modifier.Property.Position
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.Zinnia.Utility;
    using Assert = UnityEngine.Assertions.Assert;

    public class RigidbodyVelocityTest
    {
        private GameObject containingObject;
        private RigidbodyVelocity subject;
        private Rigidbody subjectRigidbody;
        private TimeSettingOverride timeOverride;

        [SetUp]
        public void SetUp()
        {
            timeOverride = new TimeSettingOverride(0.02f, 0.3333333f, 1f, 0.03f);

            containingObject = new GameObject();
            subject = containingObject.AddComponent<RigidbodyVelocity>();
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

            source.transform.position = Vector3.one;
            target.transform.position = Vector3.zero;

            Vector3 expectedVelocity = Vector3.one * 5.8f;
            Vector3 expectedAngularVelocity = Vector3.zero;

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

            source.transform.position = Vector3.one;
            target.transform.position = Vector3.zero;
            offset.transform.position = Vector3.one * 2f;

            Vector3 expectedVelocity = Vector3.one * -5.8f;
            Vector3 expectedAngularVelocity = Vector3.zero;

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

            source.transform.position = Vector3.one;
            target.transform.position = Vector3.zero;
            offset.transform.position = Vector3.one * 2f;

            Vector3 expectedVelocity = Vector3.one * 5.8f;
            Vector3 expectedAngularVelocity = Vector3.zero;

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

            source.transform.position = Vector3.one;
            target.transform.position = Vector3.zero;

            Vector3 expectedAngularVelocity = Vector3.zero;

            Assert.AreEqual(Vector3.zero, subjectRigidbody.velocity);
            Assert.AreEqual(Vector3.zero, subjectRigidbody.angularVelocity);

            subject.gameObject.SetActive(false);
            subject.Modify(source, target);

            Assert.AreEqual(Vector3.zero, subjectRigidbody.velocity);
            Assert.AreEqual(expectedAngularVelocity, subjectRigidbody.angularVelocity);

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
        }

        [Test]
        public void ModifyInactiveComponent()
        {
            GameObject source = new GameObject();
            GameObject target = subject.gameObject;

            source.transform.position = Vector3.one;
            target.transform.position = Vector3.zero;

            Vector3 expectedAngularVelocity = Vector3.zero;

            Assert.AreEqual(Vector3.zero, subjectRigidbody.velocity);
            Assert.AreEqual(Vector3.zero, subjectRigidbody.angularVelocity);

            subject.enabled = false;
            subject.Modify(source, target);

            Assert.AreEqual(Vector3.zero, subjectRigidbody.velocity);
            Assert.AreEqual(expectedAngularVelocity, subjectRigidbody.angularVelocity);

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
        }
    }
}