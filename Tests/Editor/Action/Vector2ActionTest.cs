namespace VRTK.Core.Action
{
    using UnityEngine;
    using NUnit.Framework;
    using VRTK.Core.Utility.Mock;

    public class Vector2ActionTest
    {
        private GameObject containingObject;
        private Vector2ActionMock subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<Vector2ActionMock>();
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

            subject.Receive(Vector2.one, null);

            Assert.IsTrue(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsTrue(changedListenerMock.Received);
        }

        [Test]
        public void DectivatedEmittedAtNonZero()
        {
            subject.SetState(true);
            subject.SetValue(Vector2.one);

            UnityEventListenerMock activatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock deactivatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock changedListenerMock = new UnityEventListenerMock();

            subject.Activated.AddListener(activatedListenerMock.Listen);
            subject.Deactivated.AddListener(deactivatedListenerMock.Listen);
            subject.Changed.AddListener(changedListenerMock.Listen);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);

            subject.Receive(Vector2.one, null);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsTrue(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);
        }

        [Test]
        public void DectivatedEmittedAtZero()
        {
            subject.SetState(true);
            subject.SetValue(Vector2.one);

            UnityEventListenerMock activatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock deactivatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock changedListenerMock = new UnityEventListenerMock();

            subject.Activated.AddListener(activatedListenerMock.Listen);
            subject.Deactivated.AddListener(deactivatedListenerMock.Listen);
            subject.Changed.AddListener(changedListenerMock.Listen);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);

            subject.Receive(Vector2.zero, null);

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

            subject.Receive(new Vector2(0.1f, 0.1f), null);
            Assert.IsTrue(changedListenerMock.Received);

            changedListenerMock.Reset();
            Assert.IsFalse(changedListenerMock.Received);

            subject.Receive(new Vector2(0.1f, 0.1f), null);
            Assert.IsFalse(changedListenerMock.Received);

            changedListenerMock.Reset();
            Assert.IsFalse(changedListenerMock.Received);

            subject.Receive(new Vector2(0.2f, 0.2f), null);
            Assert.IsTrue(changedListenerMock.Received);
        }
    }

    public class Vector2ActionMock : Vector2Action
    {
        public virtual void SetState(bool value)
        {
            State = value;
        }

        public virtual void SetValue(Vector2 value)
        {
            Value = value;
        }
    }
}