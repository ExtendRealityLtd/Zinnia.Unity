using VRTK.Core.Tracking.Collision.Collection;
using VRTK.Core.Utility;

namespace Test.VRTK.Core.Tracking.Collision.Collection
{
    using UnityEngine;
    using NUnit.Framework;
    using System.Collections.Generic;
    using Test.VRTK.Core.Utility.Mock;
    using Test.VRTK.Core.Utility.Stub;

    public class ActiveCollisionsBroadcastReceiverTest
    {
        private GameObject containingObject;
        private ActiveCollisionsBroadcastReceiver subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<ActiveCollisionsBroadcastReceiver>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void ReceiveWithCollisionInitiator()
        {
            UnityEventListenerMock collisionInitiatorSetMock = new UnityEventListenerMock();
            UnityEventListenerMock collisionInitiatorUnsetMock = new UnityEventListenerMock();
            UnityEventListenerMock clearedMock = new UnityEventListenerMock();

            subject.CollisionInitiatorSet.AddListener(collisionInitiatorSetMock.Listen);
            subject.CollisionInitiatorUnset.AddListener(collisionInitiatorUnsetMock.Listen);
            subject.Cleared.AddListener(clearedMock.Listen);

            GameObject broadcasterObject = new GameObject();
            ActiveCollisionsBroadcaster broadcaster = broadcasterObject.AddComponent<ActiveCollisionsBroadcaster>();
            broadcaster.collisionInitiator = broadcasterObject;

            Assert.IsFalse(collisionInitiatorSetMock.Received);
            Assert.IsFalse(collisionInitiatorUnsetMock.Received);
            Assert.IsFalse(clearedMock.Received);

            Assert.IsNull(subject.BroadcastSource);

            subject.Receive(broadcaster, null);

            Assert.IsTrue(collisionInitiatorSetMock.Received);
            Assert.IsFalse(collisionInitiatorUnsetMock.Received);
            Assert.IsFalse(clearedMock.Received);

            Assert.AreEqual(broadcaster, subject.BroadcastSource);

            Object.DestroyImmediate(broadcasterObject);
        }

        [Test]
        public void ReceiveWithoutCollisionInitiator()
        {
            UnityEventListenerMock collisionInitiatorSetMock = new UnityEventListenerMock();
            UnityEventListenerMock collisionInitiatorUnsetMock = new UnityEventListenerMock();
            UnityEventListenerMock clearedMock = new UnityEventListenerMock();

            subject.CollisionInitiatorSet.AddListener(collisionInitiatorSetMock.Listen);
            subject.CollisionInitiatorUnset.AddListener(collisionInitiatorUnsetMock.Listen);
            subject.Cleared.AddListener(clearedMock.Listen);

            GameObject broadcasterObject = new GameObject();
            ActiveCollisionsBroadcaster broadcaster = broadcasterObject.AddComponent<ActiveCollisionsBroadcaster>();
            broadcaster.collisionInitiator = null;

            Assert.IsFalse(collisionInitiatorSetMock.Received);
            Assert.IsFalse(collisionInitiatorUnsetMock.Received);
            Assert.IsFalse(clearedMock.Received);

            Assert.IsNull(subject.BroadcastSource);

            subject.Receive(broadcaster, null);

            Assert.IsFalse(collisionInitiatorSetMock.Received);
            Assert.IsTrue(collisionInitiatorUnsetMock.Received);
            Assert.IsTrue(clearedMock.Received);

            Assert.IsNull(subject.BroadcastSource);

            Object.DestroyImmediate(broadcasterObject);
        }

        [Test]
        public void ReceiveExclusion()
        {
            UnityEventListenerMock collisionInitiatorSetMock = new UnityEventListenerMock();
            UnityEventListenerMock collisionInitiatorUnsetMock = new UnityEventListenerMock();
            UnityEventListenerMock clearedMock = new UnityEventListenerMock();

            subject.CollisionInitiatorSet.AddListener(collisionInitiatorSetMock.Listen);
            subject.CollisionInitiatorUnset.AddListener(collisionInitiatorUnsetMock.Listen);
            subject.Cleared.AddListener(clearedMock.Listen);

            GameObject broadcasterObject = new GameObject();
            ActiveCollisionsBroadcaster broadcaster = broadcasterObject.AddComponent<ActiveCollisionsBroadcaster>();
            broadcaster.collisionInitiator = broadcasterObject;

            broadcasterObject.AddComponent<ExclusionRuleStub>();
            ExclusionRule exclusions = containingObject.AddComponent<ExclusionRule>();
            exclusions.checkType = ExclusionRule.CheckTypes.Script;
            exclusions.identifiers = new List<string>() { "ExclusionRuleStub" };
            subject.broadcasterValidity = exclusions;

            Assert.IsFalse(collisionInitiatorSetMock.Received);
            Assert.IsFalse(collisionInitiatorUnsetMock.Received);
            Assert.IsFalse(clearedMock.Received);

            Assert.IsNull(subject.BroadcastSource);

            subject.Receive(broadcaster, null);

            Assert.IsFalse(collisionInitiatorSetMock.Received);
            Assert.IsFalse(collisionInitiatorUnsetMock.Received);
            Assert.IsFalse(clearedMock.Received);

            Assert.IsNull(subject.BroadcastSource);

            Object.DestroyImmediate(broadcasterObject);
        }

        [Test]
        public void ReceiveInactiveGameObject()
        {
            UnityEventListenerMock collisionInitiatorSetMock = new UnityEventListenerMock();
            UnityEventListenerMock collisionInitiatorUnsetMock = new UnityEventListenerMock();
            UnityEventListenerMock clearedMock = new UnityEventListenerMock();

            subject.CollisionInitiatorSet.AddListener(collisionInitiatorSetMock.Listen);
            subject.CollisionInitiatorUnset.AddListener(collisionInitiatorUnsetMock.Listen);
            subject.Cleared.AddListener(clearedMock.Listen);

            GameObject broadcasterObject = new GameObject();
            ActiveCollisionsBroadcaster broadcaster = broadcasterObject.AddComponent<ActiveCollisionsBroadcaster>();
            broadcaster.collisionInitiator = broadcasterObject;

            subject.gameObject.SetActive(false);
            subject.Receive(broadcaster, null);

            Assert.IsFalse(collisionInitiatorSetMock.Received);
            Assert.IsFalse(collisionInitiatorUnsetMock.Received);
            Assert.IsFalse(clearedMock.Received);

            Assert.IsNull(subject.BroadcastSource);

            Object.DestroyImmediate(broadcasterObject);
        }

        [Test]
        public void ReceiveInactiveComponent()
        {
            UnityEventListenerMock collisionInitiatorSetMock = new UnityEventListenerMock();
            UnityEventListenerMock collisionInitiatorUnsetMock = new UnityEventListenerMock();
            UnityEventListenerMock clearedMock = new UnityEventListenerMock();

            subject.CollisionInitiatorSet.AddListener(collisionInitiatorSetMock.Listen);
            subject.CollisionInitiatorUnset.AddListener(collisionInitiatorUnsetMock.Listen);
            subject.Cleared.AddListener(clearedMock.Listen);

            GameObject broadcasterObject = new GameObject();
            ActiveCollisionsBroadcaster broadcaster = broadcasterObject.AddComponent<ActiveCollisionsBroadcaster>();
            broadcaster.collisionInitiator = broadcasterObject;

            subject.enabled = false;
            subject.Receive(broadcaster, null);

            Assert.IsFalse(collisionInitiatorSetMock.Received);
            Assert.IsFalse(collisionInitiatorUnsetMock.Received);
            Assert.IsFalse(clearedMock.Received);

            Assert.IsNull(subject.BroadcastSource);

            Object.DestroyImmediate(broadcasterObject);
        }

        [Test]
        public void Clear()
        {
            UnityEventListenerMock collisionInitiatorSetMock = new UnityEventListenerMock();
            UnityEventListenerMock collisionInitiatorUnsetMock = new UnityEventListenerMock();
            UnityEventListenerMock clearedMock = new UnityEventListenerMock();

            subject.CollisionInitiatorSet.AddListener(collisionInitiatorSetMock.Listen);
            subject.CollisionInitiatorUnset.AddListener(collisionInitiatorUnsetMock.Listen);
            subject.Cleared.AddListener(clearedMock.Listen);

            subject.Clear();

            Assert.IsFalse(collisionInitiatorSetMock.Received);
            Assert.IsTrue(collisionInitiatorUnsetMock.Received);
            Assert.IsTrue(clearedMock.Received);

            Assert.IsNull(subject.BroadcastSource);
        }

        [Test]
        public void ClearInactiveGameObject()
        {
            UnityEventListenerMock collisionInitiatorSetMock = new UnityEventListenerMock();
            UnityEventListenerMock collisionInitiatorUnsetMock = new UnityEventListenerMock();
            UnityEventListenerMock clearedMock = new UnityEventListenerMock();

            subject.CollisionInitiatorSet.AddListener(collisionInitiatorSetMock.Listen);
            subject.CollisionInitiatorUnset.AddListener(collisionInitiatorUnsetMock.Listen);
            subject.Cleared.AddListener(clearedMock.Listen);

            subject.gameObject.SetActive(false);
            subject.Clear();

            Assert.IsFalse(collisionInitiatorSetMock.Received);
            Assert.IsFalse(collisionInitiatorUnsetMock.Received);
            Assert.IsFalse(clearedMock.Received);

            Assert.IsNull(subject.BroadcastSource);
        }

        [Test]
        public void ClearInactiveComponent()
        {
            UnityEventListenerMock collisionInitiatorSetMock = new UnityEventListenerMock();
            UnityEventListenerMock collisionInitiatorUnsetMock = new UnityEventListenerMock();
            UnityEventListenerMock clearedMock = new UnityEventListenerMock();

            subject.CollisionInitiatorSet.AddListener(collisionInitiatorSetMock.Listen);
            subject.CollisionInitiatorUnset.AddListener(collisionInitiatorUnsetMock.Listen);
            subject.Cleared.AddListener(clearedMock.Listen);

            subject.enabled = false;
            subject.Clear();

            Assert.IsFalse(collisionInitiatorSetMock.Received);
            Assert.IsFalse(collisionInitiatorUnsetMock.Received);
            Assert.IsFalse(clearedMock.Received);

            Assert.IsNull(subject.BroadcastSource);
        }
    }
}