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
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void ContainsFound()
        {
            UnityEventListenerMock elementFoundMock = new UnityEventListenerMock();
            UnityEventListenerMock elementNotFoundMock = new UnityEventListenerMock();
            subject.Found.AddListener(elementFoundMock.Listen);
            subject.NotFound.AddListener(elementNotFoundMock.Listen);

            GameObject elementOne = new GameObject();

            subject.Add(elementOne);

            Assert.IsFalse(elementFoundMock.Received);
            Assert.IsFalse(elementNotFoundMock.Received);

            subject.Contains(elementOne);

            Assert.IsTrue(elementFoundMock.Received);
            Assert.IsFalse(elementNotFoundMock.Received);

            Object.DestroyImmediate(elementOne);
        }

        [Test]
        public void ContainsNotFound()
        {
            UnityEventListenerMock elementFoundMock = new UnityEventListenerMock();
            UnityEventListenerMock elementNotFoundMock = new UnityEventListenerMock();
            subject.Found.AddListener(elementFoundMock.Listen);
            subject.NotFound.AddListener(elementNotFoundMock.Listen);

            GameObject elementOne = new GameObject();
            GameObject elementTwo = new GameObject();

            subject.Add(elementOne);

            Assert.IsFalse(elementFoundMock.Received);
            Assert.IsFalse(elementNotFoundMock.Received);

            subject.Contains(elementTwo);

            Assert.IsFalse(elementFoundMock.Received);
            Assert.IsTrue(elementNotFoundMock.Received);

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

            Assert.IsEmpty(subject.ReadOnlyElements);

            subject.Add(elementOne);

            Assert.AreEqual(1, subject.ReadOnlyElements.Count);
            Assert.IsTrue(becamePopulatedMock.Received);
            Assert.IsTrue(elementAddedMock.Received);
            Assert.IsFalse(elementRemovedMock.Received);
            Assert.IsFalse(becameEmptyMock.Received);

            becamePopulatedMock.Reset();
            elementAddedMock.Reset();
            elementRemovedMock.Reset();
            becameEmptyMock.Reset();

            subject.Add(elementTwo);

            Assert.AreEqual(2, subject.ReadOnlyElements.Count);
            Assert.AreEqual(elementTwo, subject.ReadOnlyElements[1]);

            Assert.IsFalse(becamePopulatedMock.Received);
            Assert.IsTrue(elementAddedMock.Received);
            Assert.IsFalse(elementRemovedMock.Received);
            Assert.IsFalse(becameEmptyMock.Received);

            Object.DestroyImmediate(elementOne);
            Object.DestroyImmediate(elementTwo);
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

            Assert.IsEmpty(subject.ReadOnlyElements);

            subject.AddAt(elementOne, 0);

            Assert.AreEqual(1, subject.ReadOnlyElements.Count);
            Assert.IsTrue(becamePopulatedMock.Received);
            Assert.IsTrue(elementAddedMock.Received);
            Assert.IsFalse(elementRemovedMock.Received);
            Assert.IsFalse(becameEmptyMock.Received);

            becamePopulatedMock.Reset();
            elementAddedMock.Reset();
            elementRemovedMock.Reset();
            becameEmptyMock.Reset();

            subject.AddAt(elementTwo, 0);

            Assert.AreEqual(2, subject.ReadOnlyElements.Count);
            Assert.AreEqual(elementTwo, subject.ReadOnlyElements[0]);

            Assert.IsFalse(becamePopulatedMock.Received);
            Assert.IsTrue(elementAddedMock.Received);
            Assert.IsFalse(elementRemovedMock.Received);
            Assert.IsFalse(becameEmptyMock.Received);

            Object.DestroyImmediate(elementOne);
            Object.DestroyImmediate(elementTwo);
        }

        [Test]
        public void AddAtCurrentIndex()
        {
            UnityEventListenerMock becamePopulatedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementAddedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementRemovedMock = new UnityEventListenerMock();
            UnityEventListenerMock becameEmptyMock = new UnityEventListenerMock();
            subject.BecamePopulated.AddListener(becamePopulatedMock.Listen);
            subject.ElementAdded.AddListener(elementAddedMock.Listen);
            subject.ElementRemoved.AddListener(elementRemovedMock.Listen);
            subject.BecameEmpty.AddListener(becameEmptyMock.Listen);

            subject.CurrentIndex = 0;

            GameObject elementOne = new GameObject();
            GameObject elementTwo = new GameObject();

            Assert.IsEmpty(subject.ReadOnlyElements);

            subject.AddAtCurrentIndex(elementOne);

            Assert.AreEqual(1, subject.ReadOnlyElements.Count);
            Assert.IsTrue(becamePopulatedMock.Received);
            Assert.IsTrue(elementAddedMock.Received);
            Assert.IsFalse(elementRemovedMock.Received);
            Assert.IsFalse(becameEmptyMock.Received);

            becamePopulatedMock.Reset();
            elementAddedMock.Reset();
            elementRemovedMock.Reset();
            becameEmptyMock.Reset();

            subject.AddAtCurrentIndex(elementTwo);
            Assert.AreEqual(2, subject.ReadOnlyElements.Count);
            Assert.AreEqual(elementTwo, subject.ReadOnlyElements[0]);

            Assert.IsFalse(becamePopulatedMock.Received);
            Assert.IsTrue(elementAddedMock.Received);
            Assert.IsFalse(elementRemovedMock.Received);
            Assert.IsFalse(becameEmptyMock.Received);

            Object.DestroyImmediate(elementOne);
            Object.DestroyImmediate(elementTwo);
        }

        [Test]
        public void SetAt()
        {
            UnityEventListenerMock elementAddedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementRemovedMock = new UnityEventListenerMock();
            subject.ElementAdded.AddListener(elementAddedMock.Listen);
            subject.ElementRemoved.AddListener(elementRemovedMock.Listen);

            GameObject elementOne = new GameObject();
            GameObject elementTwo = new GameObject();
            GameObject elementThree = new GameObject();
            GameObject elementFour = new GameObject();

            subject.Add(elementOne);
            subject.Add(elementTwo);
            subject.Add(elementThree);

            elementAddedMock.Reset();
            elementRemovedMock.Reset();

            Assert.AreEqual(elementOne, subject.ReadOnlyElements[0]);
            Assert.AreEqual(elementTwo, subject.ReadOnlyElements[1]);
            Assert.AreEqual(elementThree, subject.ReadOnlyElements[2]);

            subject.SetAt(elementFour, 1);

            Assert.AreEqual(elementOne, subject.ReadOnlyElements[0]);
            Assert.AreEqual(elementFour, subject.ReadOnlyElements[1]);
            Assert.AreEqual(elementThree, subject.ReadOnlyElements[2]);

            Assert.IsTrue(elementAddedMock.Received);
            Assert.IsTrue(elementRemovedMock.Received);

            Object.DestroyImmediate(elementOne);
            Object.DestroyImmediate(elementTwo);
            Object.DestroyImmediate(elementThree);
            Object.DestroyImmediate(elementFour);
        }

        [Test]
        public void SetAtCurrentIndex()
        {
            UnityEventListenerMock elementAddedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementRemovedMock = new UnityEventListenerMock();
            subject.ElementAdded.AddListener(elementAddedMock.Listen);
            subject.ElementRemoved.AddListener(elementRemovedMock.Listen);

            subject.CurrentIndex = 1;

            GameObject elementOne = new GameObject();
            GameObject elementTwo = new GameObject();
            GameObject elementThree = new GameObject();
            GameObject elementFour = new GameObject();

            subject.Add(elementOne);
            subject.Add(elementTwo);
            subject.Add(elementThree);

            elementAddedMock.Reset();
            elementRemovedMock.Reset();

            Assert.AreEqual(elementOne, subject.ReadOnlyElements[0]);
            Assert.AreEqual(elementTwo, subject.ReadOnlyElements[1]);
            Assert.AreEqual(elementThree, subject.ReadOnlyElements[2]);

            subject.SetAtCurrentIndex(elementFour);

            Assert.AreEqual(elementOne, subject.ReadOnlyElements[0]);
            Assert.AreEqual(elementFour, subject.ReadOnlyElements[1]);
            Assert.AreEqual(elementThree, subject.ReadOnlyElements[2]);

            Assert.IsTrue(elementAddedMock.Received);
            Assert.IsTrue(elementRemovedMock.Received);

            Object.DestroyImmediate(elementOne);
            Object.DestroyImmediate(elementTwo);
            Object.DestroyImmediate(elementThree);
            Object.DestroyImmediate(elementFour);
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

            subject.Remove(elementOne);

            Assert.IsFalse(becamePopulatedMock.Received);
            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsFalse(elementRemovedMock.Received);
            Assert.IsFalse(becameEmptyMock.Received);

            subject.Add(elementOne);
            subject.Add(elementTwo);
            subject.Add(elementOne);

            becamePopulatedMock.Reset();
            elementAddedMock.Reset();
            elementRemovedMock.Reset();
            becameEmptyMock.Reset();

            Assert.AreEqual(3, subject.ReadOnlyElements.Count);

            subject.Remove(elementOne);

            Assert.IsFalse(becamePopulatedMock.Received);
            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsTrue(elementRemovedMock.Received);
            Assert.IsFalse(becameEmptyMock.Received);

            Assert.AreEqual(2, subject.ReadOnlyElements.Count);
            Assert.AreEqual(elementTwo, subject.ReadOnlyElements[0]);
            Assert.AreEqual(elementOne, subject.ReadOnlyElements[1]);

            becamePopulatedMock.Reset();
            elementAddedMock.Reset();
            elementRemovedMock.Reset();
            becameEmptyMock.Reset();

            subject.Remove(elementOne);

            Assert.IsFalse(becamePopulatedMock.Received);
            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsTrue(elementRemovedMock.Received);
            Assert.IsFalse(becameEmptyMock.Received);

            Assert.AreEqual(1, subject.ReadOnlyElements.Count);
            Assert.AreEqual(elementTwo, subject.ReadOnlyElements[0]);

            becamePopulatedMock.Reset();
            elementAddedMock.Reset();
            elementRemovedMock.Reset();
            becameEmptyMock.Reset();

            subject.Remove(elementTwo);

            Assert.IsFalse(becamePopulatedMock.Received);
            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsTrue(elementRemovedMock.Received);
            Assert.IsTrue(becameEmptyMock.Received);

            Assert.AreEqual(0, subject.ReadOnlyElements.Count);

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

            subject.RemoveLastOccurrence(elementOne);

            Assert.IsFalse(becamePopulatedMock.Received);
            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsFalse(elementRemovedMock.Received);
            Assert.IsFalse(becameEmptyMock.Received);

            subject.Add(elementOne);
            subject.Add(elementTwo);
            subject.Add(elementOne);

            becamePopulatedMock.Reset();
            elementAddedMock.Reset();
            elementRemovedMock.Reset();
            becameEmptyMock.Reset();

            Assert.AreEqual(3, subject.ReadOnlyElements.Count);

            subject.RemoveLastOccurrence(elementOne);

            Assert.IsFalse(becamePopulatedMock.Received);
            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsTrue(elementRemovedMock.Received);
            Assert.IsFalse(becameEmptyMock.Received);

            Assert.AreEqual(2, subject.ReadOnlyElements.Count);
            Assert.AreEqual(elementOne, subject.ReadOnlyElements[0]);
            Assert.AreEqual(elementTwo, subject.ReadOnlyElements[1]);

            becamePopulatedMock.Reset();
            elementAddedMock.Reset();
            elementRemovedMock.Reset();
            becameEmptyMock.Reset();

            subject.RemoveLastOccurrence(elementOne);

            Assert.IsFalse(becamePopulatedMock.Received);
            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsTrue(elementRemovedMock.Received);
            Assert.IsFalse(becameEmptyMock.Received);

            Assert.AreEqual(1, subject.ReadOnlyElements.Count);
            Assert.AreEqual(elementTwo, subject.ReadOnlyElements[0]);

            becamePopulatedMock.Reset();
            elementAddedMock.Reset();
            elementRemovedMock.Reset();
            becameEmptyMock.Reset();

            subject.RemoveLastOccurrence(elementTwo);

            Assert.IsFalse(becamePopulatedMock.Received);
            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsTrue(elementRemovedMock.Received);
            Assert.IsTrue(becameEmptyMock.Received);

            Assert.AreEqual(0, subject.ReadOnlyElements.Count);

            becamePopulatedMock.Reset();
            elementAddedMock.Reset();
            elementRemovedMock.Reset();
            becameEmptyMock.Reset();

            Object.DestroyImmediate(elementOne);
            Object.DestroyImmediate(elementTwo);
        }

        [Test]
        public void RemoveAt()
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
            GameObject elementThree = new GameObject();

            Assert.AreEqual(0, subject.ReadOnlyElements.Count);

            Assert.IsFalse(becamePopulatedMock.Received);
            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsFalse(elementRemovedMock.Received);
            Assert.IsFalse(becameEmptyMock.Received);

            subject.Add(elementOne);
            subject.Add(elementTwo);
            subject.Add(elementThree);

            becamePopulatedMock.Reset();
            elementAddedMock.Reset();
            elementRemovedMock.Reset();
            becameEmptyMock.Reset();

            Assert.AreEqual(3, subject.ReadOnlyElements.Count);

            subject.RemoveAt(1);

            Assert.IsFalse(becamePopulatedMock.Received);
            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsTrue(elementRemovedMock.Received);
            Assert.IsFalse(becameEmptyMock.Received);

            Assert.AreEqual(2, subject.ReadOnlyElements.Count);
            Assert.AreEqual(elementOne, subject.ReadOnlyElements[0]);
            Assert.AreEqual(elementThree, subject.ReadOnlyElements[1]);

            becamePopulatedMock.Reset();
            elementAddedMock.Reset();
            elementRemovedMock.Reset();
            becameEmptyMock.Reset();

            Object.DestroyImmediate(elementOne);
            Object.DestroyImmediate(elementTwo);
            Object.DestroyImmediate(elementThree);
        }

        [Test]
        public void RemoveAtCurrentIndex()
        {
            UnityEventListenerMock becamePopulatedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementAddedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementRemovedMock = new UnityEventListenerMock();
            UnityEventListenerMock becameEmptyMock = new UnityEventListenerMock();
            subject.BecamePopulated.AddListener(becamePopulatedMock.Listen);
            subject.ElementAdded.AddListener(elementAddedMock.Listen);
            subject.ElementRemoved.AddListener(elementRemovedMock.Listen);
            subject.BecameEmpty.AddListener(becameEmptyMock.Listen);

            subject.CurrentIndex = 1;

            GameObject elementOne = new GameObject();
            GameObject elementTwo = new GameObject();
            GameObject elementThree = new GameObject();

            Assert.AreEqual(0, subject.ReadOnlyElements.Count);

            Assert.IsFalse(becamePopulatedMock.Received);
            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsFalse(elementRemovedMock.Received);
            Assert.IsFalse(becameEmptyMock.Received);

            subject.Add(elementOne);
            subject.Add(elementTwo);
            subject.Add(elementThree);

            becamePopulatedMock.Reset();
            elementAddedMock.Reset();
            elementRemovedMock.Reset();
            becameEmptyMock.Reset();

            Assert.AreEqual(3, subject.ReadOnlyElements.Count);

            subject.RemoveAtCurrentIndex();

            Assert.IsFalse(becamePopulatedMock.Received);
            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsTrue(elementRemovedMock.Received);
            Assert.IsFalse(becameEmptyMock.Received);

            Assert.AreEqual(2, subject.ReadOnlyElements.Count);
            Assert.AreEqual(elementOne, subject.ReadOnlyElements[0]);
            Assert.AreEqual(elementThree, subject.ReadOnlyElements[1]);

            becamePopulatedMock.Reset();
            elementAddedMock.Reset();
            elementRemovedMock.Reset();
            becameEmptyMock.Reset();

            Object.DestroyImmediate(elementOne);
            Object.DestroyImmediate(elementTwo);
            Object.DestroyImmediate(elementThree);
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

            subject.Clear(false);

            Assert.IsFalse(becamePopulatedMock.Received);
            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsFalse(elementRemovedMock.Received);
            Assert.IsFalse(becameEmptyMock.Received);

            subject.Add(elementOne);
            subject.Add(elementTwo);
            subject.Add(elementOne);

            becamePopulatedMock.Reset();
            elementAddedMock.Reset();
            elementRemovedMock.Reset();
            becameEmptyMock.Reset();

            Assert.AreEqual(3, subject.ReadOnlyElements.Count);

            subject.Clear(false);

            Assert.IsFalse(becamePopulatedMock.Received);
            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsTrue(elementRemovedMock.Received);
            Assert.IsTrue(becameEmptyMock.Received);

            Assert.AreEqual(0, subject.ReadOnlyElements.Count);

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

            subject.Clear(true);

            Assert.IsFalse(becamePopulatedMock.Received);
            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsFalse(elementRemovedMock.Received);
            Assert.IsFalse(becameEmptyMock.Received);

            subject.Add(elementOne);
            subject.Add(elementTwo);
            subject.Add(elementOne);

            becamePopulatedMock.Reset();
            elementAddedMock.Reset();
            elementRemovedMock.Reset();
            becameEmptyMock.Reset();

            Assert.AreEqual(3, subject.ReadOnlyElements.Count);

            subject.Clear(true);

            Assert.IsFalse(becamePopulatedMock.Received);
            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsTrue(elementRemovedMock.Received);
            Assert.IsTrue(becameEmptyMock.Received);

            Assert.AreEqual(0, subject.ReadOnlyElements.Count);

            Object.DestroyImmediate(elementOne);
            Object.DestroyImmediate(elementTwo);
        }

        [Test]
        public void EmitsOnStart()
        {
            GameObjectObservableListMock mock = containingObject.AddComponent<GameObjectObservableListMock>();
            GameObject elementOne = new GameObject();
            GameObject elementTwo = new GameObject();
            mock.Add(elementOne);
            mock.Add(elementTwo);

            UnityEventListenerMock becamePopulatedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementAddedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementRemovedMock = new UnityEventListenerMock();
            UnityEventListenerMock becameEmptyMock = new UnityEventListenerMock();
            mock.BecamePopulated.AddListener(becamePopulatedMock.Listen);
            mock.ElementAdded.AddListener(elementAddedMock.Listen);
            mock.ElementRemoved.AddListener(elementRemovedMock.Listen);
            mock.BecameEmpty.AddListener(becameEmptyMock.Listen);

            mock.ManualStart();

            Assert.IsTrue(becamePopulatedMock.Received);
            Assert.IsTrue(elementAddedMock.Received);
            Assert.IsFalse(elementRemovedMock.Received);
            Assert.IsFalse(becameEmptyMock.Received);

            Object.DestroyImmediate(elementOne);
            Object.DestroyImmediate(elementTwo);
        }

        private sealed class GameObjectObservableListMock : GameObjectObservableList
        {
            public void ManualStart()
            {
                Start();
            }
        }
    }
}