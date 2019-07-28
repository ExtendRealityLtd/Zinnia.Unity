using Zinnia.Data.Collection.Stack;

namespace Test.Zinnia.Data.Collection.Stack
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using Assert = UnityEngine.Assertions.Assert;

    public class GameObjectEventObservableTest
    {
        private GameObject containingObject;
        private GameObjectObservableStack subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<GameObjectObservableStack>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void Push()
        {
            UnityEventListenerMock elementOnePushedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementOnePoppedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementOneForcePoppedMock = new UnityEventListenerMock();
            GameObjectObservableStack.GameObjectElementEvents eventsOne = new GameObjectObservableStack.GameObjectElementEvents();
            eventsOne.Pushed.AddListener(elementOnePushedMock.Listen);
            eventsOne.Popped.AddListener(elementOnePoppedMock.Listen);
            eventsOne.ForcePopped.AddListener(elementOneForcePoppedMock.Listen);

            UnityEventListenerMock elementTwoPushedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementTwoPoppedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementTwoForcePoppedMock = new UnityEventListenerMock();
            GameObjectObservableStack.GameObjectElementEvents eventsTwo = new GameObjectObservableStack.GameObjectElementEvents();
            eventsTwo.Pushed.AddListener(elementTwoPushedMock.Listen);
            eventsTwo.Popped.AddListener(elementTwoPoppedMock.Listen);
            eventsTwo.ForcePopped.AddListener(elementTwoForcePoppedMock.Listen);

            UnityEventListenerMock elementThreePushedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementThreePoppedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementThreeForcePoppedMock = new UnityEventListenerMock();
            GameObjectObservableStack.GameObjectElementEvents eventsThree = new GameObjectObservableStack.GameObjectElementEvents();
            eventsThree.Pushed.AddListener(elementThreePushedMock.Listen);
            eventsThree.Popped.AddListener(elementThreePoppedMock.Listen);
            eventsThree.ForcePopped.AddListener(elementThreeForcePoppedMock.Listen);

            GameObject objectOne = new GameObject();
            GameObject objectTwo = new GameObject();
            GameObject objectThree = new GameObject();

            subject.ElementEvents.Add(eventsOne);
            subject.ElementEvents.Add(eventsTwo);
            subject.ElementEvents.Add(eventsThree);

            Assert.IsFalse(elementOnePushedMock.Received);
            Assert.IsFalse(elementOnePoppedMock.Received);
            Assert.IsFalse(elementOneForcePoppedMock.Received);
            Assert.IsFalse(elementTwoPushedMock.Received);
            Assert.IsFalse(elementTwoPoppedMock.Received);
            Assert.IsFalse(elementTwoForcePoppedMock.Received);
            Assert.IsFalse(elementThreePushedMock.Received);
            Assert.IsFalse(elementThreePoppedMock.Received);
            Assert.IsFalse(elementThreeForcePoppedMock.Received);

            subject.Push(objectOne);

            Assert.IsTrue(elementOnePushedMock.Received);
            Assert.IsFalse(elementOnePoppedMock.Received);
            Assert.IsFalse(elementOneForcePoppedMock.Received);
            Assert.IsFalse(elementTwoPushedMock.Received);
            Assert.IsFalse(elementTwoPoppedMock.Received);
            Assert.IsFalse(elementTwoForcePoppedMock.Received);
            Assert.IsFalse(elementThreePushedMock.Received);
            Assert.IsFalse(elementThreePoppedMock.Received);
            Assert.IsFalse(elementThreeForcePoppedMock.Received);

            elementOnePushedMock.Reset();
            elementOnePoppedMock.Reset();
            elementOneForcePoppedMock.Reset();
            elementTwoPushedMock.Reset();
            elementTwoPoppedMock.Reset();
            elementTwoForcePoppedMock.Reset();
            elementThreePushedMock.Reset();
            elementThreePoppedMock.Reset();
            elementThreeForcePoppedMock.Reset();

            subject.Push(objectTwo);

            Assert.IsFalse(elementOnePushedMock.Received);
            Assert.IsFalse(elementOnePoppedMock.Received);
            Assert.IsFalse(elementOneForcePoppedMock.Received);
            Assert.IsTrue(elementTwoPushedMock.Received);
            Assert.IsFalse(elementTwoPoppedMock.Received);
            Assert.IsFalse(elementTwoForcePoppedMock.Received);
            Assert.IsFalse(elementThreePushedMock.Received);
            Assert.IsFalse(elementThreePoppedMock.Received);
            Assert.IsFalse(elementThreeForcePoppedMock.Received);

            elementOnePushedMock.Reset();
            elementOnePoppedMock.Reset();
            elementOneForcePoppedMock.Reset();
            elementTwoPushedMock.Reset();
            elementTwoPoppedMock.Reset();
            elementTwoForcePoppedMock.Reset();
            elementThreePushedMock.Reset();
            elementThreePoppedMock.Reset();
            elementThreeForcePoppedMock.Reset();

            subject.Push(objectThree);

            Assert.IsFalse(elementOnePushedMock.Received);
            Assert.IsFalse(elementOnePoppedMock.Received);
            Assert.IsFalse(elementOneForcePoppedMock.Received);
            Assert.IsFalse(elementTwoPushedMock.Received);
            Assert.IsFalse(elementTwoPoppedMock.Received);
            Assert.IsFalse(elementTwoForcePoppedMock.Received);
            Assert.IsTrue(elementThreePushedMock.Received);
            Assert.IsFalse(elementThreePoppedMock.Received);
            Assert.IsFalse(elementThreeForcePoppedMock.Received);

            Object.DestroyImmediate(objectOne);
            Object.DestroyImmediate(objectTwo);
            Object.DestroyImmediate(objectThree);
        }

        [Test]
        public void PushDuplicate()
        {
            UnityEventListenerMock elementOnePushedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementOnePoppedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementOneForcePoppedMock = new UnityEventListenerMock();
            GameObjectObservableStack.GameObjectElementEvents eventsOne = new GameObjectObservableStack.GameObjectElementEvents();
            eventsOne.Pushed.AddListener(elementOnePushedMock.Listen);
            eventsOne.Popped.AddListener(elementOnePoppedMock.Listen);
            eventsOne.ForcePopped.AddListener(elementOneForcePoppedMock.Listen);

            UnityEventListenerMock elementTwoPushedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementTwoPoppedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementTwoForcePoppedMock = new UnityEventListenerMock();
            GameObjectObservableStack.GameObjectElementEvents eventsTwo = new GameObjectObservableStack.GameObjectElementEvents();
            eventsTwo.Pushed.AddListener(elementTwoPushedMock.Listen);
            eventsTwo.Popped.AddListener(elementTwoPoppedMock.Listen);
            eventsTwo.ForcePopped.AddListener(elementTwoForcePoppedMock.Listen);

            GameObject objectOne = new GameObject();

            subject.ElementEvents.Add(eventsOne);
            subject.ElementEvents.Add(eventsTwo);

            Assert.IsFalse(elementOnePushedMock.Received);
            Assert.IsFalse(elementOnePoppedMock.Received);
            Assert.IsFalse(elementOneForcePoppedMock.Received);
            Assert.IsFalse(elementTwoPushedMock.Received);
            Assert.IsFalse(elementTwoPoppedMock.Received);
            Assert.IsFalse(elementTwoForcePoppedMock.Received);

            subject.Push(objectOne);

            Assert.IsTrue(elementOnePushedMock.Received);
            Assert.IsFalse(elementOnePoppedMock.Received);
            Assert.IsFalse(elementOneForcePoppedMock.Received);
            Assert.IsFalse(elementTwoPushedMock.Received);
            Assert.IsFalse(elementTwoPoppedMock.Received);
            Assert.IsFalse(elementTwoForcePoppedMock.Received);

            elementOnePushedMock.Reset();
            elementOnePoppedMock.Reset();
            elementOneForcePoppedMock.Reset();
            elementTwoPushedMock.Reset();
            elementTwoPoppedMock.Reset();
            elementTwoForcePoppedMock.Reset();

            subject.Push(objectOne);

            Assert.IsFalse(elementOnePushedMock.Received);
            Assert.IsFalse(elementOnePoppedMock.Received);
            Assert.IsFalse(elementOneForcePoppedMock.Received);
            Assert.IsFalse(elementTwoPushedMock.Received);
            Assert.IsFalse(elementTwoPoppedMock.Received);
            Assert.IsFalse(elementTwoForcePoppedMock.Received);

            Object.DestroyImmediate(objectOne);
        }

        [Test]
        public void PushExceedsEventCount()
        {
            UnityEventListenerMock elementOnePushedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementOnePoppedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementOneForcePoppedMock = new UnityEventListenerMock();
            GameObjectObservableStack.GameObjectElementEvents eventsOne = new GameObjectObservableStack.GameObjectElementEvents();
            eventsOne.Pushed.AddListener(elementOnePushedMock.Listen);
            eventsOne.Popped.AddListener(elementOnePoppedMock.Listen);
            eventsOne.ForcePopped.AddListener(elementOneForcePoppedMock.Listen);

            GameObject objectOne = new GameObject();
            GameObject objectTwo = new GameObject();

            subject.ElementEvents.Add(eventsOne);

            Assert.IsFalse(elementOnePushedMock.Received);
            Assert.IsFalse(elementOnePoppedMock.Received);
            Assert.IsFalse(elementOneForcePoppedMock.Received);

            subject.Push(objectOne);

            Assert.IsTrue(elementOnePushedMock.Received);
            Assert.IsFalse(elementOnePoppedMock.Received);
            Assert.IsFalse(elementOneForcePoppedMock.Received);

            elementOnePushedMock.Reset();
            elementOnePoppedMock.Reset();
            elementOneForcePoppedMock.Reset();

            subject.Push(objectTwo);

            Assert.IsFalse(elementOnePushedMock.Received);
            Assert.IsFalse(elementOnePoppedMock.Received);
            Assert.IsFalse(elementOneForcePoppedMock.Received);

            Object.DestroyImmediate(objectOne);
            Object.DestroyImmediate(objectTwo);
        }

        [Test]
        public void PushInactiveGameObject()
        {
            UnityEventListenerMock elementOnePushedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementOnePoppedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementOneForcePoppedMock = new UnityEventListenerMock();
            GameObjectObservableStack.GameObjectElementEvents eventsOne = new GameObjectObservableStack.GameObjectElementEvents();
            eventsOne.Pushed.AddListener(elementOnePushedMock.Listen);
            eventsOne.Popped.AddListener(elementOnePoppedMock.Listen);
            eventsOne.ForcePopped.AddListener(elementOneForcePoppedMock.Listen);

            GameObject objectOne = new GameObject();

            subject.ElementEvents.Add(eventsOne);
            subject.gameObject.SetActive(false);
            subject.Push(objectOne);

            Assert.IsFalse(elementOnePushedMock.Received);
            Assert.IsFalse(elementOnePoppedMock.Received);
            Assert.IsFalse(elementOneForcePoppedMock.Received);

            Object.DestroyImmediate(objectOne);
        }

        [Test]
        public void PushInactiveComponent()
        {
            UnityEventListenerMock elementOnePushedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementOnePoppedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementOneForcePoppedMock = new UnityEventListenerMock();
            GameObjectObservableStack.GameObjectElementEvents eventsOne = new GameObjectObservableStack.GameObjectElementEvents();
            eventsOne.Pushed.AddListener(elementOnePushedMock.Listen);
            eventsOne.Popped.AddListener(elementOnePoppedMock.Listen);
            eventsOne.ForcePopped.AddListener(elementOneForcePoppedMock.Listen);

            GameObject objectOne = new GameObject();

            subject.ElementEvents.Add(eventsOne);
            subject.enabled = false;
            subject.Push(objectOne);

            Assert.IsFalse(elementOnePushedMock.Received);
            Assert.IsFalse(elementOnePoppedMock.Received);
            Assert.IsFalse(elementOneForcePoppedMock.Received);

            Object.DestroyImmediate(objectOne);
        }

        [Test]
        public void Pop()
        {
            UnityEventListenerMock elementOnePushedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementOnePoppedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementOneForcePoppedMock = new UnityEventListenerMock();
            GameObjectObservableStack.GameObjectElementEvents eventsOne = new GameObjectObservableStack.GameObjectElementEvents();
            eventsOne.Pushed.AddListener(elementOnePushedMock.Listen);
            eventsOne.Popped.AddListener(elementOnePoppedMock.Listen);
            eventsOne.ForcePopped.AddListener(elementOneForcePoppedMock.Listen);

            UnityEventListenerMock elementTwoPushedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementTwoPoppedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementTwoForcePoppedMock = new UnityEventListenerMock();
            GameObjectObservableStack.GameObjectElementEvents eventsTwo = new GameObjectObservableStack.GameObjectElementEvents();
            eventsTwo.Pushed.AddListener(elementTwoPushedMock.Listen);
            eventsTwo.Popped.AddListener(elementTwoPoppedMock.Listen);
            eventsTwo.ForcePopped.AddListener(elementTwoForcePoppedMock.Listen);

            UnityEventListenerMock elementThreePushedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementThreePoppedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementThreeForcePoppedMock = new UnityEventListenerMock();
            GameObjectObservableStack.GameObjectElementEvents eventsThree = new GameObjectObservableStack.GameObjectElementEvents();
            eventsThree.Pushed.AddListener(elementThreePushedMock.Listen);
            eventsThree.Popped.AddListener(elementThreePoppedMock.Listen);
            eventsThree.ForcePopped.AddListener(elementThreeForcePoppedMock.Listen);

            GameObject objectOne = new GameObject();
            GameObject objectTwo = new GameObject();
            GameObject objectThree = new GameObject();

            subject.ElementEvents.Add(eventsOne);
            subject.ElementEvents.Add(eventsTwo);
            subject.ElementEvents.Add(eventsThree);

            subject.Push(objectOne);
            subject.Push(objectTwo);
            subject.Push(objectThree);

            elementOnePushedMock.Reset();
            elementOnePoppedMock.Reset();
            elementOneForcePoppedMock.Reset();
            elementTwoPushedMock.Reset();
            elementTwoPoppedMock.Reset();
            elementTwoForcePoppedMock.Reset();
            elementThreePushedMock.Reset();
            elementThreePoppedMock.Reset();
            elementThreeForcePoppedMock.Reset();

            Assert.IsFalse(elementOnePushedMock.Received);
            Assert.IsFalse(elementOnePoppedMock.Received);
            Assert.IsFalse(elementOneForcePoppedMock.Received);
            Assert.IsFalse(elementTwoPushedMock.Received);
            Assert.IsFalse(elementTwoPoppedMock.Received);
            Assert.IsFalse(elementTwoForcePoppedMock.Received);
            Assert.IsFalse(elementThreePushedMock.Received);
            Assert.IsFalse(elementThreePoppedMock.Received);
            Assert.IsFalse(elementThreeForcePoppedMock.Received);

            subject.Pop();

            Assert.IsFalse(elementOnePushedMock.Received);
            Assert.IsFalse(elementOnePoppedMock.Received);
            Assert.IsFalse(elementOneForcePoppedMock.Received);
            Assert.IsFalse(elementTwoPushedMock.Received);
            Assert.IsFalse(elementTwoPoppedMock.Received);
            Assert.IsFalse(elementTwoForcePoppedMock.Received);
            Assert.IsFalse(elementThreePushedMock.Received);
            Assert.IsTrue(elementThreePoppedMock.Received);
            Assert.IsFalse(elementThreeForcePoppedMock.Received);

            Object.DestroyImmediate(objectOne);
            Object.DestroyImmediate(objectTwo);
            Object.DestroyImmediate(objectThree);
        }

        [Test]
        public void PopAtMiddle()
        {
            UnityEventListenerMock elementOnePushedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementOnePoppedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementOneForcePoppedMock = new UnityEventListenerMock();
            GameObjectObservableStack.GameObjectElementEvents eventsOne = new GameObjectObservableStack.GameObjectElementEvents();
            eventsOne.Pushed.AddListener(elementOnePushedMock.Listen);
            eventsOne.Popped.AddListener(elementOnePoppedMock.Listen);
            eventsOne.ForcePopped.AddListener(elementOneForcePoppedMock.Listen);

            UnityEventListenerMock elementTwoPushedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementTwoPoppedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementTwoForcePoppedMock = new UnityEventListenerMock();
            GameObjectObservableStack.GameObjectElementEvents eventsTwo = new GameObjectObservableStack.GameObjectElementEvents();
            eventsTwo.Pushed.AddListener(elementTwoPushedMock.Listen);
            eventsTwo.Popped.AddListener(elementTwoPoppedMock.Listen);
            eventsTwo.ForcePopped.AddListener(elementTwoForcePoppedMock.Listen);

            UnityEventListenerMock elementThreePushedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementThreePoppedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementThreeForcePoppedMock = new UnityEventListenerMock();
            GameObjectObservableStack.GameObjectElementEvents eventsThree = new GameObjectObservableStack.GameObjectElementEvents();
            eventsThree.Pushed.AddListener(elementThreePushedMock.Listen);
            eventsThree.Popped.AddListener(elementThreePoppedMock.Listen);
            eventsThree.ForcePopped.AddListener(elementThreeForcePoppedMock.Listen);

            GameObject objectOne = new GameObject();
            GameObject objectTwo = new GameObject();
            GameObject objectThree = new GameObject();

            subject.ElementEvents.Add(eventsOne);
            subject.ElementEvents.Add(eventsTwo);
            subject.ElementEvents.Add(eventsThree);

            subject.Push(objectOne);
            subject.Push(objectTwo);
            subject.Push(objectThree);

            elementOnePushedMock.Reset();
            elementOnePoppedMock.Reset();
            elementOneForcePoppedMock.Reset();
            elementTwoPushedMock.Reset();
            elementTwoPoppedMock.Reset();
            elementTwoForcePoppedMock.Reset();
            elementThreePushedMock.Reset();
            elementThreePoppedMock.Reset();
            elementThreeForcePoppedMock.Reset();

            Assert.IsFalse(elementOnePushedMock.Received);
            Assert.IsFalse(elementOnePoppedMock.Received);
            Assert.IsFalse(elementOneForcePoppedMock.Received);
            Assert.IsFalse(elementTwoPushedMock.Received);
            Assert.IsFalse(elementTwoPoppedMock.Received);
            Assert.IsFalse(elementTwoForcePoppedMock.Received);
            Assert.IsFalse(elementThreePushedMock.Received);
            Assert.IsFalse(elementThreePoppedMock.Received);
            Assert.IsFalse(elementThreeForcePoppedMock.Received);

            subject.PopAt(objectTwo);

            Assert.IsFalse(elementOnePushedMock.Received);
            Assert.IsFalse(elementOnePoppedMock.Received);
            Assert.IsFalse(elementOneForcePoppedMock.Received);
            Assert.IsFalse(elementTwoPushedMock.Received);
            Assert.IsTrue(elementTwoPoppedMock.Received);
            Assert.IsFalse(elementTwoForcePoppedMock.Received);
            Assert.IsFalse(elementThreePushedMock.Received);
            Assert.IsFalse(elementThreePoppedMock.Received);
            Assert.IsTrue(elementThreeForcePoppedMock.Received);

            Object.DestroyImmediate(objectOne);
            Object.DestroyImmediate(objectTwo);
            Object.DestroyImmediate(objectThree);
        }

        [Test]
        public void PopAtStart()
        {
            UnityEventListenerMock elementOnePushedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementOnePoppedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementOneForcePoppedMock = new UnityEventListenerMock();
            GameObjectObservableStack.GameObjectElementEvents eventsOne = new GameObjectObservableStack.GameObjectElementEvents();
            eventsOne.Pushed.AddListener(elementOnePushedMock.Listen);
            eventsOne.Popped.AddListener(elementOnePoppedMock.Listen);
            eventsOne.ForcePopped.AddListener(elementOneForcePoppedMock.Listen);

            UnityEventListenerMock elementTwoPushedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementTwoPoppedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementTwoForcePoppedMock = new UnityEventListenerMock();
            GameObjectObservableStack.GameObjectElementEvents eventsTwo = new GameObjectObservableStack.GameObjectElementEvents();
            eventsTwo.Pushed.AddListener(elementTwoPushedMock.Listen);
            eventsTwo.Popped.AddListener(elementTwoPoppedMock.Listen);
            eventsTwo.ForcePopped.AddListener(elementTwoForcePoppedMock.Listen);

            UnityEventListenerMock elementThreePushedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementThreePoppedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementThreeForcePoppedMock = new UnityEventListenerMock();
            GameObjectObservableStack.GameObjectElementEvents eventsThree = new GameObjectObservableStack.GameObjectElementEvents();
            eventsThree.Pushed.AddListener(elementThreePushedMock.Listen);
            eventsThree.Popped.AddListener(elementThreePoppedMock.Listen);
            eventsThree.ForcePopped.AddListener(elementThreeForcePoppedMock.Listen);

            GameObject objectOne = new GameObject();
            GameObject objectTwo = new GameObject();
            GameObject objectThree = new GameObject();

            subject.ElementEvents.Add(eventsOne);
            subject.ElementEvents.Add(eventsTwo);
            subject.ElementEvents.Add(eventsThree);

            subject.Push(objectOne);
            subject.Push(objectTwo);
            subject.Push(objectThree);

            elementOnePushedMock.Reset();
            elementOnePoppedMock.Reset();
            elementOneForcePoppedMock.Reset();
            elementTwoPushedMock.Reset();
            elementTwoPoppedMock.Reset();
            elementTwoForcePoppedMock.Reset();
            elementThreePushedMock.Reset();
            elementThreePoppedMock.Reset();
            elementThreeForcePoppedMock.Reset();

            Assert.IsFalse(elementOnePushedMock.Received);
            Assert.IsFalse(elementOnePoppedMock.Received);
            Assert.IsFalse(elementOneForcePoppedMock.Received);
            Assert.IsFalse(elementTwoPushedMock.Received);
            Assert.IsFalse(elementTwoPoppedMock.Received);
            Assert.IsFalse(elementTwoForcePoppedMock.Received);
            Assert.IsFalse(elementThreePushedMock.Received);
            Assert.IsFalse(elementThreePoppedMock.Received);
            Assert.IsFalse(elementThreeForcePoppedMock.Received);

            subject.PopAt(objectOne);

            Assert.IsFalse(elementOnePushedMock.Received);
            Assert.IsTrue(elementOnePoppedMock.Received);
            Assert.IsFalse(elementOneForcePoppedMock.Received);
            Assert.IsFalse(elementTwoPushedMock.Received);
            Assert.IsFalse(elementTwoPoppedMock.Received);
            Assert.IsTrue(elementTwoForcePoppedMock.Received);
            Assert.IsFalse(elementThreePushedMock.Received);
            Assert.IsFalse(elementThreePoppedMock.Received);
            Assert.IsTrue(elementThreeForcePoppedMock.Received);

            Object.DestroyImmediate(objectOne);
            Object.DestroyImmediate(objectTwo);
            Object.DestroyImmediate(objectThree);
        }

        [Test]
        public void AbortPop()
        {
            UnityEventListenerMock elementOnePushedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementOnePoppedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementOneForcePoppedMock = new UnityEventListenerMock();
            GameObjectObservableStack.GameObjectElementEvents eventsOne = new GameObjectObservableStack.GameObjectElementEvents();
            eventsOne.Pushed.AddListener(elementOnePushedMock.Listen);
            eventsOne.Popped.AddListener(elementOnePoppedMock.Listen);
            eventsOne.ForcePopped.AddListener(elementOneForcePoppedMock.Listen);

            UnityEventListenerMock elementTwoPushedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementTwoPoppedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementTwoForcePoppedMock = new UnityEventListenerMock();
            GameObjectObservableStack.GameObjectElementEvents eventsTwo = new GameObjectObservableStack.GameObjectElementEvents();
            eventsTwo.Pushed.AddListener(elementTwoPushedMock.Listen);
            eventsTwo.Popped.AddListener(elementTwoPoppedMock.Listen);
            eventsTwo.ForcePopped.AddListener(elementTwoForcePoppedMock.Listen);
            eventsTwo.ForcePopped.AddListener(AbortPopAction);

            UnityEventListenerMock elementThreePushedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementThreePoppedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementThreeForcePoppedMock = new UnityEventListenerMock();
            GameObjectObservableStack.GameObjectElementEvents eventsThree = new GameObjectObservableStack.GameObjectElementEvents();
            eventsThree.Pushed.AddListener(elementThreePushedMock.Listen);
            eventsThree.Popped.AddListener(elementThreePoppedMock.Listen);
            eventsThree.ForcePopped.AddListener(elementThreeForcePoppedMock.Listen);

            GameObject objectOne = new GameObject();
            GameObject objectTwo = new GameObject();
            GameObject objectThree = new GameObject();

            subject.ElementEvents.Add(eventsOne);
            subject.ElementEvents.Add(eventsTwo);
            subject.ElementEvents.Add(eventsThree);

            subject.Push(objectOne);
            subject.Push(objectTwo);
            subject.Push(objectThree);

            elementOnePushedMock.Reset();
            elementOnePoppedMock.Reset();
            elementOneForcePoppedMock.Reset();
            elementTwoPushedMock.Reset();
            elementTwoPoppedMock.Reset();
            elementTwoForcePoppedMock.Reset();
            elementThreePushedMock.Reset();
            elementThreePoppedMock.Reset();
            elementThreeForcePoppedMock.Reset();

            Assert.IsFalse(elementOnePushedMock.Received);
            Assert.IsFalse(elementOnePoppedMock.Received);
            Assert.IsFalse(elementOneForcePoppedMock.Received);
            Assert.IsFalse(elementTwoPushedMock.Received);
            Assert.IsFalse(elementTwoPoppedMock.Received);
            Assert.IsFalse(elementTwoForcePoppedMock.Received);
            Assert.IsFalse(elementThreePushedMock.Received);
            Assert.IsFalse(elementThreePoppedMock.Received);
            Assert.IsFalse(elementThreeForcePoppedMock.Received);

            subject.PopAt(objectOne);

            Assert.IsFalse(elementOnePushedMock.Received);
            Assert.IsFalse(elementOnePoppedMock.Received);
            Assert.IsFalse(elementOneForcePoppedMock.Received);
            Assert.IsFalse(elementTwoPushedMock.Received);
            Assert.IsFalse(elementTwoPoppedMock.Received);
            Assert.IsTrue(elementTwoForcePoppedMock.Received);
            Assert.IsFalse(elementThreePushedMock.Received);
            Assert.IsFalse(elementThreePoppedMock.Received);
            Assert.IsTrue(elementThreeForcePoppedMock.Received);

            Object.DestroyImmediate(objectOne);
            Object.DestroyImmediate(objectTwo);
            Object.DestroyImmediate(objectThree);
        }

        [Test]
        public void PopAtInvalid()
        {
            UnityEventListenerMock elementOnePushedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementOnePoppedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementOneForcePoppedMock = new UnityEventListenerMock();
            GameObjectObservableStack.GameObjectElementEvents eventsOne = new GameObjectObservableStack.GameObjectElementEvents();
            eventsOne.Pushed.AddListener(elementOnePushedMock.Listen);
            eventsOne.Popped.AddListener(elementOnePoppedMock.Listen);
            eventsOne.ForcePopped.AddListener(elementOneForcePoppedMock.Listen);

            GameObject objectOne = new GameObject();
            GameObject objectTwo = new GameObject();

            subject.ElementEvents.Add(eventsOne);

            subject.Push(objectOne);

            elementOnePushedMock.Reset();
            elementOnePoppedMock.Reset();
            elementOneForcePoppedMock.Reset();

            Assert.IsFalse(elementOnePushedMock.Received);
            Assert.IsFalse(elementOnePoppedMock.Received);
            Assert.IsFalse(elementOneForcePoppedMock.Received);

            subject.PopAt(objectTwo);

            Assert.IsFalse(elementOnePushedMock.Received);
            Assert.IsFalse(elementOnePoppedMock.Received);
            Assert.IsFalse(elementOneForcePoppedMock.Received);

            Object.DestroyImmediate(objectOne);
            Object.DestroyImmediate(objectTwo);
        }

        [Test]
        public void PopAtEmptyStack()
        {
            UnityEventListenerMock elementOnePushedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementOnePoppedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementOneForcePoppedMock = new UnityEventListenerMock();
            GameObjectObservableStack.GameObjectElementEvents eventsOne = new GameObjectObservableStack.GameObjectElementEvents();
            eventsOne.Pushed.AddListener(elementOnePushedMock.Listen);
            eventsOne.Popped.AddListener(elementOnePoppedMock.Listen);
            eventsOne.ForcePopped.AddListener(elementOneForcePoppedMock.Listen);

            GameObject objectOne = new GameObject();
            GameObject objectTwo = new GameObject();

            subject.ElementEvents.Add(eventsOne);

            elementOnePushedMock.Reset();
            elementOnePoppedMock.Reset();
            elementOneForcePoppedMock.Reset();

            Assert.IsFalse(elementOnePushedMock.Received);
            Assert.IsFalse(elementOnePoppedMock.Received);
            Assert.IsFalse(elementOneForcePoppedMock.Received);

            subject.PopAt(objectTwo);

            Assert.IsFalse(elementOnePushedMock.Received);
            Assert.IsFalse(elementOnePoppedMock.Received);
            Assert.IsFalse(elementOneForcePoppedMock.Received);

            Object.DestroyImmediate(objectOne);
            Object.DestroyImmediate(objectTwo);
        }

        [Test]
        public void PopAtInactiveGameObject()
        {
            UnityEventListenerMock elementOnePushedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementOnePoppedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementOneForcePoppedMock = new UnityEventListenerMock();
            GameObjectObservableStack.GameObjectElementEvents eventsOne = new GameObjectObservableStack.GameObjectElementEvents();
            eventsOne.Pushed.AddListener(elementOnePushedMock.Listen);
            eventsOne.Popped.AddListener(elementOnePoppedMock.Listen);
            eventsOne.ForcePopped.AddListener(elementOneForcePoppedMock.Listen);

            GameObject objectOne = new GameObject();

            subject.ElementEvents.Add(eventsOne);

            elementOnePushedMock.Reset();
            elementOnePoppedMock.Reset();
            elementOneForcePoppedMock.Reset();

            subject.gameObject.SetActive(false);
            subject.PopAt(objectOne);

            Assert.IsFalse(elementOnePushedMock.Received);
            Assert.IsFalse(elementOnePoppedMock.Received);
            Assert.IsFalse(elementOneForcePoppedMock.Received);

            Object.DestroyImmediate(objectOne);
        }

        [Test]
        public void PopAtInactiveComponent()
        {
            UnityEventListenerMock elementOnePushedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementOnePoppedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementOneForcePoppedMock = new UnityEventListenerMock();
            GameObjectObservableStack.GameObjectElementEvents eventsOne = new GameObjectObservableStack.GameObjectElementEvents();
            eventsOne.Pushed.AddListener(elementOnePushedMock.Listen);
            eventsOne.Popped.AddListener(elementOnePoppedMock.Listen);
            eventsOne.ForcePopped.AddListener(elementOneForcePoppedMock.Listen);

            GameObject objectOne = new GameObject();

            subject.ElementEvents.Add(eventsOne);

            elementOnePushedMock.Reset();
            elementOnePoppedMock.Reset();
            elementOneForcePoppedMock.Reset();

            subject.enabled = false;
            subject.PopAt(objectOne);

            Assert.IsFalse(elementOnePushedMock.Received);
            Assert.IsFalse(elementOnePoppedMock.Received);
            Assert.IsFalse(elementOneForcePoppedMock.Received);

            Object.DestroyImmediate(objectOne);
        }

        [Test]
        public void PopAtIndexMiddle()
        {
            UnityEventListenerMock elementOnePushedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementOnePoppedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementOneForcePoppedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementOneRestoredMock = new UnityEventListenerMock();
            GameObjectObservableStack.GameObjectElementEvents eventsOne = new GameObjectObservableStack.GameObjectElementEvents();
            eventsOne.Pushed.AddListener(elementOnePushedMock.Listen);
            eventsOne.Popped.AddListener(elementOnePoppedMock.Listen);
            eventsOne.ForcePopped.AddListener(elementOneForcePoppedMock.Listen);
            eventsOne.Restored.AddListener(elementOneRestoredMock.Listen);

            UnityEventListenerMock elementTwoPushedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementTwoPoppedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementTwoForcePoppedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementTwoRestoredMock = new UnityEventListenerMock();
            GameObjectObservableStack.GameObjectElementEvents eventsTwo = new GameObjectObservableStack.GameObjectElementEvents();
            eventsTwo.Pushed.AddListener(elementTwoPushedMock.Listen);
            eventsTwo.Popped.AddListener(elementTwoPoppedMock.Listen);
            eventsTwo.ForcePopped.AddListener(elementTwoForcePoppedMock.Listen);
            eventsTwo.Restored.AddListener(elementTwoRestoredMock.Listen);

            UnityEventListenerMock elementThreePushedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementThreePoppedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementThreeForcePoppedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementThreeRestoredMock = new UnityEventListenerMock();
            GameObjectObservableStack.GameObjectElementEvents eventsThree = new GameObjectObservableStack.GameObjectElementEvents();
            eventsThree.Pushed.AddListener(elementThreePushedMock.Listen);
            eventsThree.Popped.AddListener(elementThreePoppedMock.Listen);
            eventsThree.ForcePopped.AddListener(elementThreeForcePoppedMock.Listen);
            eventsThree.Restored.AddListener(elementThreeRestoredMock.Listen);

            GameObject objectOne = new GameObject();
            GameObject objectTwo = new GameObject();
            GameObject objectThree = new GameObject();

            subject.ElementEvents.Add(eventsOne);
            subject.ElementEvents.Add(eventsTwo);
            subject.ElementEvents.Add(eventsThree);

            subject.Push(objectOne);
            subject.Push(objectTwo);
            subject.Push(objectThree);

            elementOnePushedMock.Reset();
            elementOnePoppedMock.Reset();
            elementOneForcePoppedMock.Reset();
            elementTwoPushedMock.Reset();
            elementTwoPoppedMock.Reset();
            elementTwoForcePoppedMock.Reset();
            elementThreePushedMock.Reset();
            elementThreePoppedMock.Reset();
            elementThreeForcePoppedMock.Reset();

            Assert.IsFalse(elementOnePushedMock.Received);
            Assert.IsFalse(elementOnePoppedMock.Received);
            Assert.IsFalse(elementOneForcePoppedMock.Received);
            Assert.IsFalse(elementOneRestoredMock.Received);
            Assert.IsFalse(elementTwoPushedMock.Received);
            Assert.IsFalse(elementTwoPoppedMock.Received);
            Assert.IsFalse(elementTwoForcePoppedMock.Received);
            Assert.IsFalse(elementTwoRestoredMock.Received);
            Assert.IsFalse(elementThreePushedMock.Received);
            Assert.IsFalse(elementThreePoppedMock.Received);
            Assert.IsFalse(elementThreeForcePoppedMock.Received);
            Assert.IsFalse(elementThreeRestoredMock.Received);

            subject.PopAt(1);

            Assert.IsFalse(elementOnePushedMock.Received);
            Assert.IsFalse(elementOnePoppedMock.Received);
            Assert.IsFalse(elementOneForcePoppedMock.Received);
            Assert.IsTrue(elementOneRestoredMock.Received);
            Assert.IsFalse(elementTwoPushedMock.Received);
            Assert.IsTrue(elementTwoPoppedMock.Received);
            Assert.IsFalse(elementTwoForcePoppedMock.Received);
            Assert.IsFalse(elementTwoRestoredMock.Received);
            Assert.IsFalse(elementThreePushedMock.Received);
            Assert.IsFalse(elementThreePoppedMock.Received);
            Assert.IsTrue(elementThreeForcePoppedMock.Received);
            Assert.IsFalse(elementThreeRestoredMock.Received);

            Object.DestroyImmediate(objectOne);
            Object.DestroyImmediate(objectTwo);
            Object.DestroyImmediate(objectThree);
        }

        [Test]
        public void PopAtIndexInvalidOutOfBounds()
        {
            UnityEventListenerMock elementOnePushedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementOnePoppedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementOneForcePoppedMock = new UnityEventListenerMock();
            GameObjectObservableStack.GameObjectElementEvents eventsOne = new GameObjectObservableStack.GameObjectElementEvents();
            eventsOne.Pushed.AddListener(elementOnePushedMock.Listen);
            eventsOne.Popped.AddListener(elementOnePoppedMock.Listen);
            eventsOne.ForcePopped.AddListener(elementOneForcePoppedMock.Listen);

            GameObject objectOne = new GameObject();

            subject.ElementEvents.Add(eventsOne);

            subject.Push(objectOne);

            elementOnePushedMock.Reset();
            elementOnePoppedMock.Reset();
            elementOneForcePoppedMock.Reset();

            Assert.IsFalse(elementOnePushedMock.Received);
            Assert.IsFalse(elementOnePoppedMock.Received);
            Assert.IsFalse(elementOneForcePoppedMock.Received);

            subject.PopAt(1);

            Assert.IsFalse(elementOnePushedMock.Received);
            Assert.IsFalse(elementOnePoppedMock.Received);
            Assert.IsFalse(elementOneForcePoppedMock.Received);

            Object.DestroyImmediate(objectOne);
        }

        [Test]
        public void PopAtIndexAlreadyPopped()
        {
            UnityEventListenerMock elementOnePushedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementOnePoppedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementOneForcePoppedMock = new UnityEventListenerMock();
            GameObjectObservableStack.GameObjectElementEvents eventsOne = new GameObjectObservableStack.GameObjectElementEvents();
            eventsOne.Pushed.AddListener(elementOnePushedMock.Listen);
            eventsOne.Popped.AddListener(elementOnePoppedMock.Listen);
            eventsOne.ForcePopped.AddListener(elementOneForcePoppedMock.Listen);

            UnityEventListenerMock elementTwoPushedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementTwoPoppedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementTwoForcePoppedMock = new UnityEventListenerMock();
            GameObjectObservableStack.GameObjectElementEvents eventsTwo = new GameObjectObservableStack.GameObjectElementEvents();
            eventsTwo.Pushed.AddListener(elementTwoPushedMock.Listen);
            eventsTwo.Popped.AddListener(elementTwoPoppedMock.Listen);
            eventsTwo.ForcePopped.AddListener(elementTwoForcePoppedMock.Listen);

            UnityEventListenerMock elementThreePushedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementThreePoppedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementThreeForcePoppedMock = new UnityEventListenerMock();
            GameObjectObservableStack.GameObjectElementEvents eventsThree = new GameObjectObservableStack.GameObjectElementEvents();
            eventsThree.Pushed.AddListener(elementThreePushedMock.Listen);
            eventsThree.Popped.AddListener(elementThreePoppedMock.Listen);
            eventsThree.ForcePopped.AddListener(elementThreeForcePoppedMock.Listen);

            GameObject objectOne = new GameObject();
            GameObject objectTwo = new GameObject();
            GameObject objectThree = new GameObject();

            subject.ElementEvents.Add(eventsOne);
            subject.ElementEvents.Add(eventsTwo);
            subject.ElementEvents.Add(eventsThree);

            subject.Push(objectOne);
            subject.Push(objectTwo);
            subject.Push(objectThree);

            subject.PopAt(objectTwo);

            elementOnePushedMock.Reset();
            elementOnePoppedMock.Reset();
            elementOneForcePoppedMock.Reset();
            elementTwoPushedMock.Reset();
            elementTwoPoppedMock.Reset();
            elementTwoForcePoppedMock.Reset();
            elementThreePushedMock.Reset();
            elementThreePoppedMock.Reset();
            elementThreeForcePoppedMock.Reset();

            Assert.IsFalse(elementOnePushedMock.Received);
            Assert.IsFalse(elementOnePoppedMock.Received);
            Assert.IsFalse(elementOneForcePoppedMock.Received);
            Assert.IsFalse(elementTwoPushedMock.Received);
            Assert.IsFalse(elementTwoPoppedMock.Received);
            Assert.IsFalse(elementTwoForcePoppedMock.Received);
            Assert.IsFalse(elementThreePushedMock.Received);
            Assert.IsFalse(elementThreePoppedMock.Received);
            Assert.IsFalse(elementThreeForcePoppedMock.Received);

            subject.PopAt(1);

            Assert.IsFalse(elementOnePushedMock.Received);
            Assert.IsFalse(elementOnePoppedMock.Received);
            Assert.IsFalse(elementOneForcePoppedMock.Received);
            Assert.IsFalse(elementTwoPushedMock.Received);
            Assert.IsFalse(elementTwoPoppedMock.Received);
            Assert.IsFalse(elementTwoForcePoppedMock.Received);
            Assert.IsFalse(elementThreePushedMock.Received);
            Assert.IsFalse(elementThreePoppedMock.Received);
            Assert.IsFalse(elementThreeForcePoppedMock.Received);

            Object.DestroyImmediate(objectOne);
            Object.DestroyImmediate(objectTwo);
            Object.DestroyImmediate(objectThree);
        }

        protected void AbortPopAction(GameObject obj)
        {
            subject.AbortPop();
        }
    }
}