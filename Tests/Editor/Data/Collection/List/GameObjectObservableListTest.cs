using Zinnia.Data.Collection.List;

namespace Test.Zinnia.Data.Collection.List
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using Assert = UnityEngine.Assertions.Assert;

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
        public void IsListEmptyIsEmpty()
        {
            UnityEventListenerMock listIsEmpty = new UnityEventListenerMock();
            UnityEventListenerMock listIsPopulated = new UnityEventListenerMock();
            subject.IsEmpty.AddListener(listIsEmpty.Listen);
            subject.IsPopulated.AddListener(listIsPopulated.Listen);

            Assert.IsFalse(listIsEmpty.Received);
            Assert.IsFalse(listIsPopulated.Received);

            subject.IsListEmpty();

            Assert.IsTrue(listIsEmpty.Received);
            Assert.IsFalse(listIsPopulated.Received);
        }

        [Test]
        public void IsListEmptyIsPopulated()
        {
            UnityEventListenerMock listIsEmpty = new UnityEventListenerMock();
            UnityEventListenerMock listIsPopulated = new UnityEventListenerMock();
            subject.IsEmpty.AddListener(listIsEmpty.Listen);
            subject.IsPopulated.AddListener(listIsPopulated.Listen);

            GameObject elementOne = new GameObject();

            subject.Add(elementOne);

            Assert.IsFalse(listIsEmpty.Received);
            Assert.IsFalse(listIsPopulated.Received);

            subject.IsListEmpty();

            Assert.IsFalse(listIsEmpty.Received);
            Assert.IsTrue(listIsPopulated.Received);

            Object.DestroyImmediate(elementOne);
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

            Assert.AreEqual(0, subject.NonSubscribableElements.Count);

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

            Assert.AreEqual(0, subject.NonSubscribableElements.Count);

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
        public void InsertAtStart()
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

            Assert.AreEqual(0, subject.NonSubscribableElements.Count);

            subject.InsertAt(elementOne, 0);

            Assert.AreEqual(1, subject.NonSubscribableElements.Count);
            Assert.IsTrue(populatedMock.Received);
            Assert.IsTrue(addedMock.Received);
            Assert.IsFalse(removedMock.Received);
            Assert.IsFalse(emptiedMock.Received);

            populatedMock.Reset();
            addedMock.Reset();
            removedMock.Reset();
            emptiedMock.Reset();

            subject.InsertAt(elementTwo, 0);

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
        public void InsertAt()
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
            GameObject elementFour = new GameObject();
            GameObject elementFive = new GameObject();

            subject.Add(elementOne);
            subject.Add(elementTwo);
            subject.Add(elementThree);

            Assert.AreEqual(3, subject.NonSubscribableElements.Count);

            populatedMock.Reset();
            addedMock.Reset();
            removedMock.Reset();
            emptiedMock.Reset();

            Assert.AreEqual(elementTwo, subject.NonSubscribableElements[1]);
            Assert.AreEqual(elementThree, subject.NonSubscribableElements[2]);

            subject.InsertAt(elementFour, 1);

            Assert.AreEqual(4, subject.NonSubscribableElements.Count);
            Assert.IsFalse(populatedMock.Received);
            Assert.IsTrue(addedMock.Received);
            Assert.IsFalse(removedMock.Received);
            Assert.IsFalse(emptiedMock.Received);
            Assert.AreEqual(elementFour, subject.NonSubscribableElements[1]);
            Assert.AreEqual(elementTwo, subject.NonSubscribableElements[2]);
            Assert.AreEqual(elementThree, subject.NonSubscribableElements[3]);

            populatedMock.Reset();
            addedMock.Reset();
            removedMock.Reset();
            emptiedMock.Reset();

            subject.InsertAt(elementFive, subject.NonSubscribableElements.Count + 1);

            Assert.AreEqual(5, subject.NonSubscribableElements.Count);
            Assert.AreEqual(elementFour, subject.NonSubscribableElements[1]);
            Assert.AreEqual(elementTwo, subject.NonSubscribableElements[2]);
            Assert.AreEqual(elementThree, subject.NonSubscribableElements[3]);
            Assert.AreEqual(elementFive, subject.NonSubscribableElements[4]);

            Assert.IsFalse(populatedMock.Received);
            Assert.IsTrue(addedMock.Received);
            Assert.IsFalse(removedMock.Received);
            Assert.IsFalse(emptiedMock.Received);

            populatedMock.Reset();
            addedMock.Reset();
            removedMock.Reset();
            emptiedMock.Reset();

            subject.InsertAt(elementFive, -3);

            Assert.AreEqual(6, subject.NonSubscribableElements.Count);
            Assert.AreEqual(elementFour, subject.NonSubscribableElements[1]);
            Assert.AreEqual(elementFive, subject.NonSubscribableElements[2]);
            Assert.AreEqual(elementTwo, subject.NonSubscribableElements[3]);
            Assert.AreEqual(elementThree, subject.NonSubscribableElements[4]);
            Assert.AreEqual(elementFive, subject.NonSubscribableElements[5]);

            Object.DestroyImmediate(elementOne);
            Object.DestroyImmediate(elementTwo);
            Object.DestroyImmediate(elementThree);
            Object.DestroyImmediate(elementFour);
            Object.DestroyImmediate(elementFive);
        }

        [Test]
        public void InsertUniqueAtStart()
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

            Assert.AreEqual(0, subject.NonSubscribableElements.Count);

            subject.InsertUniqueAt(elementOne, 0);

            Assert.AreEqual(1, subject.NonSubscribableElements.Count);
            Assert.IsTrue(populatedMock.Received);
            Assert.IsTrue(addedMock.Received);
            Assert.IsFalse(removedMock.Received);
            Assert.IsFalse(emptiedMock.Received);

            populatedMock.Reset();
            addedMock.Reset();
            removedMock.Reset();
            emptiedMock.Reset();

            subject.InsertUniqueAt(elementTwo, 0);

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

            subject.InsertUniqueAt(elementOne, 0);

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
        public void InsertAtCurrentIndex()
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

            Assert.AreEqual(0, subject.NonSubscribableElements.Count);

            subject.InsertAtCurrentIndex(elementOne);

            Assert.AreEqual(1, subject.NonSubscribableElements.Count);
            Assert.IsTrue(populatedMock.Received);
            Assert.IsTrue(addedMock.Received);
            Assert.IsFalse(removedMock.Received);
            Assert.IsFalse(emptiedMock.Received);

            populatedMock.Reset();
            addedMock.Reset();
            removedMock.Reset();
            emptiedMock.Reset();

            subject.InsertAtCurrentIndex(elementTwo);
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
        public void InsertUniqueAtCurrentIndex()
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

            Assert.AreEqual(0, subject.NonSubscribableElements.Count);

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

            GameObject elementOne = new GameObject("One");
            GameObject elementTwo = new GameObject("Two");
            GameObject elementThree = new GameObject("Three");
            GameObject elementFour = new GameObject("Four");

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
        public void SetAtEmptyCollection()
        {
            UnityEventListenerMock addedMock = new UnityEventListenerMock();
            UnityEventListenerMock removedMock = new UnityEventListenerMock();
            subject.Added.AddListener(addedMock.Listen);
            subject.Removed.AddListener(removedMock.Listen);

            GameObject elementOne = new GameObject("One");

            Assert.AreEqual(0, subject.NonSubscribableElements.Count);

            subject.SetAt(elementOne, 1);

            Assert.AreEqual(0, subject.NonSubscribableElements.Count);

            Assert.IsFalse(addedMock.Received);
            Assert.IsFalse(removedMock.Received);

            Object.DestroyImmediate(elementOne);
        }

        [Test]
        public void SetAtOrAddIfEmptyCollection()
        {
            UnityEventListenerMock addedMock = new UnityEventListenerMock();
            UnityEventListenerMock removedMock = new UnityEventListenerMock();
            subject.Added.AddListener(addedMock.Listen);
            subject.Removed.AddListener(removedMock.Listen);

            GameObject elementOne = new GameObject("One");

            Assert.AreEqual(0, subject.NonSubscribableElements.Count);

            subject.SetAtOrAddIfEmpty(elementOne, 1);

            Assert.AreEqual(elementOne, subject.NonSubscribableElements[0]);

            Assert.IsTrue(addedMock.Received);
            Assert.IsFalse(removedMock.Received);

            Object.DestroyImmediate(elementOne);
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

            subject.Clear();

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

            subject.Clear();

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