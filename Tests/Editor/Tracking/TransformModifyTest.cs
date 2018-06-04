namespace VRTK.Core.Tracking
{
    using UnityEngine;
    using NUnit.Framework;
    using VRTK.Core.Data.Type;
    using VRTK.Core.Utility.Mock;

    public class TransformModifyTest
    {
        private GameObject containingObject;
        private TransformModify subject;

        private GameObject sourceObject;
        private GameObject offsetObject;
        private GameObject targetObject;
        private TransformData targetTransformData;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<TransformModify>();

            sourceObject = new GameObject();
            offsetObject = new GameObject();
            targetObject = new GameObject();
            targetTransformData = new TransformData(targetObject.transform);
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);

            Object.DestroyImmediate(sourceObject);
            Object.DestroyImmediate(offsetObject);
            Object.DestroyImmediate(targetObject);
        }

        [Test]
        public void ModifyPositionNoOffsetInstantTransition()
        {
            targetTransformData.transform.position = Vector3.one;

            subject.source = sourceObject.transform;
            subject.applyTransformations = Data.Enum.TransformProperties.Position;

            Assert.AreEqual(Vector3.zero, sourceObject.transform.position);
            subject.Modify(targetTransformData);
            Assert.AreEqual(Vector3.one, sourceObject.transform.position);
        }

        [Test]
        public void ModifyRotationNoOffsetInstantTransition()
        {
            Quaternion targetRotation = new Quaternion(1f, 0f, 0f, 0f);
            targetTransformData.transform.position = Vector3.one;
            targetTransformData.transform.rotation = targetRotation;

            subject.source = sourceObject.transform;
            subject.applyTransformations = Data.Enum.TransformProperties.Rotation;

            Assert.AreEqual(Vector3.zero, sourceObject.transform.position);
            Assert.AreEqual(Quaternion.identity, sourceObject.transform.rotation);
            subject.Modify(targetTransformData);
            Assert.AreEqual(Vector3.zero, sourceObject.transform.position);
            Assert.AreEqual(targetRotation, sourceObject.transform.rotation);
        }

        [Test]
        public void ModifyScaleNoOffsetInstantTransition()
        {
            targetTransformData.transform.position = Vector3.one;
            targetTransformData.transform.rotation = new Quaternion(1f, 0f, 0f, 0f);
            targetTransformData.transform.localScale = Vector3.one * 2f;

            subject.source = sourceObject.transform;
            subject.applyTransformations = Data.Enum.TransformProperties.Scale;

            Assert.AreEqual(Vector3.zero, sourceObject.transform.position);
            Assert.AreEqual(Quaternion.identity, sourceObject.transform.rotation);
            Assert.AreEqual(Vector3.one, sourceObject.transform.localScale);
            subject.Modify(targetTransformData);
            Assert.AreEqual(Vector3.zero, sourceObject.transform.position);
            Assert.AreEqual(Quaternion.identity, sourceObject.transform.rotation);
            Assert.AreEqual(Vector3.one * 2f, sourceObject.transform.localScale);
        }

        [Test]
        public void ModifyTransformWithOffset()
        {
            offsetObject.transform.position = Vector3.one;
            offsetObject.transform.rotation = new Quaternion(0.5f, 0f, 0.5f, 0f);

            targetTransformData.transform.position = Vector3.one * 2f;
            targetTransformData.transform.rotation = new Quaternion(1f, 1f, 0f, 0f);
            targetTransformData.transform.localScale = Vector3.one * 3f;

            subject.source = sourceObject.transform;
            subject.offset = offsetObject.transform;
            subject.applyTransformations = Data.Enum.TransformProperties.Position | Data.Enum.TransformProperties.Rotation | Data.Enum.TransformProperties.Scale;

            subject.Modify(targetTransformData);
            Assert.AreEqual(new Vector3(-1f, -1f, 5f).ToString(), sourceObject.transform.position.ToString());
            Assert.AreEqual(new Quaternion(0.7f, 0.7f, 0f, 0f).ToString(), sourceObject.transform.rotation.ToString());
            Assert.AreEqual(Vector3.one * 3f, sourceObject.transform.localScale);
        }

        [Test]
        public void ModifyTransformWithXOffsetOnly()
        {
            offsetObject.transform.position = Vector3.one;
            offsetObject.transform.rotation = new Quaternion(0.5f, 0f, 0.5f, 0f);

            targetTransformData.transform.position = Vector3.one * 2f;
            targetTransformData.transform.rotation = new Quaternion(1f, 1f, 0f, 0f);
            targetTransformData.transform.localScale = Vector3.one * 3f;

            subject.source = sourceObject.transform;
            subject.offset = offsetObject.transform;
            subject.applyOffsetOnAxis = new Vector3State(true, false, false);
            subject.applyTransformations = Data.Enum.TransformProperties.Position | Data.Enum.TransformProperties.Rotation | Data.Enum.TransformProperties.Scale;

            subject.Modify(targetTransformData);
            Assert.AreEqual(new Vector3(2f, -1f, 2f).ToString(), sourceObject.transform.position.ToString());
            Assert.AreEqual(new Quaternion(0.7f, 0.7f, 0f, 0f).ToString(), sourceObject.transform.rotation.ToString());
            Assert.AreEqual(Vector3.one * 3f, sourceObject.transform.localScale);
        }

        [Test]
        public void ModifyTransformNoSource()
        {
            targetTransformData.transform.position = Vector3.one * 2f;
            targetTransformData.transform.rotation = new Quaternion(1f, 1f, 0f, 0f);
            targetTransformData.transform.localScale = Vector3.one * 3f;

            subject.Modify(targetTransformData);
            Assert.AreEqual(Vector3.zero, sourceObject.transform.position);
            Assert.AreEqual(Quaternion.identity, sourceObject.transform.rotation);
            Assert.AreEqual(Vector3.one, sourceObject.transform.localScale);
        }

        [Test]
        public void ModifyEvents()
        {
            UnityEventListenerMock beforeTransformUpdatedMock = new UnityEventListenerMock();
            UnityEventListenerMock afterTransformUpdatedMock = new UnityEventListenerMock();
            subject.BeforeTransformUpdated.AddListener(beforeTransformUpdatedMock.Listen);
            subject.AfterTransformUpdated.AddListener(afterTransformUpdatedMock.Listen);

            subject.source = sourceObject.transform;
            subject.Modify(targetTransformData);
            Assert.IsTrue(beforeTransformUpdatedMock.Received);
            Assert.IsTrue(afterTransformUpdatedMock.Received);
        }

        [Test]
        public void EventsNotEmittedOnInactiveGameObject()
        {
            UnityEventListenerMock beforeTransformUpdatedMock = new UnityEventListenerMock();
            UnityEventListenerMock afterTransformUpdatedMock = new UnityEventListenerMock();
            subject.BeforeTransformUpdated.AddListener(beforeTransformUpdatedMock.Listen);
            subject.AfterTransformUpdated.AddListener(afterTransformUpdatedMock.Listen);

            subject.source = sourceObject.transform;
            subject.gameObject.SetActive(false);

            subject.Modify(targetTransformData);

            Assert.IsFalse(beforeTransformUpdatedMock.Received);
            Assert.IsFalse(afterTransformUpdatedMock.Received);
        }

        [Test]
        public void EventsNotEmittedOnDisabledComponent()
        {
            UnityEventListenerMock beforeTransformUpdatedMock = new UnityEventListenerMock();
            UnityEventListenerMock afterTransformUpdatedMock = new UnityEventListenerMock();
            subject.BeforeTransformUpdated.AddListener(beforeTransformUpdatedMock.Listen);
            subject.AfterTransformUpdated.AddListener(afterTransformUpdatedMock.Listen);

            subject.source = sourceObject.transform;
            subject.enabled = false;

            subject.Modify(targetTransformData);

            Assert.IsFalse(beforeTransformUpdatedMock.Received);
            Assert.IsFalse(afterTransformUpdatedMock.Received);
        }

        [Test]
        public void NoModifyPositionOnInactiveGameObject()
        {
            targetTransformData.transform.position = Vector3.one;

            subject.source = sourceObject.transform;
            subject.applyTransformations = Data.Enum.TransformProperties.Position;
            subject.gameObject.SetActive(false);

            Assert.AreEqual(Vector3.zero, sourceObject.transform.position);
            subject.Modify(targetTransformData);
            Assert.AreEqual(Vector3.zero, sourceObject.transform.position);
        }

        [Test]
        public void NoModifyPositionOnDisabledComponent()
        {
            targetTransformData.transform.position = Vector3.one;

            subject.source = sourceObject.transform;
            subject.applyTransformations = Data.Enum.TransformProperties.Position;
            subject.enabled = false;

            Assert.AreEqual(Vector3.zero, sourceObject.transform.position);
            subject.Modify(targetTransformData);
            Assert.AreEqual(Vector3.zero, sourceObject.transform.position);
        }
    }
}