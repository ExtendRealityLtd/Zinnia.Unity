using Zinnia.Data.Type;
using Zinnia.Tracking.Follow.Modifier.Property.Rotation;
using Zinnia.Tracking.Velocity;

namespace Test.Zinnia.Tracking.Follow.Modifier.Property.Rotation
{
    using NUnit.Framework;
    using UnityEngine;
    using UnityEngine.TestTools.Utils;

    public class RotateAroundAngularVelocityTest
    {
        private GameObject containingObject;
        private RotateAroundAngularVelocity subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject("RotateAroundAngularVelocityTest");
            subject = containingObject.AddComponent<RotateAroundAngularVelocity>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void ModifyRotateAroundXAxis()
        {
            Vector3EqualityComparer vectorComparer = new Vector3EqualityComparer(0.1f);
            QuaternionEqualityComparer quaternionComparer = new QuaternionEqualityComparer(0.1f);
            MockVelocityTracker source = subject.gameObject.AddComponent<MockVelocityTracker>();
            subject.AngularVelocitySource = source;
            subject.ApplyToAxis = Vector3State.XOnly;

            GameObject unusedSource = new GameObject("RotateAroundAngularVelocityTest");
            GameObject target = new GameObject("RotateAroundAngularVelocityTest");

            float rotationAngle = 90f;

            source.AngularVelocity = Vector3.one * rotationAngle;
            target.transform.rotation = Quaternion.identity;

            Assert.That(target.transform.position, Is.EqualTo(Vector3.zero).Using(vectorComparer));
            Assert.That(target.transform.rotation, Is.EqualTo(Quaternion.identity).Using(quaternionComparer));

            subject.Modify(unusedSource, target);

            Assert.That(target.transform.position, Is.EqualTo(Vector3.zero).Using(vectorComparer));
            Assert.That(target.transform.rotation, Is.EqualTo(Quaternion.Euler(Vector3.right * rotationAngle)).Using(quaternionComparer));

            Object.DestroyImmediate(unusedSource);
            Object.DestroyImmediate(target);
        }

        [Test]
        public void ModifyRotateAroundYAxis()
        {
            Vector3EqualityComparer vectorComparer = new Vector3EqualityComparer(0.1f);
            QuaternionEqualityComparer quaternionComparer = new QuaternionEqualityComparer(0.1f);
            MockVelocityTracker source = subject.gameObject.AddComponent<MockVelocityTracker>();
            subject.AngularVelocitySource = source;
            subject.ApplyToAxis = Vector3State.YOnly;

            GameObject unusedSource = new GameObject("RotateAroundAngularVelocityTest");
            GameObject target = new GameObject("RotateAroundAngularVelocityTest");

            float rotationAngle = 90f;

            source.AngularVelocity = Vector3.one * rotationAngle;
            target.transform.rotation = Quaternion.identity;

            Assert.That(target.transform.position, Is.EqualTo(Vector3.zero).Using(vectorComparer));
            Assert.That(target.transform.rotation, Is.EqualTo(Quaternion.identity).Using(quaternionComparer));

            subject.Modify(unusedSource, target);

            Assert.That(target.transform.position, Is.EqualTo(Vector3.zero).Using(vectorComparer));
            Assert.That(target.transform.rotation, Is.EqualTo(Quaternion.Euler(Vector3.up * rotationAngle)).Using(quaternionComparer));

            Object.DestroyImmediate(unusedSource);
            Object.DestroyImmediate(target);
        }

        [Test]
        public void ModifyRotateAroundZAxis()
        {
            Vector3EqualityComparer vectorComparer = new Vector3EqualityComparer(0.1f);
            QuaternionEqualityComparer quaternionComparer = new QuaternionEqualityComparer(0.1f);
            MockVelocityTracker source = subject.gameObject.AddComponent<MockVelocityTracker>();
            subject.AngularVelocitySource = source;
            subject.ApplyToAxis = Vector3State.ZOnly;

            GameObject unusedSource = new GameObject("RotateAroundAngularVelocityTest");
            GameObject target = new GameObject("RotateAroundAngularVelocityTest");

            float rotationAngle = 90f;

            source.AngularVelocity = Vector3.one * rotationAngle;
            target.transform.rotation = Quaternion.identity;

            Assert.That(target.transform.position, Is.EqualTo(Vector3.zero).Using(vectorComparer));
            Assert.That(target.transform.rotation, Is.EqualTo(Quaternion.identity).Using(quaternionComparer));

            subject.Modify(unusedSource, target);

            Assert.That(target.transform.position, Is.EqualTo(Vector3.zero).Using(vectorComparer));
            Assert.That(target.transform.rotation, Is.EqualTo(Quaternion.Euler(Vector3.forward * rotationAngle)).Using(quaternionComparer));

            Object.DestroyImmediate(unusedSource);
            Object.DestroyImmediate(target);
        }

        [Test]
        public void ModifyRotateAroundXAxisWithOffset()
        {
            Vector3EqualityComparer vectorComparer = new Vector3EqualityComparer(0.1f);
            QuaternionEqualityComparer quaternionComparer = new QuaternionEqualityComparer(0.1f);
            MockVelocityTracker source = subject.gameObject.AddComponent<MockVelocityTracker>();
            subject.AngularVelocitySource = source;
            subject.ApplyToAxis = Vector3State.XOnly;
            subject.ApplyOffset = true;

            GameObject unusedSource = new GameObject("RotateAroundAngularVelocityTest");
            GameObject target = new GameObject("RotateAroundAngularVelocityTest");
            GameObject offset = new GameObject("RotateAroundAngularVelocityTest");

            float rotationAngle = 90f;

            source.AngularVelocity = Vector3.one * rotationAngle;
            target.transform.rotation = Quaternion.identity;
            offset.transform.position = new Vector3(0.5f, 0.5f, 0f);

            Assert.That(target.transform.position, Is.EqualTo(Vector3.zero).Using(vectorComparer));
            Assert.That(target.transform.rotation, Is.EqualTo(Quaternion.identity).Using(quaternionComparer));

            subject.Modify(unusedSource, target, offset);

            Assert.That(target.transform.position, Is.EqualTo(new Vector3(0f, 0.5f, -0.5f)).Using(vectorComparer));
            Assert.That(target.transform.rotation, Is.EqualTo(Quaternion.Euler(Vector3.right * rotationAngle)).Using(quaternionComparer));

            Object.DestroyImmediate(unusedSource);
            Object.DestroyImmediate(target);
            Object.DestroyImmediate(offset);
        }

        [Test]
        public void ModifyRotateAroundXAxisWithOffsetIgnored()
        {
            Vector3EqualityComparer vectorComparer = new Vector3EqualityComparer(0.1f);
            QuaternionEqualityComparer quaternionComparer = new QuaternionEqualityComparer(0.1f);
            MockVelocityTracker source = subject.gameObject.AddComponent<MockVelocityTracker>();
            subject.AngularVelocitySource = source;
            subject.ApplyToAxis = Vector3State.XOnly;
            subject.ApplyOffset = false;

            GameObject unusedSource = new GameObject("RotateAroundAngularVelocityTest");
            GameObject target = new GameObject("RotateAroundAngularVelocityTest");
            GameObject offset = new GameObject("RotateAroundAngularVelocityTest");

            float rotationAngle = 90f;

            source.AngularVelocity = Vector3.one * rotationAngle;
            target.transform.rotation = Quaternion.identity;
            offset.transform.position = new Vector3(0.5f, 0.5f, 0f);

            Assert.That(target.transform.position, Is.EqualTo(Vector3.zero).Using(vectorComparer));
            Assert.That(target.transform.rotation, Is.EqualTo(Quaternion.identity).Using(quaternionComparer));

            subject.Modify(unusedSource, target, offset);

            Assert.That(target.transform.position, Is.EqualTo(Vector3.zero).Using(vectorComparer));
            Assert.That(target.transform.rotation, Is.EqualTo(Quaternion.Euler(Vector3.right * rotationAngle)).Using(quaternionComparer));

            Object.DestroyImmediate(unusedSource);
            Object.DestroyImmediate(offset);
            Object.DestroyImmediate(target);
        }

        [Test]
        public void ModifyInactiveGameObject()
        {
            Vector3EqualityComparer vectorComparer = new Vector3EqualityComparer(0.1f);
            QuaternionEqualityComparer quaternionComparer = new QuaternionEqualityComparer(0.1f);
            MockVelocityTracker source = subject.gameObject.AddComponent<MockVelocityTracker>();
            subject.AngularVelocitySource = source;
            subject.ApplyToAxis = Vector3State.XOnly;

            GameObject unusedSource = new GameObject("RotateAroundAngularVelocityTest");
            GameObject target = new GameObject("RotateAroundAngularVelocityTest");

            float rotationAngle = 90f;

            source.AngularVelocity = Vector3.one * rotationAngle;
            target.transform.rotation = Quaternion.identity;

            subject.gameObject.SetActive(false);

            Assert.That(target.transform.position, Is.EqualTo(Vector3.zero).Using(vectorComparer));
            Assert.That(target.transform.rotation, Is.EqualTo(Quaternion.identity).Using(quaternionComparer));

            subject.Modify(unusedSource, target);

            Assert.That(target.transform.position, Is.EqualTo(Vector3.zero).Using(vectorComparer));
            Assert.That(target.transform.rotation, Is.EqualTo(Quaternion.identity).Using(quaternionComparer));

            Object.DestroyImmediate(unusedSource);
            Object.DestroyImmediate(target);
        }

        [Test]
        public void ModifyInactiveComponent()
        {
            Vector3EqualityComparer vectorComparer = new Vector3EqualityComparer(0.1f);
            QuaternionEqualityComparer quaternionComparer = new QuaternionEqualityComparer(0.1f);
            MockVelocityTracker source = subject.gameObject.AddComponent<MockVelocityTracker>();
            subject.AngularVelocitySource = source;
            subject.ApplyToAxis = Vector3State.XOnly;

            GameObject unusedSource = new GameObject("RotateAroundAngularVelocityTest");
            GameObject target = new GameObject("RotateAroundAngularVelocityTest");

            float rotationAngle = 90f;

            source.AngularVelocity = Vector3.one * rotationAngle;
            target.transform.rotation = Quaternion.identity;

            subject.enabled = false;

            Assert.That(target.transform.position, Is.EqualTo(Vector3.zero).Using(vectorComparer));
            Assert.That(target.transform.rotation, Is.EqualTo(Quaternion.identity).Using(quaternionComparer));

            subject.Modify(unusedSource, target);

            Assert.That(target.transform.position, Is.EqualTo(Vector3.zero).Using(vectorComparer));
            Assert.That(target.transform.rotation, Is.EqualTo(Quaternion.identity).Using(quaternionComparer));

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
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            subject.SourceMultiplier = Vector3.zero;
            Assert.That(subject.SourceMultiplier, Is.EqualTo(Vector3.zero).Using(comparer));
            subject.SetSourceMultiplierX(1f);
            Assert.That(subject.SourceMultiplier, Is.EqualTo(Vector3.right).Using(comparer));
        }

        [Test]
        public void SetSourceMultiplierY()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            subject.SourceMultiplier = Vector3.zero;
            Assert.That(subject.SourceMultiplier, Is.EqualTo(Vector3.zero).Using(comparer));
            subject.SetSourceMultiplierY(1f);
            Assert.That(subject.SourceMultiplier, Is.EqualTo(Vector3.up).Using(comparer));
        }

        [Test]
        public void SetSourceMultiplierZ()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            subject.SourceMultiplier = Vector3.zero;
            Assert.That(subject.SourceMultiplier, Is.EqualTo(Vector3.zero).Using(comparer));
            subject.SetSourceMultiplierZ(1f);
            Assert.That(subject.SourceMultiplier, Is.EqualTo(Vector3.forward).Using(comparer));
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