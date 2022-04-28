using Zinnia.Tracking.Collision.Active;

namespace Test.Zinnia.Tracking.Collision.Active
{
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using Assert = UnityEngine.Assertions.Assert;

    public class ActiveCollisionRegisteredConsumerContainerTest
    {
        private GameObject containingObject;
        private ActiveCollisionRegisteredConsumerContainerExtendedMock subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<ActiveCollisionRegisteredConsumerContainerExtendedMock>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void RegisterAndPublish()
        {
            UnityEventListenerMock registeredMock = new UnityEventListenerMock();
            UnityEventListenerMock publishedMock = new UnityEventListenerMock();

            subject.Registered.AddListener(registeredMock.Listen);
            subject.Published.AddListener(publishedMock.Listen);

            ActiveCollisionConsumerMock oneConsumer = containingObject.AddComponent<ActiveCollisionConsumerMock>();

            Assert.IsFalse(registeredMock.Received);
            Assert.AreEqual(0, subject.RegisteredConsumers.Count);

            subject.Register(oneConsumer, null);

            Assert.IsTrue(registeredMock.Received);
            Assert.AreEqual(1, subject.RegisteredConsumers.Count);

            registeredMock.Reset();

            ActiveCollisionConsumerMock twoConsumer = containingObject.AddComponent<ActiveCollisionConsumerMock>();

            subject.Register(twoConsumer, null);

            Assert.IsTrue(registeredMock.Received);
            Assert.AreEqual(2, subject.RegisteredConsumers.Count);

            registeredMock.Reset();

            Assert.IsFalse(oneConsumer.received);
            Assert.IsFalse(twoConsumer.received);
            Assert.IsFalse(publishedMock.Received);

            subject.Publish();

            Assert.IsTrue(publishedMock.Received);
            Assert.IsTrue(oneConsumer.received);
            Assert.IsTrue(twoConsumer.received);
            Assert.AreEqual("{ Consumer = New Game Object (Test.Zinnia.Tracking.Collision.Active.ActiveCollisionConsumerMock) | Payload = [null] }", subject.GetEventData().ToString());
        }

        [Test]
        public void RegisterAndPublishWithIgnored()
        {
            UnityEventListenerMock registeredMock = new UnityEventListenerMock();
            UnityEventListenerMock publishedMock = new UnityEventListenerMock();

            subject.Registered.AddListener(registeredMock.Listen);
            subject.Published.AddListener(publishedMock.Listen);

            ActiveCollisionConsumerMock oneConsumer = containingObject.AddComponent<ActiveCollisionConsumerMock>();

            Assert.IsFalse(registeredMock.Received);
            Assert.AreEqual(0, subject.RegisteredConsumers.Count);

            subject.Register(oneConsumer, null);

            Assert.IsTrue(registeredMock.Received);
            Assert.AreEqual(1, subject.RegisteredConsumers.Count);

            registeredMock.Reset();

            ActiveCollisionConsumerMock twoConsumer = containingObject.AddComponent<ActiveCollisionConsumerMock>();

            subject.Register(twoConsumer, null);

            Assert.IsTrue(registeredMock.Received);
            Assert.AreEqual(2, subject.RegisteredConsumers.Count);

            registeredMock.Reset();

            Assert.AreEqual(0, subject.IgnoredRegisteredConsumers.Count);

            subject.IgnoredRegisteredConsumers.Add(twoConsumer);

            Assert.AreEqual(1, subject.IgnoredRegisteredConsumers.Count);

            Assert.IsFalse(oneConsumer.received);
            Assert.IsFalse(twoConsumer.received);
            Assert.IsFalse(publishedMock.Received);

            subject.Publish();

            Assert.IsTrue(publishedMock.Received);
            Assert.IsTrue(oneConsumer.received);
            Assert.IsFalse(twoConsumer.received);

            Assert.AreEqual(0, subject.IgnoredRegisteredConsumers.Count);
        }

        [Test]
        public void RegisterUnregister()
        {
            UnityEventListenerMock registeredMock = new UnityEventListenerMock();
            UnityEventListenerMock unregisteredMock = new UnityEventListenerMock();

            subject.Registered.AddListener(registeredMock.Listen);
            subject.Unregistered.AddListener(unregisteredMock.Listen);

            ActiveCollisionConsumerMock oneConsumer = containingObject.AddComponent<ActiveCollisionConsumerMock>();

            Assert.IsFalse(registeredMock.Received);
            Assert.IsFalse(unregisteredMock.Received);
            Assert.AreEqual(0, subject.RegisteredConsumers.Count);

            subject.Register(oneConsumer, null);

            Assert.IsTrue(registeredMock.Received);
            Assert.IsFalse(unregisteredMock.Received);
            Assert.AreEqual(1, subject.RegisteredConsumers.Count);

            registeredMock.Reset();
            unregisteredMock.Reset();

            ActiveCollisionConsumerMock twoConsumer = containingObject.AddComponent<ActiveCollisionConsumerMock>();

            subject.Register(twoConsumer, null);

            Assert.IsTrue(registeredMock.Received);
            Assert.IsFalse(unregisteredMock.Received);
            Assert.AreEqual(2, subject.RegisteredConsumers.Count);

            registeredMock.Reset();
            unregisteredMock.Reset();

            subject.Unregister(oneConsumer);

            Assert.IsFalse(registeredMock.Received);
            Assert.IsTrue(unregisteredMock.Received);
            Assert.AreEqual(1, subject.RegisteredConsumers.Count);

            registeredMock.Reset();
            unregisteredMock.Reset();

            subject.IgnoredRegisteredConsumers.Add(twoConsumer);

            Assert.AreEqual(1, subject.IgnoredRegisteredConsumers.Count);

            subject.Unregister(twoConsumer);

            Assert.IsFalse(registeredMock.Received);
            Assert.IsTrue(unregisteredMock.Received);
            Assert.AreEqual(0, subject.RegisteredConsumers.Count);
            Assert.AreEqual(0, subject.IgnoredRegisteredConsumers.Count);
        }

        [Test]
        public void RegisterUnregisterOnConsumerContainer()
        {
            UnityEventListenerMock registeredMock = new UnityEventListenerMock();
            UnityEventListenerMock unregisteredMock = new UnityEventListenerMock();

            subject.Registered.AddListener(registeredMock.Listen);
            subject.Unregistered.AddListener(unregisteredMock.Listen);

            GameObject containerOne = new GameObject();
            GameObject containerTwo = new GameObject();

            ActiveCollisionConsumerMock oneConsumerA = containerOne.AddComponent<ActiveCollisionConsumerMock>();
            ActiveCollisionConsumerMock oneConsumerB = containerOne.AddComponent<ActiveCollisionConsumerMock>();
            ActiveCollisionConsumerMock twoConsumer = containerTwo.AddComponent<ActiveCollisionConsumerMock>();

            oneConsumerA.SetConsumerContainer(containerOne);
            oneConsumerB.SetConsumerContainer(containerOne);
            twoConsumer.SetConsumerContainer(containerTwo);

            subject.Register(oneConsumerA, null);
            subject.Register(oneConsumerB, null);
            subject.Register(twoConsumer, null);

            Assert.IsFalse(unregisteredMock.Received);
            Assert.AreEqual(3, subject.RegisteredConsumers.Count);

            registeredMock.Reset();
            unregisteredMock.Reset();

            subject.UnregisterConsumersOnContainer(containerOne);

            Assert.IsFalse(registeredMock.Received);
            Assert.IsTrue(unregisteredMock.Received);
            Assert.AreEqual(1, subject.RegisteredConsumers.Count);

            Object.DestroyImmediate(containerOne);
            Object.DestroyImmediate(containerTwo);
        }

        [Test]
        public void ClearIgnoredRegisteredConsumers()
        {
            Assert.AreEqual(0, subject.IgnoredRegisteredConsumers.Count);
            subject.IgnoredRegisteredConsumers.Add(null);
            Assert.AreEqual(1, subject.IgnoredRegisteredConsumers.Count);
            subject.ClearIgnoredRegisteredConsumers();
            Assert.AreEqual(0, subject.IgnoredRegisteredConsumers.Count);
        }

        [Test]
        public void RegisterInactiveGameObject()
        {
            UnityEventListenerMock registeredMock = new UnityEventListenerMock();
            UnityEventListenerMock publishedMock = new UnityEventListenerMock();

            subject.Registered.AddListener(registeredMock.Listen);
            subject.Published.AddListener(publishedMock.Listen);

            subject.gameObject.SetActive(false);

            ActiveCollisionConsumerMock oneConsumer = containingObject.AddComponent<ActiveCollisionConsumerMock>();

            Assert.IsFalse(registeredMock.Received);
            Assert.AreEqual(0, subject.RegisteredConsumers.Count);

            subject.Register(oneConsumer, null);

            Assert.IsFalse(registeredMock.Received);
            Assert.AreEqual(0, subject.RegisteredConsumers.Count);
        }

        [Test]
        public void RegisterInactiveComponent()
        {
            UnityEventListenerMock registeredMock = new UnityEventListenerMock();
            UnityEventListenerMock publishedMock = new UnityEventListenerMock();

            subject.Registered.AddListener(registeredMock.Listen);
            subject.Published.AddListener(publishedMock.Listen);

            subject.enabled = false;

            ActiveCollisionConsumerMock oneConsumer = containingObject.AddComponent<ActiveCollisionConsumerMock>();

            Assert.IsFalse(registeredMock.Received);
            Assert.AreEqual(0, subject.RegisteredConsumers.Count);

            subject.Register(oneConsumer, null);

            Assert.IsFalse(registeredMock.Received);
            Assert.AreEqual(0, subject.RegisteredConsumers.Count);
        }

        [Test]
        public void PublishInactiveGameObject()
        {
            UnityEventListenerMock registeredMock = new UnityEventListenerMock();
            UnityEventListenerMock publishedMock = new UnityEventListenerMock();

            subject.Registered.AddListener(registeredMock.Listen);
            subject.Published.AddListener(publishedMock.Listen);


            ActiveCollisionConsumerMock oneConsumer = containingObject.AddComponent<ActiveCollisionConsumerMock>();

            Assert.IsFalse(registeredMock.Received);
            Assert.AreEqual(0, subject.RegisteredConsumers.Count);

            subject.Register(oneConsumer, null);

            Assert.IsTrue(registeredMock.Received);
            Assert.AreEqual(1, subject.RegisteredConsumers.Count);

            registeredMock.Reset();

            Assert.IsFalse(oneConsumer.received);
            Assert.IsFalse(publishedMock.Received);

            subject.gameObject.SetActive(false);
            subject.Publish();

            Assert.IsFalse(publishedMock.Received);
            Assert.IsFalse(oneConsumer.received);
        }

        [Test]
        public void PublishInactiveComponent()
        {
            UnityEventListenerMock registeredMock = new UnityEventListenerMock();
            UnityEventListenerMock publishedMock = new UnityEventListenerMock();

            subject.Registered.AddListener(registeredMock.Listen);
            subject.Published.AddListener(publishedMock.Listen);


            ActiveCollisionConsumerMock oneConsumer = containingObject.AddComponent<ActiveCollisionConsumerMock>();

            Assert.IsFalse(registeredMock.Received);
            Assert.AreEqual(0, subject.RegisteredConsumers.Count);

            subject.Register(oneConsumer, null);

            Assert.IsTrue(registeredMock.Received);
            Assert.AreEqual(1, subject.RegisteredConsumers.Count);

            registeredMock.Reset();

            Assert.IsFalse(oneConsumer.received);
            Assert.IsFalse(publishedMock.Received);

            subject.enabled = false;
            subject.Publish();

            Assert.IsFalse(publishedMock.Received);
            Assert.IsFalse(oneConsumer.received);
        }

        private class ActiveCollisionRegisteredConsumerContainerExtendedMock : ActiveCollisionRegisteredConsumerContainer
        {
            public EventData GetEventData()
            {
                return eventData;
            }
        }
    }
}