using Zinnia.Tracking.Collision;
using Zinnia.Tracking.Collision.Active;

namespace Test.Zinnia.Tracking.Collision.Active
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.Zinnia.Utility.Helper;
    using Assert = UnityEngine.Assertions.Assert;

    public class ActiveCollisionPublisherTest
    {
        private GameObject containingObject;
        private ActiveCollisionPublisher subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<ActiveCollisionPublisher>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void SetActiveCollisions()
        {
            ActiveCollisionsContainer.EventData eventData = new ActiveCollisionsContainer.EventData();
            GameObject oneContainer;
            CollisionNotifier.EventData oneData = CollisionNotifierHelper.GetEventData(out oneContainer);
            GameObject twoContainer;
            CollisionNotifier.EventData twoData = CollisionNotifierHelper.GetEventData(out twoContainer);
            eventData.ActiveCollisions.Add(oneData);
            eventData.ActiveCollisions.Add(twoData);

            Assert.AreEqual(0, subject.Payload.ActiveCollisions.Count);
            subject.SetActiveCollisions(eventData);
            Assert.AreEqual(2, subject.Payload.ActiveCollisions.Count);

            Object.DestroyImmediate(oneContainer);
            Object.DestroyImmediate(twoContainer);
        }

        [Test]
        public void SetActiveCollisionsNoData()
        {
            ActiveCollisionsContainer.EventData nullEventData = null;
            Assert.AreEqual(0, subject.Payload.ActiveCollisions.Count);
            subject.SetActiveCollisions(nullEventData);
            Assert.AreEqual(0, subject.Payload.ActiveCollisions.Count);

            ActiveCollisionsContainer.EventData eventData = new ActiveCollisionsContainer.EventData();
            eventData.ActiveCollisions = null;

            subject.SetActiveCollisions(eventData);
            Assert.AreEqual(0, subject.Payload.ActiveCollisions.Count);
        }

        [Test]
        public void SetActiveCollisionsInactiveGameObject()
        {
            ActiveCollisionsContainer.EventData eventData = new ActiveCollisionsContainer.EventData();
            GameObject oneContainer;
            CollisionNotifier.EventData oneData = CollisionNotifierHelper.GetEventData(out oneContainer);
            GameObject twoContainer;
            CollisionNotifier.EventData twoData = CollisionNotifierHelper.GetEventData(out twoContainer);
            eventData.ActiveCollisions.Add(oneData);
            eventData.ActiveCollisions.Add(twoData);

            Assert.AreEqual(0, subject.Payload.ActiveCollisions.Count);
            subject.gameObject.SetActive(false);
            subject.SetActiveCollisions(eventData);
            Assert.AreEqual(0, subject.Payload.ActiveCollisions.Count);

            Object.DestroyImmediate(oneContainer);
            Object.DestroyImmediate(twoContainer);
        }

        [Test]
        public void SetActiveCollisionsInactiveComponent()
        {
            ActiveCollisionsContainer.EventData eventData = new ActiveCollisionsContainer.EventData();
            GameObject oneContainer;
            CollisionNotifier.EventData oneData = CollisionNotifierHelper.GetEventData(out oneContainer);
            GameObject twoContainer;
            CollisionNotifier.EventData twoData = CollisionNotifierHelper.GetEventData(out twoContainer);
            eventData.ActiveCollisions.Add(oneData);
            eventData.ActiveCollisions.Add(twoData);

            Assert.AreEqual(0, subject.Payload.ActiveCollisions.Count);
            subject.enabled = false;
            subject.SetActiveCollisions(eventData);
            Assert.AreEqual(0, subject.Payload.ActiveCollisions.Count);

            Object.DestroyImmediate(oneContainer);
            Object.DestroyImmediate(twoContainer);
        }

        [Test]
        public void Publish()
        {
            ActiveCollisionsContainer.EventData eventData = new ActiveCollisionsContainer.EventData();
            GameObject oneContainer;
            CollisionNotifier.EventData oneData = CollisionNotifierHelper.GetEventData(out oneContainer);
            ActiveCollisionConsumerMock oneConsumer = oneContainer.AddComponent<ActiveCollisionConsumerMock>();
            GameObject twoContainer;
            CollisionNotifier.EventData twoData = CollisionNotifierHelper.GetEventData(out twoContainer);
            ActiveCollisionConsumerMock twoConsumer = twoContainer.AddComponent<ActiveCollisionConsumerMock>();
            eventData.ActiveCollisions.Add(oneData);
            eventData.ActiveCollisions.Add(twoData);
            subject.SetActiveCollisions(eventData);

            Assert.IsFalse(oneConsumer.received);
            Assert.IsFalse(twoConsumer.received);

            subject.Publish();

            Assert.IsTrue(oneConsumer.received);
            Assert.IsTrue(twoConsumer.received);

            Object.DestroyImmediate(oneContainer);
            Object.DestroyImmediate(twoContainer);
        }

        [Test]
        public void PublishMultipleConsumers()
        {
            ActiveCollisionsContainer.EventData eventData = new ActiveCollisionsContainer.EventData();
            GameObject oneContainer;
            CollisionNotifier.EventData oneData = CollisionNotifierHelper.GetEventData(out oneContainer);
            ActiveCollisionConsumerMock oneConsumer = oneContainer.AddComponent<ActiveCollisionConsumerMock>();
            GameObject twoContainer;
            CollisionNotifier.EventData twoData = CollisionNotifierHelper.GetEventData(out twoContainer);
            ActiveCollisionConsumerMock twoConsumer = twoContainer.AddComponent<ActiveCollisionConsumerMock>();
            ActiveCollisionConsumerMock threeConsumer = twoContainer.AddComponent<ActiveCollisionConsumerMock>();
            eventData.ActiveCollisions.Add(oneData);
            eventData.ActiveCollisions.Add(twoData);
            subject.SetActiveCollisions(eventData);

            Assert.IsFalse(oneConsumer.received);
            Assert.IsFalse(twoConsumer.received);
            Assert.IsFalse(threeConsumer.received);

            subject.Publish();

            Assert.IsTrue(oneConsumer.received);
            Assert.IsTrue(twoConsumer.received);
            Assert.IsTrue(threeConsumer.received);

            Object.DestroyImmediate(oneContainer);
            Object.DestroyImmediate(twoContainer);
        }

        [Test]
        public void PublishInactiveGameObject()
        {
            ActiveCollisionsContainer.EventData eventData = new ActiveCollisionsContainer.EventData();
            GameObject oneContainer;
            CollisionNotifier.EventData oneData = CollisionNotifierHelper.GetEventData(out oneContainer);
            ActiveCollisionConsumerMock oneConsumer = oneContainer.AddComponent<ActiveCollisionConsumerMock>();
            GameObject twoContainer;
            CollisionNotifier.EventData twoData = CollisionNotifierHelper.GetEventData(out twoContainer);
            ActiveCollisionConsumerMock twoConsumer = twoContainer.AddComponent<ActiveCollisionConsumerMock>();
            eventData.ActiveCollisions.Add(oneData);
            eventData.ActiveCollisions.Add(twoData);
            subject.SetActiveCollisions(eventData);

            Assert.IsFalse(oneConsumer.received);
            Assert.IsFalse(twoConsumer.received);

            subject.gameObject.SetActive(false);
            subject.Publish();

            Assert.IsFalse(oneConsumer.received);
            Assert.IsFalse(twoConsumer.received);

            Object.DestroyImmediate(oneContainer);
            Object.DestroyImmediate(twoContainer);
        }

        [Test]
        public void PublishInactiveComponent()
        {
            ActiveCollisionsContainer.EventData eventData = new ActiveCollisionsContainer.EventData();
            GameObject oneContainer;
            CollisionNotifier.EventData oneData = CollisionNotifierHelper.GetEventData(out oneContainer);
            ActiveCollisionConsumerMock oneConsumer = oneContainer.AddComponent<ActiveCollisionConsumerMock>();
            GameObject twoContainer;
            CollisionNotifier.EventData twoData = CollisionNotifierHelper.GetEventData(out twoContainer);
            ActiveCollisionConsumerMock twoConsumer = twoContainer.AddComponent<ActiveCollisionConsumerMock>();
            eventData.ActiveCollisions.Add(oneData);
            eventData.ActiveCollisions.Add(twoData);
            subject.SetActiveCollisions(eventData);

            Assert.IsFalse(oneConsumer.received);
            Assert.IsFalse(twoConsumer.received);

            subject.enabled = false;
            subject.Publish();

            Assert.IsFalse(oneConsumer.received);
            Assert.IsFalse(twoConsumer.received);

            Object.DestroyImmediate(oneContainer);
            Object.DestroyImmediate(twoContainer);
        }

        [Test]
        public void PublishInactiveConsumerGameObject()
        {
            ActiveCollisionsContainer.EventData eventData = new ActiveCollisionsContainer.EventData();
            GameObject oneContainer;
            CollisionNotifier.EventData oneData = CollisionNotifierHelper.GetEventData(out oneContainer);
            ActiveCollisionConsumerMock oneConsumer = oneContainer.AddComponent<ActiveCollisionConsumerMock>();
            GameObject twoContainer;
            CollisionNotifier.EventData twoData = CollisionNotifierHelper.GetEventData(out twoContainer);
            ActiveCollisionConsumerMock twoConsumer = twoContainer.AddComponent<ActiveCollisionConsumerMock>();
            twoContainer.SetActive(false);
            eventData.ActiveCollisions.Add(oneData);
            eventData.ActiveCollisions.Add(twoData);
            subject.SetActiveCollisions(eventData);

            Assert.IsFalse(oneConsumer.received);
            Assert.IsFalse(twoConsumer.received);

            subject.Publish();

            Assert.IsTrue(oneConsumer.received);
            Assert.IsFalse(twoConsumer.received);

            Object.DestroyImmediate(oneContainer);
            Object.DestroyImmediate(twoContainer);
        }

        [Test]
        public void PublishInactiveConsumerComponent()
        {
            ActiveCollisionsContainer.EventData eventData = new ActiveCollisionsContainer.EventData();
            GameObject oneContainer;
            CollisionNotifier.EventData oneData = CollisionNotifierHelper.GetEventData(out oneContainer);
            ActiveCollisionConsumerMock oneConsumer = oneContainer.AddComponent<ActiveCollisionConsumerMock>();
            GameObject twoContainer;
            CollisionNotifier.EventData twoData = CollisionNotifierHelper.GetEventData(out twoContainer);
            ActiveCollisionConsumerMock twoConsumer = twoContainer.AddComponent<ActiveCollisionConsumerMock>();
            twoConsumer.enabled = false;
            eventData.ActiveCollisions.Add(oneData);
            eventData.ActiveCollisions.Add(twoData);
            subject.SetActiveCollisions(eventData);

            Assert.IsFalse(oneConsumer.received);
            Assert.IsFalse(twoConsumer.received);

            subject.Publish();

            Assert.IsTrue(oneConsumer.received);
            Assert.IsFalse(twoConsumer.received);

            Object.DestroyImmediate(oneContainer);
            Object.DestroyImmediate(twoContainer);
        }
    }

    public class ActiveCollisionConsumerMock : ActiveCollisionConsumer
    {
        public bool received;

        public override void Consume(ActiveCollisionPublisher.PayloadData publisher, CollisionNotifier.EventData currentCollision)
        {
            if (isActiveAndEnabled)
            {
                received = true;
            }
        }
    }
}