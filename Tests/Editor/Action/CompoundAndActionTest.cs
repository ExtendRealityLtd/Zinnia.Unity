namespace VRTK.Core.Action
{
    using UnityEngine;
    using NUnit.Framework;
    using VRTK.Core.Utility.Mock;

    public class CompoundAndActionTest
    {
        private GameObject containingObject;
        private CompoundAndActionMock subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<CompoundAndActionMock>();
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

            actionA.SetState(false);
            actionB.SetState(false);

            subject.actions = new BaseAction[]
            {
                actionA,
                actionB
            };

            UnityEventListenerMock activatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock deactivatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock changedListenerMock = new UnityEventListenerMock();

            subject.Activated.AddListener(activatedListenerMock.Listen);
            subject.Deactivated.AddListener(deactivatedListenerMock.Listen);
            subject.Changed.AddListener(changedListenerMock.Listen);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);

            subject.ManualUpdate();

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);

            actionA.SetState(true);
            actionB.SetState(true);

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

            actionA.SetState(false);
            actionB.SetState(false);

            subject.actions = new BaseAction[]
            {
                actionA,
                actionB
            };

            UnityEventListenerMock activatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock deactivatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock changedListenerMock = new UnityEventListenerMock();

            subject.Activated.AddListener(activatedListenerMock.Listen);
            subject.Deactivated.AddListener(deactivatedListenerMock.Listen);
            subject.Changed.AddListener(changedListenerMock.Listen);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);

            subject.ManualUpdate();

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);

            actionA.SetState(true);
            actionB.SetState(false);

            subject.ManualUpdate();

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);
        }

        [Test]
        public void DectivatedEmitted()
        {
            subject.SetState(true);
            subject.SetValue(true);

            MockAction actionA = containingObject.AddComponent<MockAction>();
            MockAction actionB = containingObject.AddComponent<MockAction>();

            actionA.SetState(true);
            actionB.SetState(true);

            subject.actions = new BaseAction[]
            {
                actionA,
                actionB
            };

            UnityEventListenerMock activatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock deactivatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock changedListenerMock = new UnityEventListenerMock();

            subject.Activated.AddListener(activatedListenerMock.Listen);
            subject.Deactivated.AddListener(deactivatedListenerMock.Listen);
            subject.Changed.AddListener(changedListenerMock.Listen);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);

            subject.ManualUpdate();

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);

            actionA.SetState(false);
            actionB.SetState(true);

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

            actionA.SetState(false);
            actionB.SetState(false);

            subject.actions = new BaseAction[]
            {
                actionA,
                actionB
            };

            UnityEventListenerMock changedListenerMock = new UnityEventListenerMock();

            subject.Changed.AddListener(changedListenerMock.Listen);

            Assert.IsFalse(changedListenerMock.Received);

            subject.ManualUpdate();
            Assert.IsFalse(changedListenerMock.Received);

            actionA.SetState(true);
            actionB.SetState(true);
            subject.ManualUpdate();
            Assert.IsTrue(changedListenerMock.Received);
            changedListenerMock.Reset();

            actionA.SetState(false);
            actionB.SetState(true);
            subject.ManualUpdate();
            Assert.IsTrue(changedListenerMock.Received);
            changedListenerMock.Reset();

            subject.ManualUpdate();
            Assert.IsFalse(changedListenerMock.Received);
            changedListenerMock.Reset();

            actionA.SetState(false);
            actionB.SetState(false);
            subject.ManualUpdate();
            Assert.IsFalse(changedListenerMock.Received);
            changedListenerMock.Reset();

            actionA.SetState(true);
            actionB.SetState(false);
            subject.ManualUpdate();
            Assert.IsFalse(changedListenerMock.Received);
            changedListenerMock.Reset();

            actionA.SetState(true);
            actionB.SetState(true);
            subject.ManualUpdate();
            Assert.IsTrue(changedListenerMock.Received);
        }
    }

    public class CompoundAndActionMock : CompoundAndAction
    {
        public virtual void ManualUpdate()
        {
            Update();
        }

        public virtual void SetState(bool value)
        {
            State = value;
        }

        public virtual void SetValue(bool value)
        {
            Value = value;
        }
    }

    public class MockAction : BaseAction
    {
        public virtual void SetState(bool value)
        {
            State = value;
        }
    }
}