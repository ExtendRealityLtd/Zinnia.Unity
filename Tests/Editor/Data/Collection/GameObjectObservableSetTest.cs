using Zinnia.Data.Collection;

namespace Test.Zinnia.Data.Collection
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;

    public class GameObjectObservableSetTest
    {
        private GameObject containingObject;
        private GameObjectObservableSet subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<GameObjectObservableSet>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void AddElement()
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
            subject.AddElement(elementOne);
            Assert.AreEqual(1, subject.Elements.Count);
            Assert.IsTrue(becamePopulatedMock.Received);
            Assert.IsTrue(elementAddedMock.Received);
            Assert.IsFalse(elementRemovedMock.Received);
            Assert.IsFalse(becameEmptyMock.Received);

            becamePopulatedMock.Reset();
            elementAddedMock.Reset();
            elementRemovedMock.Reset();
            becameEmptyMock.Reset();

            subject.AddElement(elementTwo);
            Assert.AreEqual(2, subject.Elements.Count);
            Assert.IsFalse(becamePopulatedMock.Received);
            Assert.IsTrue(elementAddedMock.Received);
            Assert.IsFalse(elementRemovedMock.Received);
            Assert.IsFalse(becameEmptyMock.Received);

            becamePopulatedMock.Reset();
            elementAddedMock.Reset();
            elementRemovedMock.Reset();
            becameEmptyMock.Reset();

            subject.AddElement(elementTwo);
            Assert.AreEqual(2, subject.Elements.Count);
            Assert.IsFalse(becamePopulatedMock.Received);
            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsFalse(elementRemovedMock.Received);
            Assert.IsFalse(becameEmptyMock.Received);

            Object.DestroyImmediate(elementOne);
            Object.DestroyImmediate(elementTwo);
        }

        [Test]
        public void AddElementInactiveGameObject()
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
            subject.gameObject.SetActive(false);

            Assert.IsEmpty(subject.Elements);
            subject.AddElement(elementOne);

            Assert.IsEmpty(subject.Elements);
            Assert.IsFalse(becamePopulatedMock.Received);
            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsFalse(elementRemovedMock.Received);
            Assert.IsFalse(becameEmptyMock.Received);

            Object.DestroyImmediate(elementOne);
        }

        [Test]
        public void AddElementInactiveComponent()
        {
            UnityEventListenerMock becamePopulatedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementAddedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementRemovedMock = new UnityEventListenerMock();
            UnityEventListenerMock becameEmptyMock = new UnityEventListenerMock();
            subject.BecamePopulated.AddListener(becamePopulatedMock.Listen);
            subject.ElementAdded.AddListener(elementAddedMock.Listen);
            subject.ElementRemoved.AddListener(elementRemovedMock.Listen);
            subject.BecameEmpty.AddListener(becameEmptyMock.Listen);
            subject.enabled = false;

            GameObject elementOne = new GameObject();

            Assert.IsEmpty(subject.Elements);
            subject.AddElement(elementOne);

            Assert.IsEmpty(subject.Elements);
            Assert.IsFalse(becamePopulatedMock.Received);
            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsFalse(elementRemovedMock.Received);
            Assert.IsFalse(becameEmptyMock.Received);

            Object.DestroyImmediate(elementOne);
        }

        [Test]
        public void AddElementInvalidElement()
        {
            UnityEventListenerMock becamePopulatedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementAddedMock = new UnityEventListenerMock();
            UnityEventListenerMock elementRemovedMock = new UnityEventListenerMock();
            UnityEventListenerMock becameEmptyMock = new UnityEventListenerMock();
            subject.BecamePopulated.AddListener(becamePopulatedMock.Listen);
            subject.ElementAdded.AddListener(elementAddedMock.Listen);
            subject.ElementRemoved.AddListener(elementRemovedMock.Listen);
            subject.BecameEmpty.AddListener(becameEmptyMock.Listen);

            Assert.IsEmpty(subject.Elements);
            subject.AddElement(null);

            Assert.IsEmpty(subject.Elements);
            Assert.IsFalse(becamePopulatedMock.Received);
            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsFalse(elementRemovedMock.Received);
            Assert.IsFalse(becameEmptyMock.Received);
        }

        [Test]
        public void RemoveElement()
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

            subject.AddElement(elementOne);
            subject.AddElement(elementTwo);
            Assert.AreEqual(2, subject.Elements.Count);

            becamePopulatedMock.Reset();
            elementAddedMock.Reset();
            elementRemovedMock.Reset();
            becameEmptyMock.Reset();

            subject.RemoveElement(elementTwo);

            Assert.AreEqual(1, subject.Elements.Count);
            Assert.IsFalse(becamePopulatedMock.Received);
            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsTrue(elementRemovedMock.Received);
            Assert.IsFalse(becameEmptyMock.Received);

            becamePopulatedMock.Reset();
            elementAddedMock.Reset();
            elementRemovedMock.Reset();
            becameEmptyMock.Reset();

            subject.RemoveElement(elementTwo);

            Assert.AreEqual(1, subject.Elements.Count);
            Assert.IsFalse(becamePopulatedMock.Received);
            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsFalse(elementRemovedMock.Received);
            Assert.IsFalse(becameEmptyMock.Received);

            becamePopulatedMock.Reset();
            elementAddedMock.Reset();
            elementRemovedMock.Reset();
            becameEmptyMock.Reset();

            subject.RemoveElement(elementOne);

            Assert.IsEmpty(subject.Elements);
            Assert.IsFalse(becamePopulatedMock.Received);
            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsTrue(elementRemovedMock.Received);
            Assert.IsTrue(becameEmptyMock.Received);

            Object.DestroyImmediate(elementOne);
            Object.DestroyImmediate(elementTwo);
        }

        [Test]
        public void RemoveElementInactiveGameObject()
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

            subject.AddElement(elementOne);
            subject.AddElement(elementTwo);
            Assert.AreEqual(2, subject.Elements.Count);

            becamePopulatedMock.Reset();
            elementAddedMock.Reset();
            elementRemovedMock.Reset();
            becameEmptyMock.Reset();

            subject.gameObject.SetActive(false);
            subject.RemoveElement(elementTwo);

            Assert.AreEqual(2, subject.Elements.Count);
            Assert.IsFalse(becamePopulatedMock.Received);
            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsFalse(elementRemovedMock.Received);
            Assert.IsFalse(becameEmptyMock.Received);

            becamePopulatedMock.Reset();
            elementAddedMock.Reset();
            elementRemovedMock.Reset();
            becameEmptyMock.Reset();

            subject.RemoveElement(elementOne);

            Assert.AreEqual(2, subject.Elements.Count);
            Assert.IsFalse(becamePopulatedMock.Received);
            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsFalse(elementRemovedMock.Received);
            Assert.IsFalse(becameEmptyMock.Received);

            Object.DestroyImmediate(elementOne);
            Object.DestroyImmediate(elementTwo);
        }

        [Test]
        public void RemoveElementInactiveComponent()
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

            subject.AddElement(elementOne);
            subject.AddElement(elementTwo);
            Assert.AreEqual(2, subject.Elements.Count);

            becamePopulatedMock.Reset();
            elementAddedMock.Reset();
            elementRemovedMock.Reset();
            becameEmptyMock.Reset();

            subject.enabled = false;
            subject.RemoveElement(elementTwo);

            Assert.AreEqual(2, subject.Elements.Count);
            Assert.IsFalse(becamePopulatedMock.Received);
            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsFalse(elementRemovedMock.Received);
            Assert.IsFalse(becameEmptyMock.Received);

            becamePopulatedMock.Reset();
            elementAddedMock.Reset();
            elementRemovedMock.Reset();
            becameEmptyMock.Reset();

            subject.RemoveElement(elementOne);

            Assert.AreEqual(2, subject.Elements.Count);
            Assert.IsFalse(becamePopulatedMock.Received);
            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsFalse(elementRemovedMock.Received);
            Assert.IsFalse(becameEmptyMock.Received);

            Object.DestroyImmediate(elementOne);
            Object.DestroyImmediate(elementTwo);
        }

        [Test]
        public void RemoveElementInvalidElement()
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

            subject.AddElement(elementOne);
            subject.AddElement(elementTwo);
            Assert.AreEqual(2, subject.Elements.Count);

            becamePopulatedMock.Reset();
            elementAddedMock.Reset();
            elementRemovedMock.Reset();
            becameEmptyMock.Reset();

            subject.RemoveElement(elementTwo);

            Assert.AreEqual(1, subject.Elements.Count);
            Assert.IsFalse(becamePopulatedMock.Received);
            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsTrue(elementRemovedMock.Received);
            Assert.IsFalse(becameEmptyMock.Received);

            becamePopulatedMock.Reset();
            elementAddedMock.Reset();
            elementRemovedMock.Reset();
            becameEmptyMock.Reset();

            subject.RemoveElement(elementThree);

            Assert.AreEqual(1, subject.Elements.Count);
            Assert.IsFalse(becamePopulatedMock.Received);
            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsFalse(elementRemovedMock.Received);
            Assert.IsFalse(becameEmptyMock.Received);

            subject.RemoveElement(null);

            Assert.AreEqual(1, subject.Elements.Count);
            Assert.IsFalse(becamePopulatedMock.Received);
            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsFalse(elementRemovedMock.Received);
            Assert.IsFalse(becameEmptyMock.Received);

            Object.DestroyImmediate(elementOne);
            Object.DestroyImmediate(elementTwo);
            Object.DestroyImmediate(elementThree);
        }

        [Test]
        public void ClearElements()
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

            subject.AddElement(elementOne);
            subject.AddElement(elementTwo);
            Assert.AreEqual(2, subject.Elements.Count);

            becamePopulatedMock.Reset();
            elementAddedMock.Reset();
            elementRemovedMock.Reset();
            becameEmptyMock.Reset();

            subject.ClearElements();
            Assert.IsEmpty(subject.Elements);
            Assert.IsFalse(becamePopulatedMock.Received);
            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsTrue(elementRemovedMock.Received);
            Assert.IsTrue(becameEmptyMock.Received);

            Object.DestroyImmediate(elementOne);
            Object.DestroyImmediate(elementTwo);
        }

        [Test]
        public void ClearElementsInactiveGameObject()
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

            subject.AddElement(elementOne);
            subject.AddElement(elementTwo);
            Assert.AreEqual(2, subject.Elements.Count);

            becamePopulatedMock.Reset();
            elementAddedMock.Reset();
            elementRemovedMock.Reset();
            becameEmptyMock.Reset();

            subject.gameObject.SetActive(false);
            subject.ClearElements();

            Assert.IsNotEmpty(subject.Elements);
            Assert.IsFalse(becamePopulatedMock.Received);
            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsFalse(elementRemovedMock.Received);
            Assert.IsFalse(becameEmptyMock.Received);

            Object.DestroyImmediate(elementOne);
            Object.DestroyImmediate(elementTwo);
        }

        [Test]
        public void ClearElementsInactiveComponent()
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

            subject.AddElement(elementOne);
            subject.AddElement(elementTwo);
            Assert.AreEqual(2, subject.Elements.Count);

            becamePopulatedMock.Reset();
            elementAddedMock.Reset();
            elementRemovedMock.Reset();
            becameEmptyMock.Reset();

            subject.enabled = false;
            subject.ClearElements();

            Assert.IsNotEmpty(subject.Elements);
            Assert.IsFalse(becamePopulatedMock.Received);
            Assert.IsFalse(elementAddedMock.Received);
            Assert.IsFalse(elementRemovedMock.Received);
            Assert.IsFalse(becameEmptyMock.Received);

            Object.DestroyImmediate(elementOne);
            Object.DestroyImmediate(elementTwo);
        }

        [Test]
        public void ContainsFound()
        {
            UnityEventListenerMock elementFoundMock = new UnityEventListenerMock();
            UnityEventListenerMock elementNotFoundMock = new UnityEventListenerMock();
            subject.ElementFound.AddListener(elementFoundMock.Listen);
            subject.ElementNotFound.AddListener(elementNotFoundMock.Listen);
            GameObject elementOne = new GameObject();
            GameObject elementTwo = new GameObject();

            subject.AddElement(elementOne);
            subject.AddElement(elementTwo);
            elementFoundMock.Reset();
            elementNotFoundMock.Reset();

            Assert.IsTrue(subject.Contains(elementOne));
            Assert.IsFalse(elementNotFoundMock.Received);
            Assert.IsTrue(elementFoundMock.Received);

            Object.DestroyImmediate(elementOne);
            Object.DestroyImmediate(elementTwo);
        }

        [Test]
        public void ContainsFoundInactiveGameObject()
        {
            UnityEventListenerMock elementFoundMock = new UnityEventListenerMock();
            UnityEventListenerMock elementNotFoundMock = new UnityEventListenerMock();
            subject.ElementFound.AddListener(elementFoundMock.Listen);
            subject.ElementNotFound.AddListener(elementNotFoundMock.Listen);
            GameObject elementOne = new GameObject();
            GameObject elementTwo = new GameObject();

            subject.AddElement(elementOne);
            subject.AddElement(elementTwo);
            elementFoundMock.Reset();
            elementNotFoundMock.Reset();

            subject.gameObject.SetActive(false);

            Assert.IsFalse(subject.Contains(elementOne));
            Assert.IsFalse(elementNotFoundMock.Received);
            Assert.IsFalse(elementFoundMock.Received);

            Object.DestroyImmediate(elementOne);
            Object.DestroyImmediate(elementTwo);
        }

        [Test]
        public void ContainsFoundInactiveComponent()
        {
            UnityEventListenerMock elementFoundMock = new UnityEventListenerMock();
            UnityEventListenerMock elementNotFoundMock = new UnityEventListenerMock();
            subject.ElementFound.AddListener(elementFoundMock.Listen);
            subject.ElementNotFound.AddListener(elementNotFoundMock.Listen);
            GameObject elementOne = new GameObject();
            GameObject elementTwo = new GameObject();

            subject.AddElement(elementOne);
            subject.AddElement(elementTwo);
            elementFoundMock.Reset();
            elementNotFoundMock.Reset();

            subject.enabled = false;

            Assert.IsFalse(subject.Contains(elementOne));
            Assert.IsFalse(elementNotFoundMock.Received);
            Assert.IsFalse(elementFoundMock.Received);

            Object.DestroyImmediate(elementOne);
            Object.DestroyImmediate(elementTwo);
        }

        [Test]
        public void ContainsNotFound()
        {
            UnityEventListenerMock elementFoundMock = new UnityEventListenerMock();
            UnityEventListenerMock elementNotFoundMock = new UnityEventListenerMock();
            subject.ElementFound.AddListener(elementFoundMock.Listen);
            subject.ElementNotFound.AddListener(elementNotFoundMock.Listen);
            GameObject elementOne = new GameObject();
            GameObject elementTwo = new GameObject();

            subject.AddElement(elementTwo);
            elementFoundMock.Reset();
            elementNotFoundMock.Reset();

            Assert.IsFalse(subject.Contains(elementOne));
            Assert.IsTrue(elementNotFoundMock.Received);
            Assert.IsFalse(elementFoundMock.Received);

            Object.DestroyImmediate(elementOne);
            Object.DestroyImmediate(elementTwo);
        }

        [Test]
        public void ContainsNotFoundInactiveGameObject()
        {
            UnityEventListenerMock elementFoundMock = new UnityEventListenerMock();
            UnityEventListenerMock elementNotFoundMock = new UnityEventListenerMock();
            subject.ElementFound.AddListener(elementFoundMock.Listen);
            subject.ElementNotFound.AddListener(elementNotFoundMock.Listen);
            GameObject elementOne = new GameObject();
            GameObject elementTwo = new GameObject();

            subject.AddElement(elementTwo);
            elementFoundMock.Reset();
            elementNotFoundMock.Reset();

            subject.gameObject.SetActive(false);

            Assert.IsFalse(subject.Contains(elementOne));
            Assert.IsFalse(elementNotFoundMock.Received);
            Assert.IsFalse(elementFoundMock.Received);

            Object.DestroyImmediate(elementOne);
            Object.DestroyImmediate(elementTwo);
        }

        [Test]
        public void ContainsNotFoundInactiveComponent()
        {
            UnityEventListenerMock elementFoundMock = new UnityEventListenerMock();
            UnityEventListenerMock elementNotFoundMock = new UnityEventListenerMock();
            subject.ElementFound.AddListener(elementFoundMock.Listen);
            subject.ElementNotFound.AddListener(elementNotFoundMock.Listen);
            GameObject elementOne = new GameObject();
            GameObject elementTwo = new GameObject();

            subject.AddElement(elementTwo);
            elementFoundMock.Reset();
            elementNotFoundMock.Reset();

            subject.enabled = false;

            Assert.IsFalse(subject.Contains(elementOne));
            Assert.IsFalse(elementNotFoundMock.Received);
            Assert.IsFalse(elementFoundMock.Received);

            Object.DestroyImmediate(elementOne);
            Object.DestroyImmediate(elementTwo);
        }

        [Test]
        public void EmitsOnStart()
        {
            GameObjectObservableSetMock mock = containingObject.AddComponent<GameObjectObservableSetMock>();
            GameObject elementOne = new GameObject();
            GameObject elementTwo = new GameObject();
            mock.AddElement(elementOne);
            mock.AddElement(elementTwo);

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

        private sealed class GameObjectObservableSetMock : GameObjectObservableSet
        {
            public void ManualStart()
            {
                Start();
            }
        }
    }
}