using VRTK.Core.Tracking.Follow.Modifier;

namespace Test.VRTK.Core.Tracking.Follow.Modifier
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.VRTK.Core.Utility;

    public class ModifyRigidbodyVelocityTest
    {
        private GameObject containingObject;
        private ModifyRigidbodyVelocity subject;
        private Rigidbody subjectRigidbody;

        private TimeSettingOverride timeOverride;

        [SetUp]
        public void SetUp()
        {
            timeOverride = new TimeSettingOverride(0.02f, 0.3333333f, 1f, 0.03f);

            containingObject = new GameObject();
            subject = containingObject.AddComponent<ModifyRigidbodyVelocity>();
            subjectRigidbody = containingObject.AddComponent<Rigidbody>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subjectRigidbody);
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);

            timeOverride.ResetTime();
        }

        [Test]
        public void UpdatePosition()
        {
            GameObject source = subject.gameObject;
            GameObject target = new GameObject();

            source.transform.position = Vector3.zero;
            target.transform.position = Vector3.one;

            Vector3 expectedVelocity = Vector3.one * 5.8f;
            Vector3 expectedAngularVelocity = Vector3.zero;

            Assert.AreEqual(Vector3.zero, subjectRigidbody.velocity);
            Assert.AreEqual(Vector3.zero, subjectRigidbody.angularVelocity);

            subject.UpdatePosition(source.transform, target.transform);

            Assert.AreEqual(expectedVelocity.ToString(), subjectRigidbody.velocity.ToString());
            Assert.AreEqual(expectedAngularVelocity, subjectRigidbody.angularVelocity);

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
        }

        [Test]
        public void UpdateRotation()
        {
            GameObject source = subject.gameObject;
            GameObject target = new GameObject();

            source.transform.rotation = Quaternion.identity;
            target.transform.rotation = Quaternion.Euler(90f, 0f, 0f);

            Vector3 expectedVelocity = Vector3.zero;
            Vector3 expectedAngularVelocity = Vector3.right * 10f;

            Assert.AreEqual(Vector3.zero, subjectRigidbody.velocity);
            Assert.AreEqual(Vector3.zero, subjectRigidbody.angularVelocity);

            subject.UpdateRotation(source.transform, target.transform);

            Assert.AreEqual(expectedVelocity.ToString(), subjectRigidbody.velocity.ToString());
            Assert.AreEqual(expectedAngularVelocity, subjectRigidbody.angularVelocity);

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
        }

        [Test]
        public void UpdateScale()
        {
            GameObject source = subject.gameObject;
            GameObject target = new GameObject();

            source.transform.localScale = Vector3.zero;
            target.transform.localScale = Vector3.one;

            Assert.AreEqual(Vector3.zero, subjectRigidbody.velocity);
            Assert.AreEqual(Vector3.zero, subjectRigidbody.angularVelocity);
            Assert.AreEqual(Vector3.zero, source.transform.localScale);

            subject.UpdateScale(source.transform, target.transform);

            Assert.AreEqual(Vector3.zero, subjectRigidbody.velocity);
            Assert.AreEqual(Vector3.zero, subjectRigidbody.angularVelocity);
            Assert.AreEqual(Vector3.zero, source.transform.localScale);

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
        }
    }
}