using Zinnia.Data.Operation.Extraction;

namespace Test.Zinnia.Data.Operation.Extraction
{
    using NUnit.Framework;
    using System;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using UnityEngine.Events;
    using Assert = UnityEngine.Assertions.Assert;
    using Object = UnityEngine.Object;

    public class GameObjectExtractorTest
    {
        private GameObject containingObject;
        private GameObjectExtractorImplementation subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<GameObjectExtractorImplementation>();
        }

        [TearDown]
        public void TearDown()
        {
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

    public class GameObjectExtractorImplementation : GameObjectExtractor<GameObject, GameObjectExtractorImplementation.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the specified <see cref="GameObject"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<GameObject> { }

        public void SetResult(GameObject result)
        {
            Result = result;
        }

        protected override GameObject ExtractValue()
        {
            return Result;
        }
    }
}