﻿using VRTK.Core.Tracking.Collision;
using VRTK.Core.Tracking.Collision.Active;

namespace Test.VRTK.Core.Tracking.Collision.Active
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.VRTK.Core.Utility.Helper;

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
            eventData.activeCollisions.Add(oneData);
            eventData.activeCollisions.Add(twoData);

            Assert.AreEqual(0, subject.ActiveCollisions.Count);
            subject.SetActiveCollisions(eventData);
            Assert.AreEqual(2, subject.ActiveCollisions.Count);

            Object.DestroyImmediate(oneContainer);
            Object.DestroyImmediate(twoContainer);
        }

        [Test]
        public void SetActiveCollisionsNoData()
        {
            Assert.AreEqual(0, subject.ActiveCollisions.Count);
            subject.SetActiveCollisions(null);
            Assert.AreEqual(0, subject.ActiveCollisions.Count);

            ActiveCollisionsContainer.EventData eventData = new ActiveCollisionsContainer.EventData();
            eventData.activeCollisions = null;

            subject.SetActiveCollisions(eventData);
            Assert.AreEqual(0, subject.ActiveCollisions.Count);
        }

        [Test]
        public void SetActiveCollisionsInactiveGameObject()
        {
            ActiveCollisionsContainer.EventData eventData = new ActiveCollisionsContainer.EventData();
            GameObject oneContainer;
            CollisionNotifier.EventData oneData = CollisionNotifierHelper.GetEventData(out oneContainer);
            GameObject twoContainer;
            CollisionNotifier.EventData twoData = CollisionNotifierHelper.GetEventData(out twoContainer);
            eventData.activeCollisions.Add(oneData);
            eventData.activeCollisions.Add(twoData);

            Assert.AreEqual(0, subject.ActiveCollisions.Count);
            subject.gameObject.SetActive(false);
            subject.SetActiveCollisions(eventData);
            Assert.AreEqual(0, subject.ActiveCollisions.Count);

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
            eventData.activeCollisions.Add(oneData);
            eventData.activeCollisions.Add(twoData);

            Assert.AreEqual(0, subject.ActiveCollisions.Count);
            subject.enabled = false;
            subject.SetActiveCollisions(eventData);
            Assert.AreEqual(0, subject.ActiveCollisions.Count);

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
            eventData.activeCollisions.Add(oneData);
            eventData.activeCollisions.Add(twoData);
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
            eventData.activeCollisions.Add(oneData);
            eventData.activeCollisions.Add(twoData);
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
            eventData.activeCollisions.Add(oneData);
            eventData.activeCollisions.Add(twoData);
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
            eventData.activeCollisions.Add(oneData);
            eventData.activeCollisions.Add(twoData);
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
            eventData.activeCollisions.Add(oneData);
            eventData.activeCollisions.Add(twoData);
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
            eventData.activeCollisions.Add(oneData);
            eventData.activeCollisions.Add(twoData);
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
        public bool received = false;

        public override void Consume(ActiveCollisionPublisher publisher, CollisionNotifier.EventData currentCollision)
        {
            received = true;
        }
    }
}