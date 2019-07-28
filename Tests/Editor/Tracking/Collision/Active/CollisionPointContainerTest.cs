using Zinnia.Tracking.Collision.Active;
using Zinnia.Tracking.Collision;

namespace Test.Zinnia.Tracking.Collision.Active
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using Test.Zinnia.Utility.Helper;
    using Assert = UnityEngine.Assertions.Assert;

    public class CollisionPointContainerTest
    {
        private GameObject containingObject;
        private CollisionPointContainer subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<CollisionPointContainer>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject.Container);
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void SetAndClear()
        {
            UnityEventListenerMock createdMock = new UnityEventListenerMock();
            UnityEventListenerMock clearedMock = new UnityEventListenerMock();
            subject.PointSet.AddListener(createdMock.Listen);
            subject.PointUnset.AddListener(clearedMock.Listen);

            GameObject publisherObject = new GameObject();
            ActiveCollisionPublisher.PayloadData publisher = new ActiveCollisionPublisher.PayloadData();
            publisher.SourceContainer = publisherObject;
            publisherObject.transform.position = Vector3.one;

            CollisionNotifier.EventData collisionNotifierEventData = CollisionNotifierHelper.GetEventData(out GameObject collisionNotifierContainer);
            collisionNotifierContainer.transform.position = Vector3.one * 2f;
            collisionNotifierContainer.transform.rotation = Quaternion.Euler(Vector3.forward * 90f);

            ActiveCollisionConsumer.EventData eventData = new ActiveCollisionConsumer.EventData();
            eventData.Set(publisher, collisionNotifierEventData);

            Assert.IsFalse(createdMock.Received);
            Assert.IsFalse(clearedMock.Received);
            Assert.IsNull(subject.Container);

            subject.Set(eventData);

            Assert.IsTrue(createdMock.Received);
            Assert.IsFalse(clearedMock.Received);
            Assert.IsNotNull(subject.Container);

            Assert.AreEqual(publisherObject.transform.position.ToString(), subject.Container.transform.position.ToString());
            Assert.AreEqual(publisherObject.transform.rotation.ToString(), subject.Container.transform.rotation.ToString());
            Assert.AreEqual(Vector3.one, subject.Container.transform.localScale);

            createdMock.Reset();
            clearedMock.Reset();

            subject.Unset();

            Assert.IsFalse(createdMock.Received);
            Assert.IsTrue(clearedMock.Received);
            Assert.IsNotNull(subject.Container);

            Object.DestroyImmediate(publisherObject);
            Object.DestroyImmediate(collisionNotifierContainer);
        }

        [Test]
        public void SetOnlyOnce()
        {
            UnityEventListenerMock createdMock = new UnityEventListenerMock();
            UnityEventListenerMock clearedMock = new UnityEventListenerMock();
            subject.PointSet.AddListener(createdMock.Listen);
            subject.PointUnset.AddListener(clearedMock.Listen);

            GameObject publisherObject = new GameObject();
            ActiveCollisionPublisher.PayloadData publisher = new ActiveCollisionPublisher.PayloadData();
            publisher.SourceContainer = publisherObject;
            publisherObject.transform.position = Vector3.one;

            CollisionNotifier.EventData collisionNotifierEventData = CollisionNotifierHelper.GetEventData(out GameObject collisionNotifierContainer);
            collisionNotifierContainer.transform.position = Vector3.one * 2f;
            collisionNotifierContainer.transform.rotation = Quaternion.Euler(Vector3.forward * 90f);

            ActiveCollisionConsumer.EventData eventData = new ActiveCollisionConsumer.EventData();
            eventData.Set(publisher, collisionNotifierEventData);

            GameObject publisherObjectAlt = new GameObject();
            ActiveCollisionPublisher.PayloadData publisherAlt = new ActiveCollisionPublisher.PayloadData();
            publisherAlt.SourceContainer = publisherObjectAlt;
            publisherObjectAlt.transform.position = Vector3.one;

            CollisionNotifier.EventData collisionNotifierEventDataAlt = CollisionNotifierHelper.GetEventData(out GameObject collisionNotifierContainerAlt);
            collisionNotifierContainerAlt.transform.position = Vector3.one * 4f;
            collisionNotifierContainerAlt.transform.rotation = Quaternion.Euler(Vector3.up * 90f);

            ActiveCollisionConsumer.EventData eventDataAlt = new ActiveCollisionConsumer.EventData();
            eventDataAlt.Set(publisherAlt, collisionNotifierEventDataAlt);

            Assert.IsFalse(createdMock.Received);
            Assert.IsFalse(clearedMock.Received);
            Assert.IsNull(subject.Container);

            subject.Set(eventData);

            Assert.IsTrue(createdMock.Received);
            Assert.IsFalse(clearedMock.Received);
            Assert.IsNotNull(subject.Container);

            Assert.AreEqual(publisherObject.transform.position.ToString(), subject.Container.transform.position.ToString());
            Assert.AreEqual(publisherObject.transform.rotation.ToString(), subject.Container.transform.rotation.ToString());
            Assert.AreEqual(Vector3.one, subject.Container.transform.localScale);

            createdMock.Reset();
            clearedMock.Reset();

            subject.Set(eventDataAlt);

            Assert.IsFalse(createdMock.Received);
            Assert.IsFalse(clearedMock.Received);
            Assert.IsNotNull(subject.Container);

            Assert.AreEqual(publisherObject.transform.position.ToString(), subject.Container.transform.position.ToString());
            Assert.AreEqual(publisherObject.transform.rotation.ToString(), subject.Container.transform.rotation.ToString());
            Assert.AreEqual(Vector3.one, subject.Container.transform.localScale);

            Object.DestroyImmediate(publisherObject);
            Object.DestroyImmediate(publisherObjectAlt);
            Object.DestroyImmediate(collisionNotifierContainer);
            Object.DestroyImmediate(collisionNotifierContainerAlt);
        }
    }
}