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

        [Test]
        public void ClearAngularVelocitySource()
        {
            Assert.IsNull(subject.AngularVelocitySource);
            MockVelocityTracker source = subject.gameObject.AddComponent<MockVelocityTracker>();
            subject.AngularVelocitySource = source;
            Assert.AreEqual(source, subject.AngularVelocitySource);
            subject.ClearAngularVelocitySource();
            Assert.IsNull(subject.AngularVelocitySource);
        }

        [Test]
        public void ClearAngularVelocitySourceInactiveGameObject()
        {
            Assert.IsNull(subject.AngularVelocitySource);
            MockVelocityTracker source = subject.gameObject.AddComponent<MockVelocityTracker>();
            subject.AngularVelocitySource = source;
            Assert.AreEqual(source, subject.AngularVelocitySource);
            subject.gameObject.SetActive(false);
            subject.ClearAngularVelocitySource();
            Assert.AreEqual(source, subject.AngularVelocitySource);
        }

        [Test]
        public void ClearAngularVelocitySourceInactiveComponent()
        {
            Assert.IsNull(subject.AngularVelocitySource);
            MockVelocityTracker source = subject.gameObject.AddComponent<MockVelocityTracker>();
            subject.AngularVelocitySource = source;
            Assert.AreEqual(source, subject.AngularVelocitySource);
            subject.enabled = false;
            subject.ClearAngularVelocitySource();
            Assert.AreEqual(source, subject.AngularVelocitySource);
        }

        [Test]
        public void SetSourceMultiplierX()
        {
            subject.SourceMultiplier = Vector3.zero;
            Assert.AreEqual(Vector3.zero, subject.SourceMultiplier);
            subject.SetSourceMultiplierX(1f);
            Assert.AreEqual(Vector3.right, subject.SourceMultiplier);
        }

        [Test]
        public void SetSourceMultiplierY()
        {
            subject.SourceMultiplier = Vector3.zero;
            Assert.AreEqual(Vector3.zero, subject.SourceMultiplier);
            subject.SetSourceMultiplierY(1f);
            Assert.AreEqual(Vector3.up, subject.SourceMultiplier);
        }

        [Test]
        public void SetSourceMultiplierZ()
        {
            subject.SourceMultiplier = Vector3.zero;
            Assert.AreEqual(Vector3.zero, subject.SourceMultiplier);
            subject.SetSourceMultiplierZ(1f);
            Assert.AreEqual(Vector3.forward, subject.SourceMultiplier);
        }

        [Test]
        public void SetApplyToAxisX()
        {
            subject.ApplyToAxis = Vector3State.False;
            Assert.AreEqual(Vector3State.False, subject.ApplyToAxis);
            subject.SetApplyToAxisX(true);
            Assert.AreEqual(Vector3State.XOnly, subject.ApplyToAxis);
        }

        [Test]
        public void SetApplyToAxisY()
        {
            subject.ApplyToAxis = Vector3State.False;
            Assert.AreEqual(Vector3State.False, subject.ApplyToAxis);
            subject.SetApplyToAxisY(true);
            Assert.AreEqual(Vector3State.YOnly, subject.ApplyToAxis);
        }

        [Test]
        public void SetApplyToAxisZ()
        {
            subject.ApplyToAxis = Vector3State.False;
            Assert.AreEqual(Vector3State.False, subject.ApplyToAxis);
            subject.SetApplyToAxisZ(true);
            Assert.AreEqual(Vector3State.ZOnly, subject.ApplyToAxis);
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