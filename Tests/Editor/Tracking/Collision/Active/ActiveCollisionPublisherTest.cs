using Zinnia.Tracking.Collision;
using Zinnia.Tracking.Collision.Active;

namespace Test.Zinnia.Tracking.Collision.Active
{
    using NUnit.Framework;
    using Test.Zinnia.Utility.Helper;
    using UnityEngine;
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
            Assert.AreEqual("{ SourceContainer = [null] | PublisherContainer = New Game Object (UnityEngine.GameObject) }", subject.Payload.ToString());

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
        public void PublishWithRegisteredConsumerContainer()
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

            ActiveCollisionRegisteredConsumerContainerMock reigsteredConsumerContainer = containingObject.AddComponent<ActiveCollisionRegisteredConsumerContainerMock>();
            subject.RegisteredConsumerContainer = reigsteredConsumerContainer;

            Assert.AreEqual(0, reigsteredConsumerContainer.ConsumerCount);
            Assert.AreEqual(0, reigsteredConsumerContainer.IgnoredConsumerCount);

            subject.Publish();

            Assert.AreEqual(2, reigsteredConsumerContainer.ConsumerCount);
            Assert.AreEqual(2, reigsteredConsumerContainer.IgnoredConsumerCount);

            subject.UnregisterRegisteredConsumer(twoConsumer);

            Assert.AreEqual(1, reigsteredConsumerContainer.ConsumerCount);
            Assert.AreEqual(1, reigsteredConsumerContainer.IgnoredConsumerCount);

            subject.UnregisterRegisteredConsumer(oneConsumer);

            Assert.AreEqual(0, reigsteredConsumerContainer.ConsumerCount);
            Assert.AreEqual(0, reigsteredConsumerContainer.IgnoredConsumerCount);

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

        [Test]
        public void ClearPayload()
        {
            ActiveCollisionPublisher.PayloadData payload = new ActiveCollisionPublisher.PayloadData();
            subject.Payload = payload;
            Assert.AreEqual(payload, subject.Payload);
            subject.ClearPayload();
            Assert.IsNull(subject.Payload);
        }

        [Test]
        public void ClearPayloadInactiveGameObject()
        {
            ActiveCollisionPublisher.PayloadData payload = new ActiveCollisionPublisher.PayloadData();
            subject.Payload = payload;
            Assert.AreEqual(payload, subject.Payload);
            subject.gameObject.SetActive(false);
            subject.ClearPayload();
            Assert.AreEqual(payload, subject.Payload);
        }

        [Test]
        public void ClearPayloadInactiveComponent()
        {
            ActiveCollisionPublisher.PayloadData payload = new ActiveCollisionPublisher.PayloadData();
            subject.Payload = payload;
            Assert.AreEqual(payload, subject.Payload);
            subject.enabled = false;
            subject.ClearPayload();
            Assert.AreEqual(payload, subject.Payload);
        }

        [Test]
        public void ClearRegisteredConsumerContainer()
        {
            Assert.IsNull(subject.RegisteredConsumerContainer);
            ActiveCollisionRegisteredConsumerContainerMock reigsteredConsumerContainer = containingObject.AddComponent<ActiveCollisionRegisteredConsumerContainerMock>();
            subject.RegisteredConsumerContainer = reigsteredConsumerContainer;
            Assert.AreEqual(reigsteredConsumerContainer, subject.RegisteredConsumerContainer);
            subject.ClearRegisteredConsumerContainer();
            Assert.IsNull(subject.RegisteredConsumerContainer);
        }

        [Test]
        public void ClearRegisteredConsumerContainerInactiveGameObject()
        {
            Assert.IsNull(subject.RegisteredConsumerContainer);
            ActiveCollisionRegisteredConsumerContainerMock reigsteredConsumerContainer = containingObject.AddComponent<ActiveCollisionRegisteredConsumerContainerMock>();
            subject.RegisteredConsumerContainer = reigsteredConsumerContainer;
            Assert.AreEqual(reigsteredConsumerContainer, subject.RegisteredConsumerContainer);
            subject.gameObject.SetActive(false);
            subject.ClearRegisteredConsumerContainer();
            Assert.AreEqual(reigsteredConsumerContainer, subject.RegisteredConsumerContainer);
        }

        [Test]
        public void ClearRegisteredConsumerContainerInactiveComponent()
        {
            Assert.IsNull(subject.RegisteredConsumerContainer);
            ActiveCollisionRegisteredConsumerContainerMock reigsteredConsumerContainer = containingObject.AddComponent<ActiveCollisionRegisteredConsumerContainerMock>();
            subject.RegisteredConsumerContainer = reigsteredConsumerContainer;
            Assert.AreEqual(reigsteredConsumerContainer, subject.RegisteredConsumerContainer);
            subject.enabled = false;
            subject.ClearRegisteredConsumerContainer();
            Assert.AreEqual(reigsteredConsumerContainer, subject.RegisteredConsumerContainer);
        }
    }

    public class ActiveCollisionRegisteredConsumerContainerMock : ActiveCollisionRegisteredConsumerContainer
    {
        public int ConsumerCount { get; set; }
        public int IgnoredConsumerCount => IgnoredRegisteredConsumers.Count;

        public override void Register(ActiveCollisionConsumer consumer, ActiveCollisionPublisher.PayloadData payload)
        {
            ConsumerCount++;
        }

        public override void Unregister(ActiveCollisionConsumer consumer)
        {
            ConsumerCount--;
            base.Unregister(consumer);
        }
    }
}