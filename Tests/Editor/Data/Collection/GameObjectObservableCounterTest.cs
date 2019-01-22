using Zinnia.Data.Collection;

namespace Test.Zinnia.Data.Collection
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;

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
            UnityEventListenerMock elementAddedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementRemovedMock = new UnityEventListenerMock();
            subject.ElementAdded.AddListener(elementAddedMock.Listen);
            subject.ElementRemoved.AddListener(elementRemovedMock.Listen);
            GameObject elementOne = new GameObject();

            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsFalse(elementRemovedMock.Received);
            Assert.AreEqual(0, subject.GetCount(elementOne));

            subject.IncreaseCount(elementOne);

            Assert.IsTrue(elementAddedMock.Received);
            Assert.IsFalse(elementRemovedMock.Received);
            Assert.AreEqual(1, subject.GetCount(elementOne));

            elementAddedMock.Reset();
            elementRemovedMock.Reset();

            subject.IncreaseCount(elementOne);

            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsFalse(elementRemovedMock.Received);
            Assert.AreEqual(2, subject.GetCount(elementOne));

            Object.DestroyImmediate(elementOne);
        }

        [Test]
        public void DecreaseCount()
        {
            UnityEventListenerMock elementAddedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementRemovedMock = new UnityEventListenerMock();
            subject.ElementAdded.AddListener(elementAddedMock.Listen);
            subject.ElementRemoved.AddListener(elementRemovedMock.Listen);
            GameObject elementOne = new GameObject();

            subject.IncreaseCount(elementOne);
            subject.IncreaseCount(elementOne);

            elementAddedMock.Reset();
            elementRemovedMock.Reset();

            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsFalse(elementRemovedMock.Received);
            Assert.AreEqual(2, subject.GetCount(elementOne));

            subject.DecreaseCount(elementOne);

            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsFalse(elementRemovedMock.Received);
            Assert.AreEqual(1, subject.GetCount(elementOne));

            elementAddedMock.Reset();
            elementRemovedMock.Reset();

            subject.DecreaseCount(elementOne);

            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsTrue(elementRemovedMock.Received);
            Assert.AreEqual(0, subject.GetCount(elementOne));

            Assert.IsFalse(subject.ElementsCounter.ContainsKey(elementOne));

            Object.DestroyImmediate(elementOne);
        }

        [Test]
        public void RemoveFromCount()
        {
            UnityEventListenerMock elementAddedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementRemovedMock = new UnityEventListenerMock();
            subject.ElementAdded.AddListener(elementAddedMock.Listen);
            subject.ElementRemoved.AddListener(elementRemovedMock.Listen);
            GameObject elementOne = new GameObject();

            subject.IncreaseCount(elementOne);
            subject.IncreaseCount(elementOne);

            elementAddedMock.Reset();
            elementRemovedMock.Reset();

            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsFalse(elementRemovedMock.Received);
            Assert.AreEqual(2, subject.GetCount(elementOne));

            subject.RemoveFromCount(elementOne);

            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsTrue(elementRemovedMock.Received);
            Assert.AreEqual(0, subject.GetCount(elementOne));

            Assert.IsFalse(subject.ElementsCounter.ContainsKey(elementOne));

            Object.DestroyImmediate(elementOne);
        }

        [Test]
        public void Clear()
        {
            UnityEventListenerMock elementAddedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementRemovedMock = new UnityEventListenerMock();
            subject.ElementAdded.AddListener(elementAddedMock.Listen);
            subject.ElementRemoved.AddListener(elementRemovedMock.Listen);
            GameObject elementOne = new GameObject();
            GameObject elementTwo = new GameObject();

            subject.IncreaseCount(elementOne);
            subject.IncreaseCount(elementOne);
            subject.IncreaseCount(elementTwo);
            subject.IncreaseCount(elementTwo);

            elementAddedMock.Reset();
            elementRemovedMock.Reset();

            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsFalse(elementRemovedMock.Received);
            Assert.AreEqual(2, subject.GetCount(elementOne));
            Assert.AreEqual(2, subject.GetCount(elementTwo));

            subject.Clear();

            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsTrue(elementRemovedMock.Received);
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
            UnityEventListenerMock elementAddedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementRemovedMock = new UnityEventListenerMock();
            subject.ElementAdded.AddListener(elementAddedMock.Listen);
            subject.ElementRemoved.AddListener(elementRemovedMock.Listen);
            GameObject elementOne = new GameObject();

            subject.gameObject.SetActive(false);

            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsFalse(elementRemovedMock.Received);
            Assert.AreEqual(0, subject.GetCount(elementOne));

            subject.IncreaseCount(elementOne);

            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsFalse(elementRemovedMock.Received);
            Assert.AreEqual(0, subject.GetCount(elementOne));

            elementAddedMock.Reset();
            elementRemovedMock.Reset();

            subject.IncreaseCount(elementOne);

            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsFalse(elementRemovedMock.Received);
            Assert.AreEqual(0, subject.GetCount(elementOne));

            Assert.IsFalse(subject.ElementsCounter.ContainsKey(elementOne));

            Object.DestroyImmediate(elementOne);
        }

        [Test]
        public void IncreaseCountInactiveComponent()
        {
            UnityEventListenerMock elementAddedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementRemovedMock = new UnityEventListenerMock();
            subject.ElementAdded.AddListener(elementAddedMock.Listen);
            subject.ElementRemoved.AddListener(elementRemovedMock.Listen);
            GameObject elementOne = new GameObject();

            subject.enabled = false;

            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsFalse(elementRemovedMock.Received);
            Assert.AreEqual(0, subject.GetCount(elementOne));

            subject.IncreaseCount(elementOne);

            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsFalse(elementRemovedMock.Received);
            Assert.AreEqual(0, subject.GetCount(elementOne));

            elementAddedMock.Reset();
            elementRemovedMock.Reset();

            subject.IncreaseCount(elementOne);

            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsFalse(elementRemovedMock.Received);
            Assert.AreEqual(0, subject.GetCount(elementOne));

            Assert.IsFalse(subject.ElementsCounter.ContainsKey(elementOne));

            Object.DestroyImmediate(elementOne);
        }

        [Test]
        public void DecreaseCountInactiveGameObject()
        {
            UnityEventListenerMock elementAddedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementRemovedMock = new UnityEventListenerMock();
            subject.ElementAdded.AddListener(elementAddedMock.Listen);
            subject.ElementRemoved.AddListener(elementRemovedMock.Listen);
            GameObject elementOne = new GameObject();

            subject.IncreaseCount(elementOne);
            subject.IncreaseCount(elementOne);

            elementAddedMock.Reset();
            elementRemovedMock.Reset();

            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsFalse(elementRemovedMock.Received);
            Assert.AreEqual(2, subject.GetCount(elementOne));

            subject.gameObject.SetActive(false);

            subject.DecreaseCount(elementOne);

            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsFalse(elementRemovedMock.Received);
            Assert.AreEqual(2, subject.GetCount(elementOne));

            elementAddedMock.Reset();
            elementRemovedMock.Reset();

            subject.DecreaseCount(elementOne);

            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsFalse(elementRemovedMock.Received);
            Assert.AreEqual(2, subject.GetCount(elementOne));

            Assert.IsTrue(subject.ElementsCounter.ContainsKey(elementOne));

            Object.DestroyImmediate(elementOne);
        }

        [Test]
        public void DecreaseCountInactiveComponent()
        {
            UnityEventListenerMock elementAddedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementRemovedMock = new UnityEventListenerMock();
            subject.ElementAdded.AddListener(elementAddedMock.Listen);
            subject.ElementRemoved.AddListener(elementRemovedMock.Listen);
            GameObject elementOne = new GameObject();

            subject.IncreaseCount(elementOne);
            subject.IncreaseCount(elementOne);

            elementAddedMock.Reset();
            elementRemovedMock.Reset();

            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsFalse(elementRemovedMock.Received);
            Assert.AreEqual(2, subject.GetCount(elementOne));

            subject.enabled = false;

            subject.DecreaseCount(elementOne);

            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsFalse(elementRemovedMock.Received);
            Assert.AreEqual(2, subject.GetCount(elementOne));

            elementAddedMock.Reset();
            elementRemovedMock.Reset();

            subject.DecreaseCount(elementOne);

            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsFalse(elementRemovedMock.Received);
            Assert.AreEqual(2, subject.GetCount(elementOne));

            Assert.IsTrue(subject.ElementsCounter.ContainsKey(elementOne));

            Object.DestroyImmediate(elementOne);
        }

        [Test]
        public void RemoveFromCountInactiveGameObject()
        {
            UnityEventListenerMock elementAddedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementRemovedMock = new UnityEventListenerMock();
            subject.ElementAdded.AddListener(elementAddedMock.Listen);
            subject.ElementRemoved.AddListener(elementRemovedMock.Listen);
            GameObject elementOne = new GameObject();

            subject.IncreaseCount(elementOne);
            subject.IncreaseCount(elementOne);

            elementAddedMock.Reset();
            elementRemovedMock.Reset();

            subject.gameObject.SetActive(false);

            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsFalse(elementRemovedMock.Received);
            Assert.AreEqual(2, subject.GetCount(elementOne));

            subject.RemoveFromCount(elementOne);

            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsFalse(elementRemovedMock.Received);
            Assert.AreEqual(2, subject.GetCount(elementOne));

            Assert.IsTrue(subject.ElementsCounter.ContainsKey(elementOne));

            Object.DestroyImmediate(elementOne);
        }

        [Test]
        public void RemoveFromCountInactiveComponent()
        {
            UnityEventListenerMock elementAddedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementRemovedMock = new UnityEventListenerMock();
            subject.ElementAdded.AddListener(elementAddedMock.Listen);
            subject.ElementRemoved.AddListener(elementRemovedMock.Listen);
            GameObject elementOne = new GameObject();

            subject.IncreaseCount(elementOne);
            subject.IncreaseCount(elementOne);

            elementAddedMock.Reset();
            elementRemovedMock.Reset();

            subject.enabled = false;

            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsFalse(elementRemovedMock.Received);
            Assert.AreEqual(2, subject.GetCount(elementOne));

            subject.RemoveFromCount(elementOne);

            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsFalse(elementRemovedMock.Received);
            Assert.AreEqual(2, subject.GetCount(elementOne));

            Assert.IsTrue(subject.ElementsCounter.ContainsKey(elementOne));

            Object.DestroyImmediate(elementOne);
        }

        [Test]
        public void ClearInactiveGameObject()
        {
            UnityEventListenerMock elementAddedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementRemovedMock = new UnityEventListenerMock();
            subject.ElementAdded.AddListener(elementAddedMock.Listen);
            subject.ElementRemoved.AddListener(elementRemovedMock.Listen);
            GameObject elementOne = new GameObject();
            GameObject elementTwo = new GameObject();

            subject.IncreaseCount(elementOne);
            subject.IncreaseCount(elementOne);
            subject.IncreaseCount(elementTwo);
            subject.IncreaseCount(elementTwo);

            elementAddedMock.Reset();
            elementRemovedMock.Reset();

            subject.gameObject.SetActive(false);

            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsFalse(elementRemovedMock.Received);
            Assert.AreEqual(2, subject.GetCount(elementOne));
            Assert.AreEqual(2, subject.GetCount(elementTwo));

            subject.Clear();

            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsFalse(elementRemovedMock.Received);
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
            UnityEventListenerMock elementAddedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementRemovedMock = new UnityEventListenerMock();
            subject.ElementAdded.AddListener(elementAddedMock.Listen);
            subject.ElementRemoved.AddListener(elementRemovedMock.Listen);
            GameObject elementOne = new GameObject();
            GameObject elementTwo = new GameObject();

            subject.IncreaseCount(elementOne);
            subject.IncreaseCount(elementOne);
            subject.IncreaseCount(elementTwo);
            subject.IncreaseCount(elementTwo);

            elementAddedMock.Reset();
            elementRemovedMock.Reset();

            subject.enabled = false;

            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsFalse(elementRemovedMock.Received);
            Assert.AreEqual(2, subject.GetCount(elementOne));
            Assert.AreEqual(2, subject.GetCount(elementTwo));

            subject.Clear();

            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsFalse(elementRemovedMock.Received);
            Assert.AreEqual(2, subject.GetCount(elementOne));
            Assert.AreEqual(2, subject.GetCount(elementTwo));

            Assert.IsTrue(subject.ElementsCounter.ContainsKey(elementOne));
            Assert.IsTrue(subject.ElementsCounter.ContainsKey(elementTwo));

            Object.DestroyImmediate(elementOne);
            Object.DestroyImmediate(elementTwo);
        }
    }
}