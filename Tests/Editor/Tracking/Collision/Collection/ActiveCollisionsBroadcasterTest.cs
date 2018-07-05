using VRTK.Core.Tracking.Collision;
using VRTK.Core.Tracking.Collision.Collection;

namespace Test.VRTK.Core.Tracking.Collision.Collection
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.VRTK.Core.Utility.Helper;

    public class ActiveCollisionsBroadcasterTest
    {
        private GameObject containingObject;
        private ActiveCollisionsBroadcaster subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<ActiveCollisionsBroadcaster>();
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
        public void Broadcast()
        {
            ActiveCollisionsContainer.EventData eventData = new ActiveCollisionsContainer.EventData();
            GameObject oneContainer;
            CollisionNotifier.EventData oneData = CollisionNotifierHelper.GetEventData(out oneContainer);
            ActiveCollisionsBroadcastReceiverMock oneReceiver = oneContainer.AddComponent<ActiveCollisionsBroadcastReceiverMock>();
            GameObject twoContainer;
            CollisionNotifier.EventData twoData = CollisionNotifierHelper.GetEventData(out twoContainer);
            ActiveCollisionsBroadcastReceiverMock twoReceiver = twoContainer.AddComponent<ActiveCollisionsBroadcastReceiverMock>();
            eventData.activeCollisions.Add(oneData);
            eventData.activeCollisions.Add(twoData);
            subject.SetActiveCollisions(eventData);

            Assert.IsFalse(oneReceiver.received);
            Assert.IsFalse(twoReceiver.received);

            subject.Broadcast();

            Assert.IsTrue(oneReceiver.received);
            Assert.IsTrue(twoReceiver.received);

            Object.DestroyImmediate(oneContainer);
            Object.DestroyImmediate(twoContainer);
        }

        [Test]
        public void BroadcastMultipleReceivers()
        {
            ActiveCollisionsContainer.EventData eventData = new ActiveCollisionsContainer.EventData();
            GameObject oneContainer;
            CollisionNotifier.EventData oneData = CollisionNotifierHelper.GetEventData(out oneContainer);
            ActiveCollisionsBroadcastReceiverMock oneReceiver = oneContainer.AddComponent<ActiveCollisionsBroadcastReceiverMock>();
            GameObject twoContainer;
            CollisionNotifier.EventData twoData = CollisionNotifierHelper.GetEventData(out twoContainer);
            ActiveCollisionsBroadcastReceiverMock twoReceiver = twoContainer.AddComponent<ActiveCollisionsBroadcastReceiverMock>();
            ActiveCollisionsBroadcastReceiverMock threeReceiver = twoContainer.AddComponent<ActiveCollisionsBroadcastReceiverMock>();
            eventData.activeCollisions.Add(oneData);
            eventData.activeCollisions.Add(twoData);
            subject.SetActiveCollisions(eventData);

            Assert.IsFalse(oneReceiver.received);
            Assert.IsFalse(twoReceiver.received);
            Assert.IsFalse(threeReceiver.received);

            subject.Broadcast();

            Assert.IsTrue(oneReceiver.received);
            Assert.IsTrue(twoReceiver.received);
            Assert.IsTrue(threeReceiver.received);

            Object.DestroyImmediate(oneContainer);
            Object.DestroyImmediate(twoContainer);
        }

        [Test]
        public void BroadcastInactiveGameObject()
        {
            ActiveCollisionsContainer.EventData eventData = new ActiveCollisionsContainer.EventData();
            GameObject oneContainer;
            CollisionNotifier.EventData oneData = CollisionNotifierHelper.GetEventData(out oneContainer);
            ActiveCollisionsBroadcastReceiverMock oneReceiver = oneContainer.AddComponent<ActiveCollisionsBroadcastReceiverMock>();
            GameObject twoContainer;
            CollisionNotifier.EventData twoData = CollisionNotifierHelper.GetEventData(out twoContainer);
            ActiveCollisionsBroadcastReceiverMock twoReceiver = twoContainer.AddComponent<ActiveCollisionsBroadcastReceiverMock>();
            eventData.activeCollisions.Add(oneData);
            eventData.activeCollisions.Add(twoData);
            subject.SetActiveCollisions(eventData);

            Assert.IsFalse(oneReceiver.received);
            Assert.IsFalse(twoReceiver.received);

            subject.gameObject.SetActive(false);
            subject.Broadcast();

            Assert.IsFalse(oneReceiver.received);
            Assert.IsFalse(twoReceiver.received);

            Object.DestroyImmediate(oneContainer);
            Object.DestroyImmediate(twoContainer);
        }

        [Test]
        public void BroadcastInactiveComponent()
        {
            ActiveCollisionsContainer.EventData eventData = new ActiveCollisionsContainer.EventData();
            GameObject oneContainer;
            CollisionNotifier.EventData oneData = CollisionNotifierHelper.GetEventData(out oneContainer);
            ActiveCollisionsBroadcastReceiverMock oneReceiver = oneContainer.AddComponent<ActiveCollisionsBroadcastReceiverMock>();
            GameObject twoContainer;
            CollisionNotifier.EventData twoData = CollisionNotifierHelper.GetEventData(out twoContainer);
            ActiveCollisionsBroadcastReceiverMock twoReceiver = twoContainer.AddComponent<ActiveCollisionsBroadcastReceiverMock>();
            eventData.activeCollisions.Add(oneData);
            eventData.activeCollisions.Add(twoData);
            subject.SetActiveCollisions(eventData);

            Assert.IsFalse(oneReceiver.received);
            Assert.IsFalse(twoReceiver.received);

            subject.enabled = false;
            subject.Broadcast();

            Assert.IsFalse(oneReceiver.received);
            Assert.IsFalse(twoReceiver.received);

            Object.DestroyImmediate(oneContainer);
            Object.DestroyImmediate(twoContainer);
        }

        [Test]
        public void BroadcastInactiveReceiverGameObject()
        {
            ActiveCollisionsContainer.EventData eventData = new ActiveCollisionsContainer.EventData();
            GameObject oneContainer;
            CollisionNotifier.EventData oneData = CollisionNotifierHelper.GetEventData(out oneContainer);
            ActiveCollisionsBroadcastReceiverMock oneReceiver = oneContainer.AddComponent<ActiveCollisionsBroadcastReceiverMock>();
            GameObject twoContainer;
            CollisionNotifier.EventData twoData = CollisionNotifierHelper.GetEventData(out twoContainer);
            ActiveCollisionsBroadcastReceiverMock twoReceiver = twoContainer.AddComponent<ActiveCollisionsBroadcastReceiverMock>();
            twoContainer.SetActive(false);
            eventData.activeCollisions.Add(oneData);
            eventData.activeCollisions.Add(twoData);
            subject.SetActiveCollisions(eventData);

            Assert.IsFalse(oneReceiver.received);
            Assert.IsFalse(twoReceiver.received);

            subject.Broadcast();

            Assert.IsTrue(oneReceiver.received);
            Assert.IsFalse(twoReceiver.received);

            Object.DestroyImmediate(oneContainer);
            Object.DestroyImmediate(twoContainer);
        }

        [Test]
        public void BroadcastInactiveReceiverComponent()
        {
            ActiveCollisionsContainer.EventData eventData = new ActiveCollisionsContainer.EventData();
            GameObject oneContainer;
            CollisionNotifier.EventData oneData = CollisionNotifierHelper.GetEventData(out oneContainer);
            ActiveCollisionsBroadcastReceiverMock oneReceiver = oneContainer.AddComponent<ActiveCollisionsBroadcastReceiverMock>();
            GameObject twoContainer;
            CollisionNotifier.EventData twoData = CollisionNotifierHelper.GetEventData(out twoContainer);
            ActiveCollisionsBroadcastReceiverMock twoReceiver = twoContainer.AddComponent<ActiveCollisionsBroadcastReceiverMock>();
            twoReceiver.enabled = false;
            eventData.activeCollisions.Add(oneData);
            eventData.activeCollisions.Add(twoData);
            subject.SetActiveCollisions(eventData);

            Assert.IsFalse(oneReceiver.received);
            Assert.IsFalse(twoReceiver.received);

            subject.Broadcast();

            Assert.IsTrue(oneReceiver.received);
            Assert.IsFalse(twoReceiver.received);

            Object.DestroyImmediate(oneContainer);
            Object.DestroyImmediate(twoContainer);
        }
    }

    public class ActiveCollisionsBroadcastReceiverMock : ActiveCollisionsBroadcastReceiver
    {
        public bool received = false;

        public override void Receive(ActiveCollisionsBroadcaster broadcaster, CollisionNotifier.EventData currentCollision)
        {
            received = true;
        }
    }
}
