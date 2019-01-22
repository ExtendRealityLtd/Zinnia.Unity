using Zinnia.Data.Collection;

namespace Test.Zinnia.Data.Collection
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;

    public class GameObjectObservableListTest
    {
        private GameObject containingObject;
        private GameObjectObservableList subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<GameObjectObservableList>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void AddToStart()
        {
            UnityEventListenerMock becamePopulatedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementAddedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementRemovedMock = new UnityEventListenerMock();
            UnityEventListenerMock becameEmptyMock = new UnityEventListenerMock();
            subject.BecamePopulated.AddListener(becamePopulatedMock.Listen);
            subject.ElementAdded.AddListener(elementAddedMock.Listen);
            subject.ElementRemoved.AddListener(elementRemovedMock.Listen);
            subject.BecameEmpty.AddListener(becameEmptyMock.Listen);
            GameObject elementOne = new GameObject();
            GameObject elementTwo = new GameObject();

            Assert.IsEmpty(subject.Elements);

            subject.AddToStart(elementOne);

            Assert.AreEqual(1, subject.Elements.Count);
            Assert.IsTrue(becamePopulatedMock.Received);
            Assert.IsTrue(elementAddedMock.Received);
            Assert.IsFalse(elementRemovedMock.Received);
            Assert.IsFalse(becameEmptyMock.Received);

            becamePopulatedMock.Reset();
            elementAddedMock.Reset();
            elementRemovedMock.Reset();
            becameEmptyMock.Reset();

            subject.AddToStart(elementTwo);
            Assert.AreEqual(2, subject.Elements.Count);
            Assert.AreEqual(elementTwo, subject.Elements[0]);

            Assert.IsFalse(becamePopulatedMock.Received);
            Assert.IsTrue(elementAddedMock.Received);
            Assert.IsFalse(elementRemovedMock.Received);
            Assert.IsFalse(becameEmptyMock.Received);

            Object.DestroyImmediate(elementOne);
            Object.DestroyImmediate(elementTwo);
        }

        [Test]
        public void AddToEnd()
        {
            UnityEventListenerMock becamePopulatedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementAddedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementRemovedMock = new UnityEventListenerMock();
            UnityEventListenerMock becameEmptyMock = new UnityEventListenerMock();
            subject.BecamePopulated.AddListener(becamePopulatedMock.Listen);
            subject.ElementAdded.AddListener(elementAddedMock.Listen);
            subject.ElementRemoved.AddListener(elementRemovedMock.Listen);
            subject.BecameEmpty.AddListener(becameEmptyMock.Listen);
            GameObject elementOne = new GameObject();
            GameObject elementTwo = new GameObject();

            Assert.IsEmpty(subject.Elements);

            subject.AddToEnd(elementOne);

            Assert.AreEqual(1, subject.Elements.Count);
            Assert.IsTrue(becamePopulatedMock.Received);
            Assert.IsTrue(elementAddedMock.Received);
            Assert.IsFalse(elementRemovedMock.Received);
            Assert.IsFalse(becameEmptyMock.Received);

            becamePopulatedMock.Reset();
            elementAddedMock.Reset();
            elementRemovedMock.Reset();
            becameEmptyMock.Reset();

            subject.AddToEnd(elementTwo);
            Assert.AreEqual(2, subject.Elements.Count);
            Assert.AreEqual(elementTwo, subject.Elements[1]);

            Assert.IsFalse(becamePopulatedMock.Received);
            Assert.IsTrue(elementAddedMock.Received);
            Assert.IsFalse(elementRemovedMock.Received);
            Assert.IsFalse(becameEmptyMock.Received);

            Object.DestroyImmediate(elementOne);
            Object.DestroyImmediate(elementTwo);
        }

        [Test]
        public void RemoveFirst()
        {
            UnityEventListenerMock becamePopulatedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementAddedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementRemovedMock = new UnityEventListenerMock();
            UnityEventListenerMock becameEmptyMock = new UnityEventListenerMock();
            subject.BecamePopulated.AddListener(becamePopulatedMock.Listen);
            subject.ElementAdded.AddListener(elementAddedMock.Listen);
            subject.ElementRemoved.AddListener(elementRemovedMock.Listen);
            subject.BecameEmpty.AddListener(becameEmptyMock.Listen);
            GameObject elementOne = new GameObject();
            GameObject elementTwo = new GameObject();

            subject.AddToEnd(elementOne);
            subject.AddToEnd(elementTwo);
            subject.AddToEnd(elementOne);

            becamePopulatedMock.Reset();
            elementAddedMock.Reset();
            elementRemovedMock.Reset();
            becameEmptyMock.Reset();

            Assert.AreEqual(3, subject.Elements.Count);

            subject.RemoveFirst(elementOne);

            Assert.IsFalse(becamePopulatedMock.Received);
            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsTrue(elementRemovedMock.Received);
            Assert.IsFalse(becameEmptyMock.Received);

            Assert.AreEqual(2, subject.Elements.Count);
            Assert.AreEqual(elementTwo, subject.Elements[0]);
            Assert.AreEqual(elementOne, subject.Elements[1]);

            becamePopulatedMock.Reset();
            elementAddedMock.Reset();
            elementRemovedMock.Reset();
            becameEmptyMock.Reset();

            subject.RemoveFirst(elementOne);

            Assert.IsFalse(becamePopulatedMock.Received);
            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsTrue(elementRemovedMock.Received);
            Assert.IsFalse(becameEmptyMock.Received);

            Assert.AreEqual(1, subject.Elements.Count);
            Assert.AreEqual(elementTwo, subject.Elements[0]);

            becamePopulatedMock.Reset();
            elementAddedMock.Reset();
            elementRemovedMock.Reset();
            becameEmptyMock.Reset();

            subject.RemoveFirst(elementTwo);

            Assert.IsFalse(becamePopulatedMock.Received);
            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsTrue(elementRemovedMock.Received);
            Assert.IsTrue(becameEmptyMock.Received);

            Assert.AreEqual(0, subject.Elements.Count);

            becamePopulatedMock.Reset();
            elementAddedMock.Reset();
            elementRemovedMock.Reset();
            becameEmptyMock.Reset();

            Object.DestroyImmediate(elementOne);
            Object.DestroyImmediate(elementTwo);
        }

        [Test]
        public void RemoveLast()
        {
            UnityEventListenerMock becamePopulatedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementAddedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementRemovedMock = new UnityEventListenerMock();
            UnityEventListenerMock becameEmptyMock = new UnityEventListenerMock();
            subject.BecamePopulated.AddListener(becamePopulatedMock.Listen);
            subject.ElementAdded.AddListener(elementAddedMock.Listen);
            subject.ElementRemoved.AddListener(elementRemovedMock.Listen);
            subject.BecameEmpty.AddListener(becameEmptyMock.Listen);
            GameObject elementOne = new GameObject();
            GameObject elementTwo = new GameObject();

            subject.AddToEnd(elementOne);
            subject.AddToEnd(elementTwo);
            subject.AddToEnd(elementOne);

            becamePopulatedMock.Reset();
            elementAddedMock.Reset();
            elementRemovedMock.Reset();
            becameEmptyMock.Reset();

            Assert.AreEqual(3, subject.Elements.Count);

            subject.RemoveLast(elementOne);

            Assert.IsFalse(becamePopulatedMock.Received);
            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsTrue(elementRemovedMock.Received);
            Assert.IsFalse(becameEmptyMock.Received);

            Assert.AreEqual(2, subject.Elements.Count);
            Assert.AreEqual(elementOne, subject.Elements[0]);
            Assert.AreEqual(elementTwo, subject.Elements[1]);

            becamePopulatedMock.Reset();
            elementAddedMock.Reset();
            elementRemovedMock.Reset();
            becameEmptyMock.Reset();

            subject.RemoveLast(elementOne);

            Assert.IsFalse(becamePopulatedMock.Received);
            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsTrue(elementRemovedMock.Received);
            Assert.IsFalse(becameEmptyMock.Received);

            Assert.AreEqual(1, subject.Elements.Count);
            Assert.AreEqual(elementTwo, subject.Elements[0]);

            becamePopulatedMock.Reset();
            elementAddedMock.Reset();
            elementRemovedMock.Reset();
            becameEmptyMock.Reset();

            subject.RemoveLast(elementTwo);

            Assert.IsFalse(becamePopulatedMock.Received);
            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsTrue(elementRemovedMock.Received);
            Assert.IsTrue(becameEmptyMock.Received);

            Assert.AreEqual(0, subject.Elements.Count);

            becamePopulatedMock.Reset();
            elementAddedMock.Reset();
            elementRemovedMock.Reset();
            becameEmptyMock.Reset();

            Object.DestroyImmediate(elementOne);
            Object.DestroyImmediate(elementTwo);
        }

        [Test]
        public void ClearFromBack()
        {
            UnityEventListenerMock becamePopulatedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementAddedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementRemovedMock = new UnityEventListenerMock();
            UnityEventListenerMock becameEmptyMock = new UnityEventListenerMock();
            subject.BecamePopulated.AddListener(becamePopulatedMock.Listen);
            subject.ElementAdded.AddListener(elementAddedMock.Listen);
            subject.ElementRemoved.AddListener(elementRemovedMock.Listen);
            subject.BecameEmpty.AddListener(becameEmptyMock.Listen);
            GameObject elementOne = new GameObject();
            GameObject elementTwo = new GameObject();

            subject.AddToEnd(elementOne);
            subject.AddToEnd(elementTwo);
            subject.AddToEnd(elementOne);

            becamePopulatedMock.Reset();
            elementAddedMock.Reset();
            elementRemovedMock.Reset();
            becameEmptyMock.Reset();

            Assert.AreEqual(3, subject.Elements.Count);

            subject.Clear(false);

            Assert.IsFalse(becamePopulatedMock.Received);
            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsTrue(elementRemovedMock.Received);
            Assert.IsTrue(becameEmptyMock.Received);

            Assert.AreEqual(0, subject.Elements.Count);

            Object.DestroyImmediate(elementOne);
            Object.DestroyImmediate(elementTwo);
        }

        [Test]
        public void ClearFromFront()
        {
            UnityEventListenerMock becamePopulatedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementAddedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementRemovedMock = new UnityEventListenerMock();
            UnityEventListenerMock becameEmptyMock = new UnityEventListenerMock();
            subject.BecamePopulated.AddListener(becamePopulatedMock.Listen);
            subject.ElementAdded.AddListener(elementAddedMock.Listen);
            subject.ElementRemoved.AddListener(elementRemovedMock.Listen);
            subject.BecameEmpty.AddListener(becameEmptyMock.Listen);
            GameObject elementOne = new GameObject();
            GameObject elementTwo = new GameObject();

            subject.AddToEnd(elementOne);
            subject.AddToEnd(elementTwo);
            subject.AddToEnd(elementOne);

            becamePopulatedMock.Reset();
            elementAddedMock.Reset();
            elementRemovedMock.Reset();
            becameEmptyMock.Reset();

            Assert.AreEqual(3, subject.Elements.Count);

            subject.Clear(true);

            Assert.IsFalse(becamePopulatedMock.Received);
            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsTrue(elementRemovedMock.Received);
            Assert.IsTrue(becameEmptyMock.Received);

            Assert.AreEqual(0, subject.Elements.Count);

            Object.DestroyImmediate(elementOne);
            Object.DestroyImmediate(elementTwo);
        }
    }
}