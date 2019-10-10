using Zinnia.Action.Collection;
using ZinniaAction = Zinnia.Action;

namespace Test.Zinnia.Action
{
    using UnityEngine;
    using UnityEngine.TestTools;
    using System.Collections;
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using Assert = UnityEngine.Assertions.Assert;

    public class AllActionTest
    {
        private GameObject containingObject;
        private AllActionMock subject;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            containingObject = new GameObject();
            ActionObservableList actions = containingObject.AddComponent<ActionObservableList>();

            subject = containingObject.AddComponent<AllActionMock>();
            subject.Actions = actions;

            yield return null;
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void ActivatedEmittedOnAllTrue()
        {
            MockAction actionA = containingObject.AddComponent<MockAction>();
            MockAction actionB = containingObject.AddComponent<MockAction>();

            actionA.SetIsActivated(false);
            actionB.SetIsActivated(false);

            subject.Actions.Add(actionA);
            subject.Actions.Add(actionB);

            UnityEventListenerMock activatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock deactivatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock changedListenerMock = new UnityEventListenerMock();

            subject.Activated.AddListener(activatedListenerMock.Listen);
            subject.Deactivated.AddListener(deactivatedListenerMock.Listen);
            subject.ValueChanged.AddListener(changedListenerMock.Listen);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);

            actionA.SetIsActivated(true);
            actionB.SetIsActivated(true);

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

            subject.Actions.Add(actionA);
            subject.Actions.Add(actionB);

            UnityEventListenerMock activatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock deactivatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock changedListenerMock = new UnityEventListenerMock();

            subject.Activated.AddListener(activatedListenerMock.Listen);
            subject.Deactivated.AddListener(deactivatedListenerMock.Listen);
            subject.ValueChanged.AddListener(changedListenerMock.Listen);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);

            actionA.SetIsActivated(true);
            actionB.SetIsActivated(false);

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

            subject.Actions.Add(actionA);
            subject.Actions.Add(actionB);

            UnityEventListenerMock activatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock deactivatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock changedListenerMock = new UnityEventListenerMock();

            subject.Activated.AddListener(activatedListenerMock.Listen);
            subject.Deactivated.AddListener(deactivatedListenerMock.Listen);
            subject.ValueChanged.AddListener(changedListenerMock.Listen);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);

            actionA.SetIsActivated(false);
            actionB.SetIsActivated(true);

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

            subject.Actions.Add(actionA);
            subject.Actions.Add(actionB);

            UnityEventListenerMock changedListenerMock = new UnityEventListenerMock();

            subject.ValueChanged.AddListener(changedListenerMock.Listen);

            Assert.IsFalse(changedListenerMock.Received);

            actionA.SetIsActivated(true);
            actionB.SetIsActivated(true);

            Assert.IsTrue(changedListenerMock.Received);
            changedListenerMock.Reset();

            actionA.SetIsActivated(false);
            actionB.SetIsActivated(true);

            Assert.IsTrue(changedListenerMock.Received);
            changedListenerMock.Reset();

            actionA.SetIsActivated(false);
            actionB.SetIsActivated(false);

            Assert.IsFalse(changedListenerMock.Received);
            changedListenerMock.Reset();

            actionA.SetIsActivated(true);
            actionB.SetIsActivated(false);

            Assert.IsFalse(changedListenerMock.Received);
            changedListenerMock.Reset();

            actionA.SetIsActivated(true);
            actionB.SetIsActivated(true);

            Assert.IsTrue(changedListenerMock.Received);
        }

        [Test]
        public void EventsNotEmittedOnInactiveGameObject()
        {
            MockAction actionA = containingObject.AddComponent<MockAction>();
            MockAction actionB = containingObject.AddComponent<MockAction>();

            actionA.SetIsActivated(false);
            actionB.SetIsActivated(false);

            subject.Actions.Add(actionA);
            subject.Actions.Add(actionB);

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

            actionA.SetIsActivated(true);
            actionB.SetIsActivated(true);

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

            subject.Actions.Add(actionA);
            subject.Actions.Add(actionB);

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

            actionA.SetIsActivated(true);
            actionB.SetIsActivated(true);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);
        }

        [Test]
        public void RemoveActionListenersCorrectly()
        {
            subject.enabled = false;

            GameObject otherObject = new GameObject();
            MockAction actionA = otherObject.AddComponent<MockAction>();
            MockAction actionB = otherObject.AddComponent<MockAction>();

            actionA.SetIsActivated(false);
            actionB.SetIsActivated(false);

            subject.Actions.Add(actionA);
            subject.Actions.Add(actionB);

            subject.enabled = true;

            Object.DestroyImmediate(otherObject);

            subject.enabled = false;
        }
    }

    public class AllActionMock : ZinniaAction.AllAction
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

    public class MockAction : ZinniaAction.Action
    {
        public override void AddSource(ZinniaAction.Action action) { }

        public override void ClearSources() { }

        public override void RemoveSource(ZinniaAction.Action action) { }

        public override void EmitActivationState() { }

        public virtual void SetIsActivated(bool value)
        {
            IsActivated = value;
        }
    }
}