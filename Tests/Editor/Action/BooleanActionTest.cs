using Zinnia.Action;

namespace Test.Zinnia.Action
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using Assert = UnityEngine.Assertions.Assert;

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
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void ActivatedEmitted()
        {
            UnityEventListenerMock activatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock deactivatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock changedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock unchangedListenerMock = new UnityEventListenerMock();

            subject.Activated.AddListener(activatedListenerMock.Listen);
            subject.Deactivated.AddListener(deactivatedListenerMock.Listen);
            subject.ValueChanged.AddListener(changedListenerMock.Listen);
            subject.ValueUnchanged.AddListener(unchangedListenerMock.Listen);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            subject.Receive(true);

            Assert.IsTrue(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsTrue(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);
        }

        [Test]
        public void DeactivatedEmitted()
        {
            subject.SetIsActivated(true);
            subject.SetValue(true);

            UnityEventListenerMock activatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock deactivatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock changedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock unchangedListenerMock = new UnityEventListenerMock();

            subject.Activated.AddListener(activatedListenerMock.Listen);
            subject.Deactivated.AddListener(deactivatedListenerMock.Listen);
            subject.ValueChanged.AddListener(changedListenerMock.Listen);
            subject.ValueUnchanged.AddListener(unchangedListenerMock.Listen);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            subject.Receive(false);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsTrue(deactivatedListenerMock.Received);
            Assert.IsTrue(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);
        }

        [Test]
        public void DeactivatedEmittedFromReceivingDefaultValue()
        {
            subject.SetIsActivated(true);
            subject.SetValue(true);

            UnityEventListenerMock activatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock deactivatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock changedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock unchangedListenerMock = new UnityEventListenerMock();

            subject.Activated.AddListener(activatedListenerMock.Listen);
            subject.Deactivated.AddListener(deactivatedListenerMock.Listen);
            subject.ValueChanged.AddListener(changedListenerMock.Listen);
            subject.ValueUnchanged.AddListener(unchangedListenerMock.Listen);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            subject.ReceiveDefaultValue();

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsTrue(deactivatedListenerMock.Received);
            Assert.IsTrue(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);
        }

        [Test]
        public void ChangedEmitted()
        {
            UnityEventListenerMock changedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock unchangedListenerMock = new UnityEventListenerMock();

            subject.ValueChanged.AddListener(changedListenerMock.Listen);
            subject.ValueUnchanged.AddListener(unchangedListenerMock.Listen);

            Assert.IsFalse(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            subject.Receive(true);
            Assert.IsTrue(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            changedListenerMock.Reset();
            unchangedListenerMock.Reset();
            Assert.IsFalse(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            subject.Receive(true);
            Assert.IsFalse(changedListenerMock.Received);
            Assert.IsTrue(unchangedListenerMock.Received);

            changedListenerMock.Reset();
            unchangedListenerMock.Reset();
            Assert.IsFalse(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            subject.Receive(false);
            Assert.IsTrue(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);
        }

        [Test]
        public void DefaultValueNotTypeDefault()
        {
            UnityEventListenerMock activatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock deactivatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock changedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock unchangedListenerMock = new UnityEventListenerMock();

            subject.Activated.AddListener(activatedListenerMock.Listen);
            subject.Deactivated.AddListener(deactivatedListenerMock.Listen);
            subject.ValueChanged.AddListener(changedListenerMock.Listen);
            subject.ValueUnchanged.AddListener(unchangedListenerMock.Listen);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            subject.DefaultValue = true;

            Assert.IsTrue(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsTrue(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            activatedListenerMock.Reset();
            deactivatedListenerMock.Reset();
            changedListenerMock.Reset();
            unchangedListenerMock.Reset();

            subject.Receive(true);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsTrue(deactivatedListenerMock.Received);
            Assert.IsTrue(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            activatedListenerMock.Reset();
            deactivatedListenerMock.Reset();
            changedListenerMock.Reset();
            unchangedListenerMock.Reset();

            subject.Receive(false);

            Assert.IsTrue(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsTrue(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            activatedListenerMock.Reset();
            deactivatedListenerMock.Reset();
            changedListenerMock.Reset();
            unchangedListenerMock.Reset();

            subject.Receive(true);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsTrue(deactivatedListenerMock.Received);
            Assert.IsTrue(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);
        }

        [Test]
        public void DefaultValueTypeDefault()
        {
            UnityEventListenerMock activatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock deactivatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock changedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock unchangedListenerMock = new UnityEventListenerMock();

            subject.Activated.AddListener(activatedListenerMock.Listen);
            subject.Deactivated.AddListener(deactivatedListenerMock.Listen);
            subject.ValueChanged.AddListener(changedListenerMock.Listen);
            subject.ValueUnchanged.AddListener(unchangedListenerMock.Listen);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            subject.DefaultValue = false;

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            activatedListenerMock.Reset();
            deactivatedListenerMock.Reset();
            changedListenerMock.Reset();
            unchangedListenerMock.Reset();

            subject.Receive(false);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);
            Assert.IsTrue(unchangedListenerMock.Received);

            activatedListenerMock.Reset();
            deactivatedListenerMock.Reset();
            changedListenerMock.Reset();
            unchangedListenerMock.Reset();

            subject.Receive(true);

            Assert.IsTrue(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsTrue(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            activatedListenerMock.Reset();
            deactivatedListenerMock.Reset();
            changedListenerMock.Reset();
            unchangedListenerMock.Reset();

            subject.Receive(false);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsTrue(deactivatedListenerMock.Received);
            Assert.IsTrue(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);
        }

        [Test]
        public void EventsNotEmittedOnInactiveGameObject()
        {
            UnityEventListenerMock activatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock deactivatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock changedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock unchangedListenerMock = new UnityEventListenerMock();

            subject.gameObject.SetActive(false);
            subject.Activated.AddListener(activatedListenerMock.Listen);
            subject.Deactivated.AddListener(deactivatedListenerMock.Listen);
            subject.ValueChanged.AddListener(changedListenerMock.Listen);
            subject.ValueUnchanged.AddListener(unchangedListenerMock.Listen);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            subject.Receive(true);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            activatedListenerMock.Reset();
            deactivatedListenerMock.Reset();
            changedListenerMock.Reset();
            unchangedListenerMock.Reset();

            subject.Receive(false);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);
        }

        [Test]
        public void EventsNotEmittedOnDisabledComponent()
        {
            UnityEventListenerMock activatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock deactivatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock changedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock unchangedListenerMock = new UnityEventListenerMock();

            subject.enabled = false;
            subject.Activated.AddListener(activatedListenerMock.Listen);
            subject.Deactivated.AddListener(deactivatedListenerMock.Listen);
            subject.ValueChanged.AddListener(changedListenerMock.Listen);
            subject.ValueUnchanged.AddListener(unchangedListenerMock.Listen);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            subject.Receive(true);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            activatedListenerMock.Reset();
            deactivatedListenerMock.Reset();
            changedListenerMock.Reset();
            unchangedListenerMock.Reset();

            subject.Receive(false);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);
        }

        [Test]
        public void AddSource()
        {
            UnityEventListenerMock activatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock deactivatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock changedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock unchangedListenerMock = new UnityEventListenerMock();

            subject.Activated.AddListener(activatedListenerMock.Listen);
            subject.Deactivated.AddListener(deactivatedListenerMock.Listen);
            subject.ValueChanged.AddListener(changedListenerMock.Listen);
            subject.ValueUnchanged.AddListener(unchangedListenerMock.Listen);

            GameObject sourceObject = new GameObject();
            BooleanActionMock sourceMock = sourceObject.AddComponent<BooleanActionMock>();

            Assert.AreEqual(0, subject.ReadOnlySources.Count);

            sourceMock.Receive(true);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            sourceMock.SetIsActivated(false);
            sourceMock.SetValue(false);

            subject.AddSource(sourceMock);

            Assert.AreEqual(1, subject.ReadOnlySources.Count);

            sourceMock.Receive(true);

            Assert.IsTrue(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsTrue(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            Object.DestroyImmediate(sourceObject);
        }

        [Test]
        public void RemoveSource()
        {
            UnityEventListenerMock activatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock deactivatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock changedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock unchangedListenerMock = new UnityEventListenerMock();

            subject.Activated.AddListener(activatedListenerMock.Listen);
            subject.Deactivated.AddListener(deactivatedListenerMock.Listen);
            subject.ValueChanged.AddListener(changedListenerMock.Listen);
            subject.ValueUnchanged.AddListener(unchangedListenerMock.Listen);

            GameObject sourceObject = new GameObject();
            BooleanActionMock sourceMock = sourceObject.AddComponent<BooleanActionMock>();

            subject.AddSource(sourceMock);

            Assert.AreEqual(1, subject.ReadOnlySources.Count);

            sourceMock.Receive(true);

            Assert.IsTrue(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsTrue(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            subject.RemoveSource(sourceMock);

            sourceMock.SetIsActivated(false);
            sourceMock.SetValue(false);
            activatedListenerMock.Reset();
            deactivatedListenerMock.Reset();
            changedListenerMock.Reset();
            unchangedListenerMock.Reset();

            Assert.AreEqual(0, subject.ReadOnlySources.Count);

            sourceMock.Receive(true);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            Object.DestroyImmediate(sourceObject);
        }

        [Test]
        public void ClearSources()
        {
            UnityEventListenerMock activatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock deactivatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock changedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock unchangedListenerMock = new UnityEventListenerMock();

            subject.Activated.AddListener(activatedListenerMock.Listen);
            subject.Deactivated.AddListener(deactivatedListenerMock.Listen);
            subject.ValueChanged.AddListener(changedListenerMock.Listen);
            subject.ValueUnchanged.AddListener(unchangedListenerMock.Listen);

            GameObject sourceObject = new GameObject();
            BooleanActionMock sourceMock = sourceObject.AddComponent<BooleanActionMock>();

            subject.AddSource(sourceMock);

            Assert.AreEqual(1, subject.ReadOnlySources.Count);

            sourceMock.Receive(true);

            Assert.IsTrue(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsTrue(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            subject.ClearSources();

            sourceMock.SetIsActivated(false);
            sourceMock.SetValue(false);
            activatedListenerMock.Reset();
            deactivatedListenerMock.Reset();
            changedListenerMock.Reset();
            unchangedListenerMock.Reset();

            Assert.AreEqual(0, subject.ReadOnlySources.Count);

            sourceMock.Receive(true);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            Object.DestroyImmediate(sourceObject);
        }
    }

    public class BooleanActionMock : BooleanAction
    {
        public virtual void SetIsActivated(bool value)
        {
            IsActivated = value;
        }

        public virtual void SetValue(bool value)
        {
            Value = value;
        }
    }
}