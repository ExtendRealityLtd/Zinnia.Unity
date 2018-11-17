using VRTK.Core.Data.Collection;

namespace Test.VRTK.Core.Data.Collection
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.VRTK.Core.Utility.Mock;

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
            UnityEventListenerMock becameEmptyMock = new UnityEventListenerMock();
            UnityEventListenerMock becamePopulatedMock = new UnityEventListenerMock();
            subject.BecameEmpty.AddListener(becameEmptyMock.Listen);
            subject.BecamePopulated.AddListener(becamePopulatedMock.Listen);
            GameObject elementOne = new GameObject();
            GameObject elementTwo = new GameObject();

            Assert.IsEmpty(subject.Elements);
            subject.AddElement(elementOne);
            Assert.IsNotEmpty(subject.Elements);
            Assert.IsFalse(becameEmptyMock.Received);
            Assert.IsTrue(becamePopulatedMock.Received);

            becameEmptyMock.Reset();
            becamePopulatedMock.Reset();

            subject.AddElement(elementTwo);
            Assert.IsNotEmpty(subject.Elements);

            Assert.IsFalse(becameEmptyMock.Received);
            Assert.IsFalse(becamePopulatedMock.Received);

            Object.DestroyImmediate(elementOne);
            Object.DestroyImmediate(elementTwo);
        }

        [Test]
        public void AddElementInactiveGameObject()
        {
            UnityEventListenerMock becameEmptyMock = new UnityEventListenerMock();
            UnityEventListenerMock becamePopulatedMock = new UnityEventListenerMock();
            subject.BecameEmpty.AddListener(becameEmptyMock.Listen);
            subject.BecamePopulated.AddListener(becamePopulatedMock.Listen);
            subject.gameObject.SetActive(false);

            GameObject elementOne = new GameObject();

            Assert.IsEmpty(subject.Elements);
            subject.AddElement(elementOne);

            Assert.IsEmpty(subject.Elements);
            Assert.IsFalse(becameEmptyMock.Received);
            Assert.IsFalse(becamePopulatedMock.Received);

            Object.DestroyImmediate(elementOne);
        }

        [Test]
        public void AddElementInactiveComponent()
        {
            UnityEventListenerMock becameEmptyMock = new UnityEventListenerMock();
            UnityEventListenerMock becamePopulatedMock = new UnityEventListenerMock();
            subject.BecameEmpty.AddListener(becameEmptyMock.Listen);
            subject.BecamePopulated.AddListener(becamePopulatedMock.Listen);
            subject.enabled = false;

            GameObject elementOne = new GameObject();

            Assert.IsEmpty(subject.Elements);
            subject.AddElement(elementOne);

            Assert.IsEmpty(subject.Elements);
            Assert.IsFalse(becameEmptyMock.Received);
            Assert.IsFalse(becamePopulatedMock.Received);

            Object.DestroyImmediate(elementOne);
        }

        [Test]
        public void AddElementInvalidElement()
        {
            UnityEventListenerMock becameEmptyMock = new UnityEventListenerMock();
            UnityEventListenerMock becamePopulatedMock = new UnityEventListenerMock();
            subject.BecameEmpty.AddListener(becameEmptyMock.Listen);
            subject.BecamePopulated.AddListener(becamePopulatedMock.Listen);

            Assert.IsEmpty(subject.Elements);
            subject.AddElement(null);

            Assert.IsEmpty(subject.Elements);
            Assert.IsFalse(becameEmptyMock.Received);
            Assert.IsFalse(becamePopulatedMock.Received);
        }

        [Test]
        public void RemoveElement()
        {
            UnityEventListenerMock becameEmptyMock = new UnityEventListenerMock();
            UnityEventListenerMock becamePopulatedMock = new UnityEventListenerMock();
            subject.BecameEmpty.AddListener(becameEmptyMock.Listen);
            subject.BecamePopulated.AddListener(becamePopulatedMock.Listen);
            GameObject elementOne = new GameObject();
            GameObject elementTwo = new GameObject();

            subject.AddElement(elementOne);
            subject.AddElement(elementTwo);
            becameEmptyMock.Reset();
            becamePopulatedMock.Reset();

            Assert.IsNotEmpty(subject.Elements);

            subject.RemoveElement(elementTwo);

            Assert.IsNotEmpty(subject.Elements);
            Assert.IsFalse(becameEmptyMock.Received);
            Assert.IsFalse(becamePopulatedMock.Received);

            becameEmptyMock.Reset();
            becamePopulatedMock.Reset();

            subject.RemoveElement(elementOne);

            Assert.IsEmpty(subject.Elements);
            Assert.IsTrue(becameEmptyMock.Received);
            Assert.IsFalse(becamePopulatedMock.Received);

            Object.DestroyImmediate(elementOne);
            Object.DestroyImmediate(elementTwo);
        }

        [Test]
        public void RemoveElementInactiveGameObject()
        {
            UnityEventListenerMock becameEmptyMock = new UnityEventListenerMock();
            UnityEventListenerMock becamePopulatedMock = new UnityEventListenerMock();
            subject.BecameEmpty.AddListener(becameEmptyMock.Listen);
            subject.BecamePopulated.AddListener(becamePopulatedMock.Listen);

            GameObject elementOne = new GameObject();
            GameObject elementTwo = new GameObject();

            subject.AddElement(elementOne);
            subject.AddElement(elementTwo);
            becameEmptyMock.Reset();
            becamePopulatedMock.Reset();

            subject.gameObject.SetActive(false);

            Assert.IsNotEmpty(subject.Elements);

            subject.RemoveElement(elementTwo);

            Assert.IsNotEmpty(subject.Elements);
            Assert.IsFalse(becameEmptyMock.Received);
            Assert.IsFalse(becamePopulatedMock.Received);

            becameEmptyMock.Reset();
            becamePopulatedMock.Reset();

            subject.RemoveElement(elementOne);

            Assert.IsNotEmpty(subject.Elements);
            Assert.IsFalse(becameEmptyMock.Received);
            Assert.IsFalse(becamePopulatedMock.Received);

            Object.DestroyImmediate(elementOne);
            Object.DestroyImmediate(elementTwo);
        }

        [Test]
        public void RemoveElementInactiveComponent()
        {
            UnityEventListenerMock becameEmptyMock = new UnityEventListenerMock();
            UnityEventListenerMock becamePopulatedMock = new UnityEventListenerMock();
            subject.BecameEmpty.AddListener(becameEmptyMock.Listen);
            subject.BecamePopulated.AddListener(becamePopulatedMock.Listen);

            GameObject elementOne = new GameObject();
            GameObject elementTwo = new GameObject();

            subject.AddElement(elementOne);
            subject.AddElement(elementTwo);
            becameEmptyMock.Reset();
            becamePopulatedMock.Reset();

            subject.enabled = false;

            Assert.IsNotEmpty(subject.Elements);

            subject.RemoveElement(elementTwo);

            Assert.IsNotEmpty(subject.Elements);
            Assert.IsFalse(becameEmptyMock.Received);
            Assert.IsFalse(becamePopulatedMock.Received);

            becameEmptyMock.Reset();
            becamePopulatedMock.Reset();

            subject.RemoveElement(elementOne);

            Assert.IsNotEmpty(subject.Elements);
            Assert.IsFalse(becameEmptyMock.Received);
            Assert.IsFalse(becamePopulatedMock.Received);

            Object.DestroyImmediate(elementOne);
            Object.DestroyImmediate(elementTwo);
        }

        [Test]
        public void RemoveElementInvalidElement()
        {
            UnityEventListenerMock becameEmptyMock = new UnityEventListenerMock();
            UnityEventListenerMock becamePopulatedMock = new UnityEventListenerMock();
            subject.BecameEmpty.AddListener(becameEmptyMock.Listen);
            subject.BecamePopulated.AddListener(becamePopulatedMock.Listen);

            GameObject elementOne = new GameObject();
            GameObject elementTwo = new GameObject();
            GameObject elementThree = new GameObject();

            subject.AddElement(elementOne);
            subject.AddElement(elementTwo);
            becameEmptyMock.Reset();
            becamePopulatedMock.Reset();

            Assert.IsNotEmpty(subject.Elements);

            subject.RemoveElement(elementTwo);

            Assert.IsNotEmpty(subject.Elements);
            Assert.IsFalse(becameEmptyMock.Received);
            Assert.IsFalse(becamePopulatedMock.Received);

            becameEmptyMock.Reset();
            becamePopulatedMock.Reset();

            subject.RemoveElement(elementThree);

            Assert.IsNotEmpty(subject.Elements);
            Assert.IsFalse(becameEmptyMock.Received);
            Assert.IsFalse(becamePopulatedMock.Received);

            subject.RemoveElement(null);

            Assert.IsNotEmpty(subject.Elements);
            Assert.IsFalse(becameEmptyMock.Received);
            Assert.IsFalse(becamePopulatedMock.Received);

            Object.DestroyImmediate(elementOne);
            Object.DestroyImmediate(elementTwo);
            Object.DestroyImmediate(elementThree);
        }

        [Test]
        public void ClearElements()
        {
            GameObject elementOne = new GameObject();
            GameObject elementTwo = new GameObject();

            subject.AddElement(elementOne);
            subject.AddElement(elementTwo);

            Assert.IsNotEmpty(subject.Elements);

            subject.ClearElements();

            Assert.IsEmpty(subject.Elements);

            Object.DestroyImmediate(elementOne);
            Object.DestroyImmediate(elementTwo);
        }

        [Test]
        public void ClearElementsInactiveGameObject()
        {
            GameObject elementOne = new GameObject();
            GameObject elementTwo = new GameObject();

            subject.AddElement(elementOne);
            subject.AddElement(elementTwo);

            Assert.IsNotEmpty(subject.Elements);

            subject.gameObject.SetActive(false);

            subject.ClearElements();

            Assert.IsNotEmpty(subject.Elements);

            Object.DestroyImmediate(elementOne);
            Object.DestroyImmediate(elementTwo);
        }

        [Test]
        public void ClearElementsInactiveComponent()
        {
            GameObject elementOne = new GameObject();
            GameObject elementTwo = new GameObject();

            subject.AddElement(elementOne);
            subject.AddElement(elementTwo);

            Assert.IsNotEmpty(subject.Elements);

            subject.enabled = false;

            subject.ClearElements();

            Assert.IsNotEmpty(subject.Elements);

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
    }
}