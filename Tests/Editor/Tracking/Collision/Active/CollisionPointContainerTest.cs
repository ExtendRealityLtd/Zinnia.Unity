using Zinnia.Tracking.Collision.Active;
using Zinnia.Tracking.Collision;

namespace Test.Zinnia.Tracking.Collision.Active
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using Test.Zinnia.Utility.Helper;

    public class CollisionPointContainerTest
    {
        private GameObject containingObject;
        private CollisionPointContainerMock subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<CollisionPointContainerMock>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void CreateAndClear()
        {
            UnityEventListenerMock createdMock = new UnityEventListenerMock();
            UnityEventListenerMock clearedMock = new UnityEventListenerMock();
            subject.Created.AddListener(createdMock.Listen);
            subject.Destroyed.AddListener(clearedMock.Listen);

            GameObject publisherObject = new GameObject();
            ActiveCollisionPublisher.PayloadData publisher = new ActiveCollisionPublisher.PayloadData();
            publisher.sourceContainer = publisherObject;
            publisherObject.transform.position = Vector3.one;

            GameObject collisionNotifierContainer = new GameObject(); ;
            CollisionNotifier.EventData collisionNotifierEventData = CollisionNotifierHelper.GetEventData(out collisionNotifierContainer);
            collisionNotifierContainer.transform.position = Vector3.one * 2f;
            collisionNotifierContainer.transform.rotation = Quaternion.Euler(Vector3.forward * 90f);

            ActiveCollisionConsumer.EventData eventData = new ActiveCollisionConsumer.EventData();
            eventData.Set(publisher, collisionNotifierEventData);

            Assert.IsFalse(createdMock.Received);
            Assert.IsFalse(clearedMock.Received);
            Assert.IsNull(subject.Container);

            subject.Create(eventData);

            Assert.IsTrue(createdMock.Received);
            Assert.IsFalse(clearedMock.Received);
            Assert.IsNotNull(subject.Container);

            Assert.AreEqual(publisherObject.transform.position.ToString(), subject.Container.transform.position.ToString());
            Assert.AreEqual(publisherObject.transform.rotation.ToString(), subject.Container.transform.rotation.ToString());
            Assert.AreEqual(Vector3.one, subject.Container.transform.localScale);

            createdMock.Reset();
            clearedMock.Reset();

            subject.Destroy();

            Assert.IsFalse(createdMock.Received);
            Assert.IsTrue(clearedMock.Received);
            Assert.IsNull(subject.Container);

            Object.DestroyImmediate(publisherObject);
            Object.DestroyImmediate(collisionNotifierContainer);
        }

        [Test]
        public void CreateOnlyOnce()
        {
            UnityEventListenerMock createdMock = new UnityEventListenerMock();
            UnityEventListenerMock clearedMock = new UnityEventListenerMock();
            subject.Created.AddListener(createdMock.Listen);
            subject.Destroyed.AddListener(clearedMock.Listen);

            GameObject publisherObject = new GameObject();
            ActiveCollisionPublisher.PayloadData publisher = new ActiveCollisionPublisher.PayloadData();
            publisher.sourceContainer = publisherObject;
            publisherObject.transform.position = Vector3.one;

            GameObject collisionNotifierContainer = new GameObject(); ;
            CollisionNotifier.EventData collisionNotifierEventData = CollisionNotifierHelper.GetEventData(out collisionNotifierContainer);
            collisionNotifierContainer.transform.position = Vector3.one * 2f;
            collisionNotifierContainer.transform.rotation = Quaternion.Euler(Vector3.forward * 90f);

            ActiveCollisionConsumer.EventData eventData = new ActiveCollisionConsumer.EventData();
            eventData.Set(publisher, collisionNotifierEventData);

            GameObject publisherObjectAlt = new GameObject();
            ActiveCollisionPublisher.PayloadData publisherAlt = new ActiveCollisionPublisher.PayloadData();
            publisherAlt.sourceContainer = publisherObjectAlt;
            publisherObjectAlt.transform.position = Vector3.one;

            GameObject collisionNotifierContainerAlt = new GameObject(); ;
            CollisionNotifier.EventData collisionNotifierEventDataAlt = CollisionNotifierHelper.GetEventData(out collisionNotifierContainerAlt);
            collisionNotifierContainerAlt.transform.position = Vector3.one * 4f;
            collisionNotifierContainerAlt.transform.rotation = Quaternion.Euler(Vector3.up * 90f);

            ActiveCollisionConsumer.EventData eventDataAlt = new ActiveCollisionConsumer.EventData();
            eventDataAlt.Set(publisherAlt, collisionNotifierEventDataAlt);

            Assert.IsFalse(createdMock.Received);
            Assert.IsFalse(clearedMock.Received);
            Assert.IsNull(subject.Container);

            subject.Create(eventData);

            Assert.IsTrue(createdMock.Received);
            Assert.IsFalse(clearedMock.Received);
            Assert.IsNotNull(subject.Container);

            Assert.AreEqual(publisherObject.transform.position.ToString(), subject.Container.transform.position.ToString());
            Assert.AreEqual(publisherObject.transform.rotation.ToString(), subject.Container.transform.rotation.ToString());
            Assert.AreEqual(Vector3.one, subject.Container.transform.localScale);

            createdMock.Reset();
            clearedMock.Reset();

            subject.Create(eventDataAlt);

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

    public class CollisionPointContainerMock : CollisionPointContainer
    {
        protected override void DestroyContainer()
        {
            DestroyImmediate(Container);
            Container = null;
        }
    }
}