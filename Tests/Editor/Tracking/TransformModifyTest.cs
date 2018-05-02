namespace VRTK.Core.Tracking
{
    using UnityEngine;
    using NUnit.Framework;
    using VRTK.Core.Data.Type;

    public class TransformModifyTest
    {
        private GameObject containingObject;
        private TransformModify subject;

        private GameObject sourceObject;
        private GameObject offsetObject;
        private GameObject targetObject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<TransformModify>();

            sourceObject = new GameObject();
            offsetObject = new GameObject();
            targetObject = new GameObject();
        }

        [TearDown]
        public void TearDown()
        {
            subject = null;
            containingObject = null;

            sourceObject = null;
            offsetObject = null;
            targetObject = null;
        }

        [Test]
        public void ModifyPositionNoOffsetInstantTransition()
        {
            targetObject.transform.position = Vector3.one;

            subject.source = sourceObject.transform;
            subject.applyTransformations = Data.Enum.TransformProperties.Position;

            Assert.AreEqual(Vector3.zero, sourceObject.transform.position);
            subject.Modify(targetObject.transform);
            Assert.AreEqual(Vector3.one, sourceObject.transform.position);
        }

        [Test]
        public void ModifyRotationNoOffsetInstantTransition()
        {
            Quaternion targetRotation = new Quaternion(1f, 0f, 0f, 0f);
            targetObject.transform.position = Vector3.one;
            targetObject.transform.rotation = targetRotation;

            subject.source = sourceObject.transform;
            subject.applyTransformations = Data.Enum.TransformProperties.Rotation;

            Assert.AreEqual(Vector3.zero, sourceObject.transform.position);
            Assert.AreEqual(Quaternion.identity, sourceObject.transform.rotation);
            subject.Modify(targetObject.transform);
            Assert.AreEqual(Vector3.zero, sourceObject.transform.position);
            Assert.AreEqual(targetRotation, sourceObject.transform.rotation);
        }

        [Test]
        public void ModifyScaleNoOffsetInstantTransition()
        {
            targetObject.transform.position = Vector3.one;
            targetObject.transform.rotation = new Quaternion(1f, 0f, 0f, 0f);
            targetObject.transform.localScale = Vector3.one * 2f;

            subject.source = sourceObject.transform;
            subject.applyTransformations = Data.Enum.TransformProperties.Scale;

            Assert.AreEqual(Vector3.zero, sourceObject.transform.position);
            Assert.AreEqual(Quaternion.identity, sourceObject.transform.rotation);
            Assert.AreEqual(Vector3.one, sourceObject.transform.localScale);
            subject.Modify(targetObject.transform);
            Assert.AreEqual(Vector3.zero, sourceObject.transform.position);
            Assert.AreEqual(Quaternion.identity, sourceObject.transform.rotation);
            Assert.AreEqual(Vector3.one * 2f, sourceObject.transform.localScale);
        }

        [Test]
        public void ModifyTransformWithOffset()
        {
            offsetObject.transform.position = Vector3.one;
            offsetObject.transform.rotation = new Quaternion(0.5f, 0f, 0.5f, 0f);

            targetObject.transform.position = Vector3.one * 2f;
            targetObject.transform.rotation = new Quaternion(1f, 1f, 0f, 0f);
            targetObject.transform.localScale = Vector3.one * 3f;

            subject.source = sourceObject.transform;
            subject.offset = offsetObject.transform;
            subject.applyTransformations = Data.Enum.TransformProperties.Position | Data.Enum.TransformProperties.Rotation | Data.Enum.TransformProperties.Scale;

            subject.Modify(targetObject.transform);
            Assert.AreEqual(new Vector3(-1f, -1f, 5f).ToString(), sourceObject.transform.position.ToString());
            Assert.AreEqual(new Quaternion(0.7f, 0.7f, 0f, 0f).ToString(), sourceObject.transform.rotation.ToString());
            Assert.AreEqual(Vector3.one * 3f, sourceObject.transform.localScale);
        }

        [Test]
        public void ModifyTransformWithXOffsetOnly()
        {
            offsetObject.transform.position = Vector3.one;
            offsetObject.transform.rotation = new Quaternion(0.5f, 0f, 0.5f, 0f);

            targetObject.transform.position = Vector3.one * 2f;
            targetObject.transform.rotation = new Quaternion(1f, 1f, 0f, 0f);
            targetObject.transform.localScale = Vector3.one * 3f;

            subject.source = sourceObject.transform;
            subject.offset = offsetObject.transform;
            subject.applyOffsetOnAxis = new Vector3State(true, false, false);
            subject.applyTransformations = Data.Enum.TransformProperties.Position | Data.Enum.TransformProperties.Rotation | Data.Enum.TransformProperties.Scale;

            subject.Modify(targetObject.transform);
            Assert.AreEqual(new Vector3(2f, -1f, 2f).ToString(), sourceObject.transform.position.ToString());
            Assert.AreEqual(new Quaternion(0.7f, 0.7f, 0f, 0f).ToString(), sourceObject.transform.rotation.ToString());
            Assert.AreEqual(Vector3.one * 3f, sourceObject.transform.localScale);
        }

        [Test]
        public void ModifyTransformNoSource()
        {
            targetObject.transform.position = Vector3.one * 2f;
            targetObject.transform.rotation = new Quaternion(1f, 1f, 0f, 0f);
            targetObject.transform.localScale = Vector3.one * 3f;

            subject.Modify(targetObject.transform);
            Assert.AreEqual(Vector3.zero, sourceObject.transform.position);
            Assert.AreEqual(Quaternion.identity, sourceObject.transform.rotation);
            Assert.AreEqual(Vector3.one, sourceObject.transform.localScale);
        }
    }
}