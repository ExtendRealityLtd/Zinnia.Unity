using VRTK.Core.Tracking.Collision.Active;
using VRTK.Core.Tracking.Collision;

namespace Test.VRTK.Core.Tracking.Collision.Active
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.VRTK.Core.Utility.Mock;
    using Test.VRTK.Core.Utility.Helper;

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
            subject.Cleared.AddListener(clearedMock.Listen);

            GameObject publisherObject = new GameObject();
            ActiveCollisionPublisher publisher = publisherObject.AddComponent<ActiveCollisionPublisher>();
            publisher.sourceContainer = publisherObject;
            publisherObject.transform.position = Vector3.zero;

            GameObject collisionNotifierContainer;
            CollisionNotifier.EventData collisionNotifierEventData = CollisionNotifierHelper.GetEventData(out collisionNotifierContainer);
            collisionNotifierContainer.transform.position = Vector3.one;
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

            Assert.AreEqual(collisionNotifierContainer.transform.position, subject.Container.transform.position);
            Assert.AreEqual(collisionNotifierContainer.transform.rotation, subject.Container.transform.rotation);
            Assert.AreEqual(Vector3.one, subject.Container.transform.localScale);

            createdMock.Reset();
            clearedMock.Reset();

            subject.Clear();

            Assert.IsFalse(createdMock.Received);
            Assert.IsTrue(clearedMock.Received);
            Assert.IsNull(subject.Container);

            Object.DestroyImmediate(publisherObject);
            Object.DestroyImmediate(collisionNotifierContainer);
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