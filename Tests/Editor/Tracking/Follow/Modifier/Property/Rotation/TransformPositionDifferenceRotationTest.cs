using Zinnia.Data.Type;
using Zinnia.Tracking.Follow.Modifier.Property.Rotation;

namespace Test.Zinnia.Tracking.Follow.Modifier.Property.Rotation
{
    using NUnit.Framework;
    using UnityEngine;
    using UnityEngine.TestTools.Utils;

    public class TransformPositionDifferenceRotationTest
    {
        private GameObject containingObject;
        private TransformPositionDifferenceRotation subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject("TransformPositionDifferenceRotationTest");
            subject = containingObject.AddComponent<TransformPositionDifferenceRotation>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void Modify()
        {
            QuaternionEqualityComparer comparer = new QuaternionEqualityComparer(0.1f);
            GameObject source = new GameObject("TransformPositionDifferenceRotationTest");
            GameObject target = new GameObject("TransformPositionDifferenceRotationTest");

            target.transform.position = new Vector3(0f, 0f, 0f);
            target.transform.localRotation = Quaternion.identity;

            source.transform.position = new Vector3(0.5f, 0f, -0.5f);
            subject.Modify(source, target);
            source.transform.position = new Vector3(0.5f, 0.5f, -0.5f);
            subject.Modify(source, target);
            source.transform.position = new Vector3(0.5f, 1f, -0.5f);
            subject.Modify(source, target);

            Assert.That(target.transform.localRotation, Is.EqualTo(new Quaternion(0.3f, -0.1f, 0.3f, 0.9f)).Using(comparer));

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
        }

        [Test]
        public void ModifyWithAncestor()
        {
            QuaternionEqualityComparer comparer = new QuaternionEqualityComparer(0.1f);
            GameObject ancestor = new GameObject("TransformPositionDifferenceRotationTest");
            GameObject source = new GameObject("TransformPositionDifferenceRotationTest");
            GameObject target = new GameObject("TransformPositionDifferenceRotationTest");

            ancestor.transform.position = new Vector3(0f, 0f, 0f);
            source.transform.SetParent(ancestor.transform);
            target.transform.SetParent(ancestor.transform);
            subject.Ancestor = ancestor;

            target.transform.position = new Vector3(0f, 0f, 0f);
            target.transform.localRotation = Quaternion.identity;

            source.transform.position = new Vector3(0.5f, 0f, -0.5f);
            subject.Modify(source, target);
            ancestor.transform.position += Vector3.left;
            source.transform.position = new Vector3(0.5f, 0.5f, -0.5f);
            subject.Modify(source, target);
            ancestor.transform.position += Vector3.left;
            source.transform.position = new Vector3(0.5f, 1f, -0.5f);
            subject.Modify(source, target);

            Assert.That(target.transform.localRotation, Is.EqualTo(new Quaternion(0.1f, -0.3f, 0.2f, 0.9f)).Using(comparer));

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
            Object.DestroyImmediate(ancestor);
        }

        [Test]
        public void ModifyInactiveGameObject()
        {
            QuaternionEqualityComparer comparer = new QuaternionEqualityComparer(0.1f);
            GameObject source = new GameObject("TransformPositionDifferenceRotationTest");
            GameObject target = new GameObject("TransformPositionDifferenceRotationTest");
            subject.gameObject.SetActive(false);

            target.transform.position = new Vector3(0f, 0f, 0f);
            target.transform.localRotation = Quaternion.identity;

            source.transform.position = new Vector3(0.5f, 0f, -0.5f);
            subject.Modify(source, target);
            source.transform.position = new Vector3(0.5f, 0.5f, -0.5f);
            subject.Modify(source, target);
            source.transform.position = new Vector3(0.5f, 1f, -0.5f);
            subject.Modify(source, target);

            Assert.That(target.transform.localRotation, Is.EqualTo(Quaternion.identity).Using(comparer));

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
        }


        [Test]
        public void ModifyInactiveComponent()
        {
            QuaternionEqualityComparer comparer = new QuaternionEqualityComparer(0.1f);
            GameObject source = new GameObject("TransformPositionDifferenceRotationTest");
            GameObject target = new GameObject("TransformPositionDifferenceRotationTest");
            subject.enabled = false;

            target.transform.position = new Vector3(0f, 0f, 0f);
            target.transform.localRotation = Quaternion.identity;

            source.transform.position = new Vector3(0.5f, 0f, -0.5f);
            subject.Modify(source, target);
            source.transform.position = new Vector3(0.5f, 0.5f, -0.5f);
            subject.Modify(source, target);
            source.transform.position = new Vector3(0.5f, 1f, -0.5f);
            subject.Modify(source, target);

            Assert.That(target.transform.localRotation, Is.EqualTo(Quaternion.identity).Using(comparer));

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
        }

        [Test]
        public void ClearAncestor()
        {
            Assert.IsNull(subject.Ancestor);
            subject.Ancestor = containingObject;
            Assert.AreEqual(containingObject, subject.Ancestor);
            subject.ClearAncestor();
            Assert.IsNull(subject.Ancestor);
        }

        [Test]
        public void ClearAncestorInactiveGameObject()
        {
            Assert.IsNull(subject.Ancestor);
            subject.Ancestor = containingObject;
            Assert.AreEqual(containingObject, subject.Ancestor);
            subject.gameObject.SetActive(false);
            subject.ClearAncestor();
            Assert.AreEqual(containingObject, subject.Ancestor);
        }

        [Test]
        public void ClearAncestorInactiveComponent()
        {
            Assert.IsNull(subject.Ancestor);
            subject.Ancestor = containingObject;
            Assert.AreEqual(containingObject, subject.Ancestor);
            subject.enabled = false;
            subject.ClearAncestor();
            Assert.AreEqual(containingObject, subject.Ancestor);
        }

        [Test]
        public void SetFollowOnAxisX()
        {
            subject.FollowOnAxis = Vector3State.False;
            Assert.AreEqual(Vector3State.False, subject.FollowOnAxis);
            subject.SetFollowOnAxisX(true);
            Assert.AreEqual(Vector3State.XOnly, subject.FollowOnAxis);
        }

        [Test]
        public void SetFollowOnAxisY()
        {
            subject.FollowOnAxis = Vector3State.False;
            Assert.AreEqual(Vector3State.False, subject.FollowOnAxis);
            subject.SetFollowOnAxisY(true);
            Assert.AreEqual(Vector3State.YOnly, subject.FollowOnAxis);
        }

        [Test]
        public void SetFollowOnAxisZ()
        {
            subject.FollowOnAxis = Vector3State.False;
            Assert.AreEqual(Vector3State.False, subject.FollowOnAxis);
            subject.SetFollowOnAxisZ(true);
            Assert.AreEqual(Vector3State.ZOnly, subject.FollowOnAxis);
        }
    }
}