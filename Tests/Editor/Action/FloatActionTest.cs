using Zinnia.Action;

namespace Test.Zinnia.Action
{
    using NUnit.Framework;
    using System.Collections;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using UnityEngine.TestTools;

    public class FloatActionTest
    {
        private GameObject containingObject;
        private FloatActionMock subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject("FloatActionTest");
            subject = containingObject.AddComponent<FloatActionMock>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void IgnoreEmitEvents()
        {
            UnityEventListenerMock activatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock deactivatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock changedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock unchangedListenerMock = new UnityEventListenerMock();

            subject.Activated.AddListener(activatedListenerMock.Listen);
            subject.Deactivated.AddListener(deactivatedListenerMock.Listen);
            subject.ValueChanged.AddListener(changedListenerMock.Listen);
            subject.ValueUnchanged.AddListener(unchangedListenerMock.Listen);

            subject.EmitEvents = false;

            Assert.AreEqual(0f, subject.Value);
            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            subject.Receive(1f);

            Assert.AreEqual(1f, subject.Value);
            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            subject.Receive(0f);

            Assert.AreEqual(0f, subject.Value);
            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);
        }

        [Test]
        public void ActivatedEmitted()
        {
            UnityEventListenerMock activatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock deactivatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock changedListenerMock = new UnityEventListenerMock();

            subject.Activated.AddListener(activatedListenerMock.Listen);
            subject.Deactivated.AddListener(deactivatedListenerMock.Listen);
            subject.ValueChanged.AddListener(changedListenerMock.Listen);

            Assert.AreEqual(0f, subject.Value);
            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);

            subject.Receive(1f);

            Assert.AreEqual(1f, subject.Value);
            Assert.IsTrue(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsTrue(changedListenerMock.Received);
        }

        [Test]
        public void DeactivatedEmitted()
        {
            subject.SetIsActivated(true);
            subject.Value = 1f;

            UnityEventListenerMock activatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock deactivatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock changedListenerMock = new UnityEventListenerMock();

            subject.Activated.AddListener(activatedListenerMock.Listen);
            subject.Deactivated.AddListener(deactivatedListenerMock.Listen);
            subject.ValueChanged.AddListener(changedListenerMock.Listen);

            Assert.AreEqual(1f, subject.Value);
            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);

            subject.Receive(0f);

            Assert.AreEqual(0f, subject.Value);
            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsTrue(deactivatedListenerMock.Received);
            Assert.IsTrue(changedListenerMock.Received);
        }

        [Test]
        public void ChangedEmitted()
        {
            UnityEventListenerMock changedListenerMock = new UnityEventListenerMock();

            subject.ValueChanged.AddListener(changedListenerMock.Listen);

            Assert.AreEqual(0f, subject.Value);
            Assert.IsFalse(changedListenerMock.Received);

            subject.Receive(0.1f);

            Assert.AreEqual(0.1f, subject.Value);
            Assert.IsTrue(changedListenerMock.Received);

            changedListenerMock.Reset();

            Assert.AreEqual(0.1f, subject.Value);
            Assert.IsFalse(changedListenerMock.Received);

            subject.Receive(0.1f);

            Assert.AreEqual(0.1f, subject.Value);
            Assert.IsFalse(changedListenerMock.Received);

            changedListenerMock.Reset();

            Assert.AreEqual(0.1f, subject.Value);
            Assert.IsFalse(changedListenerMock.Received);

            subject.Receive(0.2f);

            Assert.AreEqual(0.2f, subject.Value);
            Assert.IsTrue(changedListenerMock.Received);
        }

        [Test]
        public void DefaultValueNotTypeDefault()
        {
            UnityEventListenerMock activatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock deactivatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock changedListenerMock = new UnityEventListenerMock();

            subject.Activated.AddListener(activatedListenerMock.Listen);
            subject.Deactivated.AddListener(deactivatedListenerMock.Listen);
            subject.ValueChanged.AddListener(changedListenerMock.Listen);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);

            subject.DefaultValue = 1f;

            Assert.IsTrue(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsTrue(changedListenerMock.Received);

            activatedListenerMock.Reset();
            deactivatedListenerMock.Reset();
            changedListenerMock.Reset();

            subject.Receive(1f);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsTrue(deactivatedListenerMock.Received);
            Assert.IsTrue(changedListenerMock.Received);

            activatedListenerMock.Reset();
            deactivatedListenerMock.Reset();
            changedListenerMock.Reset();

            subject.Receive(0f);

            Assert.IsTrue(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsTrue(changedListenerMock.Received);

            activatedListenerMock.Reset();
            deactivatedListenerMock.Reset();
            changedListenerMock.Reset();

            subject.Receive(1f);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsTrue(deactivatedListenerMock.Received);
            Assert.IsTrue(changedListenerMock.Received);
        }

        [Test]
        public void DefaultValueTypeDefault()
        {
            UnityEventListenerMock activatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock deactivatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock changedListenerMock = new UnityEventListenerMock();

            subject.Activated.AddListener(activatedListenerMock.Listen);
            subject.Deactivated.AddListener(deactivatedListenerMock.Listen);
            subject.ValueChanged.AddListener(changedListenerMock.Listen);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);

            subject.DefaultValue = 0f;

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);

            activatedListenerMock.Reset();
            deactivatedListenerMock.Reset();
            changedListenerMock.Reset();

            subject.Receive(0f);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);

            activatedListenerMock.Reset();
            deactivatedListenerMock.Reset();
            changedListenerMock.Reset();

            subject.Receive(1f);

            Assert.IsTrue(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsTrue(changedListenerMock.Received);

            activatedListenerMock.Reset();
            deactivatedListenerMock.Reset();
            changedListenerMock.Reset();

            subject.Receive(0f);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsTrue(deactivatedListenerMock.Received);
            Assert.IsTrue(changedListenerMock.Received);
        }

        [Test]
        public void EventsNotEmittedOnInactiveGameObject()
        {
            UnityEventListenerMock activatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock deactivatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock changedListenerMock = new UnityEventListenerMock();

            subject.gameObject.SetActive(false);
            subject.Activated.AddListener(activatedListenerMock.Listen);
            subject.Deactivated.AddListener(deactivatedListenerMock.Listen);
            subject.ValueChanged.AddListener(changedListenerMock.Listen);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);

            subject.Receive(1f);

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

            subject.enabled = false;
            subject.Activated.AddListener(activatedListenerMock.Listen);
            subject.Deactivated.AddListener(deactivatedListenerMock.Listen);
            subject.ValueChanged.AddListener(changedListenerMock.Listen);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);

            subject.Receive(1f);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);
        }

        [UnityTest]
        public IEnumerator DefaultValueZeroInitialValueOne()
        {
            UnityEventListenerMock activatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock deactivatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock changedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock unchangedListenerMock = new UnityEventListenerMock();

            FloatActionLiveMock liveSubject = containingObject.AddComponent<FloatActionLiveMock>();
            liveSubject.InitialValue = 1f;

            liveSubject.Activated.AddListener(activatedListenerMock.Listen);
            liveSubject.Deactivated.AddListener(deactivatedListenerMock.Listen);
            liveSubject.ValueChanged.AddListener(changedListenerMock.Listen);
            liveSubject.ValueUnchanged.AddListener(unchangedListenerMock.Listen);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            yield return null;

            Assert.IsTrue(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsTrue(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);
        }

        [UnityTest]
        public IEnumerator DefaultValueOneInitialValueOne()
        {
            UnityEventListenerMock activatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock deactivatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock changedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock unchangedListenerMock = new UnityEventListenerMock();

            FloatActionLiveMock liveSubject = containingObject.AddComponent<FloatActionLiveMock>();
            liveSubject.DefaultValue = 1f;
            liveSubject.InitialValue = 1f;

            liveSubject.ForceAwake();

            liveSubject.Activated.AddListener(activatedListenerMock.Listen);
            liveSubject.Deactivated.AddListener(deactivatedListenerMock.Listen);
            liveSubject.ValueChanged.AddListener(changedListenerMock.Listen);
            liveSubject.ValueUnchanged.AddListener(unchangedListenerMock.Listen);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            activatedListenerMock.Reset();
            deactivatedListenerMock.Reset();
            changedListenerMock.Reset();
            unchangedListenerMock.Reset();

            yield return null;

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            activatedListenerMock.Reset();
            deactivatedListenerMock.Reset();
            changedListenerMock.Reset();
            unchangedListenerMock.Reset();

            liveSubject.Receive(0f);

            Assert.IsTrue(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsTrue(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);
        }

        [UnityTest]
        public IEnumerator DefaultValueOneInitialValueZero()
        {
            UnityEventListenerMock activatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock deactivatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock changedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock unchangedListenerMock = new UnityEventListenerMock();

            FloatActionLiveMock liveSubject = containingObject.AddComponent<FloatActionLiveMock>();
            liveSubject.DefaultValue = 1f;
            liveSubject.InitialValue = 0f;

            liveSubject.ForceAwake();

            liveSubject.Activated.AddListener(activatedListenerMock.Listen);
            liveSubject.Deactivated.AddListener(deactivatedListenerMock.Listen);
            liveSubject.ValueChanged.AddListener(changedListenerMock.Listen);
            liveSubject.ValueUnchanged.AddListener(unchangedListenerMock.Listen);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            activatedListenerMock.Reset();
            deactivatedListenerMock.Reset();
            changedListenerMock.Reset();
            unchangedListenerMock.Reset();

            yield return null;

            Assert.IsTrue(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsTrue(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            activatedListenerMock.Reset();
            deactivatedListenerMock.Reset();
            changedListenerMock.Reset();
            unchangedListenerMock.Reset();

            liveSubject.Receive(1f);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsTrue(deactivatedListenerMock.Received);
            Assert.IsTrue(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);
        }

        [UnityTest]
        public IEnumerator ReceiveInitialValue()
        {
            UnityEventListenerMock activatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock deactivatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock changedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock unchangedListenerMock = new UnityEventListenerMock();

            FloatActionLiveMock liveSubject = containingObject.AddComponent<FloatActionLiveMock>();
            liveSubject.InitialValue = 1f;

            liveSubject.Activated.AddListener(activatedListenerMock.Listen);
            liveSubject.Deactivated.AddListener(deactivatedListenerMock.Listen);
            liveSubject.ValueChanged.AddListener(changedListenerMock.Listen);
            liveSubject.ValueUnchanged.AddListener(unchangedListenerMock.Listen);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            yield return null;

            Assert.IsTrue(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsTrue(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            activatedListenerMock.Reset();
            deactivatedListenerMock.Reset();
            changedListenerMock.Reset();
            unchangedListenerMock.Reset();

            liveSubject.Receive(0f);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsTrue(deactivatedListenerMock.Received);
            Assert.IsTrue(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            activatedListenerMock.Reset();
            deactivatedListenerMock.Reset();
            changedListenerMock.Reset();
            unchangedListenerMock.Reset();

            liveSubject.ReceiveInitialValue();

            Assert.IsTrue(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsTrue(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);
        }

        [UnityTest]
        public IEnumerator ResetToInitialValue()
        {
            UnityEventListenerMock activatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock deactivatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock changedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock unchangedListenerMock = new UnityEventListenerMock();

            FloatActionLiveMock liveSubject = containingObject.AddComponent<FloatActionLiveMock>();
            liveSubject.InitialValue = 0f;

            liveSubject.Activated.AddListener(activatedListenerMock.Listen);
            liveSubject.Deactivated.AddListener(deactivatedListenerMock.Listen);
            liveSubject.ValueChanged.AddListener(changedListenerMock.Listen);
            liveSubject.ValueUnchanged.AddListener(unchangedListenerMock.Listen);

            liveSubject.ForceAwake();

            Assert.AreEqual(0f, liveSubject.Value);
            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            yield return null;

            Assert.AreEqual(0f, liveSubject.Value);
            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            activatedListenerMock.Reset();
            deactivatedListenerMock.Reset();
            changedListenerMock.Reset();
            unchangedListenerMock.Reset();

            liveSubject.Receive(1f);

            Assert.AreEqual(1f, liveSubject.Value);
            Assert.IsTrue(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsTrue(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            activatedListenerMock.Reset();
            deactivatedListenerMock.Reset();
            changedListenerMock.Reset();
            unchangedListenerMock.Reset();

            liveSubject.ResetToInitialValue();

            Assert.AreEqual(0f, liveSubject.Value);
            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);
        }

        [UnityTest]
        public IEnumerator ResetToDefaultValue()
        {
            UnityEventListenerMock activatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock deactivatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock changedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock unchangedListenerMock = new UnityEventListenerMock();

            FloatActionLiveMock liveSubject = containingObject.AddComponent<FloatActionLiveMock>();
            liveSubject.DefaultValue = 0f;

            liveSubject.Activated.AddListener(activatedListenerMock.Listen);
            liveSubject.Deactivated.AddListener(deactivatedListenerMock.Listen);
            liveSubject.ValueChanged.AddListener(changedListenerMock.Listen);
            liveSubject.ValueUnchanged.AddListener(unchangedListenerMock.Listen);

            liveSubject.ForceAwake();

            Assert.AreEqual(0f, liveSubject.Value);
            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            yield return null;

            Assert.AreEqual(0f, liveSubject.Value);
            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            activatedListenerMock.Reset();
            deactivatedListenerMock.Reset();
            changedListenerMock.Reset();
            unchangedListenerMock.Reset();

            liveSubject.Receive(1f);

            Assert.AreEqual(1f, liveSubject.Value);
            Assert.IsTrue(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsTrue(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            activatedListenerMock.Reset();
            deactivatedListenerMock.Reset();
            changedListenerMock.Reset();
            unchangedListenerMock.Reset();

            liveSubject.ResetToDefaultValue();

            Assert.AreEqual(0f, liveSubject.Value);
            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);
        }
    }

    public class FloatActionMock : FloatAction
    {
        public virtual void SetIsActivated(bool value)
        {
            IsActivated = value;
        }
    }

    public class FloatActionLiveMock : FloatAction
    {
        public virtual void ForceAwake()
        {
            IsActivated = false;
            Awake();
        }
    }
}