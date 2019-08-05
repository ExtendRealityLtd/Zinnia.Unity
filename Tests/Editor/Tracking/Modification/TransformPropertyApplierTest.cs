using Zinnia.Tracking.Modification;
using Zinnia.Data.Type;
using Zinnia.Data.Enum;

namespace Test.Zinnia.Tracking.Modification
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using Assert = UnityEngine.Assertions.Assert;

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
        public void SetSourceValid()
        {
            TransformData source = new TransformData(sourceObject);
            Assert.IsNull(subject.Source);
            subject.Source = source;
            Assert.AreEqual(source, subject.Source);
        }

        [Test]
        public void SetSourceInvalid()
        {
            TransformData source = null;
            Assert.IsNull(subject.Source);
            subject.Source = source;
            Assert.IsNull(subject.Source);
        }

        [Test]
        public void SetTargetValid()
        {
            TransformData target = new TransformData(targetObject);
            Assert.IsNull(subject.Target);
            subject.SetTarget(target);
            Assert.AreEqual(targetObject, subject.Target);
        }

        [Test]
        public void SetTargetInvalid()
        {
            TransformData target = null;
            Assert.IsNull(subject.Target);
            subject.SetTarget(target);
            Assert.IsNull(subject.Target);
        }

        [Test]
        public void SetOffsetValid()
        {
            TransformData offset = new TransformData(offsetObject);
            Assert.IsNull(subject.Offset);
            subject.SetOffset(offset);
            Assert.AreEqual(offsetObject, subject.Offset);
        }

        [Test]
        public void SetOffsetInvalid()
        {
            TransformData offset = null;
            Assert.IsNull(subject.Offset);
            subject.SetOffset(offset);
            Assert.IsNull(subject.Offset);
        }

        [Test]
        public void ModifyPositionNoOffsetInstantTransition()
        {
            subject.Source = new TransformData(sourceObject);
            subject.Target = targetObject;
            subject.ApplyTransformations = TransformProperties.Position;

            Assert.AreEqual(Vector3.zero, targetObject.transform.position);

            Vector3 finalPosition = Vector3.one + Vector3.forward;
            sourceTransformData.Transform.position = finalPosition;

            subject.Apply();

            Assert.AreEqual(finalPosition, targetObject.transform.position);
        }

        [Test]
        public void ModifyRotationNoOffsetInstantTransition()
        {
            subject.Source = new TransformData(sourceObject);
            subject.Target = targetObject;
            subject.ApplyTransformations = TransformProperties.Rotation;

            Assert.AreEqual(Vector3.zero, targetObject.transform.position);
            Assert.AreEqual(Quaternion.identity, targetObject.transform.rotation);

            Quaternion finalRotation = new Quaternion(1f, 0f, 0f, 0f);
            sourceTransformData.Transform.position = Vector3.one;
            sourceTransformData.Transform.rotation = finalRotation;
            subject.Apply();

            Assert.AreEqual(Vector3.zero, targetObject.transform.position);
            Assert.AreEqual(finalRotation, targetObject.transform.rotation);
        }

        [Test]
        public void ModifyScaleNoOffsetInstantTransition()
        {
            subject.Source = new TransformData(sourceObject);
            subject.Target = targetObject;
            subject.ApplyTransformations = TransformProperties.Scale;

            Assert.AreEqual(Vector3.zero, targetObject.transform.position);
            Assert.AreEqual(Quaternion.identity, targetObject.transform.rotation);
            Assert.AreEqual(Vector3.one, targetObject.transform.localScale);

            Vector3 finalScale = (Vector3.one * 2f) + Vector3.forward;
            sourceTransformData.Transform.position = Vector3.one;
            sourceTransformData.Transform.rotation = new Quaternion(1f, 0f, 0f, 0f);
            sourceTransformData.Transform.localScale = finalScale;

            subject.Apply();

            Assert.AreEqual(Vector3.zero, targetObject.transform.position);
            Assert.AreEqual(Quaternion.identity, targetObject.transform.rotation);
            Assert.AreEqual(finalScale, targetObject.transform.localScale);
        }

        [Test]
        public void ModifyTransformWithOffset()
        {
            subject.Source = new TransformData(sourceObject);
            subject.Target = targetObject;
            subject.Offset = offsetObject;
            subject.ApplyTransformations = TransformProperties.Position | TransformProperties.Rotation | TransformProperties.Scale;

            Assert.AreEqual(Vector3.zero, targetObject.transform.position);
            Assert.AreEqual(Quaternion.identity, targetObject.transform.rotation);
            Assert.AreEqual(Vector3.one, targetObject.transform.localScale);

            sourceTransformData.Transform.position = Vector3.one * 2f;
            sourceTransformData.Transform.rotation = new Quaternion(1f, 1f, 0f, 0f);
            sourceTransformData.Transform.localScale = Vector3.one * 3f;

            offsetObject.transform.position = Vector3.one;
            offsetObject.transform.rotation = new Quaternion(0.5f, 0f, 0.5f, 0f);

            subject.Apply();

            Assert.AreEqual(new Vector3(5f, -1f, 5f).ToString(), targetObject.transform.position.ToString());
            Assert.AreEqual(new Quaternion(0.5f, -0.5f, -0.5f, -0.5f).ToString(), targetObject.transform.rotation.ToString());
            Assert.AreEqual((Vector3.one * 3f).ToString(), targetObject.transform.localScale.ToString());
        }

        [Test]
        public void ModifyTransformWithOffsetNoRotation()
        {
            subject.Source = new TransformData(sourceObject);
            subject.Target = targetObject;
            subject.Offset = offsetObject;
            subject.ApplyTransformations = TransformProperties.Position | TransformProperties.Scale;

            Assert.AreEqual(Vector3.zero, targetObject.transform.position);
            Assert.AreEqual(Quaternion.identity, targetObject.transform.rotation);
            Assert.AreEqual(Vector3.one, targetObject.transform.localScale);

            sourceTransformData.Transform.position = Vector3.one * 2f;
            sourceTransformData.Transform.rotation = new Quaternion(1f, 1f, 0f, 0f);
            sourceTransformData.Transform.localScale = Vector3.one * 3f;

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
            subject.Source = new TransformData(sourceObject);
            subject.Target = targetObject;
            subject.Offset = offsetObject;
            subject.ApplyPositionOffsetOnAxis = new Vector3State(true, false, false);
            subject.ApplyTransformations = TransformProperties.Position | TransformProperties.Rotation | TransformProperties.Scale;

            sourceTransformData.Transform.position = Vector3.one * 2f;
            sourceTransformData.Transform.rotation = new Quaternion(1f, 1f, 0f, 0f);
            sourceTransformData.Transform.localScale = Vector3.one * 3f;

            offsetObject.transform.position = Vector3.one;
            offsetObject.transform.rotation = new Quaternion(0.5f, 0f, 0.5f, 0f);

            subject.Apply();
            Assert.AreEqual(new Vector3(2f, 2f, 5f).ToString(), targetObject.transform.position.ToString());
            Assert.AreEqual(new Quaternion(0.5f, -0.5f, -0.5f, -0.5f).ToString(), targetObject.transform.rotation.ToString());
            Assert.AreEqual((Vector3.one * 3f).ToString(), targetObject.transform.localScale.ToString());
        }

        [Test]
        public void ModifyTransformNoSourceOrTarget()
        {
            sourceTransformData.Transform.position = Vector3.one * 2f;
            sourceTransformData.Transform.rotation = new Quaternion(1f, 1f, 0f, 0f);
            sourceTransformData.Transform.localScale = Vector3.one * 3f;

            subject.Apply();

            Assert.AreEqual(Vector3.zero, targetObject.transform.position);
            Assert.AreEqual(Quaternion.identity, targetObject.transform.rotation);
            Assert.AreEqual(Vector3.one, targetObject.transform.localScale);
        }

        [Test]
        public void ModifyTransformNoSource()
        {
            subject.Target = targetObject;

            sourceTransformData.Transform.position = Vector3.one * 2f;
            sourceTransformData.Transform.rotation = new Quaternion(1f, 1f, 0f, 0f);
            sourceTransformData.Transform.localScale = Vector3.one * 3f;

            subject.Apply();

            Assert.AreEqual(Vector3.zero, targetObject.transform.position);
            Assert.AreEqual(Quaternion.identity, targetObject.transform.rotation);
            Assert.AreEqual(Vector3.one, targetObject.transform.localScale);
        }

        [Test]
        public void ModifyTransformNoTarget()
        {
            subject.Source = new TransformData(sourceObject);

            sourceTransformData.Transform.position = Vector3.one * 2f;
            sourceTransformData.Transform.rotation = new Quaternion(1f, 1f, 0f, 0f);
            sourceTransformData.Transform.localScale = Vector3.one * 3f;

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

            subject.Source = new TransformData(sourceObject);
            subject.Target = targetObject;
            sourceTransformData.Transform.position = Vector3.one;
            subject.Apply();
            Assert.IsTrue(beforeTransformUpdatedMock.Received);
            Assert.IsTrue(afterTransformUpdatedMock.Received);
        }

        [Test]
        public void NoEventsWhenNoChange()
        {
            UnityEventListenerMock beforeTransformUpdatedMock = new UnityEventListenerMock();
            UnityEventListenerMock afterTransformUpdatedMock = new UnityEventListenerMock();
            subject.BeforeTransformUpdated.AddListener(beforeTransformUpdatedMock.Listen);
            subject.AfterTransformUpdated.AddListener(afterTransformUpdatedMock.Listen);

            subject.Source = new TransformData(sourceObject);
            subject.Target = targetObject;
            subject.Apply();
            Assert.IsFalse(beforeTransformUpdatedMock.Received);
            Assert.IsFalse(afterTransformUpdatedMock.Received);
        }

        [Test]
        public void EventsNotEmittedOnInactiveGameObject()
        {
            UnityEventListenerMock beforeTransformUpdatedMock = new UnityEventListenerMock();
            UnityEventListenerMock afterTransformUpdatedMock = new UnityEventListenerMock();
            subject.BeforeTransformUpdated.AddListener(beforeTransformUpdatedMock.Listen);
            subject.AfterTransformUpdated.AddListener(afterTransformUpdatedMock.Listen);

            subject.Source = new TransformData(sourceObject);
            subject.Target = targetObject;
            sourceTransformData.Transform.position = Vector3.one;
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

            subject.Source = new TransformData(sourceObject);
            subject.Target = targetObject;
            sourceTransformData.Transform.position = Vector3.one;
            subject.enabled = false;

            subject.Apply();

            Assert.IsFalse(beforeTransformUpdatedMock.Received);
            Assert.IsFalse(afterTransformUpdatedMock.Received);
        }

        [Test]
        public void NoModifyPositionOnInactiveGameObject()
        {
            sourceTransformData.Transform.position = Vector3.one;

            subject.Source = new TransformData(sourceObject);
            subject.Target = targetObject;
            subject.ApplyTransformations = TransformProperties.Position;
            sourceTransformData.Transform.position = Vector3.one;
            subject.gameObject.SetActive(false);

            Assert.AreEqual(Vector3.zero, targetObject.transform.position);
            subject.Apply();
            Assert.AreEqual(Vector3.zero, targetObject.transform.position);
        }

        [Test]
        public void NoModifyPositionOnDisabledComponent()
        {
            sourceTransformData.Transform.position = Vector3.one;

            subject.Source = new TransformData(sourceObject);
            subject.Target = targetObject;
            subject.ApplyTransformations = TransformProperties.Position;
            sourceTransformData.Transform.position = Vector3.one;
            subject.enabled = false;

            Assert.AreEqual(Vector3.zero, targetObject.transform.position);
            subject.Apply();
            Assert.AreEqual(Vector3.zero, targetObject.transform.position);
        }
    }
}