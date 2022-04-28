using Zinnia.Data.Operation;

namespace Test.Zinnia.Data.Operation
{
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using Assert = UnityEngine.Assertions.Assert;

    public class GameObjectClonerTest
    {
        private GameObject containingObject;
        private GameObjectCloner subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<GameObjectCloner>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void DoesNotCloneNullSource()
        {
            UnityEventValueListenerMock<GameObject> clonedMock = new UnityEventValueListenerMock<GameObject>();
            subject.Cloned.AddListener(clonedMock.Listen);

            subject.Source = null;
            GameObject actual = subject.Clone();

            Assert.IsNull(actual);
            Assert.IsFalse(clonedMock.Received);
            Assert.IsNull(clonedMock.Value);

            clonedMock.Reset();

            actual = subject.Clone(null);

            Assert.IsNull(actual);
            Assert.IsFalse(clonedMock.Received);
            Assert.IsNull(clonedMock.Value);

            Object.DestroyImmediate(actual);
        }

        [Test]
        public void CreatesSourceClone()
        {
            UnityEventValueListenerMock<GameObject> clonedMock = new UnityEventValueListenerMock<GameObject>();
            subject.Cloned.AddListener(clonedMock.Listen);
            GameObject expected = new GameObject();

            subject.Source = expected;
            GameObject actual = subject.Clone();

            Assert.IsNotNull(actual);
            Assert.AreNotEqual(expected, actual);
            Assert.IsTrue(clonedMock.Received);
            Assert.AreEqual(actual, clonedMock.Value);

            clonedMock.Reset();
            Object.DestroyImmediate(actual);

            subject.Source = null;
            actual = subject.Clone(expected);

            Assert.IsNotNull(actual);
            Assert.AreNotEqual(expected, actual);
            Assert.IsTrue(clonedMock.Received);
            Assert.AreEqual(actual, clonedMock.Value);

            Object.DestroyImmediate(actual);
            Object.DestroyImmediate(expected);
        }

        [Test]
        public void ParentsCloneToParent()
        {
            UnityEventValueListenerMock<GameObject> clonedMock = new UnityEventValueListenerMock<GameObject>();
            subject.Cloned.AddListener(clonedMock.Listen);
            GameObject source = new GameObject();
            GameObject expected = new GameObject();
            subject.Parent = expected;

            subject.Source = source;
            GameObject actual = subject.Clone();
            Assert.AreEqual(expected, actual.transform.parent.gameObject);

            clonedMock.Reset();
            Object.DestroyImmediate(actual);

            subject.Source = null;
            actual = subject.Clone(source);
            Assert.AreEqual(expected, actual.transform.parent.gameObject);

            Object.DestroyImmediate(actual);
            Object.DestroyImmediate(source);
            Object.DestroyImmediate(expected);
        }

        [Test]
        public void DoesNotChangeSource()
        {
            UnityEventValueListenerMock<GameObject> clonedMock = new UnityEventValueListenerMock<GameObject>();
            subject.Cloned.AddListener(clonedMock.Listen);
            GameObject gameObject = new GameObject();

            subject.Source = gameObject;
            GameObject actual = subject.Clone();
            Assert.AreEqual(gameObject, subject.Source);

            clonedMock.Reset();
            Object.DestroyImmediate(actual);

            subject.Source = null;
            actual = subject.Clone(gameObject);
            Assert.AreEqual(null, subject.Source);

            Object.DestroyImmediate(actual);
            Object.DestroyImmediate(gameObject);
        }

        [Test]
        public void ClearSource()
        {
            Assert.IsNull(subject.Source);
            subject.Source = containingObject;
            Assert.AreEqual(containingObject, subject.Source);
            subject.ClearSource();
            Assert.IsNull(subject.Source);
        }

        [Test]
        public void ClearSourceInactiveGameObject()
        {
            Assert.IsNull(subject.Source);
            subject.Source = containingObject;
            Assert.AreEqual(containingObject, subject.Source);
            subject.gameObject.SetActive(false);
            subject.ClearSource();
            Assert.AreEqual(containingObject, subject.Source);
        }

        [Test]
        public void ClearOriginInactiveComponent()
        {
            Assert.IsNull(subject.Source);
            subject.Source = containingObject;
            Assert.AreEqual(containingObject, subject.Source);
            subject.enabled = false;
            subject.ClearSource();
            Assert.AreEqual(containingObject, subject.Source);
        }

        [Test]
        public void ClearParent()
        {
            Assert.IsNull(subject.Parent);
            subject.Parent = containingObject;
            Assert.AreEqual(containingObject, subject.Parent);
            subject.ClearParent();
            Assert.IsNull(subject.Parent);
        }

        [Test]
        public void ClearParentInactiveGameObject()
        {
            Assert.IsNull(subject.Parent);
            subject.Parent = containingObject;
            Assert.AreEqual(containingObject, subject.Parent);
            subject.gameObject.SetActive(false);
            subject.ClearParent();
            Assert.AreEqual(containingObject, subject.Parent);
        }

        [Test]
        public void ClearParentInactiveComponent()
        {
            Assert.IsNull(subject.Parent);
            subject.Parent = containingObject;
            Assert.AreEqual(containingObject, subject.Parent);
            subject.enabled = false;
            subject.ClearParent();
            Assert.AreEqual(containingObject, subject.Parent);
        }
    }
}