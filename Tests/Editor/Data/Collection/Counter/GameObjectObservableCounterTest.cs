using Zinnia.Data.Collection.Counter;

namespace Test.Zinnia.Data.Collection.Counter
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using Assert = UnityEngine.Assertions.Assert;

    public class GameObjectObservableCounterTest
    {
        private GameObject containingObject;
        private GameObjectObservableCounter subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<GameObjectObservableCounter>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void IncreaseCount()
        {
            UnityEventListenerMock addedMock = new UnityEventListenerMock();
            UnityEventListenerMock removedMock = new UnityEventListenerMock();
            subject.Added.AddListener(addedMock.Listen);
            subject.Removed.AddListener(removedMock.Listen);
            GameObject elementOne = new GameObject();

            Assert.IsFalse(addedMock.Received);
            Assert.IsFalse(removedMock.Received);
            Assert.AreEqual(0, subject.GetCount(elementOne));

            subject.IncreaseCount(elementOne);

            Assert.IsTrue(addedMock.Received);
            Assert.IsFalse(removedMock.Received);
            Assert.AreEqual(1, subject.GetCount(elementOne));

            addedMock.Reset();
            removedMock.Reset();

            subject.IncreaseCount(elementOne);

            Assert.IsFalse(addedMock.Received);
            Assert.IsFalse(removedMock.Received);
            Assert.AreEqual(2, subject.GetCount(elementOne));

            Object.DestroyImmediate(elementOne);
        }

        [Test]
        public void DecreaseCount()
        {
            UnityEventListenerMock addedMock = new UnityEventListenerMock();
            UnityEventListenerMock removedMock = new UnityEventListenerMock();
            subject.Added.AddListener(addedMock.Listen);
            subject.Removed.AddListener(removedMock.Listen);
            GameObject elementOne = new GameObject();

            subject.IncreaseCount(elementOne);
            subject.IncreaseCount(elementOne);

            addedMock.Reset();
            removedMock.Reset();

            Assert.IsFalse(addedMock.Received);
            Assert.IsFalse(removedMock.Received);
            Assert.AreEqual(2, subject.GetCount(elementOne));

            subject.DecreaseCount(elementOne);

            Assert.IsFalse(addedMock.Received);
            Assert.IsFalse(removedMock.Received);
            Assert.AreEqual(1, subject.GetCount(elementOne));

            addedMock.Reset();
            removedMock.Reset();

            subject.DecreaseCount(elementOne);

            Assert.IsFalse(addedMock.Received);
            Assert.IsTrue(removedMock.Received);
            Assert.AreEqual(0, subject.GetCount(elementOne));

            Assert.IsFalse(subject.ElementsCounter.ContainsKey(elementOne));

            Object.DestroyImmediate(elementOne);
        }

        [Test]
        public void RemoveFromCount()
        {
            UnityEventListenerMock addedMock = new UnityEventListenerMock();
            UnityEventListenerMock removedMock = new UnityEventListenerMock();
            subject.Added.AddListener(addedMock.Listen);
            subject.Removed.AddListener(removedMock.Listen);
            GameObject elementOne = new GameObject();

            subject.IncreaseCount(elementOne);
            subject.IncreaseCount(elementOne);

            addedMock.Reset();
            removedMock.Reset();

            Assert.IsFalse(addedMock.Received);
            Assert.IsFalse(removedMock.Received);
            Assert.AreEqual(2, subject.GetCount(elementOne));

            subject.RemoveFromCount(elementOne);

            Assert.IsFalse(addedMock.Received);
            Assert.IsTrue(removedMock.Received);
            Assert.AreEqual(0, subject.GetCount(elementOne));

            Assert.IsFalse(subject.ElementsCounter.ContainsKey(elementOne));

            Object.DestroyImmediate(elementOne);
        }

        [Test]
        public void Clear()
        {
            UnityEventListenerMock addedMock = new UnityEventListenerMock();
            UnityEventListenerMock removedMock = new UnityEventListenerMock();
            subject.Added.AddListener(addedMock.Listen);
            subject.Removed.AddListener(removedMock.Listen);
            GameObject elementOne = new GameObject();
            GameObject elementTwo = new GameObject();

            subject.IncreaseCount(elementOne);
            subject.IncreaseCount(elementOne);
            subject.IncreaseCount(elementTwo);
            subject.IncreaseCount(elementTwo);

            addedMock.Reset();
            removedMock.Reset();

            Assert.IsFalse(addedMock.Received);
            Assert.IsFalse(removedMock.Received);
            Assert.AreEqual(2, subject.GetCount(elementOne));
            Assert.AreEqual(2, subject.GetCount(elementTwo));

            subject.Clear();

            Assert.IsFalse(addedMock.Received);
            Assert.IsTrue(removedMock.Received);
            Assert.AreEqual(0, subject.GetCount(elementOne));
            Assert.AreEqual(0, subject.GetCount(elementTwo));

            Assert.IsFalse(subject.ElementsCounter.ContainsKey(elementOne));
            Assert.IsFalse(subject.ElementsCounter.ContainsKey(elementTwo));

            Object.DestroyImmediate(elementOne);
            Object.DestroyImmediate(elementTwo);
        }

        [Test]
        public void IncreaseCountInactiveGameObject()
        {
            UnityEventListenerMock addedMock = new UnityEventListenerMock();
            UnityEventListenerMock removedMock = new UnityEventListenerMock();
            subject.Added.AddListener(addedMock.Listen);
            subject.Removed.AddListener(removedMock.Listen);
            GameObject elementOne = new GameObject();

            subject.gameObject.SetActive(false);

            Assert.IsFalse(addedMock.Received);
            Assert.IsFalse(removedMock.Received);
            Assert.AreEqual(0, subject.GetCount(elementOne));

            subject.IncreaseCount(elementOne);

            Assert.IsFalse(addedMock.Received);
            Assert.IsFalse(removedMock.Received);
            Assert.AreEqual(0, subject.GetCount(elementOne));

            addedMock.Reset();
            removedMock.Reset();

            subject.IncreaseCount(elementOne);

            Assert.IsFalse(addedMock.Received);
            Assert.IsFalse(removedMock.Received);
            Assert.AreEqual(0, subject.GetCount(elementOne));

            Assert.IsFalse(subject.ElementsCounter.ContainsKey(elementOne));

            Object.DestroyImmediate(elementOne);
        }

        [Test]
        public void IncreaseCountInactiveComponent()
        {
            UnityEventListenerMock addedMock = new UnityEventListenerMock();
            UnityEventListenerMock removedMock = new UnityEventListenerMock();
            subject.Added.AddListener(addedMock.Listen);
            subject.Removed.AddListener(removedMock.Listen);
            GameObject elementOne = new GameObject();

            subject.enabled = false;

            Assert.IsFalse(addedMock.Received);
            Assert.IsFalse(removedMock.Received);
            Assert.AreEqual(0, subject.GetCount(elementOne));

            subject.IncreaseCount(elementOne);

            Assert.IsFalse(addedMock.Received);
            Assert.IsFalse(removedMock.Received);
            Assert.AreEqual(0, subject.GetCount(elementOne));

            addedMock.Reset();
            removedMock.Reset();

            subject.IncreaseCount(elementOne);

            Assert.IsFalse(addedMock.Received);
            Assert.IsFalse(removedMock.Received);
            Assert.AreEqual(0, subject.GetCount(elementOne));

            Assert.IsFalse(subject.ElementsCounter.ContainsKey(elementOne));

            Object.DestroyImmediate(elementOne);
        }

        [Test]
        public void DecreaseCountInactiveGameObject()
        {
            UnityEventListenerMock addedMock = new UnityEventListenerMock();
            UnityEventListenerMock removedMock = new UnityEventListenerMock();
            subject.Added.AddListener(addedMock.Listen);
            subject.Removed.AddListener(removedMock.Listen);
            GameObject elementOne = new GameObject();

            subject.IncreaseCount(elementOne);
            subject.IncreaseCount(elementOne);

            addedMock.Reset();
            removedMock.Reset();

            Assert.IsFalse(addedMock.Received);
            Assert.IsFalse(removedMock.Received);
            Assert.AreEqual(2, subject.GetCount(elementOne));

            subject.gameObject.SetActive(false);

            subject.DecreaseCount(elementOne);

            Assert.IsFalse(addedMock.Received);
            Assert.IsFalse(removedMock.Received);
            Assert.AreEqual(2, subject.GetCount(elementOne));

            addedMock.Reset();
            removedMock.Reset();

            subject.DecreaseCount(elementOne);

            Assert.IsFalse(addedMock.Received);
            Assert.IsFalse(removedMock.Received);
            Assert.AreEqual(2, subject.GetCount(elementOne));

            Assert.IsTrue(subject.ElementsCounter.ContainsKey(elementOne));

            Object.DestroyImmediate(elementOne);
        }

        [Test]
        public void DecreaseCountInactiveComponent()
        {
            UnityEventListenerMock addedMock = new UnityEventListenerMock();
            UnityEventListenerMock removedMock = new UnityEventListenerMock();
            subject.Added.AddListener(addedMock.Listen);
            subject.Removed.AddListener(removedMock.Listen);
            GameObject elementOne = new GameObject();

            subject.IncreaseCount(elementOne);
            subject.IncreaseCount(elementOne);

            addedMock.Reset();
            removedMock.Reset();

            Assert.IsFalse(addedMock.Received);
            Assert.IsFalse(removedMock.Received);
            Assert.AreEqual(2, subject.GetCount(elementOne));

            subject.enabled = false;

            subject.DecreaseCount(elementOne);

            Assert.IsFalse(addedMock.Received);
            Assert.IsFalse(removedMock.Received);
            Assert.AreEqual(2, subject.GetCount(elementOne));

            addedMock.Reset();
            removedMock.Reset();

            subject.DecreaseCount(elementOne);

            Assert.IsFalse(addedMock.Received);
            Assert.IsFalse(removedMock.Received);
            Assert.AreEqual(2, subject.GetCount(elementOne));

            Assert.IsTrue(subject.ElementsCounter.ContainsKey(elementOne));

            Object.DestroyImmediate(elementOne);
        }

        [Test]
        public void RemoveFromCountInactiveGameObject()
        {
            UnityEventListenerMock addedMock = new UnityEventListenerMock();
            UnityEventListenerMock removedMock = new UnityEventListenerMock();
            subject.Added.AddListener(addedMock.Listen);
            subject.Removed.AddListener(removedMock.Listen);
            GameObject elementOne = new GameObject();

            subject.IncreaseCount(elementOne);
            subject.IncreaseCount(elementOne);

            addedMock.Reset();
            removedMock.Reset();

            subject.gameObject.SetActive(false);

            Assert.IsFalse(addedMock.Received);
            Assert.IsFalse(removedMock.Received);
            Assert.AreEqual(2, subject.GetCount(elementOne));

            subject.RemoveFromCount(elementOne);

            Assert.IsFalse(addedMock.Received);
            Assert.IsFalse(removedMock.Received);
            Assert.AreEqual(2, subject.GetCount(elementOne));

            Assert.IsTrue(subject.ElementsCounter.ContainsKey(elementOne));

            Object.DestroyImmediate(elementOne);
        }

        [Test]
        public void RemoveFromCountInactiveComponent()
        {
            UnityEventListenerMock addedMock = new UnityEventListenerMock();
            UnityEventListenerMock removedMock = new UnityEventListenerMock();
            subject.Added.AddListener(addedMock.Listen);
            subject.Removed.AddListener(removedMock.Listen);
            GameObject elementOne = new GameObject();

            subject.IncreaseCount(elementOne);
            subject.IncreaseCount(elementOne);

            addedMock.Reset();
            removedMock.Reset();

            subject.enabled = false;

            Assert.IsFalse(addedMock.Received);
            Assert.IsFalse(removedMock.Received);
            Assert.AreEqual(2, subject.GetCount(elementOne));

            subject.RemoveFromCount(elementOne);

            Assert.IsFalse(addedMock.Received);
            Assert.IsFalse(removedMock.Received);
            Assert.AreEqual(2, subject.GetCount(elementOne));

            Assert.IsTrue(subject.ElementsCounter.ContainsKey(elementOne));

            Object.DestroyImmediate(elementOne);
        }

        [Test]
        public void ClearInactiveGameObject()
        {
            UnityEventListenerMock addedMock = new UnityEventListenerMock();
            UnityEventListenerMock removedMock = new UnityEventListenerMock();
            subject.Added.AddListener(addedMock.Listen);
            subject.Removed.AddListener(removedMock.Listen);
            GameObject elementOne = new GameObject();
            GameObject elementTwo = new GameObject();

            subject.IncreaseCount(elementOne);
            subject.IncreaseCount(elementOne);
            subject.IncreaseCount(elementTwo);
            subject.IncreaseCount(elementTwo);

            addedMock.Reset();
            removedMock.Reset();

            subject.gameObject.SetActive(false);

            Assert.IsFalse(addedMock.Received);
            Assert.IsFalse(removedMock.Received);
            Assert.AreEqual(2, subject.GetCount(elementOne));
            Assert.AreEqual(2, subject.GetCount(elementTwo));

            subject.Clear();

            Assert.IsFalse(addedMock.Received);
            Assert.IsFalse(removedMock.Received);
            Assert.AreEqual(2, subject.GetCount(elementOne));
            Assert.AreEqual(2, subject.GetCount(elementTwo));

            Assert.IsTrue(subject.ElementsCounter.ContainsKey(elementOne));
            Assert.IsTrue(subject.ElementsCounter.ContainsKey(elementTwo));

            Object.DestroyImmediate(elementOne);
            Object.DestroyImmediate(elementTwo);
        }

        [Test]
        public void ClearInactiveComponent()
        {
            UnityEventListenerMock addedMock = new UnityEventListenerMock();
            UnityEventListenerMock removedMock = new UnityEventListenerMock();
            subject.Added.AddListener(addedMock.Listen);
            subject.Removed.AddListener(removedMock.Listen);
            GameObject elementOne = new GameObject();
            GameObject elementTwo = new GameObject();

            subject.IncreaseCount(elementOne);
            subject.IncreaseCount(elementOne);
            subject.IncreaseCount(elementTwo);
            subject.IncreaseCount(elementTwo);

            addedMock.Reset();
            removedMock.Reset();

            subject.enabled = false;

            Assert.IsFalse(addedMock.Received);
            Assert.IsFalse(removedMock.Received);
            Assert.AreEqual(2, subject.GetCount(elementOne));
            Assert.AreEqual(2, subject.GetCount(elementTwo));

            subject.Clear();

            Assert.IsFalse(addedMock.Received);
            Assert.IsFalse(removedMock.Received);
            Assert.AreEqual(2, subject.GetCount(elementOne));
            Assert.AreEqual(2, subject.GetCount(elementTwo));

            Assert.IsTrue(subject.ElementsCounter.ContainsKey(elementOne));
            Assert.IsTrue(subject.ElementsCounter.ContainsKey(elementTwo));

            Object.DestroyImmediate(elementOne);
            Object.DestroyImmediate(elementTwo);
        }
    }
}