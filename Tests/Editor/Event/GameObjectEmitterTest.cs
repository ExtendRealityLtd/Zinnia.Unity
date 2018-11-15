using VRTK.Core.Event;

namespace Test.VRTK.Core.Event
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.VRTK.Core.Utility.Mock;

    public class GameObjectEmitterTest
    {
        private GameObject containingObject;
        private GameObjectEmitterImplementation subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<GameObjectEmitterImplementation>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void Extract()
        {
            GameObject source = new GameObject();

            UnityEventListenerMock extractedMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedMock.Listen);

            subject.SetResult(source);

            Assert.IsFalse(extractedMock.Received);
            subject.Extract();
            Assert.IsTrue(extractedMock.Received);
            Assert.AreEqual(subject.Result, source);

            Object.DestroyImmediate(source);
        }

        [Test]
        public void ExtractInvalidResult()
        {
            GameObject source = new GameObject();

            UnityEventListenerMock extractedMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedMock.Listen);

            Assert.IsFalse(extractedMock.Received);
            subject.Extract();
            Assert.IsFalse(extractedMock.Received);
            Assert.IsNull(subject.Result);

            Object.DestroyImmediate(source);
        }

        [Test]
        public void ExtractInactiveGameObject()
        {
            GameObject source = new GameObject();

            UnityEventListenerMock extractedMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedMock.Listen);

            subject.SetResult(source);
            subject.gameObject.SetActive(false);

            Assert.IsFalse(extractedMock.Received);
            subject.Extract();
            Assert.IsFalse(extractedMock.Received);
            Assert.IsNull(subject.Result);

            Object.DestroyImmediate(source);
        }

        [Test]
        public void ExtractInactiveComponent()
        {
            GameObject source = new GameObject();

            UnityEventListenerMock extractedMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedMock.Listen);

            subject.SetResult(source);
            subject.enabled = false;

            Assert.IsFalse(extractedMock.Received);
            subject.Extract();
            Assert.IsFalse(extractedMock.Received);
            Assert.IsNull(subject.Result);

            Object.DestroyImmediate(source);
        }
    }

    public class GameObjectEmitterImplementation : GameObjectEmitter
    {
        public void SetResult(GameObject result)
        {
            Result = result;
        }
    }
}