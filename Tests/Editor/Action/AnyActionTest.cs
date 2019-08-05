using Zinnia.Action;
using Zinnia.Action.Collection;

namespace Test.Zinnia.Action
{
    using UnityEngine;
    using UnityEngine.TestTools;
    using System.Collections;
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using Assert = UnityEngine.Assertions.Assert;

    public class AnyActionTest
    {
        private GameObject containingObject;
        private AnyActionMock subject;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            containingObject = new GameObject();
            ActionObservableList actions = containingObject.AddComponent<ActionObservableList>();

            subject = containingObject.AddComponent<AnyActionMock>();
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
        public void ActivatedEmittedOnSomeTrue()
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

            Assert.IsTrue(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsTrue(changedListenerMock.Received);
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
            actionB.SetIsActivated(false);

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

            Assert.IsFalse(changedListenerMock.Received);
            changedListenerMock.Reset();

            actionA.SetIsActivated(false);
            actionB.SetIsActivated(false);

            Assert.IsTrue(changedListenerMock.Received);
            changedListenerMock.Reset();

            actionA.SetIsActivated(true);
            actionB.SetIsActivated(false);

            Assert.IsTrue(changedListenerMock.Received);
            changedListenerMock.Reset();

            actionA.SetIsActivated(true);
            actionB.SetIsActivated(true);

            Assert.IsFalse(changedListenerMock.Received);
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
    }

    public class AnyActionMock : AnyAction
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