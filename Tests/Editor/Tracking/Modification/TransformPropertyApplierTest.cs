using VRTK.Core.Tracking.Modification;
using VRTK.Core.Data.Type;
using VRTK.Core.Data.Enum;

namespace Test.VRTK.Core.Tracking.Modification
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.VRTK.Core.Utility.Mock;

    public class TransformPropertyApplierTest
    {
        private GameObject containingObject;
        private TransformPropertyApplier subject;

        private GameObject sourceObject;
        private GameObject offsetObject;
        private GameObject targetObject;
        private TransformData sourceTransformData;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<TransformPropertyApplier>();

            sourceObject = new GameObject();
            offsetObject = new GameObject();
            targetObject = new GameObject();
            sourceTransformData = new TransformData(sourceObject.transform);
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
            subject.source = new TransformData(sourceObject);
            subject.target = targetObject;
            subject.applyTransformations = TransformProperties.Position;

            Assert.AreEqual(Vector3.zero, targetObject.transform.position);

            Vector3 finalPosition = Vector3.one + Vector3.forward;
            sourceTransformData.transform.position = finalPosition;

            subject.Apply();

            Assert.AreEqual(finalPosition, targetObject.transform.position);
        }

        [Test]
        public void ModifyRotationNoOffsetInstantTransition()
        {
            subject.source = new TransformData(sourceObject);
            subject.target = targetObject;
            subject.applyTransformations = TransformProperties.Rotation;

            Assert.AreEqual(Vector3.zero, targetObject.transform.position);
            Assert.AreEqual(Quaternion.identity, targetObject.transform.rotation);

            Quaternion finalRotation = new Quaternion(1f, 0f, 0f, 0f);
            sourceTransformData.transform.position = Vector3.one;
            sourceTransformData.transform.rotation = finalRotation;
            subject.Apply();

            Assert.AreEqual(Vector3.zero, targetObject.transform.position);
            Assert.AreEqual(finalRotation, targetObject.transform.rotation);
        }

        [Test]
        public void ModifyScaleNoOffsetInstantTransition()
        {
            subject.source = new TransformData(sourceObject);
            subject.target = targetObject;
            subject.applyTransformations = TransformProperties.Scale;

            Assert.AreEqual(Vector3.zero, targetObject.transform.position);
            Assert.AreEqual(Quaternion.identity, targetObject.transform.rotation);
            Assert.AreEqual(Vector3.one, targetObject.transform.localScale);

            Vector3 finalScale = (Vector3.one * 2f) + Vector3.forward;
            sourceTransformData.transform.position = Vector3.one;
            sourceTransformData.transform.rotation = new Quaternion(1f, 0f, 0f, 0f);
            sourceTransformData.transform.localScale = finalScale;

            subject.Apply();

            Assert.AreEqual(Vector3.zero, targetObject.transform.position);
            Assert.AreEqual(Quaternion.identity, targetObject.transform.rotation);
            Assert.AreEqual(finalScale, targetObject.transform.localScale);
        }

        [Test]
        public void ModifyTransformWithOffset()
        {
            subject.source = new TransformData(sourceObject);
            subject.target = targetObject;
            subject.offset = offsetObject;
            subject.applyTransformations = TransformProperties.Position | TransformProperties.Rotation | TransformProperties.Scale;

            Assert.AreEqual(Vector3.zero, targetObject.transform.position);
            Assert.AreEqual(Quaternion.identity, targetObject.transform.rotation);
            Assert.AreEqual(Vector3.one, targetObject.transform.localScale);

            sourceTransformData.transform.position = Vector3.one * 2f;
            sourceTransformData.transform.rotation = new Quaternion(1f, 1f, 0f, 0f);
            sourceTransformData.transform.localScale = Vector3.one * 3f;

            offsetObject.transform.position = Vector3.one;
            offsetObject.transform.rotation = new Quaternion(0.5f, 0f, 0.5f, 0f);

            subject.Apply();

            Assert.AreEqual(new Vector3(-1f, -1f, 5f).ToString(), targetObject.transform.position.ToString());
            Assert.AreEqual(new Quaternion(0.7f, 0.7f, 0f, 0f).ToString(), targetObject.transform.rotation.ToString());
            Assert.AreEqual((Vector3.one * 3f).ToString(), targetObject.transform.localScale.ToString());
        }

        [Test]
        public void ModifyTransformWithOffsetNoRotation()
        {
            subject.source = new TransformData(sourceObject);
            subject.target = targetObject;
            subject.offset = offsetObject;
            subject.applyTransformations = TransformProperties.Position | TransformProperties.Scale;

            Assert.AreEqual(Vector3.zero, targetObject.transform.position);
            Assert.AreEqual(Quaternion.identity, targetObject.transform.rotation);
            Assert.AreEqual(Vector3.one, targetObject.transform.localScale);

            sourceTransformData.transform.position = Vector3.one * 2f;
            sourceTransformData.transform.rotation = new Quaternion(1f, 1f, 0f, 0f);
            sourceTransformData.transform.localScale = Vector3.one * 3f;

            offsetObject.transform.position = Vector3.one;
            offsetObject.transform.rotation = new Quaternion(0.5f, 0f, 0.5f, 0f);

            subject.Apply();

            Assert.AreEqual(new Vector3(1f, 1f, 1f).ToString(), targetObject.transform.position.ToString());
            Assert.AreEqual(Quaternion.identity, targetObject.transform.rotation);
            Assert.AreEqual((Vector3.one * 3f).ToString(), targetObject.transform.localScale.ToString());
        }

        [Test]
        public void ModifyTransformWithXOffsetOnly()
        {
            subject.source = new TransformData(sourceObject);
            subject.target = targetObject;
            subject.offset = offsetObject;
            subject.applyOffsetOnAxis = new Vector3State(true, false, false);
            subject.applyTransformations = TransformProperties.Position | TransformProperties.Rotation | TransformProperties.Scale;

            sourceTransformData.transform.position = Vector3.one * 2f;
            sourceTransformData.transform.rotation = new Quaternion(1f, 1f, 0f, 0f);
            sourceTransformData.transform.localScale = Vector3.one * 3f;

            offsetObject.transform.position = Vector3.one;
            offsetObject.transform.rotation = new Quaternion(0.5f, 0f, 0.5f, 0f);

            subject.Apply();
            Assert.AreEqual(new Vector3(2f, -1f, 2f).ToString(), targetObject.transform.position.ToString());
            Assert.AreEqual(new Quaternion(0.7f, 0.7f, 0f, 0f).ToString(), targetObject.transform.rotation.ToString());
            Assert.AreEqual((Vector3.one * 3f).ToString(), targetObject.transform.localScale.ToString());
        }

        [Test]
        public void ModifyTransformNoSourceOrTarget()
        {
            sourceTransformData.transform.position = Vector3.one * 2f;
            sourceTransformData.transform.rotation = new Quaternion(1f, 1f, 0f, 0f);
            sourceTransformData.transform.localScale = Vector3.one * 3f;

            subject.Apply();

            Assert.AreEqual(Vector3.zero, targetObject.transform.position);
            Assert.AreEqual(Quaternion.identity, targetObject.transform.rotation);
            Assert.AreEqual(Vector3.one, targetObject.transform.localScale);
        }

        [Test]
        public void ModifyTransformNoSource()
        {
            subject.target = targetObject;

            sourceTransformData.transform.position = Vector3.one * 2f;
            sourceTransformData.transform.rotation = new Quaternion(1f, 1f, 0f, 0f);
            sourceTransformData.transform.localScale = Vector3.one * 3f;

            subject.Apply();

            Assert.AreEqual(Vector3.zero, targetObject.transform.position);
            Assert.AreEqual(Quaternion.identity, targetObject.transform.rotation);
            Assert.AreEqual(Vector3.one, targetObject.transform.localScale);
        }

        [Test]
        public void ModifyTransformNoTarget()
        {
            subject.source = new TransformData(sourceObject);

            sourceTransformData.transform.position = Vector3.one * 2f;
            sourceTransformData.transform.rotation = new Quaternion(1f, 1f, 0f, 0f);
            sourceTransformData.transform.localScale = Vector3.one * 3f;

            subject.Apply();

            Assert.AreEqual(Vector3.zero, targetObject.transform.position);
            Assert.AreEqual(Quaternion.identity, targetObject.transform.rotation);
            Assert.AreEqual(Vector3.one, targetObject.transform.localScale);
        }

        [Test]
        public void ModifyEvents()
        {
            UnityEventListenerMock beforeTransformUpdatedMock = new UnityEventListenerMock();
            UnityEventListenerMock afterTransformUpdatedMock = new UnityEventListenerMock();
            subject.BeforeTransformUpdated.AddListener(beforeTransformUpdatedMock.Listen);
            subject.AfterTransformUpdated.AddListener(afterTransformUpdatedMock.Listen);

            subject.source = new TransformData(sourceObject);
            subject.target = targetObject;
            subject.Apply();
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

            subject.source = new TransformData(sourceObject);
            subject.target = targetObject;
            subject.gameObject.SetActive(false);

            subject.Apply();

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

            subject.source = new TransformData(sourceObject);
            subject.target = targetObject;
            subject.enabled = false;

            subject.Apply();

            Assert.IsFalse(beforeTransformUpdatedMock.Received);
            Assert.IsFalse(afterTransformUpdatedMock.Received);
        }

        [Test]
        public void NoModifyPositionOnInactiveGameObject()
        {
            sourceTransformData.transform.position = Vector3.one;

            subject.source = new TransformData(sourceObject);
            subject.target = targetObject;
            subject.applyTransformations = TransformProperties.Position;
            subject.gameObject.SetActive(false);

            Assert.AreEqual(Vector3.zero, targetObject.transform.position);
            subject.Apply();
            Assert.AreEqual(Vector3.zero, targetObject.transform.position);
        }

        [Test]
        public void NoModifyPositionOnDisabledComponent()
        {
            sourceTransformData.transform.position = Vector3.one;

            subject.source = new TransformData(sourceObject);
            subject.target = targetObject;
            subject.applyTransformations = TransformProperties.Position;
            subject.enabled = false;

            Assert.AreEqual(Vector3.zero, targetObject.transform.position);
            subject.Apply();
            Assert.AreEqual(Vector3.zero, targetObject.transform.position);
        }
    }
}