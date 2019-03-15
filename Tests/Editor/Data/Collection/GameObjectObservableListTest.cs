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
            UnityEventListenerMock populatedMock = new UnityEventListenerMock();
            UnityEventListenerMock addedMock = new UnityEventListenerMock();
            UnityEventListenerMock removedMock = new UnityEventListenerMock();
            UnityEventListenerMock emptiedMock = new UnityEventListenerMock();
            subject.Populated.AddListener(populatedMock.Listen);
            subject.Added.AddListener(addedMock.Listen);
            subject.Removed.AddListener(removedMock.Listen);
            subject.Emptied.AddListener(emptiedMock.Listen);

            GameObject elementOne = new GameObject();
            GameObject elementTwo = new GameObject();

            Assert.IsEmpty(subject.NonSubscribableElements);

            subject.Add(elementOne);

            Assert.AreEqual(1, subject.NonSubscribableElements.Count);
            Assert.IsTrue(populatedMock.Received);
            Assert.IsTrue(addedMock.Received);
            Assert.IsFalse(removedMock.Received);
            Assert.IsFalse(emptiedMock.Received);

            populatedMock.Reset();
            addedMock.Reset();
            removedMock.Reset();
            emptiedMock.Reset();

            subject.Add(elementTwo);

            Assert.AreEqual(2, subject.NonSubscribableElements.Count);
            Assert.AreEqual(elementTwo, subject.NonSubscribableElements[1]);

            Assert.IsFalse(populatedMock.Received);
            Assert.IsTrue(addedMock.Received);
            Assert.IsFalse(removedMock.Received);
            Assert.IsFalse(emptiedMock.Received);

            Object.DestroyImmediate(elementOne);
            Object.DestroyImmediate(elementTwo);
        }

        [Test]
        public void AddUnique()
        {
            UnityEventListenerMock populatedMock = new UnityEventListenerMock();
            UnityEventListenerMock addedMock = new UnityEventListenerMock();
            UnityEventListenerMock removedMock = new UnityEventListenerMock();
            UnityEventListenerMock emptiedMock = new UnityEventListenerMock();
            subject.Populated.AddListener(populatedMock.Listen);
            subject.Added.AddListener(addedMock.Listen);
            subject.Removed.AddListener(removedMock.Listen);
            subject.Emptied.AddListener(emptiedMock.Listen);

            GameObject elementOne = new GameObject();
            GameObject elementTwo = new GameObject();

            Assert.IsEmpty(subject.NonSubscribableElements);

            subject.AddUnique(elementOne);

            Assert.AreEqual(1, subject.NonSubscribableElements.Count);
            Assert.IsTrue(populatedMock.Received);
            Assert.IsTrue(addedMock.Received);
            Assert.IsFalse(removedMock.Received);
            Assert.IsFalse(emptiedMock.Received);

            populatedMock.Reset();
            addedMock.Reset();
            removedMock.Reset();
            emptiedMock.Reset();

            subject.AddUnique(elementTwo);

            Assert.AreEqual(2, subject.NonSubscribableElements.Count);
            Assert.AreEqual(elementTwo, subject.NonSubscribableElements[1]);

            Assert.IsFalse(populatedMock.Received);
            Assert.IsTrue(addedMock.Received);
            Assert.IsFalse(removedMock.Received);
            Assert.IsFalse(emptiedMock.Received);

            populatedMock.Reset();
            addedMock.Reset();
            removedMock.Reset();
            emptiedMock.Reset();

            subject.AddUnique(elementTwo);

            Assert.AreEqual(2, subject.NonSubscribableElements.Count);
            Assert.AreEqual(elementTwo, subject.NonSubscribableElements[1]);

            Assert.IsFalse(populatedMock.Received);
            Assert.IsFalse(addedMock.Received);
            Assert.IsFalse(removedMock.Received);
            Assert.IsFalse(emptiedMock.Received);

            Object.DestroyImmediate(elementOne);
            Object.DestroyImmediate(elementTwo);
        }

        [Test]
        public void AddToStart()
        {
            UnityEventListenerMock populatedMock = new UnityEventListenerMock();
            UnityEventListenerMock addedMock = new UnityEventListenerMock();
            UnityEventListenerMock removedMock = new UnityEventListenerMock();
            UnityEventListenerMock emptiedMock = new UnityEventListenerMock();
            subject.Populated.AddListener(populatedMock.Listen);
            subject.Added.AddListener(addedMock.Listen);
            subject.Removed.AddListener(removedMock.Listen);
            subject.Emptied.AddListener(emptiedMock.Listen);

            GameObject elementOne = new GameObject();
            GameObject elementTwo = new GameObject();

            Assert.IsEmpty(subject.NonSubscribableElements);

            subject.AddAt(elementOne, 0);

            Assert.AreEqual(1, subject.NonSubscribableElements.Count);
            Assert.IsTrue(populatedMock.Received);
            Assert.IsTrue(addedMock.Received);
            Assert.IsFalse(removedMock.Received);
            Assert.IsFalse(emptiedMock.Received);

            populatedMock.Reset();
            addedMock.Reset();
            removedMock.Reset();
            emptiedMock.Reset();

            subject.AddAt(elementTwo, 0);

            Assert.AreEqual(2, subject.NonSubscribableElements.Count);
            Assert.AreEqual(elementTwo, subject.NonSubscribableElements[0]);

            Assert.IsFalse(populatedMock.Received);
            Assert.IsTrue(addedMock.Received);
            Assert.IsFalse(removedMock.Received);
            Assert.IsFalse(emptiedMock.Received);

            Object.DestroyImmediate(elementOne);
            Object.DestroyImmediate(elementTwo);
        }

        [Test]
        public void AddUniqueToStart()
        {
            UnityEventListenerMock populatedMock = new UnityEventListenerMock();
            UnityEventListenerMock addedMock = new UnityEventListenerMock();
            UnityEventListenerMock removedMock = new UnityEventListenerMock();
            UnityEventListenerMock emptiedMock = new UnityEventListenerMock();
            subject.Populated.AddListener(populatedMock.Listen);
            subject.Added.AddListener(addedMock.Listen);
            subject.Removed.AddListener(removedMock.Listen);
            subject.Emptied.AddListener(emptiedMock.Listen);

            GameObject elementOne = new GameObject();
            GameObject elementTwo = new GameObject();

            Assert.IsEmpty(subject.NonSubscribableElements);

            subject.AddUniqueAt(elementOne, 0);

            Assert.AreEqual(1, subject.NonSubscribableElements.Count);
            Assert.IsTrue(populatedMock.Received);
            Assert.IsTrue(addedMock.Received);
            Assert.IsFalse(removedMock.Received);
            Assert.IsFalse(emptiedMock.Received);

            populatedMock.Reset();
            addedMock.Reset();
            removedMock.Reset();
            emptiedMock.Reset();

            subject.AddUniqueAt(elementTwo, 0);

            Assert.AreEqual(2, subject.NonSubscribableElements.Count);
            Assert.AreEqual(elementTwo, subject.NonSubscribableElements[0]);

            Assert.IsFalse(populatedMock.Received);
            Assert.IsTrue(addedMock.Received);
            Assert.IsFalse(removedMock.Received);
            Assert.IsFalse(emptiedMock.Received);

            populatedMock.Reset();
            addedMock.Reset();
            removedMock.Reset();
            emptiedMock.Reset();

            subject.AddUniqueAt(elementOne, 0);

            Assert.AreEqual(2, subject.NonSubscribableElements.Count);
            Assert.AreEqual(elementTwo, subject.NonSubscribableElements[0]);

            Assert.IsFalse(populatedMock.Received);
            Assert.IsFalse(addedMock.Received);
            Assert.IsFalse(removedMock.Received);
            Assert.IsFalse(emptiedMock.Received);

            Object.DestroyImmediate(elementOne);
            Object.DestroyImmediate(elementTwo);
        }

        [Test]
        public void AddAtCurrentIndex()
        {
            UnityEventListenerMock populatedMock = new UnityEventListenerMock();
            UnityEventListenerMock addedMock = new UnityEventListenerMock();
            UnityEventListenerMock removedMock = new UnityEventListenerMock();
            UnityEventListenerMock emptiedMock = new UnityEventListenerMock();
            subject.Populated.AddListener(populatedMock.Listen);
            subject.Added.AddListener(addedMock.Listen);
            subject.Removed.AddListener(removedMock.Listen);
            subject.Emptied.AddListener(emptiedMock.Listen);

            subject.CurrentIndex = 0;

            GameObject elementOne = new GameObject();
            GameObject elementTwo = new GameObject();

            Assert.IsEmpty(subject.NonSubscribableElements);

            subject.AddAtCurrentIndex(elementOne);

            Assert.AreEqual(1, subject.NonSubscribableElements.Count);
            Assert.IsTrue(populatedMock.Received);
            Assert.IsTrue(addedMock.Received);
            Assert.IsFalse(removedMock.Received);
            Assert.IsFalse(emptiedMock.Received);

            populatedMock.Reset();
            addedMock.Reset();
            removedMock.Reset();
            emptiedMock.Reset();

            subject.AddAtCurrentIndex(elementTwo);
            Assert.AreEqual(2, subject.NonSubscribableElements.Count);
            Assert.AreEqual(elementTwo, subject.NonSubscribableElements[0]);

            Assert.IsFalse(populatedMock.Received);
            Assert.IsTrue(addedMock.Received);
            Assert.IsFalse(removedMock.Received);
            Assert.IsFalse(emptiedMock.Received);

            Object.DestroyImmediate(elementOne);
            Object.DestroyImmediate(elementTwo);
        }

        [Test]
        public void AddUniqueAtCurrentIndex()
        {
            UnityEventListenerMock populatedMock = new UnityEventListenerMock();
            UnityEventListenerMock addedMock = new UnityEventListenerMock();
            UnityEventListenerMock removedMock = new UnityEventListenerMock();
            UnityEventListenerMock emptiedMock = new UnityEventListenerMock();
            subject.Populated.AddListener(populatedMock.Listen);
            subject.Added.AddListener(addedMock.Listen);
            subject.Removed.AddListener(removedMock.Listen);
            subject.Emptied.AddListener(emptiedMock.Listen);

            subject.CurrentIndex = 0;

            GameObject elementOne = new GameObject();
            GameObject elementTwo = new GameObject();

            Assert.IsEmpty(subject.NonSubscribableElements);

            subject.AddUniqueAtCurrentIndex(elementOne);

            Assert.AreEqual(1, subject.NonSubscribableElements.Count);
            Assert.IsTrue(populatedMock.Received);
            Assert.IsTrue(addedMock.Received);
            Assert.IsFalse(removedMock.Received);
            Assert.IsFalse(emptiedMock.Received);

            populatedMock.Reset();
            addedMock.Reset();
            removedMock.Reset();
            emptiedMock.Reset();

            subject.AddUniqueAtCurrentIndex(elementTwo);
            Assert.AreEqual(2, subject.NonSubscribableElements.Count);
            Assert.AreEqual(elementTwo, subject.NonSubscribableElements[0]);

            Assert.IsFalse(populatedMock.Received);
            Assert.IsTrue(addedMock.Received);
            Assert.IsFalse(removedMock.Received);
            Assert.IsFalse(emptiedMock.Received);

            populatedMock.Reset();
            addedMock.Reset();
            removedMock.Reset();
            emptiedMock.Reset();

            subject.AddUniqueAtCurrentIndex(elementTwo);
            Assert.AreEqual(2, subject.NonSubscribableElements.Count);
            Assert.AreEqual(elementTwo, subject.NonSubscribableElements[0]);

            Assert.IsFalse(populatedMock.Received);
            Assert.IsFalse(addedMock.Received);
            Assert.IsFalse(removedMock.Received);
            Assert.IsFalse(emptiedMock.Received);

            Object.DestroyImmediate(elementOne);
            Object.DestroyImmediate(elementTwo);
        }

        [Test]
        public void SetAt()
        {
            UnityEventListenerMock addedMock = new UnityEventListenerMock();
            UnityEventListenerMock removedMock = new UnityEventListenerMock();
            subject.Added.AddListener(addedMock.Listen);
            subject.Removed.AddListener(removedMock.Listen);

            GameObject elementOne = new GameObject();
            GameObject elementTwo = new GameObject();
            GameObject elementThree = new GameObject();
            GameObject elementFour = new GameObject();

            subject.Add(elementOne);
            subject.Add(elementTwo);
            subject.Add(elementThree);

            addedMock.Reset();
            removedMock.Reset();

            Assert.AreEqual(elementOne, subject.NonSubscribableElements[0]);
            Assert.AreEqual(elementTwo, subject.NonSubscribableElements[1]);
            Assert.AreEqual(elementThree, subject.NonSubscribableElements[2]);

            subject.SetAt(elementFour, 1);

            Assert.AreEqual(elementOne, subject.NonSubscribableElements[0]);
            Assert.AreEqual(elementFour, subject.NonSubscribableElements[1]);
            Assert.AreEqual(elementThree, subject.NonSubscribableElements[2]);

            Assert.IsTrue(addedMock.Received);
            Assert.IsTrue(removedMock.Received);

            Object.DestroyImmediate(elementOne);
            Object.DestroyImmediate(elementTwo);
            Object.DestroyImmediate(elementThree);
            Object.DestroyImmediate(elementFour);
        }

        [Test]
        public void SetUniqueAt()
        {
            UnityEventListenerMock addedMock = new UnityEventListenerMock();
            UnityEventListenerMock removedMock = new UnityEventListenerMock();
            subject.Added.AddListener(addedMock.Listen);
            subject.Removed.AddListener(removedMock.Listen);

            GameObject elementOne = new GameObject();
            GameObject elementTwo = new GameObject();
            GameObject elementThree = new GameObject();

            subject.Add(elementOne);
            subject.Add(elementTwo);
            subject.Add(elementThree);

            addedMock.Reset();
            removedMock.Reset();

            Assert.AreEqual(elementOne, subject.NonSubscribableElements[0]);
            Assert.AreEqual(elementTwo, subject.NonSubscribableElements[1]);
            Assert.AreEqual(elementThree, subject.NonSubscribableElements[2]);

            subject.SetUniqueAt(elementOne, 1);

            Assert.AreEqual(elementOne, subject.NonSubscribableElements[0]);
            Assert.AreEqual(elementTwo, subject.NonSubscribableElements[1]);
            Assert.AreEqual(elementThree, subject.NonSubscribableElements[2]);

            Assert.IsFalse(addedMock.Received);
            Assert.IsFalse(removedMock.Received);

            Object.DestroyImmediate(elementOne);
            Object.DestroyImmediate(elementTwo);
            Object.DestroyImmediate(elementThree);
        }

        [Test]
        public void SetAtCurrentIndex()
        {
            UnityEventListenerMock addedMock = new UnityEventListenerMock();
            UnityEventListenerMock removedMock = new UnityEventListenerMock();
            subject.Added.AddListener(addedMock.Listen);
            subject.Removed.AddListener(removedMock.Listen);

            subject.CurrentIndex = 1;

            GameObject elementOne = new GameObject();
            GameObject elementTwo = new GameObject();
            GameObject elementThree = new GameObject();
            GameObject elementFour = new GameObject();

            subject.Add(elementOne);
            subject.Add(elementTwo);
            subject.Add(elementThree);

            addedMock.Reset();
            removedMock.Reset();

            Assert.AreEqual(elementOne, subject.NonSubscribableElements[0]);
            Assert.AreEqual(elementTwo, subject.NonSubscribableElements[1]);
            Assert.AreEqual(elementThree, subject.NonSubscribableElements[2]);

            subject.SetAtCurrentIndex(elementFour);

            Assert.AreEqual(elementOne, subject.NonSubscribableElements[0]);
            Assert.AreEqual(elementFour, subject.NonSubscribableElements[1]);
            Assert.AreEqual(elementThree, subject.NonSubscribableElements[2]);

            Assert.IsTrue(addedMock.Received);
            Assert.IsTrue(removedMock.Received);

            Object.DestroyImmediate(elementOne);
            Object.DestroyImmediate(elementTwo);
            Object.DestroyImmediate(elementThree);
            Object.DestroyImmediate(elementFour);
        }

        [Test]
        public void SetUniqueAtCurrentIndex()
        {
            UnityEventListenerMock addedMock = new UnityEventListenerMock();
            UnityEventListenerMock removedMock = new UnityEventListenerMock();
            subject.Added.AddListener(addedMock.Listen);
            subject.Removed.AddListener(removedMock.Listen);

            subject.CurrentIndex = 1;

            GameObject elementOne = new GameObject();
            GameObject elementTwo = new GameObject();
            GameObject elementThree = new GameObject();

            subject.Add(elementOne);
            subject.Add(elementTwo);
            subject.Add(elementThree);

            addedMock.Reset();
            removedMock.Reset();

            Assert.AreEqual(elementOne, subject.NonSubscribableElements[0]);
            Assert.AreEqual(elementTwo, subject.NonSubscribableElements[1]);
            Assert.AreEqual(elementThree, subject.NonSubscribableElements[2]);

            subject.SetUniqueAtCurrentIndex(elementOne);

            Assert.AreEqual(elementOne, subject.NonSubscribableElements[0]);
            Assert.AreEqual(elementTwo, subject.NonSubscribableElements[1]);
            Assert.AreEqual(elementThree, subject.NonSubscribableElements[2]);

            Assert.IsFalse(addedMock.Received);
            Assert.IsFalse(removedMock.Received);

            Object.DestroyImmediate(elementOne);
            Object.DestroyImmediate(elementTwo);
            Object.DestroyImmediate(elementThree);
        }

        [Test]
        public void RemoveFirst()
        {
            UnityEventListenerMock populatedMock = new UnityEventListenerMock();
            UnityEventListenerMock addedMock = new UnityEventListenerMock();
            UnityEventListenerMock removedMock = new UnityEventListenerMock();
            UnityEventListenerMock emptiedMock = new UnityEventListenerMock();
            subject.Populated.AddListener(populatedMock.Listen);
            subject.Added.AddListener(addedMock.Listen);
            subject.Removed.AddListener(removedMock.Listen);
            subject.Emptied.AddListener(emptiedMock.Listen);

            GameObject elementOne = new GameObject();
            GameObject elementTwo = new GameObject();

            subject.Remove(elementOne);

            Assert.IsFalse(populatedMock.Received);
            Assert.IsFalse(addedMock.Received);
            Assert.IsFalse(removedMock.Received);
            Assert.IsFalse(emptiedMock.Received);

            subject.Add(elementOne);
            subject.Add(elementTwo);
            subject.Add(elementOne);

            populatedMock.Reset();
            addedMock.Reset();
            removedMock.Reset();
            emptiedMock.Reset();

            Assert.AreEqual(3, subject.NonSubscribableElements.Count);

            subject.Remove(elementOne);

            Assert.IsFalse(populatedMock.Received);
            Assert.IsFalse(addedMock.Received);
            Assert.IsTrue(removedMock.Received);
            Assert.IsFalse(emptiedMock.Received);

            Assert.AreEqual(2, subject.NonSubscribableElements.Count);
            Assert.AreEqual(elementTwo, subject.NonSubscribableElements[0]);
            Assert.AreEqual(elementOne, subject.NonSubscribableElements[1]);

            populatedMock.Reset();
            addedMock.Reset();
            removedMock.Reset();
            emptiedMock.Reset();

            subject.Remove(elementOne);

            Assert.IsFalse(populatedMock.Received);
            Assert.IsFalse(addedMock.Received);
            Assert.IsTrue(removedMock.Received);
            Assert.IsFalse(emptiedMock.Received);

            Assert.AreEqual(1, subject.NonSubscribableElements.Count);
            Assert.AreEqual(elementTwo, subject.NonSubscribableElements[0]);

            populatedMock.Reset();
            addedMock.Reset();
            removedMock.Reset();
            emptiedMock.Reset();

            subject.Remove(elementTwo);

            Assert.IsFalse(populatedMock.Received);
            Assert.IsFalse(addedMock.Received);
            Assert.IsTrue(removedMock.Received);
            Assert.IsTrue(emptiedMock.Received);

            Assert.AreEqual(0, subject.NonSubscribableElements.Count);

            populatedMock.Reset();
            addedMock.Reset();
            removedMock.Reset();
            emptiedMock.Reset();

            Object.DestroyImmediate(elementOne);
            Object.DestroyImmediate(elementTwo);
        }

        [Test]
        public void RemoveLast()
        {
            UnityEventListenerMock populatedMock = new UnityEventListenerMock();
            UnityEventListenerMock addedMock = new UnityEventListenerMock();
            UnityEventListenerMock removedMock = new UnityEventListenerMock();
            UnityEventListenerMock emptiedMock = new UnityEventListenerMock();
            subject.Populated.AddListener(populatedMock.Listen);
            subject.Added.AddListener(addedMock.Listen);
            subject.Removed.AddListener(removedMock.Listen);
            subject.Emptied.AddListener(emptiedMock.Listen);

            GameObject elementOne = new GameObject();
            GameObject elementTwo = new GameObject();

            subject.RemoveLastOccurrence(elementOne);

            Assert.IsFalse(populatedMock.Received);
            Assert.IsFalse(addedMock.Received);
            Assert.IsFalse(removedMock.Received);
            Assert.IsFalse(emptiedMock.Received);

            subject.Add(elementOne);
            subject.Add(elementTwo);
            subject.Add(elementOne);

            populatedMock.Reset();
            addedMock.Reset();
            removedMock.Reset();
            emptiedMock.Reset();

            Assert.AreEqual(3, subject.NonSubscribableElements.Count);

            subject.RemoveLastOccurrence(elementOne);

            Assert.IsFalse(populatedMock.Received);
            Assert.IsFalse(addedMock.Received);
            Assert.IsTrue(removedMock.Received);
            Assert.IsFalse(emptiedMock.Received);

            Assert.AreEqual(2, subject.NonSubscribableElements.Count);
            Assert.AreEqual(elementOne, subject.NonSubscribableElements[0]);
            Assert.AreEqual(elementTwo, subject.NonSubscribableElements[1]);

            populatedMock.Reset();
            addedMock.Reset();
            removedMock.Reset();
            emptiedMock.Reset();

            subject.RemoveLastOccurrence(elementOne);

            Assert.IsFalse(populatedMock.Received);
            Assert.IsFalse(addedMock.Received);
            Assert.IsTrue(removedMock.Received);
            Assert.IsFalse(emptiedMock.Received);

            Assert.AreEqual(1, subject.NonSubscribableElements.Count);
            Assert.AreEqual(elementTwo, subject.NonSubscribableElements[0]);

            populatedMock.Reset();
            addedMock.Reset();
            removedMock.Reset();
            emptiedMock.Reset();

            subject.RemoveLastOccurrence(elementTwo);

            Assert.IsFalse(populatedMock.Received);
            Assert.IsFalse(addedMock.Received);
            Assert.IsTrue(removedMock.Received);
            Assert.IsTrue(emptiedMock.Received);

            Assert.AreEqual(0, subject.NonSubscribableElements.Count);

            populatedMock.Reset();
            addedMock.Reset();
            removedMock.Reset();
            emptiedMock.Reset();

            Object.DestroyImmediate(elementOne);
            Object.DestroyImmediate(elementTwo);
        }

        [Test]
        public void RemoveAt()
        {
            UnityEventListenerMock populatedMock = new UnityEventListenerMock();
            UnityEventListenerMock addedMock = new UnityEventListenerMock();
            UnityEventListenerMock removedMock = new UnityEventListenerMock();
            UnityEventListenerMock emptiedMock = new UnityEventListenerMock();
            subject.Populated.AddListener(populatedMock.Listen);
            subject.Added.AddListener(addedMock.Listen);
            subject.Removed.AddListener(removedMock.Listen);
            subject.Emptied.AddListener(emptiedMock.Listen);

            GameObject elementOne = new GameObject();
            GameObject elementTwo = new GameObject();
            GameObject elementThree = new GameObject();

            Assert.AreEqual(0, subject.NonSubscribableElements.Count);

            Assert.IsFalse(populatedMock.Received);
            Assert.IsFalse(addedMock.Received);
            Assert.IsFalse(removedMock.Received);
            Assert.IsFalse(emptiedMock.Received);

            subject.Add(elementOne);
            subject.Add(elementTwo);
            subject.Add(elementThree);

            populatedMock.Reset();
            addedMock.Reset();
            removedMock.Reset();
            emptiedMock.Reset();

            Assert.AreEqual(3, subject.NonSubscribableElements.Count);

            subject.RemoveAt(1);

            Assert.IsFalse(populatedMock.Received);
            Assert.IsFalse(addedMock.Received);
            Assert.IsTrue(removedMock.Received);
            Assert.IsFalse(emptiedMock.Received);

            Assert.AreEqual(2, subject.NonSubscribableElements.Count);
            Assert.AreEqual(elementOne, subject.NonSubscribableElements[0]);
            Assert.AreEqual(elementThree, subject.NonSubscribableElements[1]);

            populatedMock.Reset();
            addedMock.Reset();
            removedMock.Reset();
            emptiedMock.Reset();

            Object.DestroyImmediate(elementOne);
            Object.DestroyImmediate(elementTwo);
            Object.DestroyImmediate(elementThree);
        }

        [Test]
        public void RemoveAtCurrentIndex()
        {
            UnityEventListenerMock populatedMock = new UnityEventListenerMock();
            UnityEventListenerMock addedMock = new UnityEventListenerMock();
            UnityEventListenerMock removedMock = new UnityEventListenerMock();
            UnityEventListenerMock emptiedMock = new UnityEventListenerMock();
            subject.Populated.AddListener(populatedMock.Listen);
            subject.Added.AddListener(addedMock.Listen);
            subject.Removed.AddListener(removedMock.Listen);
            subject.Emptied.AddListener(emptiedMock.Listen);

            subject.CurrentIndex = 1;

            GameObject elementOne = new GameObject();
            GameObject elementTwo = new GameObject();
            GameObject elementThree = new GameObject();

            Assert.AreEqual(0, subject.NonSubscribableElements.Count);

            Assert.IsFalse(populatedMock.Received);
            Assert.IsFalse(addedMock.Received);
            Assert.IsFalse(removedMock.Received);
            Assert.IsFalse(emptiedMock.Received);

            subject.Add(elementOne);
            subject.Add(elementTwo);
            subject.Add(elementThree);

            populatedMock.Reset();
            addedMock.Reset();
            removedMock.Reset();
            emptiedMock.Reset();

            Assert.AreEqual(3, subject.NonSubscribableElements.Count);

            subject.RemoveAtCurrentIndex();

            Assert.IsFalse(populatedMock.Received);
            Assert.IsFalse(addedMock.Received);
            Assert.IsTrue(removedMock.Received);
            Assert.IsFalse(emptiedMock.Received);

            Assert.AreEqual(2, subject.NonSubscribableElements.Count);
            Assert.AreEqual(elementOne, subject.NonSubscribableElements[0]);
            Assert.AreEqual(elementThree, subject.NonSubscribableElements[1]);

            populatedMock.Reset();
            addedMock.Reset();
            removedMock.Reset();
            emptiedMock.Reset();

            Object.DestroyImmediate(elementOne);
            Object.DestroyImmediate(elementTwo);
            Object.DestroyImmediate(elementThree);
        }

        [Test]
        public void ClearFromBack()
        {
            UnityEventListenerMock populatedMock = new UnityEventListenerMock();
            UnityEventListenerMock addedMock = new UnityEventListenerMock();
            UnityEventListenerMock removedMock = new UnityEventListenerMock();
            UnityEventListenerMock emptiedMock = new UnityEventListenerMock();
            subject.Populated.AddListener(populatedMock.Listen);
            subject.Added.AddListener(addedMock.Listen);
            subject.Removed.AddListener(removedMock.Listen);
            subject.Emptied.AddListener(emptiedMock.Listen);

            GameObject elementOne = new GameObject();
            GameObject elementTwo = new GameObject();

            subject.Clear(false);

            Assert.IsFalse(populatedMock.Received);
            Assert.IsFalse(addedMock.Received);
            Assert.IsFalse(removedMock.Received);
            Assert.IsFalse(emptiedMock.Received);

            subject.Add(elementOne);
            subject.Add(elementTwo);
            subject.Add(elementOne);

            populatedMock.Reset();
            addedMock.Reset();
            removedMock.Reset();
            emptiedMock.Reset();

            Assert.AreEqual(3, subject.NonSubscribableElements.Count);

            subject.Clear(false);

            Assert.IsFalse(populatedMock.Received);
            Assert.IsFalse(addedMock.Received);
            Assert.IsTrue(removedMock.Received);
            Assert.IsTrue(emptiedMock.Received);

            Assert.AreEqual(0, subject.NonSubscribableElements.Count);

            Object.DestroyImmediate(elementOne);
            Object.DestroyImmediate(elementTwo);
        }

        [Test]
        public void ClearFromFront()
        {
            UnityEventListenerMock populatedMock = new UnityEventListenerMock();
            UnityEventListenerMock addedMock = new UnityEventListenerMock();
            UnityEventListenerMock removedMock = new UnityEventListenerMock();
            UnityEventListenerMock emptiedMock = new UnityEventListenerMock();
            subject.Populated.AddListener(populatedMock.Listen);
            subject.Added.AddListener(addedMock.Listen);
            subject.Removed.AddListener(removedMock.Listen);
            subject.Emptied.AddListener(emptiedMock.Listen);

            GameObject elementOne = new GameObject();
            GameObject elementTwo = new GameObject();

            subject.Clear(true);

            Assert.IsFalse(populatedMock.Received);
            Assert.IsFalse(addedMock.Received);
            Assert.IsFalse(removedMock.Received);
            Assert.IsFalse(emptiedMock.Received);

            subject.Add(elementOne);
            subject.Add(elementTwo);
            subject.Add(elementOne);

            populatedMock.Reset();
            addedMock.Reset();
            removedMock.Reset();
            emptiedMock.Reset();

            Assert.AreEqual(3, subject.NonSubscribableElements.Count);

            subject.Clear(true);

            Assert.IsFalse(populatedMock.Received);
            Assert.IsFalse(addedMock.Received);
            Assert.IsTrue(removedMock.Received);
            Assert.IsTrue(emptiedMock.Received);

            Assert.AreEqual(0, subject.NonSubscribableElements.Count);

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

            UnityEventListenerMock populatedMock = new UnityEventListenerMock();
            UnityEventListenerMock addedMock = new UnityEventListenerMock();
            UnityEventListenerMock removedMock = new UnityEventListenerMock();
            UnityEventListenerMock emptiedMock = new UnityEventListenerMock();
            mock.Populated.AddListener(populatedMock.Listen);
            mock.Added.AddListener(addedMock.Listen);
            mock.Removed.AddListener(removedMock.Listen);
            mock.Emptied.AddListener(emptiedMock.Listen);

            Assert.IsFalse(populatedMock.Received);
            Assert.IsFalse(addedMock.Received);
            Assert.IsFalse(removedMock.Received);
            Assert.IsFalse(emptiedMock.Received);

            mock.ManualStart();

            Assert.IsTrue(populatedMock.Received);
            Assert.IsTrue(addedMock.Received);
            Assert.IsFalse(removedMock.Received);
            Assert.IsFalse(emptiedMock.Received);

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