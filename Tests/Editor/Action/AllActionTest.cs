using ZinniaAction = Zinnia.Action;

namespace Test.Zinnia.Action
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;

    public class AllActionTest
    {
        private GameObject containingObject;
        private AllActionMock subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<AllActionMock>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void ActivatedEmittedOnAllTrue()
        {
            MockAction actionA = containingObject.AddComponent<MockAction>();
            MockAction actionB = containingObject.AddComponent<MockAction>();

            actionA.SetIsActivated(false);
            actionB.SetIsActivated(false);

            subject.actions.Add(actionA);
            subject.actions.Add(actionB);

            UnityEventListenerMock activatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock deactivatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock changedListenerMock = new UnityEventListenerMock();

            subject.Activated.AddListener(activatedListenerMock.Listen);
            subject.Deactivated.AddListener(deactivatedListenerMock.Listen);
            subject.ValueChanged.AddListener(changedListenerMock.Listen);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);

            subject.ManualUpdate();

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);

            actionA.SetIsActivated(true);
            actionB.SetIsActivated(true);

            subject.ManualUpdate();

            Assert.IsTrue(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsTrue(changedListenerMock.Received);
        }

        [Test]
        public void ActivatedNotEmittedOnNotAllTrue()
        {
            MockAction actionA = containingObject.AddComponent<MockAction>();
            MockAction actionB = containingObject.AddComponent<MockAction>();

            actionA.SetIsActivated(false);
            actionB.SetIsActivated(false);

            subject.actions.Add(actionA);
            subject.actions.Add(actionB);

            UnityEventListenerMock activatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock deactivatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock changedListenerMock = new UnityEventListenerMock();

            subject.Activated.AddListener(activatedListenerMock.Listen);
            subject.Deactivated.AddListener(deactivatedListenerMock.Listen);
            subject.ValueChanged.AddListener(changedListenerMock.Listen);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);

            subject.ManualUpdate();

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);

            actionA.SetIsActivated(true);
            actionB.SetIsActivated(false);

            subject.ManualUpdate();

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);
        }

        [Test]
        public void DeactivatedEmitted()
        {
            subject.SetIsActivated(true);
            subject.SetValue(true);

            MockAction actionA = containingObject.AddComponent<MockAction>();
            MockAction actionB = containingObject.AddComponent<MockAction>();

            actionA.SetIsActivated(true);
            actionB.SetIsActivated(true);

            subject.actions.Add(actionA);
            subject.actions.Add(actionB);

            UnityEventListenerMock activatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock deactivatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock changedListenerMock = new UnityEventListenerMock();

            subject.Activated.AddListener(activatedListenerMock.Listen);
            subject.Deactivated.AddListener(deactivatedListenerMock.Listen);
            subject.ValueChanged.AddListener(changedListenerMock.Listen);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);

            subject.ManualUpdate();

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);

            actionA.SetIsActivated(false);
            actionB.SetIsActivated(true);

            subject.ManualUpdate();

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsTrue(deactivatedListenerMock.Received);
            Assert.IsTrue(changedListenerMock.Received);
        }

        [Test]
        public void ChangedEmitted()
        {
            MockAction actionA = containingObject.AddComponent<MockAction>();
            MockAction actionB = containingObject.AddComponent<MockAction>();

            actionA.SetIsActivated(false);
            actionB.SetIsActivated(false);

            subject.actions.Add(actionA);
            subject.actions.Add(actionB);

            UnityEventListenerMock changedListenerMock = new UnityEventListenerMock();

            subject.ValueChanged.AddListener(changedListenerMock.Listen);

            Assert.IsFalse(changedListenerMock.Received);

            subject.ManualUpdate();
            Assert.IsFalse(changedListenerMock.Received);

            actionA.SetIsActivated(true);
            actionB.SetIsActivated(true);
            subject.ManualUpdate();
            Assert.IsTrue(changedListenerMock.Received);
            changedListenerMock.Reset();

            actionA.SetIsActivated(false);
            actionB.SetIsActivated(true);
            subject.ManualUpdate();
            Assert.IsTrue(changedListenerMock.Received);
            changedListenerMock.Reset();

            subject.ManualUpdate();
            Assert.IsFalse(changedListenerMock.Received);
            changedListenerMock.Reset();

            actionA.SetIsActivated(false);
            actionB.SetIsActivated(false);
            subject.ManualUpdate();
            Assert.IsFalse(changedListenerMock.Received);
            changedListenerMock.Reset();

            actionA.SetIsActivated(true);
            actionB.SetIsActivated(false);
            subject.ManualUpdate();
            Assert.IsFalse(changedListenerMock.Received);
            changedListenerMock.Reset();

            actionA.SetIsActivated(true);
            actionB.SetIsActivated(true);
            subject.ManualUpdate();
            Assert.IsTrue(changedListenerMock.Received);
        }

        [Test]
        public void EventsNotEmittedOnInactiveGameObject()
        {
            MockAction actionA = containingObject.AddComponent<MockAction>();
            MockAction actionB = containingObject.AddComponent<MockAction>();

            actionA.SetIsActivated(false);
            actionB.SetIsActivated(false);

            subject.actions.Add(actionA);
            subject.actions.Add(actionB);

            UnityEventListenerMock activatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock deactivatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock changedListenerMock = new UnityEventListenerMock();

            subject.Activated.AddListener(activatedListenerMock.Listen);
            subject.Deactivated.AddListener(deactivatedListenerMock.Listen);
            subject.ValueChanged.AddListener(changedListenerMock.Listen);
            subject.gameObject.SetActive(false);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);

            subject.ManualUpdate();

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);

            actionA.SetIsActivated(true);
            actionB.SetIsActivated(true);

            subject.ManualUpdate();

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);
        }

        [Test]
        public void EventsNotEmittedOnDisabledComponent()
        {
            MockAction actionA = containingObject.AddComponent<MockAction>();
            MockAction actionB = containingObject.AddComponent<MockAction>();

            actionA.SetIsActivated(false);
            actionB.SetIsActivated(false);

            subject.actions.Add(actionA);
            subject.actions.Add(actionB);

            UnityEventListenerMock activatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock deactivatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock changedListenerMock = new UnityEventListenerMock();

            subject.Activated.AddListener(activatedListenerMock.Listen);
            subject.Deactivated.AddListener(deactivatedListenerMock.Listen);
            subject.ValueChanged.AddListener(changedListenerMock.Listen);
            subject.enabled = false;

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);

            subject.ManualUpdate();

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);

            actionA.SetIsActivated(true);
            actionB.SetIsActivated(true);

            subject.ManualUpdate();

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);
        }
    }

    public class AllActionMock : ZinniaAction.AllAction
    {
        public virtual void ManualUpdate()
        {
            Update();
        }

        public virtual void SetIsActivated(bool value)
        {
            IsActivated = value;
        }

        public virtual void SetValue(bool value)
        {
            Value = value;
        }
    }

    public class MockAction : ZinniaAction.Action
    {
        public override void AddSource(ZinniaAction.Action action)
        {
        }

        public override void ClearSources()
        {
        }

        public override void RemoveSource(ZinniaAction.Action action)
        {
        }

        public override void EmitActivationState()
        {
        }

        public virtual void SetIsActivated(bool value)
        {
            IsActivated = value;
        }
    }
}