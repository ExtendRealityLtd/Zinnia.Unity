namespace VRTK.Core.Action
{
    using UnityEngine;
    using NUnit.Framework;
    using VRTK.Core.Utility.Mock;

    public class BooleanActionTest
    {
        private GameObject containingObject;
        private BooleanActionMock subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<BooleanActionMock>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void ActivatedEmitted()
        {
            UnityEventListenerMock activatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock deactivatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock changedListenerMock = new UnityEventListenerMock();

            subject.Activated.AddListener(activatedListenerMock.Listen);
            subject.Deactivated.AddListener(deactivatedListenerMock.Listen);
            subject.Changed.AddListener(changedListenerMock.Listen);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);

            subject.Receive(true, null);

            Assert.IsTrue(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsTrue(changedListenerMock.Received);
        }

        [Test]
        public void DectivatedEmitted()
        {
            subject.SetState(true);
            subject.SetValue(true);

            UnityEventListenerMock activatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock deactivatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock changedListenerMock = new UnityEventListenerMock();

            subject.Activated.AddListener(activatedListenerMock.Listen);
            subject.Deactivated.AddListener(deactivatedListenerMock.Listen);
            subject.Changed.AddListener(changedListenerMock.Listen);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);

            subject.Receive(false, null);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsTrue(deactivatedListenerMock.Received);
            Assert.IsTrue(changedListenerMock.Received);
        }

        [Test]
        public void ChangedEmitted()
        {
            UnityEventListenerMock changedListenerMock = new UnityEventListenerMock();

            subject.Changed.AddListener(changedListenerMock.Listen);

            Assert.IsFalse(changedListenerMock.Received);

            subject.Receive(true, null);
            Assert.IsTrue(changedListenerMock.Received);

            changedListenerMock.Reset();
            Assert.IsFalse(changedListenerMock.Received);

            subject.Receive(true, null);
            Assert.IsFalse(changedListenerMock.Received);

            changedListenerMock.Reset();
            Assert.IsFalse(changedListenerMock.Received);

            subject.Receive(false, null);
            Assert.IsTrue(changedListenerMock.Received);
        }

        [Test]
        public void EventsNotEmittedOnInactiveGameObject()
        {
            UnityEventListenerMock activatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock deactivatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock changedListenerMock = new UnityEventListenerMock();

            subject.Activated.AddListener(activatedListenerMock.Listen);
            subject.Deactivated.AddListener(deactivatedListenerMock.Listen);
            subject.Changed.AddListener(changedListenerMock.Listen);
            subject.gameObject.SetActive(false);
            subject.Receive(true, null);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);

            activatedListenerMock.Reset();
            deactivatedListenerMock.Reset();
            changedListenerMock.Reset();

            subject.Receive(false, null);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);
        }

        [Test]
        public void EventsNotEmittedOnDisabledComponent()
        {
            UnityEventListenerMock activatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock deactivatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock changedListenerMock = new UnityEventListenerMock();

            subject.Activated.AddListener(activatedListenerMock.Listen);
            subject.Deactivated.AddListener(deactivatedListenerMock.Listen);
            subject.Changed.AddListener(changedListenerMock.Listen);
            subject.enabled = false;
            subject.Receive(true, null);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);

            activatedListenerMock.Reset();
            deactivatedListenerMock.Reset();
            changedListenerMock.Reset();

            subject.Receive(false, null);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);
        }
    }

    public class BooleanActionMock : BooleanAction
    {
        public virtual void SetState(bool value)
        {
            State = value;
        }

        public virtual void SetValue(bool value)
        {
            Value = value;
        }
    }
}