using Zinnia.Data.Type;
using Zinnia.Tracking.Follow.Modifier.Property.Rotation;
using Zinnia.Tracking.Velocity;

namespace Test.Zinnia.Tracking.Follow.Modifier.Property.Rotation
{
    using NUnit.Framework;
    using UnityEngine;
    using Assert = UnityEngine.Assertions.Assert;

    public class RotateAroundAngularVelocityTest
    {
        private GameObject containingObject;
        private RotateAroundAngularVelocity subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<RotateAroundAngularVelocity>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void ModifyRotateAroundXAxis()
        {
            MockVelocityTracker source = subject.gameObject.AddComponent<MockVelocityTracker>();
            subject.AngularVelocitySource = source;
            subject.ApplyToAxis = Vector3State.XOnly;

            GameObject unusedSource = new GameObject();
            GameObject target = new GameObject();

            float rotationAngle = 90f;

            source.AngularVelocity = Vector3.one * rotationAngle;
            target.transform.rotation = Quaternion.identity;

            Assert.AreEqual(Vector3.zero, target.transform.position);
            Assert.AreEqual(Quaternion.identity.ToString(), target.transform.rotation.ToString());

            subject.Modify(unusedSource, target);

            Assert.AreEqual(Vector3.zero, target.transform.position);
            Assert.AreEqual(Quaternion.Euler(Vector3.right * rotationAngle).ToString(), target.transform.rotation.ToString());

            Object.DestroyImmediate(unusedSource);
            Object.DestroyImmediate(target);
        }

        [Test]
        public void ModifyRotateAroundYAxis()
        {
            MockVelocityTracker source = subject.gameObject.AddComponent<MockVelocityTracker>();
            subject.AngularVelocitySource = source;
            subject.ApplyToAxis = Vector3State.YOnly;

            GameObject unusedSource = new GameObject();
            GameObject target = new GameObject();

            float rotationAngle = 90f;

            source.AngularVelocity = Vector3.one * rotationAngle;
            target.transform.rotation = Quaternion.identity;

            Assert.AreEqual(Vector3.zero, target.transform.position);
            Assert.AreEqual(Quaternion.identity.ToString(), target.transform.rotation.ToString());

            subject.Modify(unusedSource, target);

            Assert.AreEqual(Vector3.zero, target.transform.position);
            Assert.AreEqual(Quaternion.Euler(Vector3.up * rotationAngle).ToString(), target.transform.rotation.ToString());

            Object.DestroyImmediate(unusedSource);
            Object.DestroyImmediate(target);
        }

        [Test]
        public void ModifyRotateAroundZAxis()
        {
            MockVelocityTracker source = subject.gameObject.AddComponent<MockVelocityTracker>();
            subject.AngularVelocitySource = source;
            subject.ApplyToAxis = Vector3State.ZOnly;

            GameObject unusedSource = new GameObject();
            GameObject target = new GameObject();

            float rotationAngle = 90f;

            source.AngularVelocity = Vector3.one * rotationAngle;
            target.transform.rotation = Quaternion.identity;

            Assert.AreEqual(Vector3.zero, target.transform.position);
            Assert.AreEqual(Quaternion.identity.ToString(), target.transform.rotation.ToString());

            subject.Modify(unusedSource, target);

            Assert.AreEqual(Vector3.zero, target.transform.position);
            Assert.AreEqual(Quaternion.Euler(Vector3.forward * rotationAngle).ToString(), target.transform.rotation.ToString());

            Object.DestroyImmediate(unusedSource);
            Object.DestroyImmediate(target);
        }

        [Test]
        public void ModifyRotateAroundXAxisWithOffset()
        {
            MockVelocityTracker source = subject.gameObject.AddComponent<MockVelocityTracker>();
            subject.AngularVelocitySource = source;
            subject.ApplyToAxis = Vector3State.XOnly;
            subject.ApplyOffset = true;

            GameObject unusedSource = new GameObject();
            GameObject target = new GameObject();
            GameObject offset = new GameObject();

            float rotationAngle = 90f;

            source.AngularVelocity = Vector3.one * rotationAngle;
            target.transform.rotation = Quaternion.identity;
            offset.transform.position = new Vector3(0.5f, 0.5f, 0f);

            Assert.AreEqual(Vector3.zero, target.transform.position);
            Assert.AreEqual(Quaternion.identity.ToString(), target.transform.rotation.ToString());

            subject.Modify(unusedSource, target, offset);

            Assert.AreEqual(new Vector3(0f, 0.5f, -0.5f).ToString(), target.transform.position.ToString());
            Assert.AreEqual(Quaternion.Euler(Vector3.right * rotationAngle).ToString(), target.transform.rotation.ToString());

            Object.DestroyImmediate(unusedSource);
            Object.DestroyImmediate(target);
        }

        [Test]
        public void ModifyRotateAroundXAxisWithOffsetIgnored()
        {
            MockVelocityTracker source = subject.gameObject.AddComponent<MockVelocityTracker>();
            subject.AngularVelocitySource = source;
            subject.ApplyToAxis = Vector3State.XOnly;
            subject.ApplyOffset = false;

            GameObject unusedSource = new GameObject();
            GameObject target = new GameObject();
            GameObject offset = new GameObject();

            float rotationAngle = 90f;

            source.AngularVelocity = Vector3.one * rotationAngle;
            target.transform.rotation = Quaternion.identity;
            offset.transform.position = new Vector3(0.5f, 0.5f, 0f);

            Assert.AreEqual(Vector3.zero, target.transform.position);
            Assert.AreEqual(Quaternion.identity.ToString(), target.transform.rotation.ToString());

            subject.Modify(unusedSource, target, offset);

            Assert.AreEqual(Vector3.zero, target.transform.position);
            Assert.AreEqual(Quaternion.Euler(Vector3.right * rotationAngle).ToString(), target.transform.rotation.ToString());

            Object.DestroyImmediate(unusedSource);
            Object.DestroyImmediate(target);
        }

        [Test]
        public void ModifyInactiveGameObject()
        {
            MockVelocityTracker source = subject.gameObject.AddComponent<MockVelocityTracker>();
            subject.AngularVelocitySource = source;
            subject.ApplyToAxis = Vector3State.XOnly;

            GameObject unusedSource = new GameObject();
            GameObject target = new GameObject();

            float rotationAngle = 90f;

            source.AngularVelocity = Vector3.one * rotationAngle;
            target.transform.rotation = Quaternion.identity;

            subject.gameObject.SetActive(false);

            Assert.AreEqual(Vector3.zero, target.transform.position);
            Assert.AreEqual(Quaternion.identity.ToString(), target.transform.rotation.ToString());

            subject.Modify(unusedSource, target);

            Assert.AreEqual(Vector3.zero, target.transform.position);
            Assert.AreEqual(Quaternion.identity.ToString(), target.transform.rotation.ToString());

            Object.DestroyImmediate(unusedSource);
            Object.DestroyImmediate(target);
        }

        [Test]
        public void ModifyInactiveComponent()
        {
            MockVelocityTracker source = subject.gameObject.AddComponent<MockVelocityTracker>();
            subject.AngularVelocitySource = source;
            subject.ApplyToAxis = Vector3State.XOnly;

            GameObject unusedSource = new GameObject();
            GameObject target = new GameObject();

            float rotationAngle = 90f;

            source.AngularVelocity = Vector3.one * rotationAngle;
            target.transform.rotation = Quaternion.identity;

            subject.enabled = false;

            Assert.AreEqual(Vector3.zero, target.transform.position);
            Assert.AreEqual(Quaternion.identity.ToString(), target.transform.rotation.ToString());

            subject.Modify(unusedSource, target);

            Assert.AreEqual(Vector3.zero, target.transform.position);
            Assert.AreEqual(Quaternion.identity.ToString(), target.transform.rotation.ToString());

            Object.DestroyImmediate(unusedSource);
            Object.DestroyImmediate(target);
        }

        protected class MockVelocityTracker : VelocityTracker
        {
            public Vector3 AngularVelocity;
            protected override Vector3 DoGetAngularVelocity()
            {
                return AngularVelocity;
            }

            protected override Vector3 DoGetVelocity()
            {
                return default;
            }
        }
    }
}